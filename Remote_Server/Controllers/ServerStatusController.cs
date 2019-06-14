using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;
using Server_Helper;
using Server_Model.BusinessModel;
using Server_Model.WebModel;

namespace Remote_Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ServerStatusController : Controller
    {
        private static readonly string _serviceSender = "服宝Remote";//服务器发送消息Sender

        private static readonly string _onlinuserset = "Redis_OnlinUserSet";

        private static string _onremotingset = "Redis_OnRemotingSet";

        private static string USER_SESSION = "RemoteUserSeesion";//Redis所有用户Session

        private static string Server_Address = "RemoteServerList";//Redis在线服务器集群IP

        public ServerStatusController()
        {

        }
        /// <summary>
        /// 获取所有服务器集群的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ServerListModel> GetServerList()
        {
            List<ServerListModel> list = new List<ServerListModel>();
            Dictionary<string, object> serverlist = RedisHelper.HashGetAll(Server_Address);
            if (serverlist.Count > 0)
            {
                foreach (var item in serverlist)
                {
                    list.Add(new ServerListModel()
                    {
                        ServerIP = item.Key,
                        ServerPoint = item.Value.ToString()
                    });
                }
            }
            return list;
        }


        /// <summary>
        /// 根据服务器IP查询该服务器下的用户列表
        /// </summary>
        /// <param name="serverIP"></param>
        /// <returns></returns>
        [HttpPost]
        public List<UserSession> GetUserListByServerID([FromBody]UserList userList)
        {

            string[] userlist = RedisHelper.HashValues(USER_SESSION);
            
            if (userlist.Length > 0)
            {
                List<string> list = new List<string>(userlist);
                if (userList.ServerIP != "all")
                {
                     list = userlist.Where(index => JObject.Parse(index)["SubIP"].ToString() == userList.ServerIP).ToList();
                }
                if (list.Count > 0)
                {
                    List<UserSession> usersession = new List<UserSession>();
                    {
                        foreach(var item in list)
                        {
                            UserSession user = SerializeHelper.Deserialize<UserSession>(item);
                            usersession.Add(user);
                        }
                    }
                    return usersession;
                }
            }
            return new List<UserSession>();
            
        }
        public List<UserSession> GetUserByID([FromBody]UserList userList)
        {
            string userlist = RedisHelper.HashGet(USER_SESSION,userList.UserID).ToString();
            List<UserSession> usersession = new List<UserSession>();
            if (userlist != null)
            {
                UserSession user = SerializeHelper.Deserialize<UserSession>(userlist);
                usersession.Add(user);
            }
            return usersession;
        }


        /// <summary>
        /// 根据环境设置Redis 的Key值
        /// </summary>
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
    }
    public class UserList
    {
        string serverIP;
        string userID;

        public string ServerIP { get => serverIP; set => serverIP = value; }
        public string UserID { get => userID; set => userID = value; }
    }
}