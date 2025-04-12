using System;
using System.Collections.Generic;
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
    
    private VertexBuffer _blockLinesVertexBuffer;
    private int _blockLinesVertexCount;
    
    //private VertexBuffer _blockPointsVertexBuffer;
    //private int _blockPointsVertexCount;

    private CameraService _cameraService;
    private ControllerService _controllerService;

    public GameCore()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        var worldGeneratorService = new WorldGeneratorService();
        var world = worldGeneratorService.GenerateNewWorld("Test world", TerrainSize.Small);
        
        var vertices = new List<VertexPositionColor>();

        if (world.Area?.Localizations == null)
            throw new Exception("Localization is not generated for area");

        foreach (var localization in world.Area.Localizations)
        {
            var gameObject = localization.GetGameObject();
            // For each bounding box, get its 8 corners
            var corners = gameObject?.GetCorners();
            
            void AddLine(int start, int end)
            {
                if(corners == null)
                    return;
                
                vertices.Add(new VertexPositionColor(corners[start], Color.Green));
                vertices.Add(new VertexPositionColor(corners[end], Color.Green));
            }

            AddLine(0, 1);
            AddLine(1, 2);
            AddLine(2, 3);
            AddLine(3, 0);
            AddLine(4, 5);
            AddLine(5, 6);
            AddLine(6, 7);
            AddLine(7, 4);
            AddLine(0, 4);
            AddLine(1, 5);
            AddLine(2, 6);
            AddLine(3, 7);
        }
        
        _blockLinesVertexCount = vertices.Count;
        _blockLinesVertexBuffer = new VertexBuffer(
            GraphicsDevice,
            typeof(VertexPositionColor),
            _blockLinesVertexCount,
            BufferUsage.WriteOnly
        );
        _blockLinesVertexBuffer.SetData(vertices.ToArray());

        
        // Create a camera and fetch the CameraService instance
        _cameraService = CameraService.GetInstance(new Camera());
        _controllerService = ControllerService.GetInstance(_cameraService);

        // Initial camera setup
        _cameraService.SetCameraPosition(0f, 5f, -5f);
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
        
        GraphicsDevice.SetVertexBuffer(_blockLinesVertexBuffer);

        // Allow front/back face drawing
        RasterizerState rasterizerState = new RasterizerState
        {
            CullMode = CullMode.None
        };
        GraphicsDevice.RasterizerState = rasterizerState;

        // Draw triangle
        foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, _blockLinesVertexCount / 2);
        }
        
        base.Draw(gameTime);
    }
}