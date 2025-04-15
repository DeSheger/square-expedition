using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Application.Services;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Terrains;

namespace SquareExpedition.Client;

public class GameCore : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    private BasicEffect _basicEffect;

    private CameraService _cameraService;
    private ControllerService _controllerService;

    public GameCore()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.GraphicsProfile = GraphicsProfile.Reach;
        _graphics.PreferredBackBufferWidth = 2000;
        _graphics.PreferredBackBufferHeight = 1000;
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        // Create a camera and fetch the CameraService instance
        _cameraService = CameraService.GetInstance(new Camera());
        _controllerService = ControllerService.GetInstance(_cameraService);

        // Initial camera setup
        _cameraService.SetCameraPosition(0f, 20f, -20f);
        _cameraService.SetCameraTarget(0f, 0f, 0f);

        // Projection matrix
        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45f),
            GraphicsDevice.DisplayMode.AspectRatio,
            1f,
            1000f
        );

        // View matrix
        _viewMatrix = Matrix.CreateLookAt(
            _cameraService.GetCameraPosition(),
            _cameraService.GetCameraTarget(),
            Vector3.Up
        );

        // World matrix
        _worldMatrix = Matrix.CreateWorld(
            _cameraService.GetCameraTarget(),
            Vector3.Forward,
            Vector3.Up
        );

        // BasicEffect
        _basicEffect = new BasicEffect(GraphicsDevice)
        {
            Alpha = 1f,
            VertexColorEnabled = true,
            LightingEnabled = false
        };
        
        var worldGeneratorService = new WorldGeneratorService();
        var world = worldGeneratorService.GenerateNewWorld("Test world", TerrainSize.Small, this, _basicEffect, _projectionMatrix, _viewMatrix, _worldMatrix);

        if (world.Area?.Localizations == null)
            throw new Exception("Localization is not generated for area");

        foreach (var loc in world.Area.Localizations)
        {
            try
            {
                var gameObj = loc.GetGameObject();
                gameObj?.Localization?.GetCoordinates();
                if(gameObj != null)
                    Components.Add(loc.GetGameObject());   
            }
            catch
            {
                Console.WriteLine(Components.Count);
            }
        }
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        _controllerService.HandleInput(gameTime, this, ref _viewMatrix);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _basicEffect.Projection = _projectionMatrix;
        _basicEffect.View = _viewMatrix;
        _basicEffect.World = _worldMatrix;

        // Allow front/back face drawing
        RasterizerState rasterizerState = new RasterizerState
        {
            CullMode = CullMode.None
        };
        GraphicsDevice.RasterizerState = rasterizerState;
        
        base.Draw(gameTime);
    }
}