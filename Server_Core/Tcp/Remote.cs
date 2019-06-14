using Server_Core.Tcp.Server;
using Server_Helper;
using Server_Helper.HttpHelper;
using Server_Helper.StackExchangeRedis;
using Server_Model.BusinessModel;
using Server_Model.Entity;
using Server_Model.Enum;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Server_Core.Tcp
{
    public class Remote : WebSocketBehavior
    {
        private static readonly ServerConfig1 ServerConfig = ServerConfig1.Instance();

        private readonly Semaphore _receiveSemaphore;

        private readonly Semaphore _sendSemaphore;

       // private RedisStackExchangeHelper _redis;

        private ServerSocket _serverSocket;

        private static readonly string _serviceSender = "服宝Remote";//服务器发送消息Sender

        private static readonly string _onlinuserset = "Redis_OnlinUserSet";

        private static  string _onremotingset = "Redis_OnRemotingSet";

        private static  string USER_SESSION = "RemoteUserSeesion";//Redis所有用户Session

        private static  string Server_Address = "RemoteServerList";//Redis在线服务器集群IP

        protected static int _number = 0;

        private string _name;

        private static int initnum = 0;

        private string _ip;

        private static Dictionary<String, WebSocket> dicWebSocket = new Dictionary<string, WebSocket>();

        public Remote(ServerSocket server, string ip)
        {
            _receiveSemaphore = new Semaphore(10000 * ServerConfig1.Instance().OperationThreads, 100000 * ServerConfig1.Instance().OperationThreads);
            _sendSemaphore = new Semaphore(10000 * ServerConfig1.Instance().OperationThreads, 100000 * ServerConfig1.Instance().OperationThreads);
            //var channelKey = IPHelper.GetLocalIP();
            // SubscribeAsync(); 
            this._serverSocket = server;
            this._ip = ip;                    
        }

        public bool IsStarted
        {
            get; private set;
        }

        /// <summary>
        ///     在线客户端数
        /// </summary>
        public int OnlineNums
        {
            get
            {
                return Sessions.Count;
            }
        }
        public void SendPubMessage(RedisChannel channel, RedisValue message)
        {
            // Console.WriteLine("Remote队列收到一条消息" + message);
            Message msg = SerializeHelper.Deserialize<Message>(message);
            var userTokenString = RedisHelper.HashGet(USER_SESSION, msg.Accepter).ToString();
            UserSession userToken = SerializeHelper.Deserialize<UserSession>(userTokenString);
            try
            {
                if (msg.Protocal != (byte)MessageProtocalEnum.File)
                {
                    if (userToken != null)
                        Sessions.SendTo(SerializeHelper.Serialize(msg), userToken.SessionID);
                    // Console.WriteLine("发出一条消息" + message);
                }
                else
                {
                    if (userToken != null)
                        Sessions.SendTo(SerializeHelper.ProtolBufSerialize(msg), userToken.SessionID);
                }
            }
            catch
            {

            }
        }

        protected override void OnOpen()
        {
            if (initnum < 1)
            {
                SetRedisKey();
                _serverSocket.pubMessage += SendPubMessage;
                Interlocked.Increment(ref initnum);
                GetServerDicAsyn();
                CheckServerClientStatus();
            }

            var context = Context;

            _name = GetName();
            IWebSocketSession webSocketSession;
            if (Sessions.TryGetSession(this.ID, out webSocketSession))
            {
                try
                {
                    if (UserTokenList.GetUserTokenByUID(_name) != null)
                    {

                        UserTokenList.DelOnlineID(_name);
                    }

                    if (RedisHelper.HashGet(USER_SESSION, _name) != null)
                    {
                       RedisHelper.HashDelete(USER_SESSION, _name);
                    }

                    UserTokenList.SetOnlineID(_name, new UserSession
                    {
                        Name = _name,
                        SubIP = _ip,
                        //WebSocketSession = webSocketSession
                        SessionID = webSocketSession.ID,
                        ActiveTime = DateTime.Now
                    });
                   RedisHelper.HashPut(USER_SESSION, _name,SerializeHelper.Serialize( new UserSession
                    {
                        Name = _name,
                        SubIP = _ip,
                        //WebSocketSession = webSocketSession
                        SessionID = webSocketSession.ID,
                        ActiveTime = DateTime.Now
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString() + DateTime.Now);
                }
            }
            Console.WriteLine(String.Format("客户端:{0}已连接" + DateTime.Now, _name));


        }
        /// <summary>
        /// 获取UID
        /// </summary>
        /// <returns></returns>
        private string GetName()
        {
            var name = Context.QueryString["name"];
            return !name.IsNullOrEmpty() ? name : GetNumber().ToString();
        }

        private static int GetNumber()
        {
            return Interlocked.Increment(ref _number);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            _receiveSemaphore.WaitOne();
            //Sessions.Broadcast(String.Format("{0}: {1}", _name, e.Data));
            if (e.Data != null)
            {
                try
                {
                    Message msg = SerializeHelper.Deserialize<Message>(e.Data);
                    if (msg != null)
                    {
                        switch (msg.Protocal)
                        {
                            case (byte)MessageProtocalEnum.Heart:
                                try
                                {
                                    msg.Sender = _serviceSender;
                                    SendCommon(msg.Accepter, msg);
                                    UpdateHeart(msg.Accepter);
                                }
                                catch
                                {
                                    Console.WriteLine("Remote", "SendHeart 171");
                                }
                                break;
                            case (byte)MessageProtocalEnum.Login:
                                //TaskHelper.Run(()=> {

                                //    //RedisHelper.HashPut(_onlinuserset, userToken.UID , userToken.LastID);
                                //});
                                Message msgLoin = new Message()
                                {
                                    Sender = _serviceSender,
                                    Protocal = (byte)MessageProtocalEnum.RLogin,
                                    Data = Encoding.UTF8.GetBytes("登录成功"),
                                    Accepter = msg.Sender
                                };
                                SendCommon(msg.Sender, msgLoin);
                                break;
                            case (byte)MessageProtocalEnum.Logout:
                                UserTokenList.DelOnlineID(msg.Sender);
                                Logout(msg.Sender);
                                break;
                            case (byte)MessageProtocalEnum.PrivateMsg:
                                SendCommonPub(msg.Accepter, msg);
                               // Console.WriteLine("收到一条PrivateMsg,发送人" + msg.Sender + DateTime.Now);

                                break;
                            case (byte)MessageProtocalEnum.File:
                                SendCommonPub(msg.Accepter, msg);
                                break;
                            case (byte)MessageProtocalEnum.FileSlice:
                                SendCommonPub(msg.Accepter, msg);
                                break;
                            case (byte)MessageProtocalEnum.RemoteConnect:
                                SendCommonPub(msg.Accepter, msg);
                                break;
                            case (byte)MessageProtocalEnum.RemoteStart:
                                if (Encoding.UTF8.GetString(msg.Data) != VerbalInfo.REFUSE_REMOTE)
                                {
                                    if (!String.IsNullOrWhiteSpace(msg.Accepter) && !String.IsNullOrWhiteSpace(msg.Sender))
                                    {
                                        if (!OnRemotingList.SetRemotingSides(msg.Accepter, msg.Sender))
                                        {
                                            string msgFull = "远程服务队列已满，请稍后再试";
                                            SendCommonPub(msg.Sender, new Message()
                                            {
                                                Sender = _serviceSender,
                                                Accepter = msg.Sender,
                                                Protocal = (byte)MessageProtocalEnum.RemoteConnect,
                                                Data = Encoding.UTF8.GetBytes(msgFull),
                                            });
                                            SendCommonPub(msg.Accepter, new Message()
                                            {
                                                Sender = _serviceSender,
                                                Accepter = msg.Accepter,
                                                Protocal = (byte)MessageProtocalEnum.RemoteConnect,
                                                Data = Encoding.UTF8.GetBytes(msgFull),
                                            });
                                        }
                                        else
                                        {
                                            TaskHelper.Run(() =>
                                            {
                                                try
                                                {
                                                    string msgData = Encoding.UTF8.GetString(msg.Data);
                                                    //双方建立连接的数据库日志
                                                    string result = HttpProxy.GetRequestCommon(InterfaceUrl.REMOTE_ESCONNECTION, GetESConnection(msg.Accepter, msg.Sender, "1"));
                                                    //放入Redis正在远程列表
                                                    RedisHelper.HashPut(_onremotingset, msg.Sender, msg.Accepter);
                                                    RedisHelper.HashPut(_onremotingset, msg.Accepter, msg.Sender);
                                                    ////发送客户详细信息
                                                    string resultDetails = HttpProxy.GetRequestCommon(InterfaceUrl.USER_INFO, GetUserInfo(msg.Sender, msgData));
                                                    SendCommonJson(msg.Accepter, JsonResultCommon(Server_Helper.HttpHelper.ResultType.USER_INFO, resultDetails));
                                                    ////发送客服详细信息
                                                    string kefuresultDetails = HttpProxy.GetRequestCommon(InterfaceUrl.USER_INFO, GetUserInfo(msg.Accepter,""));
                                                    SendCommonJson(msg.Sender, JsonResultCommon(Server_Helper.HttpHelper.ResultType.USER_INFO, kefuresultDetails));
                                                    //    // RedisHelper.HashPut(_onremotingset, msg.Accepter, SerializeHelper.Serialize(GetStartConnectMode(msg.Accepter, msg.Sender)));
                                                    //    //  RedisHelper.HashPut(_onremotingset, msg.Sender, SerializeHelper.Serialize(GetStartConnectMode(msg.Accepter, msg.Sender)));
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Remote" + "243借口访问异常");
                                                }

                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    SendCommonPub(msg.Accepter, msg);
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString() + DateTime.Now.ToString());
                }


            }
            else
            {
                Message msgFile = SerializeHelper.ProtolBufDeserialize<Message>(e.RawData);
                SendCommonPub(msgFile.Accepter, msgFile);
                //Console.WriteLine("收到一条FileMsg,发送人" + msgFile.Sender + DateTime.Now);
            }
            _receiveSemaphore.Release();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            //  Sessions.Broadcast(String.Format("服务器已关闭，{0}", DateTime.Now));
         
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine(DateTime.Now + "|" + this.ID.ToString() + "|" + e.Exception.Message);
        }

        #region SendMessage

        /// <summary>
        /// 公共发布方法（Redis消息队列使用）
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="msg"></param>
        protected async void SendCommonPub(string UID, Message msg)
        {
            try
            {
                // var count = UserTokenList.GetCount();
                //if (!UserTokenList.IsEmpty() && (msg != null))
                // {
                var userTokenString =  RedisHelper.HashGet(USER_SESSION, UID).ToString();
                UserSession userToken = SerializeHelper.Deserialize<UserSession>(userTokenString);
                //var userToken = UserTokenList.GetUserTokenByUID(UID);
                //userToken.ActiveDateTime = DateTime.Now;
                if (userToken != null)
                    if (userToken.SubIP != _ip)
                    {
                        try
                        {
                            //Sessions.SendTo(SerializeHelper.Serialize(msg), userToken.ID);
                            //_redis.Publish(userToken.SubIP, SerializeHelper.Serialize(msg));
                            //Console.WriteLine("发布一条消息" + msg.Sender);
                            //  }
                            if(dicWebSocket.ContainsKey(userToken.SubIP))
                            {
                                WebSocket socket;
                                if(dicWebSocket.TryGetValue(userToken.SubIP,out socket))
                                {
                                    if (msg.Protocal == (byte)MessageProtocalEnum.File || msg.Protocal == (byte)MessageProtocalEnum.FileSlice)
                                    {
                                        try
                                        {
                                            socket.Send(SerializeHelper.ProtolBufSerialize(msg));
                                           // Console.WriteLine("服务器转发一条FileMsg,发送人" + msg.Sender + DateTime.Now + userToken.SessionID);
                                        }
                                        catch(Exception ex)
                                        {
                                            if(dicWebSocket.ContainsKey(userToken.SubIP))
                                            dicWebSocket.Remove(userToken.SubIP);
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            socket.Send(SerializeHelper.Serialize(msg));
                                           // Console.WriteLine("服务器转发一条PrivateMsg,发送人" + msg.Sender + DateTime.Now + userToken.SessionID);
                                        }
                                        catch
                                        {
                                            if (dicWebSocket.ContainsKey(userToken.SubIP))
                                                dicWebSocket.Remove(userToken.SubIP);
                                            Thread.Sleep(5000);
                                        }
                                    }
                                }
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            if (msg.Protocal == (byte)MessageProtocalEnum.File || msg.Protocal == (byte)MessageProtocalEnum.FileSlice)
                            {
                                Sessions.SendTo(SerializeHelper.ProtolBufSerialize(msg), userToken.SessionID);
                               // Console.WriteLine("发送一条FileMsg,发送人" + msg.Sender + DateTime.Now + userToken.SessionID);
                            }
                            else
                            {

                                    Sessions.SendTo(SerializeHelper.Serialize(msg), userToken.SessionID);
                                  //  Console.WriteLine("发送一条PrivateMsg,发送人" + msg.Sender + DateTime.Now + userToken.SessionID);

                            }
                        }
                        catch
                        {
                            if (dicWebSocket.ContainsKey(userToken.SubIP))
                                dicWebSocket.Remove(userToken.SubIP);
                            Thread.Sleep(5000);
                            Console.WriteLine("发送失败,发送人" + msg.Sender + DateTime.Now + userToken.SessionID);

                        }
                    }
            }
            catch (Exception ex)
            {
                
            }
        }
        /// <summary>
        /// 公共发送消息不走消息队列
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="msg"></param>
        protected void SendCommon(string UID, Message msg)
        {
            try
            {
                var userToken = SerializeHelper.Deserialize<UserSession
                    >(RedisHelper.HashGet(USER_SESSION, UID).ToString());
                if (userToken != null)
                  
                        Sessions.SendTo(SerializeHelper.Serialize(msg), userToken.SessionID);
                    
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 公共服务器返回消息Json走转发
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="json"></param>
        protected void SendCommonJson(string UID, string json)
        {
            if (!UserTokenList.IsEmpty() && (json != null))
            {
                // var userToken = UserTokenList.GetUserTokenByUID(UID);
                var userToken = SerializeHelper.Deserialize<UserSession
                    >(RedisHelper.HashGet(USER_SESSION, UID).ToString());
                Message msg = new Message()
                {
                    Sender = _serviceSender,
                    Data = Encoding.UTF8.GetBytes(json),
                    Protocal = (byte)MessageProtocalEnum.RemoteConnect,
                    Accepter = UID
                };
                if (userToken != null)
                    if (userToken.SubIP != _ip)
                    {
                        try
                        {

                            if (dicWebSocket.ContainsKey(userToken.SubIP))
                            {
                                WebSocket socket;
                                if (dicWebSocket.TryGetValue(userToken.SubIP, out socket))
                                {
                                        socket.Send(SerializeHelper.Serialize(msg));
                                        Console.WriteLine("服务器转发一条CommonJson,发送人" + msg.Sender + DateTime.Now + userToken.SessionID);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        Sessions.SendTo(SerializeHelper.Serialize(msg), userToken.SessionID);
                    }
            }
        }

        protected void SendFile(string UID, Message msg)
        {

            if (!UserTokenList.IsEmpty() && (msg != null))
            {
                var userToken = UserTokenList.GetUserTokenByUID(UID);
                //userToken.ActiveDateTime = DateTime.Now;
                if (userToken != null)
                    Sessions.SendTo(SerializeHelper.ProtolBufSerialize(msg), userToken.SessionID);
            }
        }

        public void Logout(string uid)
        {
            try
            {
                string accepter = RedisHelper.HashGet(_onremotingset, uid).ToString();
                if (accepter == null || accepter == "")
                    return;
                UserSession session = SerializeHelper.Deserialize<UserSession> (RedisHelper.HashGet(USER_SESSION, accepter).ToString());
                if (session != null)
                {
                    if (session.SubIP == _ip)
                    {
                        Sessions.SendTo(SerializeHelper.Serialize(new Message
                        {
                            Sender = _serviceSender,
                            Protocal = (byte)MessageProtocalEnum.RemoteConnect,
                            Accepter = accepter,
                            Data = Encoding.UTF8.GetBytes(JsonResultCommon(Server_Helper.HttpHelper.ResultType.OTHER_LOGOUT, VerbalInfo.OTHER_LOGOUT))
                        }), session.SessionID);
                    }
                    else
                    {
                        SendCommonPub(accepter, new Message
                        {
                            Sender = _serviceSender,
                            Protocal = (byte)MessageProtocalEnum.RemoteConnect,
                            Accepter = accepter,
                            Data = Encoding.UTF8.GetBytes(JsonResultCommon(Server_Helper.HttpHelper.ResultType.OTHER_LOGOUT, VerbalInfo.OTHER_LOGOUT))
                        });
                    }
                    //双方结束远程的数据库日志 "2"结束远程
                    string result = HttpProxy.GetRequestCommon(InterfaceUrl.REMOTE_ESCONNECTION, GetESConnection(uid, accepter, "2"));
                }

                //删除双方在onRemoing Key中的值
                RedisHelper.HashDelete(_onremotingset, uid);
                RedisHelper.HashDelete(_onremotingset, accepter);
            }
            catch
            {
                Console.WriteLine("Remote" + "401");
            }
        }
        /// <summary>
        /// 更新心跳时间
        /// </summary>
        /// <param name="uid"></param>
        private void UpdateHeart(string uid)
        {
            try
            {
                //更新Redis心跳
                UserSession session = SerializeHelper.Deserialize<UserSession>(RedisHelper.HashGet(USER_SESSION, uid).ToString());
                if (session != null)
                    session.ActiveTime = DateTime.Now;
                //更新UserList心跳

                var item = UserTokenList.GetUserTokenByUID(uid);
                if (item != null)
                    item.ActiveTime = DateTime.Now;

            }
            catch
            {

            }
        }
        /// <summary>
        /// 与其他服务器连接
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public WebSocket ServerClientConnect(string ip)
        {
            string wsip = "ws://" + ip + ":" + "8080" + "/Remote" + "?name=" + _ip.ToString();
            WebSocket itemSocket = new WebSocket(wsip);
            itemSocket.Connect();
            Console.WriteLine("与" + ip + "服务器建立了连接");
            return itemSocket;
        }
        /// <summary>
        /// 动态从Redis获取服务器集群列表加入Dictionary
        /// </summary>
        public void GetServerDicAsyn()
        {
            Task.Factory.StartNew(()=> 
            {
                Console.WriteLine("动态从Redis获取服务器方法启动");
                while (true)
                {
                    try
                    { 
                    int isLive = 0;
                    string currentIP;
                    if(!RedisHelper.KeyExists(Server_Address))
                    {
                            RedisHelper.HashPut(Server_Address, _ip, "8080");
                    }
                    string[] serverList = RedisHelper.HashKeys(Server_Address);
                    if (serverList == null || serverList.Length < 1)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (var item in serverList)
                        {
                            currentIP = item;
                            try
                            {
                                if (item != _ip && !dicWebSocket.ContainsKey(item))//不连接自己且 dictionary里面没有该连接
                                {
                                    WebSocket itemSocket = ServerClientConnect(item);
                                    dicWebSocket.Add(item, itemSocket);
                                }
                                else if (dicWebSocket.ContainsKey(item))//dictionary里面有该连接
                                {

                                }
                                else//自己的IP
                                {
                                    isLive++;
                                }
                            }
                            catch (Exception ex)
                            {
                                RedisHelper.HashDelete(Server_Address, currentIP);//如果一个IP连不上删除该IP
                                Console.WriteLine("IP:" + currentIP + "连接不上已从" + Server_Address + "中删除");
                            }
                        }
                        if (isLive == 0)
                        {
                            RedisHelper.HashPut(Server_Address, IPAddress.Parse(_ip).ToString(), "8080");
                        }
                    }
                    Thread.Sleep(3000);
                }
                catch
                    {
                        continue;
                    }
                }
            }); 
        }
        /// <summary>
        /// 定时检查当前Dictionary中客户端连接状态断线的重新连接
        /// </summary>
        public void CheckServerClientStatus()
        {
            Task.Factory.StartNew(()=> 
            {
                Console.WriteLine("定时检测连接状态方法启动");
                while (true)
                {
                    try
                    {
                        if(dicWebSocket != null && dicWebSocket.Count > 0)
                        {
                            foreach(var item in dicWebSocket)
                            {
                                if(item.Value.ReadyState != WebSocketState.Open)
                                {
                                    ReConnect(item.Value,item.Key);
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                    Thread.Sleep(1000);
                }
            });
        }
        public void ReConnect(WebSocket webSocket,string ip)
        {
            try
            {

                if (webSocket != null)
                {
                    if (webSocket.ReadyState != WebSocketState.Open)
                    {
                        //webSocket.Close();
                        //webSocket = null;
                    }
                    webSocket.Connect();

                }
                else
                {
                    if(dicWebSocket.ContainsKey(ip))
                    {
                        dicWebSocket.Remove(ip);
                    }
                    WebSocket itemSocket = ServerClientConnect(ip);
                    dicWebSocket.Add(ip, itemSocket);
                }
                Thread.Sleep(5000);
             }
                catch
                {
                    RedisHelper.HashDelete(Server_Address, ip);//如果一个IP连不上删除该IP
                    Console.WriteLine("IP:" + ip + "连接不上已从" + Server_Address + "中删除");
                    dicWebSocket.Remove(ip);
                }
            
            Console.WriteLine("服务器客户端" + ip + "尝试了一次重连");
        }

        public void SetRedisKey()
        {
            string enviroment = Environment.GetEnvironmentVariable("ConfigType");
            if (enviroment == "moni")
            {
                _onremotingset = "moni-Redis_OnRemotingSet";
                Server_Address = "moni-RemoteServerList";
                USER_SESSION = "moni-RemoteUserSeesion";
            }
            if (enviroment == "prod")
            {
                _onremotingset = "prod-Redis_OnRemotingSet";
                Server_Address = "prod-RemoteServerList";
                USER_SESSION = "prod-RemoteUserSeesion";
            }
            else
            {
                _onremotingset = "Redis_OnRemotingSet";
                Server_Address = "RemoteServerList";
                USER_SESSION = "RemoteUserSeesion";
            }
        }
        #endregion


        #region 参数模型
        /// <summary>
        /// 建立连接存数据库日志
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="accepterID"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetESConnection(string senderID, string accepterID, string option = "1")
        {
            IDictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("sender_id", senderID);
            keyValues.Add("accept_id", accepterID);
            keyValues.Add("operate", option);
            return keyValues;
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetUserInfo(string code, string dgKey)
        {
            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("code", code);
            keyValuePairs.Add("key_code",dgKey);
            return keyValuePairs;
        }
        /// <summary>
        /// 根据IP获取地区参数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetIPAdress(string ip)
        {
            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("user_ip", ip);
            return keyValuePairs;
        }
        #endregion

        public string JsonResultCommon(Server_Helper.HttpHelper.ResultType type, string result)
        {
            //string userip = IPAddress.Parse(Context.UserEndPoint.Address.ToString()).ToString();
            ////获取客户IP，需要在nginx里进行配置，暂不能使用
            string X_Real_IP = IPAddress.Parse(Context.Headers["X-Real-IP"].ToString()).ToString();
            string X_Forwarded_For = IPAddress.Parse(Context.Headers["X-Forwarded-For"].ToString()).ToString();
            //Console.WriteLine("连接用户userip:" + X_Forwarded_For + " X-Real-IP:" + X_Real_IP + "    X-Forwarded-For:" + X_Forwarded_For);

            IDictionary<string, string> valuePairs = new Dictionary<string, string>();
            valuePairs.Add("type", type.ToString());
            valuePairs.Add("result", result);
            valuePairs.Add("ip", X_Real_IP);
            try
            {

                 string resultDetails = HttpProxy.GetRequestCommon(InterfaceUrl.IP_Address, GetIPAdress(X_Real_IP));
                //string resultDetails = "";
                valuePairs.Add("ip_Adress", resultDetails);
            }
            catch
            {
                Console.WriteLine("查询IP地址出错");
                valuePairs.Add("ip_Adress", "");
            }
            return SerializeHelper.Serialize(valuePairs);
        }

    }
}
