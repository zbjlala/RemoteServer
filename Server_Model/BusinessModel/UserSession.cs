using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;

namespace Server_Model.BusinessModel
{
    public class UserSession
    {
        //用户名
        private string name;

        //订阅主机名
        private string subIP;

        //Session
       // private IWebSocketSession webSocketSession;

        //SessionID
        private string sessionID;
        //心跳时间
        private DateTime activeTime;

        public string Name { get => name; set => name = value; }
        public string SubIP { get => subIP; set => subIP = value; }
      //  public IWebSocketSession WebSocketSession { get => webSocketSession; set => webSocketSession = value; }
        public string SessionID { get => sessionID; set => sessionID = value; }
        public DateTime ActiveTime { get => activeTime; set => activeTime = value; }
    }
}
