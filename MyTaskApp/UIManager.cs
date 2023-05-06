using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using MyTaskApp;
using MyTaskAppServer.Enums;
using server;
using server.Models;


namespace MyTaskAppServer.Models
{
    public static class UIManager
    {
        private static BitmapImage TurnOnBitmap;
        private static BitmapImage TurnOffBitmap;
        public static string randomText;
        public static bool LightStatus;
        private static MainWindow mainWindow;
        private static DataTransferObject dataTransferObject;


        #region Configuration

        public static void UIManagerConfiguration(MainWindow MainWindow)
        {
            mainWindow = MainWindow;
            dataTransferObject = new DataTransferObject();
            GetLightImages();
            TurnOnLight();
            ChangeTextBoxRandom("0");
        }
        public static void GetLightImages()
        {
            var path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.IndexOf("\\bin"));
            path = Path.Combine(path, "Images");
            TurnOnBitmap = new BitmapImage(new Uri(path + "\\TurnOn.jpg", UriKind.RelativeOrAbsolute));
            TurnOffBitmap = new BitmapImage(new Uri(path + "\\TurnOff.jpg", UriKind.RelativeOrAbsolute));
        }
        #endregion

        #region LightOperation

        public static void TurnOnLight()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LightStatus = true;
                mainWindow.ImgLight.Source = TurnOnBitmap;
            });
        }

        public static void TurnOffLight()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LightStatus = false;
                mainWindow.ImgLight.Source = TurnOffBitmap;
            });
        }

        public static void SetLightStatus(string status)
        {
            if (status == "True" || status == "true")
            {
                TurnOnLight();
            }
            else if (status == "False" || status == "false")
            {
                TurnOffLight();
            }

            SendLightStatus();
        }

        public static void SendLightStatus()
        {
            dataTransferObject.RequestType = RequestEnums.LightStatus.ToString();
            dataTransferObject.Request = LightStatus.ToString();
            AllClientResponses.ResponseType(dataTransferObject);
        }
        #endregion

        #region TextOperation
        public static void ChangeTextBoxRandom(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindow.TbRandom.Text = text;
            });
            randomText = text;
        }
        public static void SendText()
        {
            dataTransferObject.RequestType = RequestEnums.TextChanged.ToString();
            dataTransferObject.Request = randomText;
            AllClientResponses.ResponseType(dataTransferObject);
        }
        #endregion


    }
}