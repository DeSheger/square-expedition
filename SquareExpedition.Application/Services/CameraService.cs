using Microsoft.Xna.Framework;
using SquareExpedition.Data.Interactions;

namespace SquareExpedition.Application.Services;

public class CameraService
{
    private CameraService()
    {
    }

    private static CameraService? _cameraInstance;

    private static readonly object Lock = new();

    public static CameraService GetInstance(Camera camera)
    {
        if (_cameraInstance == null)
            lock (Lock)
            {
                if (_cameraInstance == null)
                {
                    _cameraInstance = new CameraService();
                    _cameraInstance.Camera = camera;
                }
            }

        return _cameraInstance;
    }

    private Camera? Camera { get; set; }

    public void SetCameraPosition(float x, float y, float z)
    {
        if (_cameraInstance?.Camera == null)
            throw new Exception("Camera is not initialized.");

        _cameraInstance.Camera.CamPosition = new Vector3(x, y, z);
    }

    public void SetCameraTarget(float x, float y, float z)
    {
        if (_cameraInstance?.Camera == null)
            throw new Exception("Camera is not initialized.");

        _cameraInstance.Camera.CamTarget = new Vector3(x, y, z);
    }

    public void UpdateCameraPosition(float? x, float? y, float? z)
    {
        if (_cameraInstance?.Camera?.CamPosition == null)
            throw new Exception("Camera is not initialized.");

        var current = _cameraInstance.Camera.CamPosition.Value;
        _cameraInstance.Camera.CamPosition = new Vector3(
            x.HasValue ? current.X + x.Value : current.X,
            y.HasValue ? current.Y + y.Value : current.Y,
            z.HasValue ? current.Z + z.Value : current.Z
        );
    }

    public void UpdateCameraTarget(float? x, float? y, float? z)
    {
        if (_cameraInstance?.Camera?.CamTarget == null)
            throw new Exception("Camera is not initialized.");

        var current = _cameraInstance.Camera.CamTarget.Value;
        _cameraInstance.Camera.CamTarget = new Vector3(
            x.HasValue ? current.X + x.Value : current.X,
            y.HasValue ? current.Y + y.Value : current.Y,
            z.HasValue ? current.Z + z.Value : current.Z
        );
    }

    public Vector3 GetCameraPosition()
    {
        if (_cameraInstance?.Camera?.CamPosition == null)
            throw new Exception("Camera is not initialized.");

        return (Vector3)_cameraInstance.Camera.CamPosition;
    }

    public Vector3 GetCameraTarget()
    {
        if (_cameraInstance?.Camera?.CamTarget == null)
            throw new Exception("Camera is not initialized.");

        return (Vector3)_cameraInstance.Camera.CamTarget;
    }
}