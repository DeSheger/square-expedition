using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects;

public class GameObject : DrawableGameComponent
{
    private Localization? _localization;
    protected BasicEffect BasicEffect { get; set; }
    protected VertexBuffer? VertexBuffer { get; set; }
    protected VertexPositionColor[]? VertexPositionColors { get; set; }
    
    protected Matrix ProjectionMatrix { get; set; }
    protected Matrix ViewMatrix { get; set; }
    protected Matrix WorldMatrix { get; set; }

    public Guid? Id { get; set; }
    public bool IsEditable { get; set; }
    public Color BasicColor { get; set; }

    public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
    public ICollection<Physic> Physics { get; set; } = new List<Physic>();

    public Vector3[]? Corners { get; set; }

    public Localization? Localization
    {
        get => _localization;
        set
        {
            _localization = value;
            UpdateLocalization();
            UpdateVertexPositionColor();
        }
    }

    public GameObject(Game game, BasicEffect basicEffect, Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
        : base(game)
    {
        Id = Guid.NewGuid();
        BasicEffect = basicEffect;

        ProjectionMatrix = projectionMatrix;
        ViewMatrix = viewMatrix;
        WorldMatrix = worldMatrix;
        
        IsEditable = true;
        BasicColor = Color.White;
    }

    protected virtual void UpdateLocalization()
    {
    }

    protected virtual void UpdateVertexPositionColor()
    {
    }

    public override void Draw(GameTime gameTime)
    {
        if (GraphicsDevice == null || VertexBuffer == null || Corners == null)
        {
            base.Draw(gameTime);
            return;
        }
        
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        BasicEffect.Projection = ProjectionMatrix;
        BasicEffect.View = ViewMatrix;
        BasicEffect.World = WorldMatrix;

        GraphicsDevice.SetVertexBuffer(VertexBuffer);
        
        RasterizerState rasterizerState = new RasterizerState
        {
            CullMode = CullMode.None
        };
        GraphicsDevice.RasterizerState = rasterizerState;
        
        foreach (var pass in BasicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();

            for (var i = 0; i < 6; i++) GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 4 * i, 2);
        }

        base.Draw(gameTime);
    }

    public Vector3[]? GetCorners()
    {
        return Corners?.ToArray();
    }
}