using System;
using NetMQ;
using NetMQ.Sockets;

namespace App2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReceiveMessage();
        }

        public static void ReceiveMessage()
        {
            var connection = "@tcp://*:5555";
            using (var responseSocket = new ResponseSocket(connection))
            {
                while (true)
                {
                    var message = responseSocket.ReceiveFrameString();
                    Console.WriteLine("responseSocket Received :{0}", message);
                    responseSocket.SendFrame("Success");
                }
            }
            
        }

    }
}
