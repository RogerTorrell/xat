using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace client
{
    class UserController_Server
    {
        //Això representa el model de dades User
        public User user { get; set; }

        public TcpClient tcpClient { get; set; }
        public NetworkStream NStream { get; set; }

        //Constructor
        public UserController_Server()
        {
            
        }
    }
}
