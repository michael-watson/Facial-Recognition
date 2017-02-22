using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace IdentifyMobi.Views
{
	public class ProfilePictureView : CachedImage
	{
		public ProfilePictureView()
		{
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
		}
	}
}
