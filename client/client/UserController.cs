using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Windows.Threading;

namespace client
{
    public class UserController
    {
        //Això representa el model de dades User
        public User user { get; set; }

        //Això representa la vista pel panell d'usuari
        public UserPanel Panel { get; set; }

        //Constructor
        public UserController(User _User, UserPanel _PanelUser)
        {
            //this.Panel = new UserPanel();
            this.user = _User;
            this.Panel = _PanelUser;            
        }

        //Actualitza dades del panell
        public void CreatePanel()
        {
            this.Panel.Name = this.user.UserName;
            this.Panel.Title = this.user.UserName + " panel";
        }

        /// <summary>
        /// Mostra el panell en pantalla
        /// </summary>
        public void ShowPanel()
        {
            this.Panel.Show();
        }

        /// <summary>
        /// Afegeix un missatge al contenidor de missatges
        /// </summary>
        /// <param name="msg">missatge</param>
        /// <param name="issuer">emissor del missatge</param>
        public void ShowMessage(string msg, string issuer)
        {
            Label LabelMsg = new Label();

            if (issuer.Equals(this.user.UserName))
            {
                LabelMsg.Content = "You says: " + msg;
            }
            else
            {
                LabelMsg.Content = issuer + " says: " + msg;
            }
            this.Panel.StackPanelConversation.Children.Add(LabelMsg);
        }

        /// <summary>
        /// Afegeix un nou usuari al contenidor d'usuaris
        /// </summary>
        /// <param name="user">Usuari que s'ha d'afegir</param>
        public void AddUserToPanel(User user)
        {
            Button btnUser = new Button();
            btnUser.Content = user.UserName;
            btnUser.Name = user.UserName;
            this.Panel.ConnectedUsers.Children.Add(btnUser);
        }
    }
}
