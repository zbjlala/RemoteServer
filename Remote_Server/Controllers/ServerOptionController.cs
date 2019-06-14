using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server_Helper;
using Server_Helper.HttpHelper;
using Server_Model.Entity;
using Server_Model.Enum;
using Server_Model.ResponseModel;
using WebSocketSharp;

namespace Remote_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ServerOptionController : Controller
    {
        /// <summary>
        /// 检查两台服务器是否正在连接
        /// </summary>
        /// <param name="testIP">连接服务器IP</param>
        /// <param name="targetIP">目标服务器IP</param>
        /// <returns>1:testIP无法连接 2:目标IP无法连接 3:两个IP连接成功</returns>
        [HttpPost]
        public async Task<RespnoseCode> CheckTwoServerStatus([FromBody]CheckStatus check)
        {
            int checkResult = await SendTest(check.TestIP, check.TargetIP);
            if (checkResult == (int)EnumCode.SERVERIP_REFUSE)
            {
                return new RespnoseCode()
                {
                    Code = (int)EnumCode.SERVERIP_REFUSE,
                    Msg = VerbalInfo.SERVERIP_REFUSE
                };
            }
            else if (checkResult == (int)EnumCode.TARGETIP_REFUSE)
            {
                return new RespnoseCode()
                {
                    Code = (int)EnumCode.TARGETIP_REFUSE,
                    Msg = VerbalInfo.TARGETIP_REFUSE
                };
            }
            else if (checkResult == (int)EnumCode.SERVERIP_TARGETIP)
            {
                return new RespnoseCode()
                {
                    Code = (int)EnumCode.SERVERIP_TARGETIP,
                    Msg = VerbalInfo.SERVERIP_TARGETIP
                };
            }

            return new RespnoseCode()
            {
                Code = (int)EnumCode.CHECK_FALSE,
                Msg = VerbalInfo.CHECK_FALSE
            };
        }

        /// <summary>
        /// 重连两个断开的服务
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RespnoseCode> ReConnectTwoServer([FromBody]CheckStatus check)
        {
            int checkResult = await SendTest(check.TestIP, check.TargetIP);
           // if (checkResult == (int)EnumCode.TARGETIP_REFUSE)
           // {
                try
                {
                    this.WebSocketOption(check.TestIP, check.TargetIP, Controllers.WebSocketOption.ReConnection);
                }
                catch
                {
                    return new RespnoseCode() {
                        Code = (int)EnumCode.RECONNECTION_FALSE,
                        Msg = VerbalInfo.RECONNECTION_FALSE
                    };
                }
               
           // }
            return new RespnoseCode() {
                Code = (int)EnumCode.RECONNECTION_TRUE,
                Msg = "IP" + check.TestIP + VerbalInfo.RECONNECTION_TRUE
            };
        }
        /// <summary>
        /// 全局检查状态 0:初始,1:testIP无法连接 2:目标IP无法连接 3:两个IP连接成功
        /// </summary>
        static int result = 0;
        public async Task<int> SendTest(string testIP, string targetIP)
        {
            try
            {
                try
                {
                    WebSocketOption(testIP, targetIP, Controllers.WebSocketOption.CheckStatus);
                }
                catch
                {
                    return 1;
                }

                while (result == 0)
                {
                    Thread.Sleep(100);
                }
                if (result == 3)
                {
                    return 3;
                }

            }
            catch
            {

            }
            return 2;


        }
        /// <summary>
        /// WebSocket初始化
        /// </summary>
        /// <param name="ws"></param>
        public void WsInite(WebSocket ws)
        {
            ws.OnMessage += (sender, e) =>
            {
                if (e.Data != null)
                {
                    try
                    {

                        Message msg = SerializeHelper.Deserialize<Message>(e.Data);
                        if (msg != null)
                        {
                            switch (msg.Protocal)
                            {
                                case (byte)MessageProtocalEnum.CheckStatus:
                                    result = Convert.ToInt32( Encoding.UTF8.GetString(msg.Data));
                                    break;
                            }
                        }
                    }
                    catch
                    {
                        result = 2;
                    }
                }

            };
        }
        /// <summary>
        /// 连接WebSocket操作
        /// </summary>
        /// <param name="testIP"></param>
        /// <param name="targetIP"></param>
        /// <param name="option"></param>
        public void WebSocketOption(string testIP, string targetIP, WebSocketOption option)
        {
            string _id = RandomHelper.GetUniqueKey(8);
            string _ip = "ws://" + testIP + ":8080" + "/Remote/" + "?name=" + _id;
            result = 0;
            WebSocket ws;

            ws = new WebSocket(_ip);
            WsInite(ws);
            ws.Connect();
            if (option == Controllers.WebSocketOption.ReConnection)
            {
                ws.Send(SerializeHelper.Serialize(new Message()
                {
                    Sender = _id,
                    Protocal = (byte)MessageProtocalEnum.ReConnection,
                    Data = Encoding.UTF8.GetBytes("reconntion"),
                    Accepter = testIP
                }));
            }
            else
            {
                ws.Send(SerializeHelper.Serialize(new Message()
                {
                    Sender = _id,
                    Protocal = (byte)MessageProtocalEnum.CheckStatus,
                    Data = Encoding.UTF8.GetBytes("test"),
                    Accepter = targetIP
                }));
            }
        }
    }

    public class CheckStatus
    {
        private string testIP;
        private string targetIP;

        public string TestIP { get => testIP; set => testIP = value; }
        public string TargetIP { get => targetIP; set => targetIP = value; }
    }

    public enum WebSocketOption
    {
        CheckStatus = 0,
        ReConnection = 1
    }
}