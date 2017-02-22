using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IdentifyMobi.Interfaces;
using Android.Graphics;
using System.IO;
using IdentifyMobi.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Resizer_Droid))]

namespace IdentifyMobi.Droid
{
    public class Resizer_Droid : IResizer
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap 
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            float newHeight = 0;
            float newWidth = 0;

            var originalImageHeight = originalImage.Height;
            var originalImageWidth = originalImage.Width;

            if (originalImageHeight > originalImageWidth)
            {
                newHeight = height;
                float scale = originalImageHeight / height;
                newWidth = originalImageWidth / scale;
            }
            else
            {
                newWidth = width;
                float scale = originalImageWidth / width;
                newHeight = originalImageHeight / scale;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}