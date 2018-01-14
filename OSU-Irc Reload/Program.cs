using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSU_Irc_Reload
{
    class Program
    {
        static void Main(string[] args)
        {//程序开端 实例化对象
            Irc osuIrc = new Irc();
            string User;
            string Password;
            bool a = true;
            int choose;
            Console.WriteLine("Osu_Irc-Reload 聊天器重写版 V1.0 By Small_Miao");
            //服务器连接
            osuIrc.Connect();           
            //登录判断
            while (a)
            {
                Console.Write("用户名：");
                User = Console.ReadLine();
                Console.Write("密码：");
                Password = Console.ReadLine();
                if (osuIrc.Login(User, Password) == true)
                {                    
                    a = false;
                }             
            }
            bool b = true;
            while (b)
            {
                Console.WriteLine("选择使用模式：1.私聊模式 2.频道模式 3.查看帮助 4.退出程序");
                //选择模式判断 防止程序崩溃
                try
                {
                    choose = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("错误的选择");
                    choose = 3;

                }
                switch (choose)
                {
                    case 1:
                        osuIrc.ThredStart();
                        string ID;
                        string msg;
                        Console.Write("输入要私聊人的ID:");
                        ID = Console.ReadLine();
                        osuIrc.RefreshMessageListLoad();
                        Console.WriteLine("发送信息:");
                        do
                        {
                           
                            msg = Console.ReadLine();
                            if (msg == "Exit Msg")
                            {
                                Console.WriteLine("退出当前模式");
                                osuIrc.ThredStop();
                                break;
                            }
                            osuIrc.IRCsend(msg, ID);
                        } while (true);
                        break;
                    case 2:
                        string chanl;
                        string chanlmsg;
                        Console.WriteLine("请输入要加入的频道名称，无需加#号，如为MP房间则mp_房间号即可加入");
                        Console.Write("加入的频道:");
                       chanl = "#"+ Console.ReadLine();
                        osuIrc.JoinChanl(chanl);
                        osuIrc.ThredStart();
                        osuIrc.RefreshMessageListLoad();
                        Console.WriteLine("发送信息:");
                        do
                        {
                            
                            chanlmsg = Console.ReadLine();
                            if (chanlmsg == "Exit Msg")
                            {
                                Console.WriteLine("退出当前模式");
                                osuIrc.ExitChanl(chanl);
                                osuIrc.ThredStop();
                                break;
                            }
                            osuIrc.Send(chanlmsg, chanl);
                        } while (true);
                        break;
                    case 3:
                        Console.WriteLine("使用Exit Msg 即可退出当前使用模式");
                        break;
                    case 4:b = false;
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
