using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace blastrs
{
    public class Blast : Microsoft.Xna.Framework.GameComponent
    {
        public Blast(Game game)
            : base(game)
        {
        }
        public float Radius;
        public Vector2 Position;
        public float Power;
        public Vector2 Direction;
        public Circle Area;
        public Texture2D Sprite;
        public bool Ready;
        public TimeSpan blastTime = new TimeSpan(0,0,3);

        public override void Initialize()
        {
            Area = new Circle();
            Ready = true;

            Power = 10;
            Radius = 150;

            base.Initialize();
        }

        public void Update(GameTime gameTime, Player Player, int index)
        {
            if (!Ready)
            {
                blastTime -= gameTime.ElapsedGameTime;
                Position += Direction;

                Area.Center = Position;
                Area.Radius = Radius;

                if (Area.Intersects(new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 1, 1)))
                {
                    Player.Speed += Direction;
                    GamePad.SetVibration((PlayerIndex)(index), 1.0f, 1.0f);
                }
                else
                {
                    GamePad.SetVibration((PlayerIndex)(index), 0f, 0f);
                }
                if (blastTime <= TimeSpan.Zero)
                {
                    Ready = true;
                    Player.Blasting = false;
                    blastTime = new TimeSpan(0, 0, 3);
                    if (index == 3)
                    {
                        blastTime = new TimeSpan(0, 0, 9);
                    }
                }
            }

            base.Update(gameTime);
        }

        public void UpdateForBot(GameTime gameTime, Bot bot)
        {
            if (!Ready)
            {
                if (Area.Intersects(new Rectangle((int)bot.Position.X, (int)bot.Position.Y, 1, 1)))
                {
                    bot.blastSpeed = Direction;
                }
            }

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            if (!Ready)
            {
                if (Direction.Y <= 0)
                { sb.Draw(Sprite, Position, null, Color.White, (float)(Math.Atan(-Direction.X / Direction.Y)), new Vector2(Sprite.Width / 2, Sprite.Height / 2), (float)(Power / (12)), SpriteEffects.None, 0f); }
                if (Direction.Y > 0)
                { sb.Draw(Sprite, Position, null, Color.White, (float)(Math.PI + Math.Atan(-Direction.X / Direction.Y)), new Vector2(Sprite.Width / 2, Sprite.Height / 2), (float)(Power / (12)), SpriteEffects.None, 0f); }
            }
            sb.End();
        }
    }
}