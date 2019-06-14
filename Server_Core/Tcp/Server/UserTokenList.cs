using Server_Core.Tcp.Model;
using Server_Helper;
using Server_Helper.HttpHelper;
using Server_Helper.Log4net;
using Server_Helper.StackExchangeRedis;
using Server_Model.BusinessModel;
using Server_Model.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WebSocketSharp.Server;

namespace Server_Core.Tcp.Server
{
    /// <summary>
    ///     订阅用户列表
    /// </summary>
    public static class UserTokenList
    {

        private static readonly string USER_SESSION = "RemoteUserSeesion";

        //private static RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        ///     在线用户集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, UserSession> _OnlineUsers =
            new ConcurrentDictionary<string, UserSession>();

     

        /// <summary>
        ///     多线程类
        /// </summary>
        private static MutiThreadHelper mutiThreadHelper = new MutiThreadHelper();



        public static int GetCount()
        {
            return _OnlineUsers.Count;
        }
        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="asyncUserToken"></param>
        /// <returns></returns>
        public static void SetOnlineID(string UID, UserSession asyncUserToken)
        {
            if (!string.IsNullOrWhiteSpace(UID))
            {
                UserSession u;
                if (_OnlineUsers.TryGetValue(UID,out u))
                {
                    _OnlineUsers.TryRemove(UID, out u);
                }

                _OnlineUsers.TryAdd(UID, asyncUserToken);

            }
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public static bool DelOnlineID(string UID)
        {

            UserSession u;
            if (_OnlineUsers.TryRemove(UID, out u))
            {
                return true;
            }
            return false;
           
        }

        public static bool IsEmpty()
        {
            if(_OnlineUsers.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static List<IUserToken> GetListByChannelID(string channelID)
        //{
        //    if (_AsyncUserTokenList != null && _AsyncUserTokenList.Values != null)
        //    {
        //        var list = _AsyncUserTokenList.Values.ToList();
        //        if (list != null)
        //        {
        //            return list.Where(b => (b.ChannelID == channelID) && (b.UserToken != null) && (b.UserToken.ConnectSocket != null) && b.UserToken.ConnectSocket.Connected)
        //                       .Select(b => b.UserToken)
        //                       .Distinct()
        //                       .ToList();
        //        }
        //    }
        //    return null;
        //}

        //#endregion

        //#region 私信模式        




        public static UserSession GetUserTokenByUID(string uid)
        {
            if (!_OnlineUsers.IsEmpty)
            {
                try
                {
                    UserSession u;
                    if (_OnlineUsers.TryGetValue(uid, out u))
                        return u;
                }
                catch
                {

                }
            }
            return null;
        }

        public static ConcurrentDictionary<string, UserSession> GetOnlinUserList()
        {
            return _OnlineUsers;
        }

        //#endregion
        //#region 参数模型
        ///// <summary>
        ///// 服务器登出
        ///// </summary>
        ///// <param name="item"></param>
        ///// 操作（1=登录 2=登出 3=服务器登出）登录来源int型（1=客户端，2=网页端，3=其他）角色(1=客服、2=客户)
        ///// <returns></returns>
        //private static IDictionary<string, string> GetLoginOut_Service(IUserToken item)
        //{
        //    IDictionary<string, string> keyValues = new Dictionary<string, string>();
        //    keyValues.Add("user_id", item.UID);
        //    keyValues.Add("login_from", "3");
        //    keyValues.Add("operate", "3");
        //    keyValues.Add("user_role", "2");
        //    return keyValues;
        //}

    }
}
