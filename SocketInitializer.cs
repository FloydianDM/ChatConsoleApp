using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatConsoleApp
{
    public class ProgramInitializer
    {
        private Socket _socket;
        private EndPoint _epLocal;
        private EndPoint _epRemote;
        private string _localIp;
        private string _remoteIP;

        public void SetSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            _localIp = GetLocalIp();
            _remoteIP = GetLocalIp();
        }
        
        private string GetLocalIp()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            string localHost = "127.0.0.1";

            foreach (IPAddress ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress.ToString();
                }
            }

            return localHost;
        }

        private void ExecuteMessageCallback(IAsyncResult asyncResult)
        {
            try
            {
                int dataSize = _socket.EndReceiveFrom(asyncResult, ref _epRemote);

                if (dataSize > 0)
                {
                    byte[] receivedData = new byte[1464];
                    receivedData = (byte[])asyncResult.AsyncState;

                    ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();  // for byte to string conversion
                    string receivedMessage = aSCIIEncoding.GetString(receivedData);
                
                    Console.WriteLine($"Message: {receivedMessage}");
                }

                byte[] buffer = new byte[1500];
                _socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref _epRemote, new AsyncCallback(ExecuteMessageCallback), buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void StartServer(string localPortNumber, string remotePortNumber)
        {
            try
            {
                _epLocal = new IPEndPoint(IPAddress.Parse(_localIp), Convert.ToInt32(localPortNumber));
                _socket.Bind(_epLocal);

                _epRemote = new IPEndPoint(IPAddress.Parse(_remoteIP), Convert.ToInt32(remotePortNumber));
                _socket.Connect(_epRemote);

                byte[] buffer = new byte[1500];
                _socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref _epRemote, new AsyncCallback(ExecuteMessageCallback), buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SendMessage(string msg)
        {
            try
            {
                ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
                
                byte[] message = new byte[1500];
                message = aSCIIEncoding.GetBytes(msg);

                _socket.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
