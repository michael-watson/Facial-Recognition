using Android.Support.V4.App;
using Android.Support.V7.App;

using Xamarin.Forms;

using IdentifyMobi.Droid;
using IdentifyMobi.Interfaces;
using IdentifyMobi.Enums;
using System;
using IdentifyMobi.Models;

[assembly: Dependency(typeof(Dialog_Droid))]

namespace IdentifyMobi.Droid
{
	public class Dialog_Droid : IDialogService
	{
		public void ShowAlert(Action callback, IdentityResponse identityResponse)
		{
			FragmentManager fm = ((AppCompatActivity)Forms.Context).SupportFragmentManager;

			MainActivity.dialog.CallBack = callback;
			MainActivity.dialog.IdentityVerification = identityResponse.Response;

			if (!string.IsNullOrEmpty(identityResponse.Data))
				MainActivity.dialog.IdentifiedEmail = identityResponse.Data;

			MainActivity.dialog.Show(fm, "Alert");
		}

		public void ShowAlert(Action callback, string messageToDisplay, bool success = false)
		{
			FragmentManager fm = ((AppCompatActivity)Forms.Context).SupportFragmentManager;

			MainActivity.dialog.CallBack = callback;

			if (success)
				MainActivity.dialog.IdentityVerification = IdentityType.Success;
			else
				MainActivity.dialog.IdentityVerification = IdentityType.Failure;

			MainActivity.dialog.UseCustomMessage = true;
			MainActivity.dialog.CustomMessage = messageToDisplay;

			MainActivity.dialog.Show(fm, "Alert");
		}
	}
}