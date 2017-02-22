using System;
using System.Collections.Generic;

namespace IdentifyMobi.Models
{
	public class ImageUpload
	{
		public ImageUpload()
		{
			messageId = Guid.NewGuid();
		}

		public Guid messageId { get; set; }
		public List<User> users { get; set; } = new List<User>();

		public static ImageUpload CreateNew()
		{
			var imageUpload = new ImageUpload();
			imageUpload.messageId = Guid.NewGuid();

			return imageUpload;
		}
	}

	public class User
	{
		public User()
		{
		}

		public User(string imageData)
		{
			data = imageData;
		}

		public string useremail { get; set; }
		public string data { get; set; }
	}
}