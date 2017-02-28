using FacialRecognition.Backend.DataObjects;
using FacialRecognition.Backend.Models;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacialRecognition.Backend
{
	public static class FaceAnalyzer
	{
		public static readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("{your api key here}");

		public static async Task<AnalyzedFacialResults> DetectFaceAsync(byte[] imageBytes)
		{
			var faces = await DetectFacesAsync(imageBytes);

			if (faces.Count() > 1)
				return new AnalyzedFacialResults("Detected more than one face in picture");
			else if (faces.Count() == 0)
				return new AnalyzedFacialResults("No face detected");
			else
			{
				var face = faces[0];
				if (face.FaceAttributes != null)
				{
					var glasses = face.FaceAttributes.Glasses;

					if (face.FaceAttributes.Glasses.ToString() != "NoGlasses")
						return new AnalyzedFacialResults("Please take off glasses");
				}

				try
				{
					var faceIds = faces.Select(x => x.FaceId).ToArray();
					var personGroups = await FaceAnalyzer.faceServiceClient.ListPersonGroupsAsync();
					var xamGroup = personGroups.Where(x => x.Name == "Xamarin").FirstOrDefault();

					// Step 4b - Identify the person in the photo, based on the face.
					var results = await faceServiceClient.IdentifyAsync(xamGroup.PersonGroupId, faceIds);

					if (results[0].Candidates.Count() == 0)
					{
						return new AnalyzedFacialResults("Unable to Identify User");
					}
					else if (results[0].Candidates.Count() == 1)
					{
						return new AnalyzedFacialResults(results[0].Candidates[0]);
					}
				}
				catch (FaceAPIException e)
				{

				}

				return null;
			}
		}

		private static async Task<Face[]> DetectFacesAsync(byte[] imageBytes)
		{
			try
			{
				using (Stream imageFileStream = new MemoryStream(imageBytes))
				{
					var faces = await faceServiceClient.DetectAsync(imageFileStream, returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Glasses });

					return faces;

				}
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}