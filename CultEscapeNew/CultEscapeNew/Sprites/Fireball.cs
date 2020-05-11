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
    class Fireball : Sprite
    {
        protected float speed = 2f;
        protected string Direction = "S";


        public bool hasCollided = false;

        public Fireball(Texture2D texture, string direction, Vector2 position)
            : base(texture)
        {
            this.Direction = direction;
            this.Position = position;
        }

        public void Update(GameTime gameTime, List<SolidTile> tiles, List<Sprite> sprites)
        {
            if (Direction == "N")
            {
                _velocity.Y = -speed;
            }
            else if (Direction == "S")
            {
                _velocity.Y = speed;
            }
            else if (Direction == "W")
            {
                _velocity.X = -speed;
            }
            else if (Direction == "E")
            {
                _velocity.X = speed;
            }
            

            Position += _velocity; // move enemy towards player

            foreach (var sprite in sprites)
            {
                if (IsTouchingLeft(sprite))
                {
                    sprite.hasBeenHitR = true;
                    hasCollided = true;
                }

                if (IsTouchingRight(sprite))
                {
                    sprite.hasBeenHitL = true;
                    hasCollided = true;
                }

                if (IsTouchingTop(sprite))
                {
                    sprite.hasBeenHitU = true;
                    hasCollided = true;
                }

                if (IsTouchingBottomAttack(sprite))
                {
                    sprite.hasBeenHitD = true;
                    hasCollided = true;
                }
            }

            foreach (var tile in tiles)
            {
                if ((_velocity.X > 0 && IsBlockedLeft(tile)) ||
                        (_velocity.X < 0 && IsBlockedRight(tile)))
                    hasCollided = true;

                if ((_velocity.Y > 0 && IsBlockedTop(tile)) ||
                    (_velocity.Y < 0 && IsBlockedBottom(tile)))
                    hasCollided = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, new Rectangle(0, 0, 32, 32), Color.White);
        }
    }
}
