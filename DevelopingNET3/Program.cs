using System.Runtime.InteropServices;

namespace DevelopingNET3
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            if (args.Length == 0)
            {
                await Server.AcceptMsq(cts.Token);
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    await Client.SendMsq(args[0]);
                }
            }
            cts.Cancel();
        }
    }
}