using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Forms;

using IdentifyMobi.Enums;
using IdentifyMobi.Models;
using IdentifyMobi.Interfaces;

namespace IdentifyMobi.Services
{
	public class IdentityService
	{
		HttpClient client = new HttpClient();

		public static IdentityService Instance { get; set; } = new IdentityService();

		public IdentityService()
		{
			client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
			client.DefaultRequestHeaders
			  .Accept
			  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		/// <summary>
		/// Identifies the image async. Image will be scaled to approximately 400 x 400 pixels to be processed
		/// </summary>
		/// <returns>Whether the user was identified or not</returns>
		/// <param name="imageLargerThan400x400">Image larger than400x400.</param>
		public async Task<IdentityResponse> IdentifyImageAsync(byte[] imageLargerThan400x400)
		{
			var returnValue = new IdentityResponse();
			var resizedBytes = DependencyService.Get<IResizer>().ResizeImage(imageLargerThan400x400, 400, 400);

			var contentObject = ImageUpload.CreateNew();
			contentObject.users.Add(new User(Convert.ToBase64String(resizedBytes)));

			var content = new StringContent(JsonConvert.SerializeObject(contentObject), System.Text.Encoding.UTF8, "application/json");

			var result = await client.PostAsync("http://michaels-it-org-facialrecognition.azurewebsites.net/api/identify/v1.0/check", content);
			var response = await result.Content.ReadAsStringAsync();
			response = response.Replace("\"", "");

			System.Diagnostics.Debug.WriteLine($"{response}");

			if (response.Contains("@"))
			{
				returnValue.Response = IdentityType.Success;
				returnValue.Data = response;
				return returnValue;
			}

			switch (response)
			{
				case "Please take off glasses":
					returnValue.Response = IdentityType.Glasses;
					break;
				case "No face detected":
					returnValue.Response = IdentityType.NoFacesDetected;
					break;
				case "User not found":
					returnValue.Response = IdentityType.UserNotFound;
					break;
				case "Unable to Identify User":
					returnValue.Response = IdentityType.Failure;
					break;
				case "Error":
					returnValue.Response = IdentityType.Error;
					break;
				default:
					returnValue.Response = IdentityType.Failure;
					break;
			}

			return returnValue;
		}

		/// <summary>
		/// Uploads an identity to the backend service. 
		/// The identity will be processed through the Cognitive Services Identity Model and Identified Users Database.
		/// </summary>
		/// <returns>A string with the given results from the backend service</returns>
		/// <param name="email">Email.</param>
		/// <param name="image">Image.</param>
		public async Task<string> UploadIdentity(string email, byte[] image)
		{
			var resizedBytes = DependencyService.Get<IResizer>().ResizeImage(image, 400, 400);

			var contentObject = ImageUpload.CreateNew();
			contentObject.users.Add(new User(Convert.ToBase64String(resizedBytes)));

			var content = new StringContent(JsonConvert.SerializeObject(contentObject), System.Text.Encoding.UTF8, "application/json");

			var result = await client.PostAsync("http://michaels-it-org-facialrecognition.azurewebsites.net/api/identify/v1.0/add", content);
			var response = await result.Content.ReadAsStringAsync();

			return response ?? "Error";
		}
	}
}