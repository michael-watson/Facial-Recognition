using IdentifyMobi.Enums;

namespace IdentifyMobi.Models
{
	public class IdentityResponse
	{
		public IdentityResponse()
		{
		}

		public IdentityResponse(IdentityType type, string data)
		{
			Response = type;
			Data = data;
		}
		public IdentityType Response { get; set; }
		public string Data { get; set; }
	}
}