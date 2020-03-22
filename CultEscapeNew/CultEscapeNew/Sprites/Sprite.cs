using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;

namespace CultEscapeNew.Sprites
{
    public class Sprite
    {
        protected Texture2D _texture;
        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        public Vector2 _position;
        public Vector2 _velocity;


        public bool hasBeenHitL;
        public bool hasBeenHitR;
        public bool hasBeenHitU;
        public bool hasBeenHitD;

        public bool isPlayer = false;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                if (_texture != null)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
                }
                else
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, Position, Color.White);
            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(spriteBatch);
            }
            else throw new Exception("error with sprite draw method");

        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites, TiledMap map)
        {
            _animationManager.Update(gameTime);

        }

        public virtual void UpdateEnemy(GameTime gameTime, Vector2 playerPos, List<Sprite> sprites, List<Sprite> enemies, TiledMap map)
        {

        }

        protected bool IsTouchingLeft(Sprite sprite)
        {
            return Rectangle.Right + _velocity.X > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Left &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {
            return Rectangle.Left + _velocity.X < sprite.Rectangle.Right &&
              Rectangle.Right > sprite.Rectangle.Right &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {
            return Rectangle.Bottom + _velocity.Y > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Top &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {
            return Rectangle.Top + _velocity.Y < sprite.Rectangle.Bottom &&
              Rectangle.Bottom > sprite.Rectangle.Bottom &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingLeftAttack(Sprite sprite)
        {
            return Rectangle.Right + 16 + _velocity.X > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Left &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRightAttack(Sprite sprite)
        {
            return Rectangle.Left - 16 + _velocity.X < sprite.Rectangle.Right &&
              Rectangle.Right > sprite.Rectangle.Right &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTopAttack(Sprite sprite)
        {
            return Rectangle.Bottom + 16 + _velocity.Y > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Top &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottomAttack(Sprite sprite)
        {
            return Rectangle.Top - 16 + _velocity.Y < sprite.Rectangle.Bottom &&
              Rectangle.Bottom > sprite.Rectangle.Bottom &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

    }
}
