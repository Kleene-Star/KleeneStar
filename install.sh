#!/usr/bin/env bash
#
# KleeneStar quick install script for Linux, macOS, WSL2 and Termux.
#
# Usage:
#   curl -fsSL https://raw.githubusercontent.com/kleenestar-project/KleeneStar/develop/install.sh | bash
#
# Environment variables:
#   KLEENESTAR_DIR      Installation directory (default: ./KleeneStar)
#   KLEENESTAR_BRANCH   Branch to check out (default: develop)
#   KLEENESTAR_NO_RUN   Set to 1 to skip 'dotnet run' after the build
#
set -euo pipefail

REPOS="KleeneStar KleeneStar.Core KleeneStar.Model KleeneStar.Portal KleeneStar.Templates"
ORG_URL="https://github.com/kleenestar-project"
INSTALL_DIR="${KLEENESTAR_DIR:-KleeneStar}"
BRANCH="${KLEENESTAR_BRANCH:-develop}"

info() { printf '\033[0;32m==>\033[0m %s\n' "$*"; }
warn() { printf '\033[0;33m==> %s\033[0m\n' "$*" >&2; }
fail() { printf '\033[0;31mERROR: %s\033[0m\n' "$*" >&2; exit 1; }

# --- check prerequisites -----------------------------------------------------

command -v git >/dev/null 2>&1 || fail "git is not installed. Install git and re-run this script."

if ! command -v dotnet >/dev/null 2>&1; then
	warn "The .NET 10 SDK is not installed."
	warn "Install it from https://dot.net/download and re-run this script."
	exit 1
fi

SDK_VERSION="$(dotnet --version)"
case "$SDK_VERSION" in
	10.*) ;;
	*) warn ".NET SDK $SDK_VERSION detected. KleeneStar targets .NET 10."
	   warn "Install the .NET 10 SDK from https://dot.net/download if the build fails." ;;
esac

# --- clone / update the workspace --------------------------------------------
# KleeneStar consists of multiple sibling repositories. The main repository
# references the others via relative project references, so all repositories
# must live side by side in the same parent directory.

info "Setting up KleeneStar workspace in '$INSTALL_DIR' (branch: $BRANCH)"

if [ -d "$INSTALL_DIR/.git" ]; then
	BASE_DIR="$(cd "$INSTALL_DIR" && pwd)"
	info "Existing installation found, updating repositories"
else
	BASE_DIR="$(cd "$(dirname "$INSTALL_DIR")" && pwd)/$(basename "$INSTALL_DIR")"
fi

mkdir -p "$BASE_DIR"
cd "$BASE_DIR"

for repo in $REPOS; do
	if [ -d "$repo/.git" ]; then
		info "Updating $repo"
		git -C "$repo" fetch --quiet origin "$BRANCH" || warn "Could not fetch $repo"
		git -C "$repo" checkout --quiet "$BRANCH" 2>/dev/null || warn "Could not checkout $BRANCH in $repo"
		git -C "$repo" pull --ff-only --quiet || warn "Could not fast-forward $repo"
	else
		info "Cloning $repo"
		if ! git clone --quiet --branch "$BRANCH" "$ORG_URL/$repo.git" "$repo" 2>/dev/null; then
			# fall back to the default branch when the requested branch does not exist
			git clone --quiet "$ORG_URL/$repo.git" "$repo" || fail "Failed to clone $ORG_URL/$repo.git"
		fi
	fi
done

# --- restore & build ----------------------------------------------------------

cd "$BASE_DIR/KleeneStar/src/KleeneStar"

info "Restoring dependencies (this includes the WebExpress framework packages)"
dotnet restore

info "Building KleeneStar"
dotnet build --configuration Release --no-restore

# --- run ----------------------------------------------------------------------

if [ "${KLEENESTAR_NO_RUN:-0}" = "1" ]; then
	info "Installation finished. Start the server with:"
	info "  cd $BASE_DIR/KleeneStar/src/KleeneStar && dotnet run"
	exit 0
fi

info "Starting KleeneStar (Ctrl+C to stop)"
info "The server listens on http://localhost"
dotnet run
