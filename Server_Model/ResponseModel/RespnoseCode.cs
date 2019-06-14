using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Model.ResponseModel
{
    public class RespnoseCode
    {
        private int code;//状态码

        private string msg;//返回信息string

        private byte[] data;//返回信息二进制

        public int Code { get => code; set => code = value; }
        public string Msg { get => msg; set => msg = value; }
        public byte[] Data { get => data; set => data = value; }
    }
}
