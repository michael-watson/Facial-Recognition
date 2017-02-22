using Android.OS;
using Android.App;
using Android.Content.PM;

using IdentifyMobi.Droid.Fragments;
using Plugin.Permissions;
using FFImageLoading.Forms.Droid;
using Android.Views;
using HockeyApp.Android;

namespace IdentifyMobi.Droid
{
	[Activity(Label = "IdentifyMobi.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public static IdentityDialog dialog = new IdentityDialog();

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);
			CachedImageRenderer.Init();

			var pixels = Resources.DisplayMetrics.WidthPixels;
			var density = Resources.DisplayMetrics.Density;

			App.ScreenWidth = (double)(Resources.DisplayMetrics.WidthPixels - 0.5f) / density;
			App.ScreenHeight = (double)(Resources.DisplayMetrics.HeightPixels - 0.5f) / density;

			CrashManager.Register(this, "c1be25c3171644ffb6550bf4cd55b15e");

			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}