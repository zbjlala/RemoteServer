/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Core.Tcp.Server
* 类名称：OnRemotingList
* 创建时间：2018/11/5
* 创建人：zhangbaoj
* 创建说明：  在线正在远程双方列表
*****************************************************************************************************/
using Server_Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server_Core.Tcp.Server
{
    public static class OnRemotingList
    {
        /// <summary>
        ///     在线正在远程双方用户集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> _OnlineRemotingList =
            new ConcurrentDictionary<string, string>();

        /// <summary>
        ///     多线程类
        /// </summary>
        private static MutiThreadHelper mutiThreadHelper = new MutiThreadHelper();

        /// <summary>
        /// 添加Remoting用户组
        /// </summary>
        /// <param name="initiator"></param>
        /// <param name="receiver"></param>
        public static bool SetRemotingSides(string initiator,string receiver)
        {
            if(_OnlineRemotingList.Count >= 1000)
            {
                return false;
            }

            if(!(String.IsNullOrWhiteSpace(initiator) && String.IsNullOrWhiteSpace(receiver)))
            {
                _OnlineRemotingList.AddOrUpdate(initiator,receiver,(x, y) =>{
                    return x;
                });
            }
            return true;
        }

        /// <summary>
        /// 根据接收方ID获取发起方ID
        /// </summary>
        /// <param name="receiver"></param>
        public static string GetKeyByValue(string value)
        {
            //var key = _OnlineRemotingList.Where(q => q.Value == value).Select(q => q.Key);

            var firstKey = _OnlineRemotingList.FirstOrDefault( q => q.Value == value).Key;

            return firstKey.ToString();

        }

        /// <summary>
        /// 根据发起方ID删除 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>接收方ID</returns>
        public static string DelRemotingByKey(string key)
        {
            string value;

            _OnlineRemotingList.TryRemove(key,out value);

            return value;
        }

        /// <summary>
        /// 根据接收方ID删除
        /// </summary>
        /// <param name="value"></param>
        /// <returns>发起方ID</returns>
        public static string DelRemotingByValue(string value)
        {
            string ovalue;

            string key = GetKeyByValue(value);
           
            _OnlineRemotingList.TryRemove(key, out ovalue);

            return key;
        }

        /// <summary>
        /// 获取正在远程列表
        /// </summary>
        /// <returns></returns>
        public static ConcurrentDictionary<string, string> GetRemotingList()
        {
            return _OnlineRemotingList;
        }

    }
}
