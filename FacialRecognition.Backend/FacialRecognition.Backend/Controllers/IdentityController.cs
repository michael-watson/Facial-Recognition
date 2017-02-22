using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;
using System.IO;
using Microsoft.ProjectOxford.Face;
using System.Linq;

using FacialRecognition.Backend.DataObjects;
using FacialRecognition.Backend.Models;

namespace FacialRecognition.Backend.Controllers
{
    [MobileAppController]
    [RoutePrefix("api/identify/v1.0")]
    public class IdentifyController : ApiController
    {
        MobileServiceContext context = new MobileServiceContext();

        // POST api/identify/v1.0/add
        [HttpPost, Route("add")]
        public async Task<string> PostAddUser(MessageAttachments model)
        {
            int counter = 0;
            var personGroups = await FaceAnalyzer.faceServiceClient.ListPersonGroupsAsync();
            var xamGroup = personGroups.Where(x => x.Name == "Xamarin").FirstOrDefault();

            foreach (var user in model.Users)
            {
                CreatePersonResult person;
                var bytes = user.GetByteArray();

                var detectedFaceResults = await FaceAnalyzer.DetectFaceAsync(bytes);

                if (detectedFaceResults.Success)
                {
                    var isUserInDatabase = context.IdentifiedUsers.Where(usr => usr.Id == detectedFaceResults.FaceId.ToString()).ToList().FirstOrDefault();

                    if (isUserInDatabase == null)
                        context.IdentifiedUsers.Add(new IdentifiedUser { Id = detectedFaceResults.FaceId.ToString(), Email = user.UserEmail });
                    else
                        return $"User {isUserInDatabase.Email} is already in the database";
                }
                else
                {
                    //Check if CognitiveServices XamarinGroup doesn't know of our user
                    if (detectedFaceResults.Error == "Unable to Identify User")
                    {
                        using (Stream imageFileStream = new MemoryStream(bytes))
                        {
                            person = await FaceAnalyzer.faceServiceClient.CreatePersonAsync(xamGroup.PersonGroupId, user.UserEmail);

                            await FaceAnalyzer.faceServiceClient.AddPersonFaceAsync(xamGroup.PersonGroupId, person.PersonId, imageFileStream);
                            await FaceAnalyzer.faceServiceClient.TrainPersonGroupAsync(xamGroup.PersonGroupId);

                            context.IdentifiedUsers.Add(new IdentifiedUser { Id = person.PersonId.ToString(), Email = user.UserEmail });
                        }
                    }
                    //If it doesn't know about our user, then there was some error i.e. multiple faces, glasses 
                    else
                        return detectedFaceResults.Error;
                }

                counter++;
            }

            await context.SaveChangesAsync();

            return $"Saved {counter} users out of {model.Users.Count} total users";
        }

        // POST api/identify/v1.0/check
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