using IdentifyMobi.Extensions;
using IdentifyMobi.Models;
using IdentifyMobi.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using IdentifyMobi.Interfaces;
namespace IdentifyMobi.ViewModels
{
	public class AddIdentityViewModel : BaseViewModel
	{
		public AddIdentityViewModel()
		{
		}

		bool isLoading;
		MediaFile file;
		string email = string.Empty;
		ImageSource profilePic = null;

		public string AddPhotoButtonTitle
		{
			get
			{
				if (file != null)
					return "Re-take photo";

				return "Take photo";
			}
		}

		public bool IsLoading
		{
			get { return isLoading; }
			set
			{
				isLoading = value;
				OnPropertyChanged();
			}
		}

		public string Email
		{
			get { return email; }
			set
			{
				if (email == value)
					return;
				email = value;
				OnPropertyChanged();
			}
		}

		public ImageSource ProfilePicSource
		{
			get
			{
				OnPropertyChanged("AddPhotoButtonTitle");

				if (file == null)
					return null;

				return ImageSource.FromFile(file.Path);
			}
		}

		public async void VerifyUser(object sender, System.EventArgs e)
		{
			file = await CrossMedia.Current.TakePhotoAsync(
				new StoreCameraMediaOptions
				{
					Directory = "AddIdentity",
					Name = $"{Email}.jpg"
				});

			if (file == null)
				return;

			IsLoading = true;

			using (var fileStream = file.GetStream())
			{
				var fileBytes = fileStream.ReadToEnd();
				var IdentityResponse = await IdentityService.Instance.IdentifyImageAsync(fileBytes);

				DependencyService.Get<IDialogService>().ShowAlert(null, IdentityResponse);
			}

			IsLoading = false;
		}

		public async void AddPhotoButton_Clicked(object sender, System.EventArgs e)
		{
			file = await CrossMedia.Current.TakePhotoAsync(
				new StoreCameraMediaOptions
				{
					Directory = "AddIdentity",
					Name = $"{Email}.jpg"
				});

			if (file == null)
				return;

			OnPropertyChanged("ProfilePicSource");
		}

		public async void SubmitButton_Clicked(object sender, System.EventArgs e)
		{
			if (IsLoading)
				return;

			IsLoading = true;

			if (string.IsNullOrEmpty(Email))
			{
				DependencyService.Get<IDialogService>().ShowAlert(null, "You must enter an email to add the user.");
				IsLoading = false;
				return;
			}
			else if (!Email.Contains("@") || !Email.Contains(".com"))
			{
				//Improperly Formatted Email
				DependencyService.Get<IDialogService>().ShowAlert(null, "Email is not properly formatted.");
				IsLoading = false;
				return;
			}

			//Acceptable Email

			if (file == null)
			{
				//No Image taken so far
				DependencyService.Get<IDialogService>().ShowAlert(null, "You must take a picture first!");
				IsLoading = false;
				return;
			}

			using (var fileStream = file.GetStream())
			{
				var fileBytes = fileStream.ReadToEnd();
				var uploadResponse = await IdentityService.Instance.UploadIdentity(Email, fileBytes);

				System.Diagnostics.Debug.WriteLine(uploadResponse);

				if (uploadResponse.Contains("Saved"))
				{
					DependencyService.Get<IDialogService>().ShowAlert(null, new IdentityResponse(Enums.IdentityType.Success, $"Saved {Email} successfully and trained model."));
				}
				else if (uploadResponse.Contains("Glasses"))
				{
					DependencyService.Get<IDialogService>().ShowAlert(null, new IdentityResponse(Enums.IdentityType.Glasses, uploadResponse));
				}
				else if (uploadResponse.Contains("No face detected"))
				{
					DependencyService.Get<IDialogService>().ShowAlert(null, new IdentityResponse(Enums.IdentityType.NoFacesDetected, uploadResponse));
				}
				else
					DependencyService.Get<IDialogService>().ShowAlert(null, uploadResponse);
			}

			file = null;
			IsLoading = false;

			OnPropertyChanged("ProfilePicSource");
		}
	}
}