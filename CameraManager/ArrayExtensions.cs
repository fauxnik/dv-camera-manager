namespace CameraManager;

static class ArrayExtensions
{
	public static T? FirstOrDefault<T>(this T?[] array)
	{
		foreach(T? camera in array)
		{
			if (camera != null) { return camera; }
		}
		return default;
	}
}
