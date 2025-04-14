using Common.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Data.Interactions;

namespace SquareExpedition.Data.Objects.Blocks;

public class Block : GameObject
{
    private readonly GraphicsDevice? _gd;

    private readonly BasicEffect _basicEffect;

    private VertexBuffer? _vertexBuffer;

    private VertexPositionColor[]? _vertexPositionColors;

    public Block(Game game,
        BasicEffect effect,
        GraphicsDevice graphicDevice,
        Localization? localization,
        bool isEditable, Color color)
        : base(game)
    {
        _localization = localization;
        _basicEffect = effect;
        _gd = graphicDevice;
        Id = Guid.NewGuid();
        IsEditable = isEditable;
        BasicColor = color;
    }

    private Color BasicColor { get; set; }
    
    private Localization? _localization;

    public new Localization? Localization
    {
        get => _localization;
        set
        {
            _localization = value;
            UpdateLocalization();
            UpdateVertexPositionColor();
        }
    }

    private void UpdateLocalization()
    {
        if (Localization != null)
        {
            var blockRadiusLength = BlockProperties.DefaultBlockSize / 2.0f;

            var dx = blockRadiusLength;
            var dy = blockRadiusLength;
            var dz = blockRadiusLength;

            var center = Localization.GetCoordinates();

            Corners =
            [
                center + new Vector3(-dx, -dy, dz),
                center + new Vector3(dx, -dy, dz),
                center + new Vector3(dx, dy, dz),
                center + new Vector3(-dx, dy, dz),

                center + new Vector3(-dx, -dy, -dz),
                center + new Vector3(dx, -dy, -dz),
                center + new Vector3(dx, dy, -dz),
                center + new Vector3(-dx, dy, -dz)
            ];
        }
        else
        {
            Corners = null;
            _vertexPositionColors = null;
            _vertexBuffer = null;
        }
    }

    private void UpdateVertexPositionColor()
    {
        if(Corners == null)
            return;
        
        _vertexPositionColors =
        [
            new VertexPositionColor(Corners[3], BasicColor),
            new VertexPositionColor(Corners[2], BasicColor),
            new VertexPositionColor(Corners[0], BasicColor),
            new VertexPositionColor(Corners[1], BasicColor),

            new VertexPositionColor(Corners[7], BasicColor),
            new VertexPositionColor(Corners[4], BasicColor),
            new VertexPositionColor(Corners[6], BasicColor),
            new VertexPositionColor(Corners[5], BasicColor),
            
            new VertexPositionColor(Corners[3], BasicColor),
            new VertexPositionColor(Corners[7], BasicColor),
            new VertexPositionColor(Corners[2], BasicColor),
            new VertexPositionColor(Corners[6], BasicColor),

            new VertexPositionColor(Corners[0], BasicColor),
            new VertexPositionColor(Corners[1], BasicColor),
            new VertexPositionColor(Corners[4], BasicColor),
            new VertexPositionColor(Corners[5], BasicColor),
            
            new VertexPositionColor(Corners[3], BasicColor),
            new VertexPositionColor(Corners[0], BasicColor),
            new VertexPositionColor(Corners[7], BasicColor),
            new VertexPositionColor(Corners[4], BasicColor),

            new VertexPositionColor(Corners[1], BasicColor),
            new VertexPositionColor(Corners[2], BasicColor),
            new VertexPositionColor(Corners[5], BasicColor),
            new VertexPositionColor(Corners[6], BasicColor)
        ];

        _vertexBuffer = new VertexBuffer(
            _gd,
            VertexPositionColor.VertexDeclaration,
            _vertexPositionColors.Length,
            BufferUsage.WriteOnly);

        _vertexBuffer.SetData(_vertexPositionColors);
    }

    public override void Draw(GameTime gameTime)
    {
        if (_gd == null)
            throw new Exception($"Graphic Device is null for Block with Id {Id}");
        
        if(_vertexBuffer == null)
            return;
        
        if(Corners == null)
            return;

        _gd.SetVertexBuffer(_vertexBuffer);
        foreach (var pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            Console.WriteLine("test");

            for (var i = 0; i < 6; ++i)
                _gd.DrawPrimitives(PrimitiveType.TriangleStrip, 4 * i, 2);
        }

        base.Draw(gameTime);
    }
}