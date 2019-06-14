/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server
* 类名称：ServerConfig
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明：  Server配置信息类
*****************************************************************************************************/
using Server_Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server_Core.Tcp
{
    public class ServerConfig1
    {
        private static readonly string jsonFilePath = Environment.CurrentDirectory + "\\serverConfig.json";

        /// <summary>
        ///     服务器监听IP
        /// </summary>
        public string IP { get; set; } = "0.0.0.0"; //服务器监听IP

        /// <summary>
        ///     服务器监听端口
        /// </summary>
        public int Port { get; set; } = 6666; //服务器监听端口
        /// <summary>
        /// http服务器端口
        /// </summary>
        public int HttpPort { get; set; } = 8080; //http服务器端口       


        public int WSPort { get; set; } = 8082; //ws服务器端口       

        /// <summary>
        ///     解析命令初始缓存大小
        /// </summary>
        public int InitBufferSize { get; set; } = 1024 * 1; //解析命令初始缓存大小  

        /// <summary>
        ///     Socket超时设置为60秒
        /// </summary>
        public int SocketTimeOutMS { get; set; } = 60 * 1000; //Socket超时设置为60秒

        /// <summary>
        ///     最大连接数
        /// </summary>
        public int MaxClientSize { get; set; } = 6000; //最大连接数     

        /// <summary>
        ///     服务器监听队列长度
        /// </summary>
        public int Backlog { get; set; } = 10000; //服务器监听队列长度   
        /// <summary>
        /// 控制服务器任务处理速度
        /// </summary>
        public int OperationThreads
        {
            get;
            set;
        } = 10;

        /// <summary>
        ///     服务器配置实例
        /// </summary>
        /// <returns></returns>
        public static ServerConfig1 Instance()
        {
            var config = new ServerConfig1();
            try
            {
                var json = File.ReadAllText(jsonFilePath);
                return SerializeHelper.JsonDeserialize<ServerConfig1>(json);
            }
            catch (FileNotFoundException fex)
            {
                Save(config);
            }
            return config;
        }

        /// <summary>
        ///     保存服务器配置
        /// </summary>
        /// <param name="config"></param>
        public static void Save(ServerConfig1 config)
        {
            var json = SerializeHelper.JsonSerialize(config);
            File.WriteAllText(jsonFilePath, json);
        }
    }
}
