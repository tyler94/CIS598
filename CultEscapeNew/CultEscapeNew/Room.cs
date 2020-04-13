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
    public class Room {
        public Vector2 gridPos;
        public int type;
        public bool doorTop, doorBot, doorLeft, doorRight;
        public TiledMap map;
        public bool cleared = false;

        public Room(Vector2 _gridPos, int _type) {
            gridPos = _gridPos;
            type = _type;
        }
    }
}