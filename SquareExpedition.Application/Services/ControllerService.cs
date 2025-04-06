using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SquareExpedition.Application.Services;

public class ControllerService
{
    private ControllerService() {}

    private static ControllerService? _controllerInstance;

    private static readonly object Lock = new();

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
    private bool _orbit;

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
        if (keyboardState.IsKeyDown(Keys.Left))
        {
            _cameraService.UpdateCameraPosition(-1f, 0f, 0f);
            _cameraService.UpdateCameraTarget(-1f, 0f, 0f);
        }

        if (keyboardState.IsKeyDown(Keys.Right))
        {
            _cameraService.UpdateCameraPosition(1f, 0f, 0f);
            _cameraService.UpdateCameraTarget(1f, 0f, 0f);
        }

        if (keyboardState.IsKeyDown(Keys.Up))
        {
            _cameraService.UpdateCameraPosition(0f, -1f, 0f);
            _cameraService.UpdateCameraTarget(0f, -1f, 0f);
        }

        if (keyboardState.IsKeyDown(Keys.Down))
        {
            _cameraService.UpdateCameraPosition(0f, 1f, 0f);
            _cameraService.UpdateCameraTarget(0f, 1f, 0f);
        }

        // Zoom
        if (keyboardState.IsKeyDown(Keys.OemPlus)) _cameraService.UpdateCameraPosition(0f, 0f, 1f);
        if (keyboardState.IsKeyDown(Keys.OemMinus)) _cameraService.UpdateCameraPosition(0f, 0f, -1f);

        // Toggle orbit
        if (keyboardState.IsKeyDown(Keys.Space)) _orbit = !_orbit;

        // Orbit logic
        if (_orbit)
        {
            var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
            var newPosition = Vector3.Transform(_cameraService.GetCameraPosition(), rotationMatrix);
            _cameraService.SetCameraPosition(newPosition.X, newPosition.Y, newPosition.Z);
        }

        // Update the view matrix
        viewMatrix = Matrix.CreateLookAt(
            _cameraService.GetCameraPosition(),
            _cameraService.GetCameraTarget(),
            Vector3.Up
        );
    }
}