/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Helper.Log4net
* 类名称：ILog
* 创建时间：2018/11/21
* 创建人：zhangbaoj
* 创建说明：  日志操作帮助类
*****************************************************************************************************/
using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server_Helper.Log4net
{
    public  class Log4netHelper 
    {
        static log4net.ILog log;
        public Log4netHelper()
        {
            try
            {
                ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
                //获取配置文件路径
                string test = new DirectoryInfo("../../../../").FullName;
                // string m_Config_File_Path = AppDomain.CurrentDomain.BaseDirectory + "log4net." +
                //"config";
                #region windows下的路径
                //string m_Config_File_Path = new DirectoryInfo("../../../../").FullName + "Server_Helper" + "\\" + "Log4net" + "\\" + "log4net." + "config";
                #endregion
                #region linux下的路径
                string m_Config_File_Path = new DirectoryInfo("..").FullName + "/" + "Server_Helper" + "/" + "Log4net" + "/" + "log4net." + "config";
                #endregion
                if (System.IO.File.Exists(m_Config_File_Path))
                {
                    //加载配置信息
                    log4net.Config.XmlConfigurator.Configure(repository, new System.IO.FileInfo(m_Config_File_Path));
                    //生成日志对象
                    log = LogManager.GetLogger(repository.Name, "NETCorelog4net");

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("加载日志对象出错" + ex.Message);
            }
        }
           

        public static void WriteError(Exception ex, string logType)
        {
            log.Error(String.Format("文件类型：{0}，错误信息:{1}",logType,ex.ToString()));
        }

        public static void WriteMessage(string message, string logType)
        {
            WriteMessage(message, logType, LogLevel.Error);
            //log.Error(String.Format("文件类型：{0}，打印信息:{1}", logType, message));
        }

        public static void WriteMessage(string message, string logType, LogLevel loglevel = LogLevel.Error)
        {
            switch(loglevel)
            {
                case LogLevel.Error:
                    log.Error(String.Format("文件类型：{0}，打印信息:{1}", logType, message));
                    break;
                case LogLevel.info:
                    log.Info(String.Format("文件类型：{0}，打印信息:{1}", logType, message));
                    break;
                case LogLevel.InterfaceError:
                    log.Warn(String.Format("文件类型：{0}，打印信息:{1}", logType, message));
                    break;
            }
        }
    }
}
