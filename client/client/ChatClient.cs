using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace client
{
    public class ChatClient
    {
        public UserController ConnectedUser {get; set; }
		
		//Socket de connexió amb el server
        public TcpClient tcpClient { get; set; }
        public NetworkStream NStream { get; set; }		

        /// <summary>
        /// Afegeix un nou usuari al xat
        /// </summary>
        /// <param name="_UserName">Nom d'usuari</param>
        public void JoinUserClient(string _UserName)
        {
            //Creem un nou usuari
            User myUser = new User();
            myUser.UserName = _UserName;

            //Creem un nou controlador per la gestió del UserPanel
            this.ConnectedUser = new UserController(myUser, new UserPanel());
            tcpClient = new TcpClient();

            ConnectionToServer();
            NStream = tcpClient.GetStream();
            			
			SendUserNameToServer();

            //Thread d'escolta per missatges
            Thread ReceiveMessages = new Thread(Receive);
            ReceiveMessages.Start(NStream);

            //Creem i mostrem un nou panell
            ConnectedUser.CreatePanel();
            ConnectedUser.ShowPanel();

            //Mostrem la llista d'usuaris connectats
            //AddUsersToPanel(myUserController);

            //Actualitzem la resta d'usuaris connectats per tal de mostrar el nou usuari connectat
            //UpdateUsersPanels(myUserController);
        }

		public void SendUserNameToServer()
        {
            Byte[] msg;

            msg = Encoding.Unicode.GetBytes(ConnectedUser.user.UserName);

            NStream.Write(msg,0,msg.Length);
        }
		
		public void SendMessageToServer(string Message, string Destination)
		{
			Byte[] msg;
            string MessageToSend;
			
			MessageToSend = ConnectedUser.user.UserName + ":" + Message + ":" + Destination;
			
            msg = Encoding.Unicode.GetBytes(MessageToSend);
            NStream.Write(msg,0,msg.Length);

            ConnectedUser.ShowMessage(Message, ConnectedUser.user.UserName);
		}
		
        private void ConnectionToServer()
        {
            //Creem un IPEndPoint amb l'adreça del server IP + Port
            IPAddress ipadress = Dns.GetHostAddresses("localhost")[1];
            IPEndPoint ipendpoint = new IPEndPoint(ipadress, 11000);

            //Connectem amb el Server
            this.tcpClient.Connect(ipendpoint);
        }

        private void Receive (object DataReceive)
        {
            Byte[] bytes = new Byte[256];
            int BytesReceived;
            string MessageNoSplit = String.Empty;
			string[] Messages;

            //NetworkStream myNStream = (NetworkStream)DataReceive;
            
            while (true)
            {
                MessageNoSplit = String.Empty;
				//Rebem missatge	
				//Format dels missatges emissor:missatge:receptor
                BytesReceived = NStream.Read(bytes, 0, bytes.Length);
                MessageNoSplit = MessageNoSplit + System.Text.Encoding.Unicode.GetString(bytes, 0, BytesReceived);

                while (NStream.DataAvailable)
                {
                    BytesReceived = NStream.Read(bytes, 0, bytes.Length);
                    MessageNoSplit = MessageNoSplit + System.Text.Encoding.Unicode.GetString(bytes, 0, BytesReceived);
                }

				Messages = MessageNoSplit.Split(':');
                
                Action action = delegate()
                {
                    ConnectedUser.ShowMessage(Messages[1], Messages[0]);										
                };

                ConnectedUser.Panel.Dispatcher.Invoke(action);
            }
        }
    }
}
