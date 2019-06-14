/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Core.Tcp
* 类名称：ServerSocket
* 创建时间：2018/10/30
* 创建人：zhangbaoj
* 创建说明：  Server Socket操作类
*****************************************************************************************************/
using Server_Core.Tcp.Server;
using Server_Helper;
using Server_Helper.Log4net;
using Server_Model;
using Server_Model.Handler;
using Server_Model.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Server_Helper.StackExchangeRedis;
using StackExchange.Redis;

namespace Server_Core.Tcp
{
    public class ServerSocket
    {
       public HttpServer  _wsStocket; // 用于侦听传入的连接请求的套接字  

        // private RedisStackExchangeHelper _redis;//Redis操作

        private static  ServerConfig1 config = ServerConfig1.Instance();

       public delegate void GetPubMessage(RedisChannel channel,RedisValue message);

        public  GetPubMessage pubMessage;

        public string channelKey;

        private static  string Server_Address = "RemoteServerList";//服务器列表

        string _ip;
        int _port;
        /// <summary>  
        /// 服务器是否正在运行  
        /// </summary>  
        public bool IsRunning { get; private set; }

        private bool disposed = false;


        //private HttpServer httpServer;
        
   

        public ServerSocket()
        {

            this._ip = config.IP;
            this._port = config.Port;
            //_redis = new RedisStackExchangeHelper();
            string enviroment = Environment.GetEnvironmentVariable("ConfigType");
            if (enviroment == "moni")
            {
                Server_Address = "moni-RemoteServerList";
            }
            if (enviroment == "prod")
            {
                Server_Address = "prod-RemoteServerList";
            }
            else
            {
                Server_Address = "RemoteServerList";
            }

        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void StartAsync()
        {
            channelKey = IPHelper.GetLocalIP();

           // SubscribeAsync();

            this._port = 8080;

            _wsStocket = new HttpServer(_port);

            _wsStocket.DocumentRootPath = "../../../Public";

            // Set the HTTP GET request event.
            _wsStocket.OnGet += (sender, e) => {
                var req = e.Request;
                var res = e.Response;

                //var path = req.RawUrl;
                //if (path == "/")
                //    path += "index.html";

                //byte[] contents;
                //if (!e.TryReadFile(path, out contents))
                //{
                //    res.StatusCode = (int)HttpStatusCode.NotFound;
                //    return;
                //}

                //if (path.EndsWith(".html"))
                //{
                //    res.ContentType = "text/html";
                //    res.ContentEncoding = Encoding.UTF8;
                //}
                //else if (path.EndsWith(".js"))
                //{
                //    res.ContentType = "application/javascript";
                //    res.ContentEncoding = Encoding.UTF8;
                //}
                byte[] contents = Encoding.UTF8.GetBytes("OK");
                res.WriteContent(contents);

            };
            _wsStocket.AddWebSocketService<Remote>("/Remote",()=> new Remote(this, channelKey));
            _wsStocket.Start();
            //_redis.HashSet<String>(Server_Address,  Convert.ToBase64String( SerializeHelper.ProtolBufSerialize (new IPEndPoint(IPAddress.Parse(channelKey), _port).ToString())),"1");
            RedisHelper.HashPut(Server_Address,IPAddress.Parse(channelKey).ToString(), _port.ToString());
            if (_wsStocket.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", _wsStocket.Port);
                foreach (var path in _wsStocket.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);

            }

           
        }

        public  void SubscribeAsync()
        {
            
            // _redis.Subscribe(channelKey, (channel, message) =>
            //{
            //    try
            //    {
            //       // Console.WriteLine("接收到消息"+ message);
            //        pubMessage(channel,message);
            //        //Console.WriteLine("接收到消息" + message);

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //});
            //Console.WriteLine("您订阅的通道为：<< " + channelKey + " >> ! 请耐心等待消息的到来！！");
        }

        public Dictionary<string, string> IPAdress(string ip, int port)
        {
            Dictionary<string, string> ipAdress = new Dictionary<string, string>();
            ipAdress.Add("IP", new IPEndPoint(IPAddress.Parse(channelKey), _port).ToString());
            return ipAdress;
        }



    }
}
