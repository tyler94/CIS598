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

        public override void UpdateEnemy(GameTime gameTime, Vector2 playerPos)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


            Vector2 moveDir = playerPos - Position;
            //moveDir.Normalize();
            //Position += moveDir * speed * dt;
            float distanceToPlayer = Vector2.Distance(playerPos, Position);
            //int state = 0;

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
                // Everything the enemy does during "following"
                Position += moveDir * speed * dt; // move enemy towards player
            }

            var xDiff = Math.Abs(playerPos.X - Position.X);
            var yDiff = Math.Abs(playerPos.Y - Position.Y);

            if (xDiff > yDiff && playerPos.X < Position.X)
            {
                Direction = "W";
            }
            else if (xDiff > yDiff && playerPos.X > Position.X)
            {
                Direction = "E";
            }
            else if (xDiff < yDiff && playerPos.Y < Position.Y)
            {
                Direction = "N";
            }
            else if (xDiff < yDiff && playerPos.Y > Position.Y)
            {
                Direction = "S";
            }

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
            spriteBatch.Draw(_texture, Position, new Rectangle(32, 0, 32, 32), Color.White);
        }
    

    }
}
