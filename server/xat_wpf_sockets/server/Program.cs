using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using client;

namespace server
{
    class Program
    {
        public static ChatServer myChatServer = new ChatServer();

        static void Main(string[] args)
        {            
            IPAddress ipAddress = Dns.GetHostAddresses("localhost")[1];
            IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, 11000);

            TcpListener tcplistener = new TcpListener(ipLocalEndPoint);
            tcplistener.Start();

            while (true)
            {
                
                //Acceptem connexions pendents
                TcpClient client = tcplistener.AcceptTcpClient();

                Thread ProcessaClient = new Thread(ClientChat);
                ProcessaClient.Start(client);
            }
        }

        static void ClientChat (object ClientData)
        {
            Byte[] bytes = new Byte[256];
            Byte[] msg;
            int BytesReceived;
            string UserName  = String.Empty;
            string missatge_abans_split = String.Empty;
            

            TcpClient client = (TcpClient)ClientData;
            NetworkStream netStream = client.GetStream();

            //Rebem el nom de l'usuari
            BytesReceived = netStream.Read(bytes, 0, bytes.Length);
            UserName = UserName + System.Text.Encoding.Unicode.GetString(bytes, 0, BytesReceived);

            while (netStream.DataAvailable)
            {
                BytesReceived = netStream.Read(bytes, 0, bytes.Length);
                UserName = UserName + System.Text.Encoding.Unicode.GetString(bytes, 0, BytesReceived);
            }

            //Afegim usuari al Chat
            myChatServer.JoinUserServer(UserName, netStream);

            while (true)
            {
                string[] missatges;
                //Rebre missatge 
                //El format dels missatges serà el següent emissor:missatge:receptor#

                BytesReceived = netStream.Read(bytes, 0, bytes.Length);
                missatge_abans_split = missatge_abans_split + System.Text.Encoding.Unicode.GetString(bytes, 0, BytesReceived);

                while (netStream.DataAvailable)
                {
                    BytesReceived = netStream.Read(bytes, 0, bytes.Length);
                    missatge_abans_split = missatge_abans_split + System.Text.Encoding.Unicode.GetString(bytes, 0, BytesReceived);
                }

                //Les dades les obtenim en el buffer que li hem indicat en el fer el BeginRead. 
                //phrase.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                //missatge_abans_split = System.Text.Encoding.Unicode.GetString(, 0, BytesReceived);
                missatges = missatge_abans_split.Split(':');

                //Envia missatge
                myChatServer.SendMessage(missatge_abans_split, missatges[2]);
                missatge_abans_split = String.Empty;
            }
        }
    }
}
