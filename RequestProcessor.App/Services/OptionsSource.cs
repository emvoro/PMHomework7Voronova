using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RequestProcessor.App.Models;
using RequestProcessor.App.Exceptions;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RequestProcessor.App.Services
{
    internal class OptionsSource: IOptionsSource
    {
        private readonly string _path;

        public OptionsSource(string path)
        {
            _path = path;
        }

        public async Task<IEnumerable<(IRequestOptions, IResponseOptions)>> GetOptionsAsync() 
        {
            List<RequestOptions> requestOptions = new List<RequestOptions>();
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };

            try
            {
                using (StreamReader sr = new StreamReader(_path))
                {
                    requestOptions = JsonSerializer.Deserialize<List<RequestOptions>>(await sr.ReadToEndAsync(), options);
                }
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is JsonException) 
            {
                throw new PerformException(" options.json are missing or corrupted");
            }

            var optionsResult = requestOptions.Select(x => ((IRequestOptions)x, (IResponseOptions)x)).ToList();
            return optionsResult.AsEnumerable();
        }
    }
}
