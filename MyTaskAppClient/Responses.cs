using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using client.Models;

namespace MyTaskAppClient
{
    public class Responses
    {
        public static void ResponseType(DataTransferObject dataTransferObject)
        {
            switch (dataTransferObject.RequestType)
            {
                case "LightStatus":
                    UIManager.SetLightStatus(dataTransferObject.Request);
                    break;
                case "TextChanged":
                    UIManager.ChangeRandomButtonContent(dataTransferObject.Request);
                    break;
                case "LoginServer":
                    UIManager.SetLightStatus(dataTransferObject.Request.Split(",")[0]);
                    UIManager.ChangeRandomButtonContent(dataTransferObject.Request.Split(",")[1]);
                    break;
            }
        }
    }
}
