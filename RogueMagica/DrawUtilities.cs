using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace RogueMagica;

public static class DrawUtilities
{
    public static void DrawField(IMap _map, SpriteBatch _spriteBatch, Texture2D _floor, Texture2D _wall, float _scale)
    {
        int sizeOfSprites = 64;
        foreach (var cell in _map.GetAllCells().Cast<Cell>())
        {
            if (!cell.IsInFov)
            {
                continue;
            }

            var position = new Vector2(cell.X * sizeOfSprites * _scale, cell.Y * sizeOfSprites * _scale);
            var destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, (int)(sizeOfSprites * _scale), (int)(sizeOfSprites * _scale));

            if (cell.IsWalkable)
            {
                _spriteBatch.Draw(_floor, destinationRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            else
            {
                _spriteBatch.Draw(_wall, destinationRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
    }
}
