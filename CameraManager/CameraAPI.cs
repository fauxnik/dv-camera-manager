using UnityEngine;
using static UnityEngine.Object;

namespace CameraManager;

public static class CameraAPI
{
	public static Camera CloneCamera(Camera source, bool copyParenting)
	{
		var gameObject = source.gameObject;
		if (copyParenting && gameObject.transform.parent != null)
		{
			gameObject = Instantiate(gameObject, gameObject.transform.parent);
		}
		else
		{
			gameObject = Instantiate(gameObject);
		}
		for (int i = gameObject.transform.childCount; i > 0; --i)
		{
			Destroy(gameObject.transform.GetChild(i - 1).gameObject);
		}
		return gameObject.GetComponent<Camera>();
	}
}
