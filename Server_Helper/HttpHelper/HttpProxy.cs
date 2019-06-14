/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Helper
* 类名称：HttpProxy
* 创建时间：2018/11/2 
* 创建人：zhangbaoj
* 创建说明： Http代理类
*****************************************************************************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server_Helper
{
    public static class HttpProxy
    {
        static HttpHelper1 httpHelper = new HttpHelper1();
        static HttpItem httpItem;

        /// <summary>
        /// 通常接口访问类
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="parameter">参数键值对</param>
        /// <param name="isOrder">是否排序</param>
        /// <param name="method">请求方式（GET POST）</param>
        /// <param name="dataType">参数类型（DICTIONARY,JSON）</param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string GetRequestCommon(string url, IDictionary<string, string> parameter, bool isOrder = true, string method = "POST", DataType dataType = DataType.DICTIONARY, string contentType = "application/x-www-form-urlencoded")
        {

            httpItem = new HttpItem()
            {
                URL = url,
                Method = method.ToUpper(),
                Encoding = System.Text.Encoding.GetEncoding("utf-8"),
                Accept = "*/*",
                ContentType = ContentType.APP_LOGININFO,
                Postdata = GetParameter(parameter, isOrder, dataType)
            };
            HttpResult httpResult = httpHelper.GetHtml(httpItem);

            if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return "";
            }
            else
            {
                JObject jsondata = JObject.Parse(httpResult.Html);
                if (jsondata["success"].ToString() == "false")
                {
                    return "false";
                }
            }
            return httpResult.Html;
        }

        private static string GetParameter(IDictionary<string, string> parameter, bool isOrder, DataType dataType)
        {
            string result;
            parameter = parameter.OrderBy(key => key.Key).ToDictionary(keyItem => keyItem.Key, valueItem => valueItem.Value);
            if (dataType == DataType.JSON)
            {
                result = JsonConvert.SerializeObject(parameter);
            }
            else
            {
                var sbPara = new StringBuilder(1024);
                //foreach (var para in parameter.Where(para => !String.IsNullOrWhiteSpace(para.Value)))
                //para.Value.IsNullOrWhiteSpace()))
                foreach (var para in parameter)
                {
                    sbPara.AppendFormat("{0}={1}&", para.Key, para.Value);
                }


                result = sbPara.ToString().TrimEnd('&');
            }
            return result;
        }
    }

    public static class ContentType
    {
        public static readonly string APP_LOGIN = "application/x-www-form-urlencoded; charset=utf-8";
        public static readonly string APP_LOGININFO = "application/x-www-form-urlencoded";

        public static readonly string APP_TEXT = "text/html";

    }
    /// <summary>
    /// 接口传参数据类型
    /// </summary>
    public enum DataType
    {
        JSON = 1,
        DICTIONARY = 2
    }
}
