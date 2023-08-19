using System;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManager;

public class AmalgamCamera : MonoBehaviour
{
	private const int DEFAULT_DEPTH_OFFSET = 10;

	private Dictionary<CameraType, Camera> cameras = new Dictionary<CameraType, Camera>();
	private int depthOffset = DEFAULT_DEPTH_OFFSET;
	private float _fieldOfView = -1f;
	private float _zoomFactor = 1f;

	/**
	 * TODO: write doc block
	 */
	public void Init(CameraType cameraTypes, int depthOffset = DEFAULT_DEPTH_OFFSET)
	{
		if (cameras.Count > 0)
		{
			throw new Exception("This AmalgamCamera has already been initialized. It can't be initialized again.");
		}

		this.depthOffset = depthOffset;

		if ((cameraTypes & CameraType.World) > 0)
		{
			Camera? camera = CloneCamera(PlayerManager.PlayerCamera, "AmalgamCamera (World)");
			if (camera == null)
			{
				// TODO: log error/warning
			}
			else { cameras.Add(CameraType.World, camera); }
		}

		if ((cameraTypes & CameraType.UI) > 0)
		{
			Camera? camera = CloneCamera(new List<Camera>(Camera.allCameras).Find(cam => cam.name == "SecondCamera"), "AmalgamCamera (UI)");
			if (camera == null)
			{
				// TODO: log error/warning
			}
			else { cameras.Add(CameraType.UI, camera); }
		}

		if ((cameraTypes & CameraType.Effects) > 0)
		{
			Camera? camera = CloneCamera(new List<Camera>(Camera.allCameras).Find(cam => cam.name == "ThirdCamera"), "AmalgamCamera (Effects)");
			if (camera == null)
			{
				// TODO: log error/warning
			}
			else { cameras.Add(CameraType.Effects, camera); }
		}

		foreach ((_, Camera camera) in cameras)
		{
			if (camera == null) { continue; }
			camera.gameObject.transform.SetParent(gameObject.transform, false);
			if (_fieldOfView < 0f)
				_fieldOfView = camera.fieldOfView;
			else if (!Mathf.Approximately(camera.fieldOfView, _fieldOfView))
				camera.fieldOfView = _fieldOfView;
		}
	}

	public float fieldOfView
	{
		get { return _fieldOfView; }
		set
		{
			_fieldOfView = value;
			UpdateFieldOfView();
		}
	}

	public float zoomFactor
	{
		get { return _zoomFactor; }
		set
		{
			_zoomFactor = value;
			UpdateFieldOfView();
		}
	}

	private void UpdateFieldOfView()
	{
		// TODO: use coroutine to wait until the next frame to do this
		foreach ((_, Camera camera) in cameras)
		{
			if (camera == null) { continue; }
			camera.fieldOfView = fieldOfView / zoomFactor;
		}
	}

	public float nearClipPlane
	{
		get { return cameras.FirstOrDefault()?.nearClipPlane ?? -1f; }
		set
		{
			// TODO: use coroutine to wait until the next frame to do this
			foreach ((_, Camera camera) in cameras)
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
			// TODO: use coroutine to wait until the next frame to do this
			foreach ((_, Camera camera) in cameras)
			{
				if (camera == null) { continue; }
				camera.stereoTargetEye = value;
			}
		}
	}

	public int cullingMask
	{
		get { return cameras.ContainsKey(CameraType.World) ? cameras[CameraType.World].cullingMask : 0; }
		set
		{
			// TODO: use coroutine to wait until the next frame to do this
			if (cameras.ContainsKey(CameraType.World))
			{
				Camera camera = cameras[CameraType.World];
				camera.cullingMask = value;
			}
		}
	}

	public int targetDisplay
	{
		get { return cameras.FirstOrDefault()?.targetDisplay ?? 0; }
		set
		{
			// TODO: use coroutine to wait until the next frame to do this
			foreach ((_, Camera camera) in cameras)
			{
				if (camera == null) { continue; }
				camera.targetDisplay = value;
			}
		}
	}

	public RenderTexture? targetTexture
	{
		get { return cameras.FirstOrDefault()?.targetTexture ?? default; }
		set
		{
			// TODO: use coroutine to wait until the next frame to do this
			foreach ((_, Camera camera) in cameras)
			{
				if (camera == null) { continue; }
				camera.targetTexture = value;
			}
		}
	}

	public new bool enabled
	{
		get { return base.enabled; }
		set
		{
			base.enabled = value;
			foreach ((_, Camera camera) in cameras)
			{
				if (camera == null) { continue; }
				camera.enabled = value;
			}
		}
	}

	private Camera CloneCamera(Camera source, string name)
	{
		Camera camera = CameraAPI.CloneCamera(source);
		camera.gameObject.name = name;
		camera.enabled = base.enabled;
		camera.depth += depthOffset;
		return camera;
	}
}
