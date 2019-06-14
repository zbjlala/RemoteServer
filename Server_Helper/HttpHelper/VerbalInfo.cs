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
    }
}
