using System.Text.Json.Serialization;

namespace RequestProcessor.App.Models
{
    internal class RequestOptions : IRequestOptions, IResponseOptions
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("method")]
        public RequestMethod Method { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Name.Trim()) || string.IsNullOrEmpty(Address.Trim()) || string.IsNullOrEmpty(Path.Trim()))
                    return false;

                if ((Method == RequestMethod.Post || Method == RequestMethod.Put) && Body != null)
                    return true;

                return Method == RequestMethod.Get || Method == RequestMethod.Patch;
            }
           
        }
    }
}
