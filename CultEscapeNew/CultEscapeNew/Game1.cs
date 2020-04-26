using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CultEscapeNew.Core;
using CultEscapeNew.Sprites;
using System;
using System.Threading.Tasks;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;


namespace CultEscapeNew
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        // The tile map
        private TiledMap map;
        private List<TiledMap> level1maps;
        private List<TiledMap> doors;
        private List<TiledMap> activeDoors;
        // The renderer for the map
        private TiledMapRenderer mapRenderer;
        private TiledMapRenderer leftDoorRenderer;
        private TiledMapRenderer rightDoorRenderer;
        private TiledMapRenderer topDoorRenderer;
        private TiledMapRenderer bottomDoorRenderer;
        private List<TiledMapRenderer> activeDoorRenderers;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera _camera;

        private TextComponent _healthDisplay;

        private TextComponent _roomCountDisplay;

        private List<Sprite> _sprites;

        private List<Sprite> _enemies;

        private LevelGeneration _level;

        private List<SolidTile> _tiles;

        private Player _player;

        private SpriteFont gameFont;

        private Room currentRoom;

        private Vector2 currentRoomPos;

        private int roomsRemaining;

        private bool loadingEnemies = false;

        private bool start = false;

        public static int ScreenHeight;
        public static int ScreenWidth;

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
            _level = new LevelGeneration();
            //graphics.PreferredBackBufferWidth = 500;  // set this value to the desired width of your window
            //graphics.PreferredBackBufferHeight = 500;   // set this value to the desired height of your window
            //graphics.ApplyChanges();
            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;

            base.Initialize();

            level1maps = new List<TiledMap>()
            {
                Content.Load<TiledMap>("startingroom"),
                Content.Load<TiledMap>("room1"),
                Content.Load<TiledMap>("room2"),
                Content.Load<TiledMap>("room3"),
                Content.Load<TiledMap>("room4"),
                Content.Load<TiledMap>("room5"),
            };

            doors = new List<TiledMap>()
            {
                Content.Load<TiledMap>("doorleft"),
                Content.Load<TiledMap>("doorright"),
                Content.Load<TiledMap>("doortop"),
                Content.Load<TiledMap>("doorbottom")
            };

            ResetRooms();

            
            CreateEnemies();
            checkDoors();
            //setTiles();
            _healthDisplay = new TextComponent("HP", new Vector2(5, 0), spriteBatch, gameFont, GraphicsDevice);
            _healthDisplay.Update(_player.health.ToString(), Color.SpringGreen);

            Vector2 stringDimensions = gameFont.MeasureString("HP" + ": " + _player.health.ToString());

            _roomCountDisplay = new TextComponent("Remaining Rooms", new Vector2((int)stringDimensions.X + 20, 0), spriteBatch, gameFont, GraphicsDevice);
            _roomCountDisplay.Update(roomsRemaining.ToString(), Color.SpringGreen);
        }

        protected void setTiles()
        {
            _tiles = new List<SolidTile>()
            {
            };

            foreach (var tileLayer in currentRoom.map.TileLayers)
            {

                for (var x = 0; x < tileLayer.Width; x++)
                {
                    for (var y = 0; y < tileLayer.Height; y++)
                    {
                        var tile = tileLayer.GetTile((ushort)x, (ushort)y);
                        var tileWidth = currentRoom.map.TileWidth;
                        var tileHeight = currentRoom.map.TileHeight;

                        SolidTile thisTile = new SolidTile();
                        thisTile._position.X = x * tileWidth;
                        thisTile._position.Y = y * tileHeight;


                        if (tile.GlobalIdentifier == 19)
                        {
                            _tiles.Add(thisTile);
                        }
                        else if (tile.GlobalIdentifier == 14)
                        {
                            _tiles.Add(thisTile);
                        }
                        else if (tile.GlobalIdentifier == 15)
                        {
                            _tiles.Add(thisTile);
                        }
                        else if (tile.GlobalIdentifier == 2)
                        {
                            _tiles.Add(thisTile);
                        }
                    }
                }
            }
            foreach (var door in activeDoors)
            {
                foreach (var tileLayer in door.TileLayers)
                {
                    for (var x = 0; x < tileLayer.Width; x++)
                    {
                        for (var y = 0; y < tileLayer.Height; y++)
                        {
                            var tile = tileLayer.GetTile((ushort)x, (ushort)y);
                            var tileWidth = currentRoom.map.TileWidth;
                            var tileHeight = currentRoom.map.TileHeight;

                            SolidTile thisTile = new SolidTile();
                            thisTile._position.X = x * tileWidth;
                            thisTile._position.Y = y * tileHeight;


                            if (tile.GlobalIdentifier == 19)
                            {
                                _tiles.Add(thisTile);
                            }
                            else if (tile.GlobalIdentifier == 14)
                            {
                                _tiles.Add(thisTile);
                            }
                            else if (tile.GlobalIdentifier == 15)
                            {
                                _tiles.Add(thisTile);
                            }
                            else if (tile.GlobalIdentifier == 2)
                            {
                                _tiles.Add(thisTile);
                            }
                        }
                    }
                }
            }
        }

        protected void checkDoors()
        {
            activeDoors = new List<TiledMap>()
                {
                    doors[0],
                    doors[1],
                    doors[2],
                    doors[3]
                };
            activeDoorRenderers = new List<TiledMapRenderer>()
                {
                    leftDoorRenderer,
                    rightDoorRenderer,
                    topDoorRenderer,
                    bottomDoorRenderer
                };
            if (_enemies.Count > 0)
            {
                
            }
            else
            {
                if (currentRoom.doorLeft)
                {
                    activeDoors.Remove(doors[0]);
                    activeDoorRenderers.Remove(leftDoorRenderer);
                }

                if (currentRoom.doorRight)
                {
                    activeDoors.Remove(doors[1]);
                    activeDoorRenderers.Remove(rightDoorRenderer);
                }

                if (currentRoom.doorTop)
                {
                    activeDoors.Remove(doors[2]);
                    activeDoorRenderers.Remove(topDoorRenderer);
                }

                if (currentRoom.doorBot)
                {
                    activeDoors.Remove(doors[3]);
                    activeDoorRenderers.Remove(bottomDoorRenderer);
                }
            }
                
            setTiles();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameFont = Content.Load<SpriteFont>("GameOver");
            
            _camera = new Camera();
            ResetPlayer();

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

        protected void ResetPlayer()
        {
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
            _player = new Player(playerAnimations);
            _player.isPlayer = true;
            _player._position.X = 370;
            _player._position.Y = 205;
            _camera.Follow(_player);
        }

        protected void ResetRooms()
        {
            activeDoorRenderers = new List<TiledMapRenderer>()
            {
            };
            activeDoors = new List<TiledMap>()
            {
            };

            currentRoomPos = new Vector2(4, 4);

            _level.Start();

            _level.rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y].map = level1maps[0];

            currentRoom = _level.rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y];
            Random random = new Random();
            roomsRemaining = 1;

            for (int i = 0; i < _level.rooms.GetLength(0); i++)
            {
                for (int j = 0; j < _level.rooms.GetLength(1); j++)
                {
                    if (i == 4 && j == 4)
                    {
                        //do nothing
                    }
                    else
                    {
                        if (_level.rooms[i, j] != null)
                        {
                            TiledMap tempMap = level1maps[(int)random.Next(1, level1maps.Count)];
                            _level.rooms[i, j].map = tempMap;
                            roomsRemaining++;
                        }

                    }

                }
            }

            /*Tileset local identity = tilemap global identity - 1*/
            mapRenderer = new TiledMapRenderer(GraphicsDevice, currentRoom.map);
            leftDoorRenderer = new TiledMapRenderer(GraphicsDevice, doors[0]);
            rightDoorRenderer = new TiledMapRenderer(GraphicsDevice, doors[1]);
            topDoorRenderer = new TiledMapRenderer(GraphicsDevice, doors[2]);
            bottomDoorRenderer = new TiledMapRenderer(GraphicsDevice, doors[3]);
        }

        protected void CreateEnemies()
        {
            _enemies = new List<Sprite>()
            {
            };
            Random random = new Random();
            int randx, randy;
            this.loadingEnemies = true;
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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!start)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    start = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    Exit();
                }
            }
            else if(_player.Health <= 0 || roomsRemaining == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    ResetRooms();
                    ResetPlayer();
                    CreateEnemies();
                    _sprites = new List<Sprite>()
                    {
                        _player
                    };
                    checkDoors();
                    

                    
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    Exit();
                }
            }
            else
            {
                mapRenderer.Update(gameTime);
                

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                //Load enemies when entering a new room
                if (_enemies.Count == 0 && !loadingEnemies && _player.Position.X != 0 && currentRoom.cleared == false)
                {
                    currentRoom.cleared = true;
                    roomsRemaining--;
                    checkDoors();
                    //CreateEnemies();

                }
                    foreach (var renderer in activeDoorRenderers.ToArray())
                    {
                        renderer.Update(gameTime);
                    }

                    foreach (var sprite in _sprites)
                        sprite.Update(gameTime, _enemies, _tiles);

                    foreach (var sprite in _enemies.ToArray())
                    {
                        if (sprite.health <= 0)
                        {
                            _enemies.Remove(sprite);
                        }
                        else
                        {
                            sprite.UpdateEnemy(gameTime, _player.Position, _sprites, _enemies, _tiles);

                        }
                    }
                    //_camera.Follow(_player);

                checkRoomTransition();
                _healthDisplay.Update(_player.health.ToString(), Color.SpringGreen);
                _roomCountDisplay.Update(roomsRemaining.ToString(), Color.SpringGreen);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!start)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
                spriteBatch.DrawString(gameFont, "WELCOME", new Vector2(360, 10), Color.SpringGreen);
                spriteBatch.DrawString(gameFont, "Carrie awakens in a strange place", new Vector2(295, 150), Color.MediumPurple);
                spriteBatch.DrawString(gameFont, "Cursed Jack-o'-lanterns observe her as she gets her bearings", new Vector2(220, 170), Color.MediumPurple);
                spriteBatch.DrawString(gameFont, "Suddenly she is attacked by the damned souls of an old forgotten cult", new Vector2(200, 190), Color.MediumPurple);
                spriteBatch.DrawString(gameFont, "Help Carrie escape by banishing the angry spirits", new Vector2(250, 210), Color.MediumPurple);
                spriteBatch.DrawString(gameFont, "Use the arrow keys to move and attack with 'Space'", new Vector2(250, 400), Color.MediumPurple);
                spriteBatch.DrawString(gameFont, "Press 'Enter' to Start or 'Q' to quit", new Vector2(295, 420), Color.MediumPurple);
                spriteBatch.End();
            }
            else if (_player.Health <= 0)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
                spriteBatch.DrawString(gameFont, "YOU DIED!", new Vector2(360, 180), Color.Red);
                spriteBatch.DrawString(gameFont, "Press 'R' to Retry or 'Q' to quit", new Vector2(295, 205), Color.Red);
                spriteBatch.End();
            }
            else if (roomsRemaining == 0)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
                spriteBatch.DrawString(gameFont, "CONGRATULATIONS!", new Vector2(320, 180), Color.SpringGreen);
                spriteBatch.DrawString(gameFont, "You banished the disgruntled spirits!", new Vector2(270, 205), Color.MediumPurple);
                spriteBatch.DrawString(gameFont, "Press 'R' to Reset or 'Q' to quit", new Vector2(295, 230), Color.MediumPurple);
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                //mapRenderer.Draw(map, _camera.Transform);

                // TODO: Add your drawing code here
                spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);

                mapRenderer.Draw(_camera.Transform);
                foreach (var renderer in activeDoorRenderers.ToArray())
                {
                    renderer.Draw(_camera.Transform);
                }

                foreach (var sprite in _sprites)
                    sprite.Draw(gameTime, spriteBatch);

                foreach (var sprite in _enemies)
                    sprite.Draw(gameTime, spriteBatch);
                _healthDisplay.Draw();
                _roomCountDisplay.Draw();
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        protected void checkRoomTransition()
        {
            if(_player.Position.X > ScreenWidth)
            {
                currentRoomPos.X += 1;
                changeRoom("right");
            }
            else if(_player.Position.X < 0)
            {
                currentRoomPos.X -= 1;
                changeRoom("left");
            }
            else if(_player.Position.Y > ScreenHeight)
            {
                currentRoomPos.Y -= 1;
                changeRoom("bottom");
            }
            else if(_player.Position.Y < 0)
            {
                currentRoomPos.Y += 1;
                changeRoom("top");
            }
        }

        protected void changeRoom(string doorEntered)
        {
            currentRoom = _level.rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y];
            switch (doorEntered)
            {
                case "right":
                    _player._position.X = 15;
                    break;
                case "left":
                    _player._position.X = ScreenWidth - 100;
                    break;
                case "top":
                    _player._position.Y = ScreenHeight - 100;
                    break;
                case "bottom":
                    _player._position.Y = 15;
                    break;

            }

            /*Tileset local identity = tilemap global identity - 1*/
            mapRenderer = new TiledMapRenderer(GraphicsDevice, currentRoom.map);
            if (!currentRoom.cleared)
            {
                CreateEnemies();
            }
            
            checkDoors();
            //setTiles();
        }
    }
}
