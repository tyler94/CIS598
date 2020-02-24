using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CultEscape.Core;
using CultEscape.Sprites;
//using MonoGame.Extended.Graphics;
//using MonoGame.Extended.Tiled;
//using MonoGame.Extended.Tiled.Renderers;

namespace CultEscape
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera _camera;

        private List<Component> _components;

        private List<Component> _enemies;

        private Player _player;

        private Ghost _testenemy;

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
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;
            // Load the compiled map
            //map = Content.Load<TiledMap>("tilemap1");
            // Create the map renderer
            //mapRenderer = new TiledMapRenderer(GraphicsDevice);

            base.Initialize();
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
                {"WalkRight", new Animation(Content.Load<Texture2D>("Protagonist64WalkRight"), 3) }
            };
            _player = new Player(playerAnimations);
            _testenemy = new Ghost(Content.Load<Texture2D>("ghost"));

            //the order things are in the _components list determines the order in which they are drawn
            _components = new List<Component>()
            {
                _player
            };

            _enemies = new List<Component>()
            {
                _testenemy
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //mapRenderer.Update(gameTime);

            foreach (var component in _components)
                component.Update(gameTime);

            foreach (var component in _enemies)
                component.UpdateEnemy(gameTime, _player.Position);

            //_testenemy.Update(gameTime, _player.Position);
            _camera.Follow(_player);

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
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            foreach (var component in _enemies)
                component.Draw(gameTime, spriteBatch);
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
