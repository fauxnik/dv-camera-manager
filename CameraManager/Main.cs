using System;
using UnityModManagerNet;

namespace CameraManager;

public static class Main
{
	public static Action<string> Log = (_) => {};
	public static Action<string> LogWarning = (message) => { Log($"[Warning] {message}"); };
	public static Action<string> LogError = (message) => { Log($"[Error] {message}"); };

	// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
	private static bool Load(UnityModManager.ModEntry modEntry)
	{
		Log = modEntry.Logger.Log;
		return true;
	}
}
