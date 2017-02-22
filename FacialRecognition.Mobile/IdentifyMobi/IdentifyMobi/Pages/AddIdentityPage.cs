using Xamarin.Forms;

using FFImageLoading.Forms;

using IdentifyMobi.ViewModels;
using IdentifyMobi.Views;

namespace IdentifyMobi.Pages
{
	public class AddIdentityPage : ContentPage
	{
		public AddIdentityPage()
		{
			NavigationPage.SetHasBackButton(this, false);

			var enterEmailLabel = new Label { Text = "Please enter email to be added", HorizontalOptions = LayoutOptions.Start };
			var emailEntry = new Entry { Placeholder = "myemail@emailhost.com", HorizontalOptions = LayoutOptions.FillAndExpand };
			var photoImage = new ProfilePictureView
			{
				AutomationId = "profilePicImage",
				Margin = new Thickness(40, 0),
				WidthRequest = App.ScreenWidth * 0.5,
				DownsampleToViewSize = true,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			var addPhotoButton = new Button { AutomationId = "addPhotoButton", Text = "Take photo", BackgroundColor = Color.Blue, TextColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };
			var submitButton = new Button { AutomationId = "submitButton", Text = "Submit", BackgroundColor = Color.Blue, TextColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End };
			var activityIndicator = new ActivityIndicator { AutomationId = "loadingIndicator" };

			var layout = new StackLayout
			{
				Padding = 20,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					enterEmailLabel,
					emailEntry,
					photoImage,
					activityIndicator,
					addPhotoButton,
					submitButton
				}
			};

			var verifyUserToolBarItem = new ToolbarItem { Text = "Verify" };
			var viewModel = new AddIdentityViewModel();

			Content = layout;
			Title = "Add Identified User";
			BindingContext = viewModel;
			ToolbarItems.Add(verifyUserToolBarItem);

			emailEntry.SetBinding(Entry.TextProperty, "Email");
			photoImage.SetBinding(CachedImage.SourceProperty, "ProfilePicSource", BindingMode.TwoWay);
			addPhotoButton.SetBinding(Button.TextProperty, "AddPhotoButtonTitle");
			activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
			activityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsLoading");

			addPhotoButton.Clicked += viewModel.AddPhotoButton_Clicked;
			submitButton.Clicked += viewModel.SubmitButton_Clicked;
			verifyUserToolBarItem.Clicked += viewModel.VerifyUser;
		}
	}
}