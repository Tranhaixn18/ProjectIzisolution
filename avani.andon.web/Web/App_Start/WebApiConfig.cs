using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace avSVAW.App_Start
{
    public class WebApiConfig
    {

        string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        private static string host = System.Configuration.ConfigurationSettings.AppSettings["host"];
        private static string username = System.Configuration.ConfigurationSettings.AppSettings["username"];
        private static string password = System.Configuration.ConfigurationSettings.AppSettings["password"];
        private static string port = System.Configuration.ConfigurationSettings.AppSettings["port"];
        private static string queue = System.Configuration.ConfigurationSettings.AppSettings["queue"];
        private static string routing_key = System.Configuration.ConfigurationSettings.AppSettings["routing_key"];
        private static ConnectionFactory factory;
        private static IConnection connection;
        private static IModel channel;
        private static QueueingBasicConsumer consumer;
        //private static BasicDeliverEventArgs ea;


        public static void Register(HttpConfiguration configuration)
        {

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = System.Web.Http.RouteParameter.Optional }
            );
            // create connection factory
            factory = new ConnectionFactory()
            {
                HostName = host,
                Port = Int32.Parse(port),
                UserName = username,
                Password = password
            };
            using (var connection = factory.CreateConnection())
            //connection = factory.CreateConnection();
            using (channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue, true, consumer);
            }




        }
        public static string openListenRabbitMQ()
        {
            string result = "";
            try
            {
                BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var value = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                value.TryGetValue("cardid", out result);
                Debug.WriteLine(result);
            }
            catch
            {
            }
            return result;
        }
    }
}