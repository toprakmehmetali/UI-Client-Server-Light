using System;
using System.Net.Mime;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using MyTaskApp;
using MyTaskAppServer;
using MyTaskAppServer.Models;
using Newtonsoft.Json;

namespace server.Models
{
    public class TcpUser : Tcp
    {
        public DataTransferObject dataTransferObject;
        public TcpUser(int id)
        {
            this.Id = id;
            dataTransferObject = new DataTransferObject();
        }

        public void Disconnect()
        {
            DisconnectSocket();
            this.Stream = null;
            this.Socket = null;
        }

        public void Connect(TcpClient socket)
        {
            ConnectSocket(socket);
            ReadStream();
        }

        /*
         Request için ara yazılımlar çalıştırılır ve tcp deki virtual metodu ezerek gelen requesti tipine göre yapılacak işleme yönlendirir.
         */
        public override void IncomingRequest(string InComingText)
        {
            DataTransferObject dataTransferObject = JsonConvert.DeserializeObject<DataTransferObject>(InComingText);
            Requests.RequestType(dataTransferObject);
        }

        private void BuildDataTransferObject(string requestType, string request)
        {
            dataTransferObject.RequestType = requestType;
            dataTransferObject.Request = request;
        }
        /*
         Request tiplerine göre işlemleri yönlendirir
        */
       
    }
}
