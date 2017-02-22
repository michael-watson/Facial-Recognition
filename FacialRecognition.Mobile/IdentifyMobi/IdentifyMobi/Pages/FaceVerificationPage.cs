
using System;
using System.Net.Http;
using System.Net.Http.Headers;

using Xamarin.Forms;

using FFImageLoading.Forms;

using Plugin.Media;

using IdentifyMobi.Interfaces;
using IdentifyMobi.Extensions;
using IdentifyMobi.Models;
using Newtonsoft.Json;
using IdentifyMobi.Services;
using IdentifyMobi.Enums;
using IdentifyMobi.ViewModels;
using System.Linq;

namespace IdentifyMobi.Pages
{
	public class FaceVerificationPage : ContentPage
	{
		public static FaceVerificationViewModel ViewModel = new FaceVerificationViewModel();

		bool isInitialized = false;

		public FaceVerificationPage()
		{
			var msftLogo = new CachedImage
			{
				Source = "msft.png",
				WidthRequest = App.ScreenWidth * 0.5,
				HeightRequest = App.ScreenHeight * 0.4,
				DownsampleToViewSize = true,
				Margin = new Thickness(0, 0, 0, App.ScreenHeight * 0.2),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			var label = new Label { Text = "Tap the Microsoft Logo to verify your identity", HorizontalTextAlignment = TextAlignment.Center };
			var loadingIndicator = new ActivityIndicator { Margin = new Thickness(0, 0, 0, App.ScreenHeight * 0.2) };

			var grid = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition { Height = GridLength.Star },
					new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
					new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
					new RowDefinition { Height = GridLength.Star },
				},
				RowSpacing = 20,
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				},
			};

			grid.Children.Add(msftLogo, 0, 3, 1, 2);
			grid.Children.Add(loadingIndicator, 1, 2, 2, 3);
			grid.Children.Add(label, 1, 2, 4, 5);

			Content = grid;
			BindingContext = ViewModel;

			loadingIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsLoading");
			loadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");

			NavigationPage.SetHasBackButton(this, false);
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

#if DEBUG
			if (App.TestingFlag)
			{
				if (Device.OS == TargetPlatform.iOS)
					await Navigation.PopAsync();
				else
				{
					if (Navigation.NavigationStack.Count() == 1)
						await Navigation.PushAsync(new AddIdentityPage());
				}

				return;
			}
#endif
			if (Device.OS == TargetPlatform.iOS)
				await DisplayAlert("Verification Required", "We must verify your identity using facial recognition software provided by Cognitive Services.", "Ok, take the picture!");

			await ViewModel.VerifyIdentityByPhotoAsync();
		}
	}
}