using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ToastUtil
{
    public class ToastHttpClient : HttpClient
    {
        private static DecompressionMethods AutomaticDecompressionValue =
            DecompressionMethods.GZip | DecompressionMethods.Deflate;

        public ToastHttpClient() 
            : base(new HttpClientHandler() { AutomaticDecompression = AutomaticDecompressionValue })
        { }

        public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return base.SendAsync(request);
        }
    }
}
