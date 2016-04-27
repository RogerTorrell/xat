using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using client;

namespace server
{
    class ChatServer
    {
        List<UserController_Server> ConnectedUsers = new List<UserController_Server>();
        
        /// <summary>
        /// Afegeix un nou usuari al xat
        /// </summary>
        /// <param name="UserName">Nom d'usuari</param>
        public void JoinUserServer(string UserName,NetworkStream NStream)
        {
            //Creem un nou usuari
            User myUser = new User();
            myUser.UserName = UserName;

            //Creem un nou controlador
            UserController_Server myUserController = new UserController_Server();
            
            //Afegim usuari a la llista d'usuaris connecatats
            myUserController.NStream = NStream;
            myUserController.user = myUser;
            ConnectedUsers.Add(myUserController);

            //Mostrem la llista d'usuaris connectats
            //AddUsersToPanel(myUserController);

            //Actualitzem la resta d'usuaris connectats per tal de mostrar el nou usuari connectat
            //UpdateUsersPanels(myUserController);
        }

        public int GetUserByName (string UserName)
        {
            bool found = false;
            int index = 0;

            while (!found && index < ConnectedUsers.Count)
            {
                if (ConnectedUsers.ElementAt(index).user.UserName.Equals(UserName))
                {
                    found = true;
                }
                index++;
            }

            if (found)
            {
                return index - 1;
            }
            else
            {
                return -1;
            }
        }

        public void SendMessage (string message, string UserName)
        { 
            Byte[] msg;
            int index;

			index = GetUserByName(UserName);
            msg = Encoding.Unicode.GetBytes(message);
            ConnectedUsers.ElementAt(index).NStream.Write(msg, 0, msg.Length);
        }
    }
}
