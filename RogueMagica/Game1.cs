using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.Random;

namespace RogueMagica;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly float _scale = 0.25f;

    private Texture2D _floor;
    private Texture2D _wall;

    private Player _player;

    private IMap _map;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        IMapCreationStrategy<Map> mapCreationStrategy =
            new RandomRoomsMapCreationStrategy<Map>(width: 50, height: 30, maxRooms: 100, roomMaxSize: 7, roomMinSize: 3);
        
        _map = Map.Create(mapCreationStrategy);

        Cell startingCell = GetRandomEmptyCell();
        _player = new Player
        {
            X = startingCell.X,
            Y = startingCell.Y,
            Scale = _scale,
            Sprite = Content.Load<Texture2D>("player")
        };
        _player.UpdatePlayerFieldOfView(_map);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _floor = Content.Load<Texture2D>("floor");
        _wall = Content.Load<Texture2D>("wall");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _player.Move(_map);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

        DrawUtilities.DrawField(_map, _spriteBatch, _floor, _wall, _scale);

        _player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private Cell GetRandomEmptyCell()
    {
        IRandom random = new DotNetRandom();

        while (true)
        {
            int x = random.Next(49);
            int y = random.Next(29);
            if (_map.IsWalkable(x, y))
            {
                return (Cell)_map.GetCell(x, y);
            }
        }
    }
}
