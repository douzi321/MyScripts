using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace APILibrary.API
{
    public class MessageCore : IDisposable
    {
        #region 定义委托和数据结构
        /// <summary>
        /// 定义一个当获取消息的时候被执行的函数
        /// </summary>
        /// <param name="Message"></param>
        public delegate void GetMessage(MessageStruct Message);
        /// <summary>
        /// 消息数据结构
        /// </summary>
        public class MessageStruct
        {
            /// <summary>
            /// 用于解析的字符
            /// </summary>
            private static char Desstring = '\0';
            /// <summary>
            /// 对应的游戏对象的名称
            /// </summary>
            public string GameObjectName;
            /// <summary>
            /// 要被执行的方法名称
            /// </summary>
            public string MethodName;
            /// <summary>
            /// 方法的参数(只支持字符串)
            /// </summary>
            public string MethodValue;
            /// <summary>
            /// 转换为字符串
            /// </summary>
            /// <returns>返回转换的结果</returns>
            public new string ToString()
            {
                string restring = "";
                int userint = 0;
                int leng = 0;
                if (GameObjectName != null && GameObjectName != "")
                {
                    restring += GameObjectName + Desstring;
                    userint += 100;
                    leng++;
                }
                if (MethodName != null && MethodName != "")
                {
                    restring += MethodName + Desstring;
                    userint += 10;
                    leng++;
                }
                if (MethodValue != null)
                {
                    restring += MethodValue + Desstring;
                    userint += 1;
                    leng++;
                }
                leng++;
                restring = leng.ToString() + Desstring + restring + userint.ToString();
                return restring;
            }
            /// <summary>
            /// 将消息转换为此结构体
            /// </summary>
            /// <param name="message">要被转换的消息</param>
            public bool ToStruct(string message)
            {
                try
                {
                    string[] desstring = message.Split(Desstring);
                    int leng = int.Parse(desstring[0]);
                    int userint = int.Parse(desstring[leng]);
                    if (userint % 10 == 1)
                    {
                        MethodValue = desstring[leng - 1];
                    }
                    userint /= 10;
                    if (userint % 10 == 1)
                    {
                        MethodName = desstring[leng - 2];
                    }
                    userint /= 10;
                    if (userint % 10 == 1)
                    {
                        GameObjectName = desstring[leng - 3];
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion
        #region 自定义属性
        /// <summary>
        /// UDP接受线程
        /// </summary>
        private Thread _udpThreadRec;
        /// <summary>
        /// UDP发送对象
        /// </summary>
        private Socket _udpSend;
        /// <summary>
        /// 发送消息端的端口
        /// </summary>
        private IPEndPoint _udpSendIPEndPoint;
        /// <summary>
        /// 接受消息端的端口
        /// </summary>
        private EndPoint _udpRecIPEndPoint;
        /// <summary>
        /// UDP接受对象
        /// </summary>
        private Socket _udpRec;
        /// <summary>
        /// 内置端口
        /// </summary>
        private int _port = 8888;
        /// <summary>
        /// 内置ip
        /// </summary>
        private string _ip = "127.0.0.1";
        /// <summary>
        /// 获取到window消息时候的回调函数
        /// </summary>
        private GetMessage _getWindowMessage;
        /// <summary>
        /// 接受的消息列表
        /// </summary>
        private List<MessageStruct> _recMessageList = new List<MessageStruct>();
        /// <summary>
        /// 是否启动异步接受信息
        /// </summary>
        private bool _isOpenAnyscRec = true;
        #endregion
        #region 读取器
        /// <summary>
        /// 获取到window消息时候的回调函数
        /// </summary>
        public GetMessage GetWindowMessage
        {
            get
            {
                return _getWindowMessage;
            }

            set
            {
                _getWindowMessage = value;
            }
        }
        /// <summary>
        /// 是否已经开启接受线程
        /// </summary>
        public bool IsRe
        {
            get
            {
                if (_udpThreadRec != null)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 是否已经发送初始化
        /// </summary>
        public bool IsSend
        {
            get
            {
                if (_udpSend != null)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 接受的消息列表
        /// </summary>
        public List<MessageStruct> RecMessageList
        {
            get
            {
                lock (this)
                {
                    return _recMessageList;
                }
            }
        }
        /// <summary>
        /// 是否有消息true为有
        /// </summary>
        public bool HaveMessage
        {
            get
            {
                if (RecMessageList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 是否启动异步接受信息
        /// </summary>
        public bool IsOpenAnyscRec
        {
            get
            {
                return _isOpenAnyscRec;
            }

            set
            {
                _isOpenAnyscRec = value;
            }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Port">指定端口</param>
        public MessageCore(int Port)
        {
            _port = Port;
        }
        #endregion
        #region 自定义函数
        /// <summary>
        /// 消息发送机制初始化
        /// </summary>
        public void MessageSendInit()
        {
            if (IsSend)
                return;
            _udpSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输      
            _udpSendIPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port); //直接绑定ip和端口
            _udpSend.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式   
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="SendMsg">消息内容</param>
        /// <returns>消息是否发送成功</returns>
        public bool MessageSend(MessageStruct SendMsg)
        {
            if (_udpSend == null)
                return false;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(SendMsg.ToString());
            _udpSend.SendTo(buffer, _udpSendIPEndPoint);
            return true;
        }
        /// <summary>
        /// 接受信息初始化
        /// </summary>
        public virtual void MessageRecInit()
        {
            if (IsRe)
                return;
            _udpRec = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议           
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(_ip), _port);
            //IPEndPoint iep = new IPEndPoint(IPAddress.Any, UDPport);//初始化一个侦听局域网内部所有IP和指定端口
            _udpRecIPEndPoint = (EndPoint)iep;
            _udpRec.Bind(iep);//绑定这个实例

            _udpThreadRec = new Thread(UDPRecThread);
            _udpThreadRec.IsBackground = true;
            _udpThreadRec.Start();
        }
        /// <summary>
        /// UDP接受线程
        /// </summary>
        protected void UDPRecThread()
        {
            while (true)
            {
                byte[] buffer = new byte[2048];//设置缓冲数据流
                _udpRec.ReceiveFrom(buffer, ref _udpRecIPEndPoint);//接收数据,并确把数据设置到缓冲流里
                if (GetWindowMessage != null)
                {
                    MessageStruct st = new MessageStruct();
                    st.ToStruct(System.Text.Encoding.UTF8.GetString(buffer));
                    if (IsOpenAnyscRec)
                    {
                        AddRecMessage(st);      //添加一个消息
                    }
                    GetWindowMessage(st);  //转换为字符串
                }
            }
        }
        /// <summary>
        /// 添加一个接受的消息
        /// </summary>
        /// <param name="str">消息</param>
        protected void AddRecMessage(MessageStruct str)
        {
            RecMessageList.Add(str);
        }

        /// <summary>
        /// 获取一个消息
        /// </summary>
        /// <returns>获取的消息如果没有则是返回NULL</returns>
        public MessageStruct GetWindowsMessage()
        {
            if (RecMessageList.Count > 0)
            {
                MessageStruct str = RecMessageList[0];
                RecMessageList.Remove(str);
                return str;
            }
            return null;
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (_udpThreadRec != null)
            {
                _udpThreadRec.Abort();
            }
            if (_udpRec != null)
            {
                _udpRec.Close();
                //_udpRec.Dispose();
            }
            if (_udpSend != null)
            {
                _udpSend.Close();
                //_udpSend.Dispose();
            }
        }
        #endregion
    }
}
