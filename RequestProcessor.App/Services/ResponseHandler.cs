using System;
using System.Threading.Tasks;
using System.IO;
using RequestProcessor.App.Models;

namespace RequestProcessor.App.Services
{
    internal class ResponseHandler : IResponseHandler
    {
        public async Task HandleResponseAsync(IResponse response, IRequestOptions requestOptions, IResponseOptions responseOptions)
        {
            if (response == null || requestOptions == null || responseOptions == null)
                throw new ArgumentNullException(" Some arguments are null.");

            string responseToFile = response.Code.ToString();

            if (response.Handled && response.Code > 199 && response.Code < 300)
                responseToFile += " OK\n";
            else
                responseToFile += " ERROR\n";

            responseToFile += response.Content;
            await File.WriteAllTextAsync(responseOptions.Path, responseToFile);
        }
    }
}
