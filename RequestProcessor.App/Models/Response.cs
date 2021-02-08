namespace RequestProcessor.App.Models
{
    public class Response : IResponse
    {
        public bool Handled { get; set; }

        public int Code { get; }

        public string Content { get; }

        public Response(int code, string content)
        {
            Handled = true;
            Code = code;
            Content = content;
        }
    }
}
