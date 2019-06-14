using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.ResponseModel
{
    public enum EnumCode
    {
        /// <summary>
        /// CheckStatus
        /// </summary>
        CHECK_FALSE = 0,//"检查失败，请联系开发人员";//状态（0）
        SERVERIP_REFUSE = 1,//"ServerIP拒绝访问";//ServerIP连接不上（1）
        TARGETIP_REFUSE = 2,//"TargetIP拒绝访问";//TargetIP连接不上（2）
        SERVERIP_TARGETIP = 3,//ServerIP-TargetIP连接正常（3）
        /// <summary>
        /// ReConntion
        /// </summary>
        RECONNECTION_TRUE = 4,//重连操作成功
        RECONNECTION_FALSE = 5//重连操作失败
    }
}
