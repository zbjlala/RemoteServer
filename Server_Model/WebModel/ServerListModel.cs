using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.WebModel
{
    public class ServerListModel
    {
        private string serverIP;
        private string serverPoint;

        public string ServerIP { get => serverIP; set => serverIP = value; }
        public string ServerPoint { get => serverPoint; set => serverPoint = value; }
    }
}
