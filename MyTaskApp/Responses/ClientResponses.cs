using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using server;
using server.Models;

namespace MyTaskAppServer.Models
{
    public class ClientResponses
    {
        public static void ResponseType(int Id, DataTransferObject dataTransferObject)
        {
            switch (dataTransferObject.RequestType)
            {
                case "LoginServer":
                    Server.SendMessageBySocketId(Id,dataTransferObject);
                    break;
            }
        }
    }
}
