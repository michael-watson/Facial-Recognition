using System;
using System.Threading.Tasks;

using Plugin.Media;

using Xamarin.Forms;

using IdentifyMobi.Enums;
using IdentifyMobi.Pages;
using IdentifyMobi.Models;
using IdentifyMobi.Services;
using IdentifyMobi.Extensions;
using IdentifyMobi.Interfaces;

namespace IdentifyMobi.ViewModels
{
	public class FaceVerificationViewModel : BaseViewModel
	{
		public FaceVerificationViewModel()
		{
		}

		public IdentityResponse IdentityResponse = new IdentityResponse();

		bool isLoading;
		public bool IsLoading
		{
			get
			{
				return isLoading;
			}
			set
			{
				isLoading = value;
				OnPropertyChanged();
			}
		}

		public async Task VerifyIdentityByPhotoAsync()
		{
			if (IsLoading)
				return;

			IsLoading = true;

			var response = await verifyIdentityAsync();

			if (response == null)
			{
				reRunVerification.Invoke();
			}
			else
				DependencyService.Get<IDialogService>().ShowAlert(reRunVerification, response);
		}

		async Task<IdentityResponse> verifyIdentityAsync()
		{
			var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				Directory = "Sample",
				Name = "test.jpg"
			});

			if (file == null)
				return null;

			using (var fileStrem = file.GetStream())
			{
				var fileBytes = fileStrem.ReadToEnd();
				IdentityResponse = await IdentityService.Instance.IdentifyImageAsync(fileBytes);
			}

			return IdentityResponse;
		}

		Action reRunVerification = async () =>
		{
			FaceVerificationPage.ViewModel.IsLoading = false;

			if (FaceVerificationPage.ViewModel.IdentityResponse.Response == IdentityType.Success)
			{
				if (Device.OS == TargetPlatform.iOS)
					await App.Current.MainPage.Navigation.PopAsync(true);
				else
					await App.Current.MainPage.Navigation.PushAsync(new AddIdentityPage());
			}
			else
				await FaceVerificationPage.ViewModel.VerifyIdentityByPhotoAsync();
		};
	}
}