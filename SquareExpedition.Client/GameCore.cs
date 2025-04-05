using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Application.Services;
using SquareExpedition.Data.Interactions;

namespace SquareExpedition.Client;

public class GameCore : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    private BasicEffect _basicEffect;
    private VertexPositionColor[] _triangleVertices;
    private VertexBuffer _vertexBuffer;

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
        
        // Create a camera and fetch the CameraService instance
        _cameraService = CameraService.GetInstance(new Camera());
        _controllerService = ControllerService.GetInstance(_cameraService);

        // Initial camera setup
        _cameraService.SetCameraPosition(0f, 0f, -100f);
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

        // Simple triangle vertices
        _triangleVertices = new VertexPositionColor[3];
        _triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
        _triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
        _triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);

        // Vertex buffer
        _vertexBuffer = new VertexBuffer(
            GraphicsDevice, 
            typeof(VertexPositionColor), 
            3, 
            BufferUsage.WriteOnly
        );
        _vertexBuffer.SetData(_triangleVertices);
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

        GraphicsDevice.SetVertexBuffer(_vertexBuffer);

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
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
        }


        base.Draw(gameTime);
    }
}