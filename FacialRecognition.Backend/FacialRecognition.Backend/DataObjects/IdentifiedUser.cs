using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacialRecognition.Backend.DataObjects
{
    public class IdentifiedUser : EntityData
    {
        public string Email { get; set; }
    }
}