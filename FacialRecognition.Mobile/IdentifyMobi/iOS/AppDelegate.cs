using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;
using HockeyApp;
using HockeyApp.iOS;

namespace IdentifyMobi.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			CachedImageRenderer.Init();

			// Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("5f78cdcad3b9495c9721ebcf1b3d097a");
			manager.StartManager();
			manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds

			App.ScreenWidth = (double)(UIScreen.MainScreen.Bounds.Width);
			App.ScreenHeight = (double)(UIScreen.MainScreen.Bounds.Height);

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
