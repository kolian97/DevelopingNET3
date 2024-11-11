using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DevelopingNET3
{
    internal class Client
    {
        public static async Task SendMsq(string name)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6060);
            UdpClient udpClient = new UdpClient();
            while (true)
            {
                Console.WriteLine("Введите сообщение(или 'Exit' для завершения): ");
                string text = Console.ReadLine();


                Message msg = new Message(name, text);
                string responseMsqJs = msg.ToJson();
                byte[] responseData = Encoding.UTF8.GetBytes(responseMsqJs);
                await udpClient.SendAsync(responseData, responseData.Length, ep);

                byte[] answerData = udpClient.Receive(ref ep);
                string answerMsgJs = Encoding.UTF8.GetString(answerData);
                Message answerMsq = Message.FromJson(answerMsgJs);
                Console.WriteLine(answerMsq.ToString());

            }
        }
    }
}
