using Common.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Data.Interactions;

namespace SquareExpedition.Data.Objects.Blocks;

public sealed class Block : GameObject
{
    public Block(
        Game game,
        BasicEffect basicEffect,
        Matrix projectionMatrix,
        Matrix viewMatrix,
        Matrix worldMatrix,
        Localization? localization,
        bool isEditable,
        Color color)
        : base(game, basicEffect, projectionMatrix, viewMatrix, worldMatrix)
    {
        Localization = localization;
        IsEditable = isEditable;
        BasicColor = color;
        BasicEffect = basicEffect;
    }

    protected override void UpdateLocalization()
    {
        base.UpdateLocalization();

        if (Localization != null)
        {
            var blockRadiusLength = BlockProperties.DefaultBlockSize / 2.0f;

            var dx = blockRadiusLength;
            var dy = blockRadiusLength;
            var dz = blockRadiusLength;

            var center = Localization.GetCoordinates();

            Corners = new[]
            {
                center + new Vector3(-dx, -dy, dz),
                center + new Vector3(dx, -dy, dz),
                center + new Vector3(dx, dy, dz),
                center + new Vector3(-dx, dy, dz),

                center + new Vector3(-dx, -dy, -dz),
                center + new Vector3(dx, -dy, -dz),
                center + new Vector3(dx, dy, -dz),
                center + new Vector3(-dx, dy, -dz)
            };
        }
        else
        {
            Corners = null;
            VertexPositionColors = null;
            VertexBuffer = null;
        }
    }

    protected override void UpdateVertexPositionColor()
    {
        base.UpdateVertexPositionColor();

        if (Corners == null || GraphicsDevice == null)
            return;

        VertexPositionColors = new[]
        {
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
        };

        VertexBuffer = new VertexBuffer(
            GraphicsDevice,
            VertexPositionColor.VertexDeclaration,
            VertexPositionColors.Length,
            BufferUsage.WriteOnly);

        VertexBuffer.SetData(VertexPositionColors);
    }
}