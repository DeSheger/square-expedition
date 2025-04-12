using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SquareExpedition.Application.Services;

public class ControllerService
{
    private ControllerService() {}

    private static ControllerService? _controllerInstance;

    private static readonly object Lock = new();

    private float CameraSpeed { get; set; } = 0.25f;

    public static ControllerService GetInstance(CameraService cameraService)
    {
        if (_controllerInstance == null)
            lock (Lock)
            {
                if (_controllerInstance == null)
                {
                    _controllerInstance = new ControllerService();
                    _controllerInstance._cameraService = cameraService;
                }
            }

        return _controllerInstance;
    }

    private CameraService? _cameraService;

    public void HandleInput(GameTime gameTime, Game game, ref Matrix viewMatrix)
    {
        // Escape check
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            game.Exit();

        if (_cameraService == null)
            throw new Exception("Camera is not initialized in ControllerService");

        var keyboardState = Keyboard.GetState();

        // Move camera
        if (keyboardState.IsKeyDown(Keys.W))
        {
            if(_cameraService.GetCameraPosition().Z+25 > 500)
                return;
            
            _cameraService.UpdateCameraPosition(0f, 0f, CameraSpeed);
            _cameraService.UpdateCameraTarget(0f, 0f, CameraSpeed);
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            // TODO: Move to service ITerrainGenerator and get size terrain info
            if(_cameraService.GetCameraPosition().X < -500)
                return;
            
            _cameraService.UpdateCameraPosition(-CameraSpeed, 0f, 0f);
            _cameraService.UpdateCameraTarget(-CameraSpeed, 0f, 0f);
        }

        if (keyboardState.IsKeyDown(Keys.S))
        {
            if(_cameraService.GetCameraPosition().Z+25 < -500)
                return;
            
            _cameraService.UpdateCameraPosition(0f, 0f, -CameraSpeed);
            _cameraService.UpdateCameraTarget(0f, 0f, -CameraSpeed);
        }

        if (keyboardState.IsKeyDown(Keys.A))
        {
            // TODO: Move to service ITerrainGenerator and get size terrain info
            if(_cameraService.GetCameraPosition().X > 500)
                return;
            
            _cameraService.UpdateCameraPosition(CameraSpeed, 0f, 0f);
            _cameraService.UpdateCameraTarget(CameraSpeed, 0f, 0f);
        }

        // Update the view matrix
        viewMatrix = Matrix.CreateLookAt(
            _cameraService.GetCameraPosition(),
            _cameraService.GetCameraTarget(),
            Vector3.Up
        );
    }
}