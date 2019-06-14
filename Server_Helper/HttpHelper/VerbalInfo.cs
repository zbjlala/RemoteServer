using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Helper.HttpHelper
{
    //话术信息
    public static class VerbalInfo
    {
        public static readonly string RECONNCED_INFO = "网络状况异常请重新退出重新登录";//重连失败信息
        public static readonly string OTHER_LOGOUT = "对方已登出";//一方对出通知另一方
        public static readonly string REFUSE_REMOTE = "远程用户不接受远程协助";//用户拒绝远程协助
        /// <summary>
        /// 后台网页检查两个服务器是否连接正常
        /// </summary>
        public static readonly string CHECK_FALSE = "检查失败，请联系开发人员";//状态（0）
        public static readonly string SERVERIP_REFUSE = "ServerIP拒绝访问";//ServerIP连接不上（1）
        public static readonly string TARGETIP_REFUSE = "TargetIP拒绝访问";//TargetIP连接不上（2）
        public static readonly string SERVERIP_TARGETIP = "ServerIP-TargetIP连接正常";//ServerIP-TargetIP连接正常（3）
        /// <summary>
        /// 重连操作是否成功
        /// </summary>
        public static readonly string RECONNECTION_TRUE = "重连操作成功";
        public static readonly string RECONNECTION_FALSE = "重连操作失败";
    }
}
