using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace server.Models
{
    public class Tcp
    {
        public int Id;
        public static int Buffersize = 4096;
        public byte[] Buffer ;
        public TcpClient Socket;
        public NetworkStream Stream;
        public byte[] Data;

        public Tcp()
        {
            Buffer = new byte[Buffersize];
        }
        /*
         Clientlere mesaj göndermeyi sağlar
         */
        public void SendMessage(byte[] messageBytes)
        {
            try
            {
                if (Stream != null)
                {
                    Stream.BeginWrite(messageBytes, 0,
                        messageBytes.Length, SendCallBack, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                throw;
            }


        }

        public void SendCallBack(IAsyncResult asyncResult)
        {
            Stream.EndWrite(asyncResult);
            
        }

        public void ConnectSocket(TcpClient socket)
        {
            this.Socket = socket;
            socket.ReceiveBufferSize = Buffersize;
            socket.SendBufferSize = Buffersize;
            Stream = socket.GetStream();
        }

        public void DisconnectSocket()
        {
            Socket.Close();
        }
        /*
         Stream Okuma işlemini başlatır
         */
        public void ReadStream()
        {
            Stream.BeginRead(Buffer, 0, Buffersize, ReveiveCallBack, null);
        }

        /*
         Okunan veriyi string ifade olarak virtual metoda yollar.
        */
        private void ReveiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                int dataLength = Stream.EndRead(asyncResult);
                if (dataLength <= 0)
                {
                    Server.clients[Id].Disconnect();
                    Console.WriteLine("Bağlantı Koptu.");
                    return;
                }
                Data = new byte[dataLength];
                Array.Copy(Buffer, Data, dataLength);
                string InComingText = Encoding.UTF8.GetString(Data);
                Task task = new Task(() => IncomingRequest(InComingText));
                task.Start();
                if (Stream != null)
                {
                    ReadStream();
                }

            }
            catch (Exception e)
            {
                Server.clients[Id].Disconnect();
                return;
            }

        }

        /*
         Okuma yapıldığında ne yapılacak ise metod ezilerek işlemin gerçekleşmesi sağlanır
         */
        public virtual void IncomingRequest(string InComingText)
        {

        }


    }
}
