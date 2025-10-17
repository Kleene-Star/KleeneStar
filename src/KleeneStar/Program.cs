using System.Reflection;
using WebExpress.WebCore;

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

            app.Execution(args);
        }
    }
}