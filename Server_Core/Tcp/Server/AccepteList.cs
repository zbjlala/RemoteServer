using Server_Helper;
using Server_Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Core.Tcp.Server
{
    public static class AccepteList
    {
        private static List<IUserToken> m_list = new List<IUserToken>();

        static AccepteList()
        {
            TaskHelper.Run(() =>
            {
                while (true)
                {
                    try
                    {

                    }
                    catch
                    {

                    }
                }
            });
         }
      

        public static void Add(IUserToken s)
        {
            lock (m_list)
            {
                m_list.Add(s);
            }
        }

        public static void Remove(IUserToken s)
        {
            lock (m_list)
            {
                m_list.Remove(s);
            }
        }

        public static void CopyList(ref IUserToken[] array)
        {
            lock (m_list)
            {
                array = new IUserToken[m_list.Count];
                m_list.CopyTo(array);
            }
        }

        public static List<IUserToken> GetList()
        {
            return m_list;
        }
    }
}
