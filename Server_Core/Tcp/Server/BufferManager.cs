/*****************************************************************************************************
* 项目名称：FBIM_Remote_Server
* 命名空间：Server_Core.Tcp.Server
* 类名称：BufferManager
* 创建时间：2018/10/30 
* 创建人：zhangbaoj
* 创建说明：分配缓冲池使用的缓冲空间
*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server_Core.Tcp.Server
{
    /// <summary>
    /// 分配缓冲池使用的缓冲空间
    /// </summary>
    public class BufferManager
    {
        int m_numBytes;                 // 缓冲池控制的总字节数。
        byte[] m_buffer;                // 缓冲区管理器维护的底层字节数组
        Stack<int> m_freeIndexPool;     // 
        int m_currentIndex;
        int m_bufferSize;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// 分配缓冲池使用的缓冲空间
        /// </summary>
        public void InitBuffer()
        {
            // 创建一个大的缓冲区并将其分割 
            // 给每个socketasynceventarg对象
            m_buffer = new byte[m_numBytes];
        }

        /// <summary>
        /// 将缓冲池中的缓冲区分配给 socketasynceventargs指定对象
        /// </summary>
        /// <param name="args"></param>
        /// <returns>如果成功设置缓冲区，则为true，否则为false。</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {

            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        // 释放缓冲回缓冲池
        public void returnBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
        }

        /// <summary>
        /// 删除从一个socketasynceventarg对象缓冲区。
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            lock (args)
            {
                if (args.Buffer == m_buffer)
                {
                    returnBuffer(args);
                }
                args.SetBuffer(null, 0, 0);
            }
        }
    }
}
