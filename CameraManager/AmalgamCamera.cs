using System;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManager;

public class AmalgamCamera : MonoBehaviour
{
	private const int DEFAULT_DEPTH_OFFSET = 10;

	private Camera?[] cameras = {};
	private int depthOffset = DEFAULT_DEPTH_OFFSET;

	/**
	 * TODO: write doc block
	 */
	public void Init(CameraType cameraTypes, int depthOffset = DEFAULT_DEPTH_OFFSET)
	{
		if (cameras.Length > 0)
		{
			throw new Exception("This AmalgamCamera has already been initialized. It can't be initialized again.");
		}

		this.depthOffset = depthOffset;

		uint cameraCount = Count1Bits((uint)cameraTypes);
		cameras = new Camera?[cameraCount];

		if ((cameraTypes & CameraType.World) > 0)
		{
			Camera? camera = CloneCamera(PlayerManager.PlayerCamera, "AmalgamCamera (World)");
			if (camera == null)
			{
				// TODO: log error/warning
			}
			else { cameras.SetValue(camera, cameras.Length - cameraCount--); }
		}

		if ((cameraTypes & CameraType.UI) > 0)
		{
			Camera? camera = CloneCamera(new List<Camera>(Camera.allCameras).Find(cam => cam.name == "SecondCamera"), "AmalgamCamera (UI)");
			if (camera == null)
			{
				// TODO: log error/warning
			}
			cameras.SetValue(camera, cameras.Length - cameraCount--);
		}

		if ((cameraTypes & CameraType.Effects) > 0)
		{
			Camera? camera = CloneCamera(new List<Camera>(Camera.allCameras).Find(cam => cam.name == "ThirdCamera"), "AmalgamCamera (Effects)");
			if (camera == null)
			{
				// TODO: log error/warning
			}
			cameras.SetValue(camera, cameras.Length - cameraCount--);
		}

		foreach(Camera? camera in cameras)
		{
			if (camera == null) { continue; }
			camera.gameObject.transform.SetParent(gameObject.transform, false);
		}
	}

	public float fieldOfView
	{
		get { return cameras.FirstOrDefault()?.fieldOfView ?? -1f; }
		set
		{
			// TODO: use coroutine to wait until the next frame to do this
			foreach (Camera? camera in cameras)
			{
				if (camera == null) { continue; }
				camera.fieldOfView = value;
			}
		}
	}

	public float nearClipPlane
	{
		get { return cameras.FirstOrDefault()?.nearClipPlane ?? -1f; }
		set
		{
			// TODO: use coroutine to wait until the next frame to do this
			foreach (Camera? camera in cameras)
			{
				if (camera == null) { continue; }
				camera.nearClipPlane = value;
			}
		}
	}

	public StereoTargetEyeMask stereoTargetEye
	{
		get { return cameras.FirstOrDefault()?.stereoTargetEye ?? StereoTargetEyeMask.None; }
		set
		{
			// TODO: user coroutine to wait until the next frame to do this
			foreach (Camera? camera in cameras)
			{
				if (camera == null) { continue; }
				camera.stereoTargetEye = value;
			}
		}
	}

	public new bool enabled
	{
		get { return base.enabled; }
		set
		{
			base.enabled = value;
			foreach (Camera? camera in cameras)
			{
				if (camera == null) { continue; }
				camera.enabled = value;
			}
		}
	}

	private Camera CloneCamera(Camera source, string name)
	{
		Camera camera = CameraAPI.CloneCamera(source);
		camera.gameObject.transform.SetParent(source.gameObject.transform, false);
		camera.gameObject.name = name;
		camera.enabled = base.enabled;
		camera.depth += depthOffset;
		return camera;
	}

	private uint Count1Bits(uint number)
	{
		uint count = 0;
		while (number > 0)
		{
			if (number % 2 != 0)
				count++;
			number /= 2;
		}
		return count;
	}
}
