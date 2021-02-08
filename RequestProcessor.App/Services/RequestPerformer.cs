using System;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Models;
using RequestProcessor.App.Menu;
using RequestProcessor.App.Exceptions;

namespace RequestProcessor.App.Services
{
    /// <summary>
    /// Request performer.
    /// </summary>
    internal class RequestPerformer : IRequestPerformer 
    {
        public IRequestHandler _requestHandler { get; }
        public IResponseHandler _responseHandler { get; }
        public ILogger _logger { get; }

        /// <summary>
        /// Constructor with DI.
        /// </summary>
        /// <param name="requestHandler">Request handler implementation.</param>
        /// <param name="responseHandler">Response handler implementation.</param>
        /// <param name="logger">Logger implementation.</param>
        public RequestPerformer(
            IRequestHandler requestHandler, 
            IResponseHandler responseHandler,
            ILogger logger)
        {
            _requestHandler = requestHandler;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<bool> PerformRequestAsync(
            IRequestOptions requestOptions, 
            IResponseOptions responseOptions)
        {
            Response response = null;
            try
            {
                MainMenu MainMenu = new MainMenu(_logger);
                MainMenu.DisplayAction(" Request  : " + requestOptions.Name);
                response = (Response)await _requestHandler.HandleRequestAsync(requestOptions);
                MainMenu.DisplayAction(" Response : " + requestOptions.Name + ", Code - " + response.Code);
                await _responseHandler.HandleResponseAsync(response, requestOptions, responseOptions);
                return true;
            }
            catch (Exception ex)
            {
                if (response != null)
                {
                    response.Handled = false;
                    await _responseHandler.HandleResponseAsync(response, requestOptions, responseOptions);
                }

                throw new PerformException(ex.Message);
            }
        }
    }
}
