using System;
using System.Linq;
using System.Reflection;

using UIKit;

using Xamarin.Forms.Platform.iOS;

using IdentifyMobi.iOS;
using IdentifyMobi.Interfaces;
using Foundation;
using IdentifyMobi.Enums;
using IdentifyMobi.Models;

[assembly: Xamarin.Forms.Dependency(typeof(Dialog_iOS))]

namespace IdentifyMobi.iOS
{
	public class Dialog_iOS : IDialogService
	{
		public Dialog_iOS()
		{
		}

		NSAttributedString title, message;

		Action _callBack;

		UIAlertAction okButton;
		UIAlertController alertController;

		public void ShowAlert(Action callback, string messageToDisplay, bool success = false)
		{
			_callBack = callback;

			buildAlert(messageToDisplay, success);
		}

		public void ShowAlert(Action callback, IdentityResponse identityResponse)
		{
			_callBack = callback;

			buildAlert(identityResponse);
		}

		void buildAlert(string messageToDisplay, bool success)
		{
			initializeDialog();

			if (success)
				title = new NSAttributedString("Success!", foregroundColor: UIColor.FromRGB(82, 158, 63));
			else
				title = new NSAttributedString("Failure!", foregroundColor: UIColor.Red);

			message = new NSAttributedString(messageToDisplay); ;

			presentDialog();
		}

		void buildAlert(IdentityResponse identityResponse)
		{
			initializeDialog();

			switch (identityResponse.Response)
			{
				case IdentityType.Success:
					title = new NSAttributedString("Success!", foregroundColor: UIColor.FromRGB(82, 158, 63));
					message = new NSAttributedString($"Thank you {identityResponse.Data} for verifying yourself. Glad you could make it back!");

					break;
				case IdentityType.Glasses:
					title = new NSAttributedString("Failure!", foregroundColor: UIColor.Red);
					message = new NSAttributedString("Unfortunrately we were unable to verify your identity because of your glasses. Please take off your glasses and try again.");

					break;
				case IdentityType.UserNotFound:
					title = new NSAttributedString("Failure!", foregroundColor: UIColor.Red);
					message = new NSAttributedString("Unfortunrately, this user was not found in our database.");

					break;
				case IdentityType.NoFacesDetected:
					title = new NSAttributedString("No Face Detected!");
					message = new NSAttributedString("There were no faces detected in the picture. I'm not crazy and you can't trick me like that. Please take a picture of yourself and I'll verify it again.");

					break;
				case IdentityType.Error:
					title = new NSAttributedString("Error!");
					message = new NSAttributedString("KERNAL PANIC - not syncing: Attempted to kill init! exitcode=0x00000100. Restarting.... Whoa that was crazy, something happened. Can you try again?");

					break;
				default:
					title = new NSAttributedString("Failure!", foregroundColor: UIColor.Red);
					message = new NSAttributedString("Unfortunrately we were unable to verify your identity. Please try again.");

					break;
			}

			presentDialog();
		}

		void presentDialog()
		{
			alertController.SetValueForKey(title, new NSString("attributedTitle"));
			alertController.SetValueForKey(message, new NSString("attributedMessage"));

			var appDelegate = ((FormsApplicationDelegate)UIApplication.SharedApplication.Delegate);
			BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo field = (typeof(FormsApplicationDelegate)).GetField("_window", bindFlags);
			UIWindow window = (UIWindow)field.GetValue(appDelegate);

			window.RootViewController.PresentViewController(alertController, true, null);
		}

		void initializeDialog()
		{
			if (alertController == null)
				alertController = new UIAlertController();

			if (okButton == null)
				okButton = UIAlertAction.Create("Ok", UIAlertActionStyle.Default, DismissDialog);

			if (!alertController.Actions.Contains(okButton))
				alertController.AddAction(okButton);
		}

		void DismissDialog(UIAlertAction obj)
		{
			alertController.DismissViewController(true, null);

			okButton.Dispose();
			alertController.Dispose();

			title = null;
			message = null;
			okButton = null;
			alertController = null;

			if (_callBack != null)
				_callBack.Invoke();

			_callBack = null;
		}
	}
}