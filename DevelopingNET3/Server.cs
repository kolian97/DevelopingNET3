using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DevelopingNET3
{
    internal class Server
    {
        private static bool exitRequested = false;
        public static async Task AcceptMsq(CancellationToken cancellationToken)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpClient = new UdpClient(6060);
            Console.WriteLine("Сервер ожидает сообщения.Для завершения нажмите клавишу...");
            Task exitTask = Task.Run(() =>
            {
                Console.ReadKey();
                exitRequested = true;
            },cancellationToken);
            try
            {
                while (!exitRequested)
                {
                    var data = udpClient.Receive(ref ep);
                    string data1 = Encoding.UTF8.GetString(data);
                    await Task.Run(async () =>
                    {
                        Message msq = Message.FromJson(data1);
                        Console.WriteLine(msq.ToString());
                        Message responseMsq = new Message("Server", "Message accept on serv!");
                        string responseMsqJs = responseMsq.ToJson();
                        byte[] responseData = Encoding.UTF8.GetBytes(responseMsqJs);
                        await udpClient.SendAsync(responseData, responseData.Length, ep);

                    }, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Сервер остановлен.");
            }
            finally
            {
                udpClient.Close();
            }

            // Дождитесь завершения задачи по нажатию клавиши
            await exitTask;
        }
    }
}
