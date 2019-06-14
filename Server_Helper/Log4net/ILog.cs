/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Helper.Log4net
* 类名称：ILog
* 创建时间：2018/11/21
* 创建人：zhangbaoj
* 创建说明：  日志操作接口
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Helper.Log4net
{
    public interface ILog
    {
        void WriteMessage(string message, string logType);
        void WriteMessage(string message, string logType, LogLevel loglevel);
        void WriteError(Exception ex, string logType);
    }
}
