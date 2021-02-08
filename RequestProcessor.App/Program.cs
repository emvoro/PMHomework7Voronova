using System;
using System.Net.Http;
using System.Threading.Tasks;
using RequestProcessor.App.Menu;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Services;

namespace RequestProcessor.App
{
    /// <summary>
    /// Entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <returns>Returns exit code.</returns>
        private static async Task<int> Main()
        {
            try
            {
                var logger = new Logger();
                var options = new OptionsSource("options.json");
                var performer = new RequestPerformer(new RequestHandler(new HttpClient()), new ResponseHandler(), logger);
                var mainMenu = new MainMenu(performer, options, logger);

                return await mainMenu.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical unhandled exception");
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
