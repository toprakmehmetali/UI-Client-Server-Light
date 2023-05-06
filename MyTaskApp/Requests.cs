using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MyTaskAppServer.Models;
using server.Models;

namespace MyTaskAppServer
{
    public class Requests
    {
        public static void RequestType(DataTransferObject dataTransferObject)
        {
            switch (dataTransferObject.RequestType)
            {
                case "LightStatus":
                    UIManager.SetLightStatus(dataTransferObject.Request);
                    break;
                case "TextChanged":
                    UIManager.ChangeTextBoxRandom(dataTransferObject.Request);
                    break;
            }
        }
    }
}
