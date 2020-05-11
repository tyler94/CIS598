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
    class PowerUp : Sprite
    {
        public bool hasBeenTouched = false;

        public PowerUp(Texture2D texture)
            : base(texture)
        {

        }

        public void Update(GameTime gameTime, Sprite player)
        {
            //float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
            if (IsTouchingLeft(player) || IsTouchingRight(player) || IsTouchingTop(player) || IsTouchingBottom(player))
            {
                hasBeenTouched = true;
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, new Rectangle(0, 0, 32, 32), Color.White);
        }

    }
}
