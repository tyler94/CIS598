using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultEscape.Sprites
{
    public class Player : Sprite
    {//private Vector2 position;
        protected int health;
        protected float speed;
        protected string Direction = "S";

        protected int state;

        public int Health
        {
            get { return health; }
            set { health = value; }

        }

        public Player(Dictionary<string, Animation> animations) : base(animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            speed = 3f;
            health = 4;
        }

        public Player(Texture2D texture)
            : base(texture)
        {
            speed = 3f;
            health = 4;
        }

        public override void Update(GameTime gameTime)
        {
            var velocity = new Vector2();
            var speed = 3f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                velocity.Y = -speed;
                Direction = "N";
                _animationManager.Play(_animations["WalkUp"]);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                velocity.Y = speed;
                Direction = "S";
                _animationManager.Play(_animations["WalkDown"]);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -speed;
                Direction = "W";
                _animationManager.Play(_animations["WalkLeft"]);
            }  
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = speed;
                Direction = "E";
                _animationManager.Play(_animations["WalkRight"]);
            }
            else
            {
                _animationManager.Stop();

            }
                

            Position += velocity;
            _animationManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch);
            /*switch (Direction)
            {
                case "S":
                    spriteBatch.Draw(_texture, Position, new Rectangle(32, 0, 32, 32), Color.White);
                    break;
            }*/
                
        }
    }
}
