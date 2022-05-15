using System.Net;

namespace CovidData.Models.Returns
{
    public class Response
    {
        public object Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

    }
}
