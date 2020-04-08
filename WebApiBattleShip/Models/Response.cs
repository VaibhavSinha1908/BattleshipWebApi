using System.Net;

namespace WebApiBattleShip.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }

}