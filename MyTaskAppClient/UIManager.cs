using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using client;
using client.Models;
using MyTaskAppServer.Enums;

namespace MyTaskAppClient
{
    public class UIManager
    {
        private static DataTransferObject dataTransferObject;
        private static MainWindow mainWindow;
        public static bool lightStatus;
        

        #region Configuration

        public static void UIManagerConfiguration(MainWindow MainWindow)
        {
            dataTransferObject = new DataTransferObject();
            mainWindow = MainWindow;
        }

        #endregion

        #region ButtonOperation

        public static void TurnOnLight(string status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                lightStatus = bool.Parse(status);
                mainWindow.BtnLight.Content = "Light On";
            });
        }

        public static void TurnOffLight(string status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                lightStatus = bool.Parse(status);
                mainWindow.BtnLight.Content = "Light Off";
            });
        }

        public static void SetLightStatus(string status)
        {

            if (status == "True" || status == "true")
            {
                TurnOnLight(status);
            }
            else if (status == "False" || status == "false")
            {
                TurnOffLight(status);
            }
        }

        public static void SendLightStatus()
        {
            if (Client.networkStream != null)
            {
                dataTransferObject.RequestType = RequestEnums.LightStatus.ToString();
                dataTransferObject.Request = lightStatus.ToString();
                Requests.Request(dataTransferObject);
            }
        }
        public static void ChangeRandomButtonContent(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindow.BtnRandom.Content = text;
            });
        }
        public static void DisableButons()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindow.BtnRandom.IsEnabled = false;
                mainWindow.BtnLight.IsEnabled = false;
            });
        }


        public static void SendText()
        {
            dataTransferObject.RequestType = RequestEnums.TextChanged.ToString();
            dataTransferObject.Request = mainWindow.BtnRandom.Content.ToString();
            Requests.Request(dataTransferObject);
        }
        #endregion

        #region FormOperation

        public static void CloseForm()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindow.Close();
            });
        }


        #endregion
        

    }
}
