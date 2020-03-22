using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CultEscape.Core;
using CultEscape.Sprites;
using System;
using System.Threading.Tasks;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;


namespace CultEscape
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        // The tile map
        private TiledMap map;
        // The renderer for the map
        private TiledMapRenderer mapRenderer;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera _camera;

        private List<Sprite> _sprites;

        private List<Sprite> _enemies;

        private Player _player;

        private bool loadingEnemies = false;

        public static int ScreenHeight;
        public static int ScreenWidth;

        //private TiledMap map;
        // The renderer for the map
        //private TiledMapRenderer mapRenderer;

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any Sprites
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //graphics.PreferredBackBufferWidth = 500;  // set this value to the desired width of your window
            //graphics.PreferredBackBufferHeight = 500;   // set this value to the desired height of your window
            //graphics.ApplyChanges();
            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;
            // Load the compiled map
            //map = Content.Load<TiledMap>("tilemap1");
            // Create the map renderer
            //mapRenderer = new TiledMapRenderer(GraphicsDevice);

            base.Initialize();

            map = Content.Load<TiledMap>("startingroom");
            // Create the map renderer
            mapRenderer = new TiledMapRenderer(GraphicsDevice);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _camera = new Camera();
            var playerAnimations = new Dictionary<string, Animation>()
            {
                {"WalkUp", new Animation(Content.Load<Texture2D>("Protagonist64WalkUp"), 3) },
                {"WalkDown", new Animation(Content.Load<Texture2D>("Protagonist64WalkDown"), 3) },
                {"WalkLeft", new Animation(Content.Load<Texture2D>("Protagonist64WalkLeft"), 3) },
                {"WalkRight", new Animation(Content.Load<Texture2D>("Protagonist64WalkRight"), 3) },
                {"AttackUp", new Animation(Content.Load<Texture2D>("Protagonist64AttackUp"), 4) },
                {"AttackDown", new Animation(Content.Load<Texture2D>("Protagonist64AttackDown"), 4) },
                {"AttackLeft", new Animation(Content.Load<Texture2D>("Protagonist64AttackLeft"), 4) },
                {"AttackRight", new Animation(Content.Load<Texture2D>("Protagonist64AttackRight"), 4) }
            };
            /*var playerAnimations = new Dictionary<string, Animation>()
            {
                {"WalkUp", new Animation(Content.Load<Texture2D>("TestPlayer64"), 3) },
                {"WalkDown", new Animation(Content.Load<Texture2D>("TestPlayer64"), 3) },
                {"WalkLeft", new Animation(Content.Load<Texture2D>("TestPlayer64"), 3) },
                {"WalkRight", new Animation(Content.Load<Texture2D>("TestPlayer64"), 3) }
            };*/
            _player = new Player(playerAnimations);
            _player.isPlayer = true;

            //the order things are in the _sprites list determines the order in which they are drawn
            _sprites = new List<Sprite>()
            {
                _player
            };

            _enemies = new List<Sprite>()
            {
            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mapRenderer.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Load enemies when entering a new room
            if (_enemies.Count == 0 && !loadingEnemies && _player.Position.X != 0)
            {
                Random random = new Random();
                int randx, randy;
                    this.loadingEnemies = true;
                //System.Threading.Thread.Sleep(1000);
                //this.waves--;
                //this.currentwave++;
                int enemiesToSpawn = 3;
                        for (var i = 0; i < enemiesToSpawn; i++)
                        {
                            randx = random.Next(0, ScreenWidth);
                            randy = random.Next(0, ScreenHeight);
                            do
                            {
                                randx = random.Next(0, ScreenWidth);
                                randy = random.Next(0, ScreenHeight);
                            }
                            while (randx == _player.Position.X && randy == _player.Position.Y);
                    //Ghost newGhost = new Ghost(Content.Load<Texture2D>("ghost"));
                    Ghost newGhost = new Ghost(Content.Load<Texture2D>("ghost"));
                    newGhost.Position = new Vector2(randx, randy);
                            _enemies.Add(newGhost);
                        }
                        this.loadingEnemies = false;
                
            }
            else
            {

                foreach (var sprite in _sprites)
                    sprite.Update(gameTime, _enemies);

                foreach (var sprite in _enemies)
                {
                    sprite.UpdateEnemy(gameTime, _player.Position, _sprites, _enemies);
                } 

                _camera.Follow(_player);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //mapRenderer.Draw(map, _camera.Transform);

            // TODO: Add your drawing code here
            spriteBatch.Begin(transformMatrix: _camera.Transform);

            mapRenderer.Draw(_camera.Transform);

            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);

            foreach (var sprite in _enemies)
                sprite.Draw(gameTime, spriteBatch);
            /*spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
                );*/
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
