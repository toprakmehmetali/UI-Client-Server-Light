using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MyTaskApp;
using MyTaskAppServer.Enums;
using MyTaskAppServer.Models;
using Newtonsoft.Json;
using server.Models;
using server.Config;
namespace server
{
    public class Server
    {
        public static int _maxUser { get; private set; }
        public static int _port { get; private set; }

        public static TcpListener serverListener;

        public static TcpUser[] clients;

        public static Tcp tcp;

        private static DataTransferObject dataTransferObject;

        private static byte[] serverIsFullBytes;


        #region StartServer

         public static void StartServer()
        {
            
            serverListener = new TcpListener(IPAddress.Any, _port);
            MessageBox.Show($"Server Kuruldu ! : Maksimum user sayısı {_maxUser} : Dinlenen port {_port}");
            serverListener.Start();
            MessageBox.Show(Messages.Messages.ServerStart);
            MessageBox.Show(Messages.Messages.WaitingForUsersToConnect);
        }

        #endregion

        #region ListenServer

        public static void ListenServer()
        {

            serverListener.BeginAcceptTcpClient(AcceptClientCallBack, null);

        }
        // Sunucuya bağlantı isteklerini karşılar. Duruma göre kabul eder veya sunucu dolu der bağlantıyı kapatır.
        public static void AcceptClientCallBack(IAsyncResult asyncResult)
        {
            TcpClient socket = serverListener.EndAcceptTcpClient(asyncResult);
            foreach (var client in clients)
            {
                if (client.Socket == null)
                {
                    client.Connect(socket);
                    dataTransferObject.RequestType = RequestEnums.LoginServer.ToString();
                    dataTransferObject.Request = $"{UIManager.LightStatus},{UIManager.randomText}";
                    ClientResponses.ResponseType(client.Id, dataTransferObject);
                    serverListener.BeginAcceptTcpClient(AcceptClientCallBack, null);
                    return;
                }
            }

            tcp.ConnectSocket(socket);
            tcp.SendMessage(serverIsFullBytes);
            tcp.DisconnectSocket();
            MessageBox.Show(Messages.Messages.ServerFull);
            serverListener.BeginAcceptTcpClient(AcceptClientCallBack, null);
        }

        #endregion
        
        #region SendMessage


        // Bütün clientlere mesaj gönderir
        public static void SendMessageAllSocket(DataTransferObject dataTransferObject)
        {
            string JsonString = JsonConvert.SerializeObject(dataTransferObject);
            var result = Encoding.UTF8.GetBytes(JsonString);
            foreach (var client in clients)
            {
                if (client.Socket != null)
                    client.SendMessage(result);
            }
        }
        public static void SendMessageBySocketId(int id, DataTransferObject dataTransferObject)
        {
            string JsonString = JsonConvert.SerializeObject(dataTransferObject);
            var result = Encoding.UTF8.GetBytes(JsonString);
            if (clients[id].Socket != null)
                clients[id].SendMessage(result);
        }
        #endregion

        #region Configuration
        
        // Clientsi boş TcpUser nesneleri ile doldurur.
        public static void InitializeServerData()
        {
            for (int i = 0; i < _maxUser; i++)
            {
                clients[i] = new TcpUser(i);
            }
        }

        // Konfigürasyon dosyasından yayın yapılacak port ve maksimum kullanıcı sayısı çekilir
        public static void SetServerSettings()
        {
            dataTransferObject = new DataTransferObject();
            dataTransferObject.RequestType = RequestEnums.ServerIsFull.ToString();
            dataTransferObject.Request = Messages.Messages.ServerFull;
            string JsonString = JsonConvert.SerializeObject(dataTransferObject);
            serverIsFullBytes = Encoding.UTF8.GetBytes(JsonString);

            _maxUser = Config.Config.ConfigJson.ServerSettings.MaxUser;
            _port = Config.Config.ConfigJson.ServerSettings.Port;
        }
        /* Maksimum kullanıcı sayısına göre TcpUser array ve
         sunucuya bağlanamayan kişilere cevap verebilmek için tcp nesnesi oluşturur.*/
        public static void SetEmptyArrayClients()
        {
            clients = new TcpUser[_maxUser];
            tcp = new Tcp();
        }

        #endregion

        

    }
}
