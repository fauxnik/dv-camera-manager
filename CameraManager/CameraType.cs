namespace CameraManager;

// Derail Valley uses 3 cameras
//   1. Camera (eye) - renders most things in the world, depth 0 (Layers 267386643)
//   2. SecondCamera - renders UI, depth 1 (Layer 32)
public enum CameraType
{
	World = 1,
	UI = 2,
	All = 3,
}
