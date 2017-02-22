using IdentifyMobi.Models;

namespace IdentifyMobi.Interfaces
{
	public interface IDialogService
	{
		void ShowAlert(System.Action callBack, IdentityResponse identityResponse);
		void ShowAlert(System.Action callBack, string message, bool success = false);
	}
}