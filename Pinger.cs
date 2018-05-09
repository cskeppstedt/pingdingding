using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Globalization;

namespace pingdingding
{
    public class Pinger
    {
        private HttpClient Client = new HttpClient();
        private string CurrentMessage = "";

        private string UnpackErrorMessage(HttpRequestException exception)
        {
            try
            {
                var outer = exception?.Message;
                var inner = exception?.InnerException?.Message;
                return $"\"{outer}\" - \"{inner}\"";
            }
            catch
            {
                return "undefined error";
            }
        }

        private string InternalIP()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address.ToString();
                }
            }
            catch
            {
                return "<unknown>";
            }
        }

        public async Task<PingResult> Ping()
        {
            string message;

            try
            {
                var ip = await Client.GetStringAsync("https://api.ipify.org");
                message = $"UP\tLink is now UP - External IP is {ip} - Internal IP is {this.InternalIP()}";
            }
            catch (Exception exc)
            {
                if (exc is HttpRequestException)
                {
                    message = $"DOWN\tLink is now DOWN - {this.UnpackErrorMessage((HttpRequestException)exc)}";
                }
                else
                {
                    message = $"DOWN\tLink is now DOWN - generic error - \"{exc.Message}\"";
                }
            }

            if (message.Equals(this.CurrentMessage))
            {
                return new PingResult(message, stateChanged: false);
            }
            else
            {
                this.CurrentMessage = message;
                return new PingResult(message, stateChanged: true);
            }
        }
    }
}