using FacialRecognition.Portable.Models;
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
        private static readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("0687852a1cee452a8cc6acbd5d8e83cc");

        public static async Task<AnalyzedFacialResults> DetectFaceAsync(byte[] imageBytes)
        {
            var faces = await UploadAndDetectFaces(imageBytes);

            if (faces.Count() > 1)
                return new AnalyzedFacialResults("Detected more than one face in picture");
            else if (faces.Count() == 0)
                return new AnalyzedFacialResults("No face detected");
            else
            {
                var detectedFace = faces[0];

                if(detectedFace.FaceAttributes!= null)
                {
                    var test = detectedFace.FaceAttributes.Glasses;
                    var test1 = test;
                }

                return new AnalyzedFacialResults(detectedFace);
            }
        }

        private static async Task<Face[]> UploadAndDetectFaces(byte[] imageBytes)
        {
            try
            {
                using (Stream imageFileStream = new MemoryStream(imageBytes))
                {
                    var faces = await faceServiceClient.DetectAsync(imageFileStream);

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