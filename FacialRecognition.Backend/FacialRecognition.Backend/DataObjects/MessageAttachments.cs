using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacialRecognition.Backend.DataObjects
{
    public class MessageAttachments
    {
        public Guid MessageId { get; set; }
        public IList<CreateAttachment> Users { get; set; }

    }
    public class CreateAttachment
    {
        public string Data { get; set; }
        public string UserEmail { get; set; }

        public string GetBase64()
        {
            if (string.IsNullOrWhiteSpace(Data))
                return null;

            var index = Data.LastIndexOf("base64");

            if (index == -1)
                return Data;

            return Data.Substring(index + 7);
        }

        public byte[] GetByteArray()
        {
            try
            {
                var base64 = GetBase64();

                if (string.IsNullOrWhiteSpace(base64))
                    return null;

                return Convert.FromBase64String(base64);
            }
            catch
            {
                return null;
            }

        }
    }
}