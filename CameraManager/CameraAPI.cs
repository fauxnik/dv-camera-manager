using UnityEngine;
using static UnityEngine.Object;

namespace CameraManager;

public static class CameraAPI
{
	public static Camera CloneCamera(Camera source)
	{
		// Instantiate duplicates all components attached to the game object
		// This is necessary for fog, etc. to work
		var gameObject = Instantiate(source.gameObject);

		// The player camera has child objects that we don't need
		for (int i = gameObject.transform.childCount; i > 0; --i)
		{
			Destroy(gameObject.transform.GetChild(i - 1).gameObject);
		}

		return gameObject.GetComponent<Camera>();
	}
}
