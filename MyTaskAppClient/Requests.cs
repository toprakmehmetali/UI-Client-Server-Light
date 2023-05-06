using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Models;

namespace client
{
    public class Requests
    {
        public static void Request(DataTransferObject dataTransferObject)
        {
           
            switch (dataTransferObject.RequestType)
            {
                case "TurnOnLight":
                    Client.SendMessage(dataTransferObject);
                    break;
                case "TurnOffLight":
                    Client.SendMessage(dataTransferObject);
                    break;
                case "TextChanged":
                    Client.SendMessage(dataTransferObject);
                    break;
                case "LightStatus":
                    Client.SendMessage(dataTransferObject);
                    break;
            }
        }
    }
}
