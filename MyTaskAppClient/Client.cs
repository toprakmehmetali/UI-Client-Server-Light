using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using client.Models;
using MyTaskAppClient;
using Newtonsoft.Json;

namespace client
{
    public class Client
    {
        public static TcpClient socket = new TcpClient();
        public static NetworkStream networkStream;
        public static bool ConFlag = false;
        public static byte[] buffer = new byte[4096];

        #region Message

        /*
      Mesaj gönderme işlemini gerçekleştirir.
      */
        public static void SendMessage(DataTransferObject Dto)
        {
            string JsonString = JsonConvert.SerializeObject(Dto);
            var result = Encoding.UTF8.GetBytes(JsonString);
            try
            {
                if (networkStream != null)
                {
                    networkStream.BeginWrite(result, 0, result.Length, SendCallBack, null);

                }
            }
            catch (Exception ex)
            {
                UIManager.DisableButons();
                MessageBox.Show("Serverle bağlantı kesildi.");
                UIManager.CloseForm();
            }
        }
        public static void SendCallBack(IAsyncResult asyncResult)
        {
            networkStream.EndWrite(asyncResult);
        }
        #endregion

        #region ConnectServer

        public static void Connect()
        {
            try
            {
                socket.BeginConnect(Config.Config.ConfigJson.ServerSettings.Host, Config.Config.ConfigJson.ServerSettings.Port, new AsyncCallback(ConnectCallBack), null);
            }
            catch (Exception e)
            {
                ConFlag = true;
                UIManager.DisableButons();
                MessageBox.Show("Server ile bağlantı kesildi.");
                UIManager.CloseForm();
            }

        }

        /*
         Sunucu bağlantı işlemini yapar.
         Sunucu okuma işlemi başlatır.
        */
        private static void ConnectCallBack(IAsyncResult asyncResult)
        {
            try
            {
                socket.EndConnect(asyncResult);
                networkStream = socket.GetStream();
                Read();
            }
            catch (Exception e)
            {
                ConFlag = true;
                UIManager.DisableButons();
                MessageBox.Show("Serverle bağlantı kurulamadı.");
                UIManager.CloseForm();
            }
        }

        #endregion

        #region ListenServer
        /*
        Sunucudan gelen verilerin okuma işlemini başlatır.
        */

        public static void Read()
        {
            networkStream.BeginRead(buffer, 0, 4096, ReveiveCallBack, null);
        }

        /*
         Sunucudan gelen verileri okur ekrana yazar.
         */
        public static void ReveiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                int gelenVeriUzunluğu = networkStream.EndRead(asyncResult);
                byte[] data = new byte[gelenVeriUzunluğu];
                Array.Copy(buffer, data, gelenVeriUzunluğu);
                string gelenmetin = Encoding.UTF8.GetString(data);
                if (gelenmetin != "")
                {
                    var result = JsonConvert.DeserializeObject<DataTransferObject>(gelenmetin);
                    Task task = new Task(() => Responses.ResponseType(result));
                    task.Start();
                    networkStream.BeginRead(buffer, 0, 4096, ReveiveCallBack, null);
                }
                else
                {
                    ConFlag = true;
                    UIManager.DisableButons();
                    MessageBox.Show("Serverle bağlantı kesildi.");
                    UIManager.CloseForm();
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        #endregion

    }
}
