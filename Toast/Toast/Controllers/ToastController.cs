using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Toast.Controllers
{
    [RoutePrefix("api/toast")]
    public class ToastController : ApiController
    {
        private ToastMessages _toastmessages = new ToastMessages();

        [Route("french")]
        [HttpPost]
        public string GetFrench() => Toast("French Toast!");

        [Route("melba")]
        [HttpPost]
        public string GetMelba() => Toast("Melba Toast!");

        [Route("burnt")]
        [HttpPost]
        public string GetBurnt() => Toast("911!");

        private string Toast(string message)
        {
            _toastmessages.send(message);
            return message;
        }

        [HubName("toastMessages")]
        public class ToastMessages : Hub
        {
            private IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<ToastMessages>();

            public void send(string message)
            {
                _context.Clients.All.addMessage(message);
            }
        }
    }
}
