using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;
using System.IO;
using Microsoft.ProjectOxford.Face;
using System.Linq;
using System;
using System.Web;
using System.Net.Http;
using System.Diagnostics;
using FacialRecognition.Backend.DataObjects;
using FacialRecognition.Backend.Models;

namespace FacialRecognition.Backend.Controllers
{
    // Use the MobileAppController attribute for each ApiController you want to use  
    // from your mobile clients 
    [MobileAppController]
    [RoutePrefix("api/values/v1.0")]
    public class ValuesController : ApiController
    {
        MobileServiceContext context = new MobileServiceContext();

        // POST api/values/v1.0/
        [HttpPost, Route("add")]
        public async Task<string> PostAddUser(MessageAttachments model)
        {
            int counter = 0;
            var personGroups = await FaceAnalyzer.faceServiceClient.ListPersonGroupsAsync();
            var xamGroup = personGroups.Where(x => x.Name == "Xamarin").FirstOrDefault();

            foreach (var user in model.Users)
            {
                var bytes = user.GetByteArray();
                var person = await FaceAnalyzer.faceServiceClient.CreatePersonAsync(xamGroup.PersonGroupId, user.UserEmail);

                using (Stream imageFileStream = new MemoryStream(bytes))
                    await FaceAnalyzer.faceServiceClient.AddPersonFaceAsync(xamGroup.PersonGroupId, person.PersonId, imageFileStream);

                await FaceAnalyzer.faceServiceClient.TrainPersonGroupAsync(xamGroup.PersonGroupId);

                context.IdentifiedUsers.Add(new IdentifiedUser {Id = person.PersonId.ToString(), Email = user.UserEmail });

                counter++;
            }

            return $"Saved {counter} users out of {model.Users.Count} total users";
        }

        // POST api/values/v1.0/
        [HttpPost, Route("check")]
        public async Task<string> PostCheckUser(MessageAttachments model)
        {
            foreach (var user in model.Users)
            {
                // Do what you need to with the bytes from the uploaded attachments
                var bytes = user.GetByteArray();
                AnalyzedFacialResults faceResponse = await FaceAnalyzer.DetectFaceAsync(bytes);

                if (faceResponse == null)
                    return "Error";

                if (faceResponse.Success)
                {
                    var identifiedUser = context.IdentifiedUsers.Where(x => x.Id == faceResponse.FaceId);

                    if (identifiedUser.Count() == 0)
                        return "User not found";
                    else if (identifiedUser.Count() >= 1)
                        return identifiedUser.First().Email;
                }

                return faceResponse.Error;
            }

            return "Something went wrong";
        }
    }
}