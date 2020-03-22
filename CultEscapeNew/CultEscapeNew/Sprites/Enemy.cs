using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;

namespace CultEscapeNew.Sprites
{
    class Enemy : Sprite
    {
        //private Vector2 position;
        protected int health;
        protected float speed;
        protected int radius;
        protected string Direction = "S";

        protected int state;

        public static List<Enemy> enemies = new List<Enemy>();

        public int Health
        {
            get { return health; }
            set { health = value; }

        }


        public int Radius
        {
            get { return radius; }
        }

        public Enemy(Texture2D texture)
            : base(texture)
        {

        }

        public override void UpdateEnemy(GameTime gameTime, Vector2 playerPos, List<Sprite> sprites, List<Sprite> enemies, TiledMap map)
        {
            //float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


            Vector2 moveDir = playerPos - Position;
            //moveDir.Normalize();
            //Position += moveDir * speed * dt;
            float distanceToPlayer = Vector2.Distance(playerPos, Position);
            //int state = 0;

            List<Sprite> allSprites = sprites.Concat(enemies)
                                    .ToList();



            var xDiff = Math.Abs(playerPos.X - Position.X);
            var yDiff = Math.Abs(playerPos.Y - Position.Y);

            if (xDiff > yDiff && playerPos.X < Position.X)
            {
                Direction = "W";
                _velocity.X = -speed;
            }
            else if (xDiff > yDiff && playerPos.X > Position.X)
            {
                Direction = "E";
                _velocity.X = speed;
            }
            else if (xDiff < yDiff && playerPos.Y < Position.Y)
            {
                Direction = "N";
                _velocity.Y = -speed;
            }
            else if (xDiff < yDiff && playerPos.Y > Position.Y)
            {
                Direction = "S";
                _velocity.Y = speed;
            }

            if (state == 0)
            {
                // Everything the enemy does during "idle"
                if (distanceToPlayer < 600)
                {
                    state = 1;
                }
            }
            else if (state == 1)
            {
                foreach (var sprite in allSprites)
                {
                    if (sprite == this)
                        continue;

                    if (sprite.isPlayer)
                    {

                    }
                    else
                    {
                        if ((_velocity.X > 0 && IsTouchingLeft(sprite)) ||
                        (_velocity.X < 0 && IsTouchingRight(sprite)))
                            _velocity.X = 0;

                        if ((_velocity.Y > 0 && IsTouchingTop(sprite)) ||
                            (_velocity.Y < 0 && IsTouchingBottom(sprite)))
                            _velocity.Y = 0;
                    }

                }
                // Everything the enemy does during "following"
            }

            if (this.hasBeenHitL == true)
            {
                health--;
                _velocity.X = -100;
            }
            else if (this.hasBeenHitR == true)
            {
                health--;
                _velocity.X = 100;
            }
            else if (this.hasBeenHitU == true)
            {
                health--;
                _velocity.Y = 100;
            }
            else if (this.hasBeenHitD == true)
            {
                health--;
                _velocity.Y = -100;
            }

            Position += _velocity; // move enemy towards player

            _velocity = Vector2.Zero;
            hasBeenHitL = false;
            hasBeenHitR = false;
            hasBeenHitU = false;
            hasBeenHitD = false;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }



    class Ghost : Enemy
    {
        public Ghost(Texture2D texture) : base(texture)
        {
            speed = 0.5f;
            radius = 45;
            health = 1;
        }
        //state = 0;
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, new Rectangle(0, 0, 32, 32), Color.White);
        }


    }
}
