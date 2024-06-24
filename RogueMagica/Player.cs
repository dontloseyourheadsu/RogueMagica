using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSharp;

namespace RogueMagica;

public class Player
{
    public int X { get; set; }
    public int Y { get; set; }
    public float Scale { get; set; }
    public Texture2D Sprite { get; set; }
    public Microsoft.Xna.Framework.Rectangle DestinationRectangle { get; set; }

    private readonly int speed = 4;
    private int speedTick = 0;
    
    public void Draw(SpriteBatch spriteBatch)
    {
        float multiplier = Scale * Sprite.Width;
        DestinationRectangle = new Microsoft.Xna.Framework.Rectangle((int)(X * multiplier), (int)(Y * multiplier), (int)multiplier, (int)multiplier);
        spriteBatch.Draw(Sprite, DestinationRectangle, Color.White);
    }

    public void Move(IMap map)
    {
        KeyboardState state = Keyboard.GetState();

        int dx = 0, dy = 0;

        if (state.IsKeyDown(Keys.W)) dy = -1;
        else if (state.IsKeyDown(Keys.S)) dy = 1;
        else if (state.IsKeyDown(Keys.A)) dx = -1;
        else if (state.IsKeyDown(Keys.D)) dx = 1;
        else return;
        
        if (!map.GetCell(X+dx, Y+dy).IsWalkable)
        {
            return;
        }

        speedTick++;
        if (speedTick < speed) return;
        speedTick = 0;

        X += dx;
        Y += dy;

        UpdatePlayerFieldOfView(map);
    }

    public void UpdatePlayerFieldOfView(IMap map)
    {
        map.ComputeFov(X, Y, 30, true);
        foreach (Cell cell in map.GetAllCells().Cast<Cell>())
        {
            if (map.IsInFov(cell.X, cell.Y))
            {
                map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }
    }
}