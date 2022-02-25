using CsvHelper;
using CsvHelper.Configuration;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


namespace App1
{
    public class Program
    {

        public class People
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
        }

        public static List<People> ReadInCSV()
        {
            string filepath = @"..\People.csv";
            List<People> result = new List<People>();

            using (var reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                {
                    People record = new People();

                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (!values[0].ToLower().Contains("id"))
                    {
                        record.Id = values[0].ToString();
                        record.FirstName = values[1].ToString();
                        record.LastName = values[2].ToString();
                        record.City = values[3].ToString();
                        record.State = values[4].ToString();
                        record.Country = values[5].ToString();

                        result.Add(record);
                    }
                }
            }
            return result;
        }

        public static void SetPocoData(List<People> records)
        {
            //Unable to complete this part. Pending the clarifying requirements questions sent.
        }

        public static void SendMessage(List<People> records)
        {
            //Modified this method to recieve the records list for sending to application 2

            var timer = new NetMQTimer(TimeSpan.FromSeconds(1));
            using (var poller = new NetMQPoller())
            using (var requestSocket = new RequestSocket(">tcp://localhost:5555"))
            {
                var message = "";
                foreach (People record in records)
                {
                    message = $"Id:{record.Id} Name:{record.FirstName} {record.LastName} City:{record.City} State:{record.State} Country:{record.Country}";
                    requestSocket.SendFrame(message);
                    message = requestSocket.ReceiveFrameString();
                }
                Console.ReadLine();
            }
        }

        public static void Main(string[] args)
        {
            List<People> records = ReadInCSV();
            SetPocoData(records);
            SendMessage(records);
        }
    }
}
