using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;
using System.Threading;

namespace OSU_Irc_Reload
{
    class Irc
    {
        public static IrcClient IRC = new IrcClient();
        private static Thread _ListenThread;
        Program program = new Program();
        List<String> messageList = new List<String>();
        private string Addres = "irc.ppy.sh";
        private int Port = 6667;
        public Irc()
        {//基础设定
            IRC.Encoding = Encoding.UTF8;
            IRC.OnChannelMessage += IRC_OnChannelMessage;
            IRC.OnQueryAction += IRC_OnQueryAction;
            IRC.OnQueryMessage += IRC_OnQueryMessage;
        }
        private void IRC_OnQueryMessage(object sender, IrcEventArgs e)
        {//私聊信息
            string msg = "[" + System.DateTime.Now + "私聊" + "]" + e.Data.Nick + ":" + e.Data.Message;
            messageList.Add("                                                                                                                       ");
            messageList.Add(msg);
            this.RefreshMessageList();
        }
        private void IRC_OnQueryAction(object sender, ActionEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void IRC_OnChannelMessage(object sender, IrcEventArgs e)
        {
            //频道信息
            string msg = "[" + System.DateTime.Now + e.Data.Channel + "]" + e.Data.Nick + ":" + e.Data.Message;
            
            messageList.Add(msg+"                                                   ");
            this.RefreshMessageList();


        }
        public void RefreshMessageListLoad()
        {
            for (int i = 0; i < 21; i++)
            {
                string b = Convert.ToString(i)+"屏幕重载中                                                                     ";
                messageList.Add(b);
                this.RefreshMessageList();
            }
            
        }
        private void RefreshMessageList()
        {//输出列表刷新
            const int kScreenHeight = 20;
            int kShowMax = messageList.Count > kScreenHeight ? kScreenHeight : messageList.Count;
            int cur = messageList.Count - kShowMax;
            for (int line = 0; line < kShowMax; line++, cur++)
            {
                Console.SetCursorPosition(0, line);
                Console.WriteLine(messageList[cur]);
            }
        }
        public bool Connect()
        {   //服务器连接
            try
            {
                IRC.Connect(this.Addres, this.Port);
                Console.WriteLine("服务器连接成功");
                return true;
            }
            catch(CouldNotConnectException)
            {
                Console.WriteLine("无法连接到服务器 请检查网络");
                return false;
            }
            catch(AlreadyConnectedException)
            {
                Console.WriteLine("服务器已连接");
                return false;

            }
        }
        public bool Login(string User,string Password)
        {   //登录=
            try
            {
                Console.WriteLine("登录成功");
                IRC.Login(User, User, 0, User, Password);
                return true;
            }
            catch 
            {
                Console.WriteLine("登录失败");
                return false;
                
            }
           
        }
        public void Send(string message, string Chanl)
        {
            //发送频道消息
            IRC.SendMessage(SendType.Message, Chanl, message);
        }
        public void IRCsend(string Msg, string id)
        {
            //发送私聊消息
            IRC.SendMessage(SendType.Message, id, Msg);
        }
        public void JoinChanl(string Chanl)
        {
            //加入频道
            IRC.RfcJoin(Chanl);
        }
        
        public void ExitChanl(string Chanl)
        {
            //退出频道
            IRC.RfcQuit(Chanl);
        }
        public bool ThredStart()
        {   //监听线程
            _ListenThread = new Thread(new ThreadStart(IRCThread));
            try
            {
                Console.WriteLine("线程启动成功");
                _ListenThread.Start();
                return true;
            }
            catch 
            {
                Console.WriteLine("线程启动失败");
                return false;
               
            } 
        }
        public void ThredStop()
        {//停止线程
            _ListenThread.Abort();
        }
       
        public void IRCThread()
        {
            try

            {
                IRC.Listen();
            }
            catch
            {
            }
        }
    }
  

}

    

