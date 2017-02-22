using Xamarin.Forms;

using IdentifyMobi.Pages;

namespace IdentifyMobi
{
	public class App : Application
	{
		public static double ScreenWidth { get; set; }
		public static double ScreenHeight { get; set; }

#if DEBUG
		public static bool TestingFlag { get; set; } = true;
#endif

		public App()
		{
			var identifyPage = new FaceVerificationPage();
			var navPage = new NavigationPage(identifyPage);

			if (Device.OS == TargetPlatform.iOS)
				navPage.Navigation.InsertPageBefore(new AddIdentityPage(), identifyPage);

			NavigationPage.SetHasBackButton(identifyPage, false);
			NavigationPage.SetHasNavigationBar(identifyPage, false);

			MainPage = navPage;
		}

		protected async override void OnStart()
		{
			// Handle when your app starts
			await Plugin.Media.CrossMedia.Current.Initialize();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
