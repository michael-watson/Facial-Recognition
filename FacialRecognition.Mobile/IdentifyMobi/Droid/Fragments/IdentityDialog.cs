using System;

using Android.Graphics;
using Android.Widget;

using IdentifyMobi.Enums;

namespace IdentifyMobi.Droid.Fragments
{
	public class IdentityDialog : Android.Support.V4.App.DialogFragment
	{
		public bool UseCustomMessage { get; set; }
		public string CustomMessage { get; set; }
		public Action CallBack { get; set; }
		public string IdentifiedEmail { get; set; }
		public IdentityType IdentityVerification { get; set; }

		public override void OnCancel(Android.Content.IDialogInterface dialog)
		{
			CloseDialog(this, EventArgs.Empty);

			base.OnCancel(dialog);
		}

		public override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.IdentityDialog, container, false);

			var okButton = view.FindViewById<Button>(Resource.Id.accept);
			okButton.Click += CloseDialog;

			var image = view.FindViewById<ImageView>(Resource.Id.image);
			var title = view.FindViewById<TextView>(Resource.Id.title);
			var subtext = view.FindViewById<TextView>(Resource.Id.subtext);

			if (UseCustomMessage)
			{
				switch (IdentityVerification)
				{
					case IdentityType.Success:
						title.Text = "Success!";
						title.SetTextColor(Color.Rgb(82, 158, 63));
						subtext.Text = CustomMessage;
						image.SetImageResource(Resource.Drawable.success);
						break;
					case IdentityType.Failure:
						title.Text = "Failure!";
						title.SetTextColor(Color.Red);
						subtext.Text = CustomMessage;
						image.SetImageResource(Resource.Drawable.success);
						break;
				}
			}
			else
			{
				switch (IdentityVerification)
				{
					case IdentityType.Success:
						title.Text = "Success!";
						title.SetTextColor(Color.Rgb(82, 158, 63));
						subtext.Text = $"Thank you {IdentifiedEmail ?? ""} for verifying yourself. Glad you could make it back!";
						image.SetImageResource(Resource.Drawable.success);
						break;
					case IdentityType.Failure:
						title.Text = "Failure!";
						title.SetTextColor(Color.Red);
						subtext.Text = "Unfortunately we were unable to verify your identity.";
						image.SetImageResource(Resource.Drawable.success);
						break;
					case IdentityType.UserNotFound:
						title.Text = "Failure!";
						title.SetTextColor(Color.Red);
						subtext.Text = "Unfortunrately, this user was not found in our database";
						image.SetImageResource(Resource.Drawable.icon);
						break;
					case IdentityType.NoFacesDetected:
						title.Text = "No Face Detected!";
						title.SetTextColor(Color.Blue);
						subtext.Text = "There were no faces detected in the picture. I'm not crazy and you can't trick me like that. Please take a picture of yourself and I'll verify it again.";
						image.SetImageResource(Resource.Drawable.icon);
						break;
					case IdentityType.Error:
						title.Text = "Error!";
						title.SetTextColor(Color.Red);
						subtext.Text = "KERNAL PANIC - not syncing: Attempted to kill init! exitcode=0x00000100. Restarting.... Whoa that was crazy, something happened. Can you try again?";
						image.SetImageResource(Resource.Drawable.icon);
						break;
					case IdentityType.Glasses:
						title.Text = "Failure!";
						title.SetTextColor(Color.Blue);
						subtext.Text = "Unfortunrately we were unable to verify your identity because of your glasses. Please take off your glasses and try again.";
						image.SetImageResource(Resource.Drawable.icon);
						break;
				}
			}

			return view;
		}

		void CloseDialog(object sender, EventArgs e)
		{
			var okButton = View.FindViewById<Button>(Resource.Id.accept);
			okButton.Click -= CloseDialog;

			if (CallBack != null)
				CallBack.Invoke();

			CallBack = null;
			UseCustomMessage = false;
			CustomMessage = string.Empty;
			IdentifiedEmail = string.Empty;
			IdentityVerification = IdentityType.Failure;

			MainActivity.dialog.Dismiss();
		}
	}
}