using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;

namespace CultEscapeNew
{
    public class SolidTile
    {
        public Vector2 _position;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
            }
        }

    }
}
