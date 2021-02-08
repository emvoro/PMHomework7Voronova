using System;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Services;
using RequestProcessor.App.Exceptions;
using System.Linq;

namespace RequestProcessor.App.Menu
{
    /// <summary>
    /// Main menu.
    /// </summary>
    internal class MainMenu : IMainMenu
    {
        private readonly IRequestPerformer _requestPerformer;
        private readonly IOptionsSource _optionsSource;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor with DI.
        /// </summary>
        /// <param name="options">Options source</param>
        /// <param name="performer">Request performer.</param>
        /// <param name="logger">Logger implementation.</param>
        public MainMenu(
            IRequestPerformer performer, 
            IOptionsSource options, 
            ILogger logger)
        {
            _requestPerformer = performer;
            _optionsSource = options;
            _logger = logger;
        }

        public MainMenu(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<int> StartAsync()
        {
            try
            {
                DisplayAction(" Downloading options from source...");
                var optionsValue = (await _optionsSource.GetOptionsAsync()).Where(x => x.Item1.IsValid);
                DisplayAction(" Options are successfully downloaded.");
                DisplayAction(" Starting to perform tasks...");
                var tasks = optionsValue.Select(x => _requestPerformer.PerformRequestAsync(x.Item1, x.Item2)).ToArray();
                DisplayAction(" Waiting for all tasks...");
                Task.WaitAll(tasks);
                tasks.ToList().ForEach(x =>
                {
                    DisplayAction("\n Id       : " + x.Id + "\n Status   : " + x.Status.ToString() + "\n Result   : " + x.Result);
                });
                return 0;
            }
            catch (PerformException ex)
            {
                DisplayAction(ex.Message);
                return -1;
            }
        }

        public void DisplayAction(string message)
        {
            Console.WriteLine(message);
            _logger.Log(message);
        }
    }
}
