#requires -Version 5.1
<#
.SYNOPSIS
	KleeneStar quick install script for native Windows (PowerShell).

.DESCRIPTION
	Heads up: Native Windows runs KleeneStar without WSL - the WebExpress-based
	web server and all plugins work natively. If you would rather use WSL2, the
	Linux/macOS one-liner (install.sh) works there too.

.EXAMPLE
	iex (irm https://raw.githubusercontent.com/kleenestar-project/KleeneStar/develop/install.ps1)

.NOTES
	Environment variables:
	  KLEENESTAR_DIR      Installation directory (default: .\KleeneStar)
	  KLEENESTAR_BRANCH   Branch to check out (default: develop)
	  KLEENESTAR_NO_RUN   Set to 1 to skip 'dotnet run' after the build
#>
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$Repos     = @('KleeneStar', 'KleeneStar.Core', 'KleeneStar.Model', 'KleeneStar.Portal', 'KleeneStar.Templates')
$OrgUrl    = 'https://github.com/kleenestar-project'
$InstallDir = if ($env:KLEENESTAR_DIR) { $env:KLEENESTAR_DIR } else { 'KleeneStar' }
$Branch    = if ($env:KLEENESTAR_BRANCH) { $env:KLEENESTAR_BRANCH } else { 'develop' }

function Write-Info    { param([string]$Message) Write-Host "==> $Message" -ForegroundColor Green }
function Write-WarnMsg { param([string]$Message) Write-Host "==> $Message" -ForegroundColor Yellow }
function Fail          { param([string]$Message) Write-Host "ERROR: $Message" -ForegroundColor Red; exit 1 }

# --- check prerequisites ------------------------------------------------------

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
	Fail 'git is not installed. Install git from https://git-scm.com and re-run this script.'
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
	Write-WarnMsg 'The .NET 10 SDK is not installed.'
	Write-WarnMsg 'Install it from https://dot.net/download and re-run this script.'
	exit 1
}

$sdkVersion = (& dotnet --version)
if ($sdkVersion -notlike '10.*') {
	Write-WarnMsg ".NET SDK $sdkVersion detected. KleeneStar targets .NET 10."
	Write-WarnMsg 'Install the .NET 10 SDK from https://dot.net/download if the build fails.'
}

# --- clone / update the workspace ---------------------------------------------
# KleeneStar consists of multiple sibling repositories. The main repository
# references the others via relative project references, so all repositories
# must live side by side in the same parent directory.

Write-Info "Setting up KleeneStar workspace in '$InstallDir' (branch: $Branch)"

$baseDir = $null
if (Test-Path (Join-Path $InstallDir '.git')) {
	$baseDir = (Resolve-Path $InstallDir).Path
	Write-Info 'Existing installation found, updating repositories'
} else {
	$baseDir = Join-Path ((Get-Location).Path) $InstallDir
}

New-Item -ItemType Directory -Force -Path $baseDir | Out-Null
Push-Location $baseDir

try {
	foreach ($repo in $Repos) {
		if (Test-Path (Join-Path $repo '.git')) {
			Write-Info "Updating $repo"
			& git -C $repo fetch --quiet origin $Branch 2>$null
			if ($LASTEXITCODE -ne 0) { Write-WarnMsg "Could not fetch $repo" }
			& git -C $repo checkout --quiet $Branch 2>$null
			if ($LASTEXITCODE -ne 0) { Write-WarnMsg "Could not checkout $Branch in $repo" }
			& git -C $repo pull --ff-only --quiet 2>$null
			if ($LASTEXITCODE -ne 0) { Write-WarnMsg "Could not fast-forward $repo" }
		} else {
			Write-Info "Cloning $repo"
			& git clone --quiet --branch $Branch "$OrgUrl/$repo.git" $repo 2>$null
			if ($LASTEXITCODE -ne 0) {
				# fall back to the default branch when the requested branch does not exist
				& git clone --quiet "$OrgUrl/$repo.git" $repo
				if ($LASTEXITCODE -ne 0) { Fail "Failed to clone $OrgUrl/$repo.git" }
			}
		}
	}

	# --- restore & build -------------------------------------------------------

	Push-Location (Join-Path $baseDir 'KleeneStar\src\KleeneStar')
	try {
		Write-Info 'Restoring dependencies (this includes the WebExpress framework packages)'
		& dotnet restore
		if ($LASTEXITCODE -ne 0) { Fail 'dotnet restore failed.' }

		Write-Info 'Building KleeneStar'
		& dotnet build --configuration Release --no-restore
		if ($LASTEXITCODE -ne 0) { Fail 'dotnet build failed.' }

		# --- run ----------------------------------------------------------------

		if ($env:KLEENESTAR_NO_RUN -eq '1') {
			Write-Info 'Installation finished. Start the server with:'
			Write-Info "  cd $baseDir\KleeneStar\src\KleeneStar; dotnet run"
			exit 0
		}

		Write-Info 'Starting KleeneStar (Ctrl+C to stop)'
		Write-Info 'The server listens on http://localhost'
		& dotnet run
	} finally {
		Pop-Location
	}
} finally {
	Pop-Location
}
