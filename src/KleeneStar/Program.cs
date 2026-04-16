using System.Reflection;
using WebExpress.WebCore;
using WebExpress.WebCore.WebLog;

namespace KleeneStar
{
    /// <summary>
    /// Serves as the entry point for the kleenestar application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The entry point of the WebExpress.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        private static void Main(string[] args)
        {
            var app = new WebEx()
            {
                Name = Assembly.GetExecutingAssembly().GetName().Name
            };

            app.Initialization += (s, e) =>
            {
                var log = WebEx.ComponentHub.LogManager.DefaultLog;
                using var frame = new LogFrame(log, "KleeneStar startup");
                log.Info($"{app.Name} version {Assembly.GetExecutingAssembly().GetName().Version} initializing...");
            };

            app.Execution(args);
        }
    }
}