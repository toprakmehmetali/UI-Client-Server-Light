using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using server;
using server.Models;

namespace MyTaskAppServer.Models
{
    public class AllClientResponses
    {
        public static void ResponseType(DataTransferObject dataTransferObject)
        {
            switch (dataTransferObject.RequestType)
            {
                case "LightStatus":
                    Server.SendMessageAllSocket(dataTransferObject);
                    break;
                case "TextChanged":
                    Server.SendMessageAllSocket(dataTransferObject);
                    break;
            }
        }
    }
}
