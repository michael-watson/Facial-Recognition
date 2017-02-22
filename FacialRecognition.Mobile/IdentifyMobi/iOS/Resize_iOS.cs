using System;
using System.Drawing;

using UIKit;
using Foundation;

using IdentifyMobi.iOS;
using IdentifyMobi.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Resize_iOS))]

namespace IdentifyMobi.iOS
{
	public class Resize_iOS : IResizer
	{
		public byte[] ResizeImage(byte[] imageData, float width, float height)
		{
			UIImage originalImage = ImageFromByteArray(imageData);

			var originalImageHeight = originalImage.Size.Height;
			var originalImageWidth = originalImage.Size.Width;

			nfloat newHeight = 0;
			nfloat newWidth = 0;


			if (originalImageHeight > originalImageWidth)
			{
				newHeight = height;
				nfloat scale = originalImageHeight / height;
				newWidth = originalImageWidth / scale;
			}
			else
			{
				newWidth = width;
				nfloat scale = originalImageWidth / width;
				newHeight = originalImageHeight / scale;
			}

			width = (float)newWidth;
			height = (float)newHeight;

			UIGraphics.BeginImageContext(new SizeF(width, height));
			originalImage.Draw(new RectangleF(0, 0, width, height));

			var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			var bytesImagen = resizedImage.AsJPEG().ToArray();
			resizedImage.Dispose();

			return bytesImagen;
		}

		UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			UIImage image;

			try
			{
				image = new UIImage(NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}

			return image;
		}
	}
}