using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;
using CultEscapeNew;

namespace CultEscapeNew.Sprites
{
    public class Player : Sprite
    {//private Vector2 position;
        protected float speed;
        protected string Direction = "S";

        protected int state;

        protected int recoveryTime;

        protected bool inRecovery = false;

        protected enum attackState { NotAttacking, Attacking, AttackHeldDown, AttackReleased };

        protected attackState myAttackState;

        private List<Fireball> _fireballs;

        public List<Texture2D> _fireballTextures;

        public bool poweredUp = false;

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
            _fireballs = new List<Fireball>();

            myAttackState = attackState.NotAttacking;
        }

        public Player(Texture2D texture)
            : base(texture)
        {
            speed = 3f;
            health = 4;
        }


        public void SetAttackStatus()
        {

            switch (myAttackState)
            {
                case attackState.NotAttacking:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        myAttackState = attackState.Attacking;
                        if (poweredUp)
                        {
                            switch (Direction)
                            {
                                case "N":
                                    _fireballs.Add(new Fireball(_fireballTextures[2], "N", new Vector2(Position.X+15, Position.Y-30)));
                                    break;
                                case "S":
                                    _fireballs.Add(new Fireball(_fireballTextures[3], "S", new Vector2(Position.X+15, Position.Y + 50)));
                                    break;
                                case "W":
                                    _fireballs.Add(new Fireball(_fireballTextures[0], "W", new Vector2(Position.X-10, Position.Y+15)));
                                    break;
                                case "E":
                                    _fireballs.Add(new Fireball(_fireballTextures[1], "E", new Vector2(Position.X+50, Position.Y+15)));
                                    break;
                            }
                            
                        }
                    }
                    break;
                case attackState.Attacking:
                    if (_animationManager.getCurrentFrame() == 3)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            myAttackState = attackState.AttackHeldDown;
                        }
                        if (Keyboard.GetState().IsKeyUp(Keys.Space))
                        {
                            myAttackState = attackState.NotAttacking;
                            switch (Direction)
                            {
                                case "N":
                                    _animationManager.Play(_animations["WalkUp"]);
                                    break;
                                case "S":
                                    _animationManager.Play(_animations["WalkDown"]);
                                    break;
                                case "W":
                                    _animationManager.Play(_animations["WalkLeft"]);
                                    break;
                                case "E":
                                    _animationManager.Play(_animations["WalkRight"]);
                                    break;
                            }
                        }
                    }
                    break;
                case attackState.AttackHeldDown:
                    if (Keyboard.GetState().IsKeyUp(Keys.Space))
                    {
                        myAttackState = attackState.AttackReleased;
                    }
                    break;
                case attackState.AttackReleased:
                    if (Keyboard.GetState().IsKeyUp(Keys.Space))
                    {
                        myAttackState = attackState.NotAttacking;
                    }
                    break;
            }

        }





        public override void Update(GameTime gameTime, List<Sprite> sprites, List<SolidTile> tiles)
        {
            var speed = 3f;

            SetAttackStatus();


            switch (myAttackState)
            {
                case attackState.NotAttacking:
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        _velocity.Y = -speed;
                        Direction = "N";
                        _animationManager.Play(_animations["WalkUp"]);

                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        _velocity.Y = speed;
                        Direction = "S";
                        _animationManager.Play(_animations["WalkDown"]);

                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        _velocity.X = -speed;
                        Direction = "W";
                        _animationManager.Play(_animations["WalkLeft"]);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        _velocity.X = speed;
                        Direction = "E";
                        _animationManager.Play(_animations["WalkRight"]);
                    }
                    else
                    {
                        _animationManager.Stop();

                    }
                    break;
                case attackState.Attacking:
                    if (Direction == "N")
                    {
                        _animationManager.Play(_animations["AttackUp"]);

                    }
                    else if (Direction == "S")
                    {
                        _animationManager.Play(_animations["AttackDown"]);

                    }
                    else if (Direction == "W")
                    {
                        _animationManager.Play(_animations["AttackLeft"]);
                    }
                    else if (Direction == "E")
                    {
                        _animationManager.Play(_animations["AttackRight"]);
                    }
                    else
                    {
                        _animationManager.Stop();

                    }
                    break;
                case attackState.AttackHeldDown:
                    _animationManager.Stop();
                    break;
                case attackState.AttackReleased:
                    _animationManager.Stop();
                    break;
            }


            if(_fireballs.Count > 0)
            {
                foreach (var fireball in _fireballs.ToArray())
                {
                    fireball.Update(gameTime, tiles, sprites);
                    if (fireball.hasCollided)
                    {
                        _fireballs.Remove(fireball);
                    }
                }
            }
            

            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;

                if (myAttackState == attackState.Attacking && IsTouchingLeftAttack(sprite) && Direction == "E")
                {
                    sprite.hasBeenHitR = true;
                }
                else if (IsTouchingLeft(sprite))
                {
                    this.hasBeenHitL = true;
                }


                if (myAttackState == attackState.Attacking && IsTouchingRightAttack(sprite) && Direction == "W")
                {
                    sprite.hasBeenHitL = true;
                }
                else if (IsTouchingRight(sprite))
                {
                    this.hasBeenHitR = true;
                }

                if (myAttackState == attackState.Attacking && IsTouchingTopAttack(sprite) && Direction == "S")
                {
                    sprite.hasBeenHitU = true;
                }
                else if (IsTouchingTop(sprite))
                {
                    this.hasBeenHitD = true;
                }

                if (myAttackState == attackState.Attacking && IsTouchingBottomAttack(sprite) && Direction == "N")
                {
                    sprite.hasBeenHitD = true;
                }
                else if (IsTouchingBottom(sprite))
                {
                    this.hasBeenHitU = true;
                }
            }

            if(inRecovery == false)
            {
                if (this.hasBeenHitL == true)
                {
                    health--;
                    _velocity.X = -100;
                    inRecovery = true;
                }
                else if (this.hasBeenHitR == true)
                {
                    health--;
                    _velocity.X = 100;
                    inRecovery = true;
                }
                else if (this.hasBeenHitU == true)
                {
                    health--;
                    _velocity.Y = 100;
                    inRecovery = true;
                }
                else if (this.hasBeenHitD == true)
                {
                    health--;
                    _velocity.Y = -100;
                    inRecovery = true;
                }
            }
            else
            {
                recoveryTime += gameTime.ElapsedGameTime.Milliseconds;
                if(recoveryTime >= 5000)
                {
                    inRecovery = false;
                    recoveryTime = 0;
                }
            }
            

            foreach (var tile in tiles)
            {
                if ((_velocity.X > 0 && IsBlockedLeft(tile)) ||
                        (_velocity.X < 0 && IsBlockedRight(tile)))
                    _velocity.X = 0;

                if ((_velocity.Y > 0 && IsBlockedTop(tile)) ||
                    (_velocity.Y < 0 && IsBlockedBottom(tile)))
                    _velocity.Y = 0;
            }


            Position += _velocity;
            _velocity = Vector2.Zero;
            hasBeenHitL = false;
            hasBeenHitR = false;
            hasBeenHitU = false;
            hasBeenHitD = false;
            _animationManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch);

            foreach(Fireball fireball in _fireballs)
            {
                fireball.Draw(gameTime, spriteBatch);
            }
        }

    }
}
