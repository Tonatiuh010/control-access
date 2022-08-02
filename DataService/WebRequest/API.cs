using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http.Headers;

namespace DataService.WebRequest
{
    public abstract class WebRequest : IDisposable
    {
        public HttpClient Client { get; set; }
        public string Url { get; set; }
        
        public WebRequest(string url)
        {
            Client = new();
            Url = url;
        }
        

        private void SetClient(string url)
        {
            if (Client != null)
            {
                Client.BaseAddress = new Uri(url);
                Client.DefaultRequestHeaders.Accept.Clear();
                //Client.DefaultRequestHeaders.Add(GetValidHeaders());
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public static List<MediaTypeWithQualityHeaderValue> GetValidHeaders() =>
            new() {
                new ("application/json"),
                new ("application/text"),
                new ("application/html"),
            };
    }
}
