using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacialRecognition.Backend.Models
{
    public class AnalyzedFacialResults
    {
        public AnalyzedFacialResults()
        {
        }

        public AnalyzedFacialResults(string error)
        {
            Success = false;
            Error = error;
        }


        public AnalyzedFacialResults(Candidate identifiedResult)
        {
            Success = true;
            FaceId = identifiedResult.PersonId.ToString();
            Confidence = identifiedResult.Confidence;
        }

        public bool Success { get; set; }
        public string Error { get; set; }
        public string FaceId { get; set; }
        public double Confidence { get; set; }
    }
}
