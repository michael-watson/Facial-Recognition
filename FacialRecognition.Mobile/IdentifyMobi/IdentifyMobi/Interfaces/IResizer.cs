namespace IdentifyMobi.Interfaces
{
	public interface IResizer
	{
		byte[] ResizeImage(byte[] imageData, float width, float height);
	}
}
