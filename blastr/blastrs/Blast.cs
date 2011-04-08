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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Blast : Microsoft.Xna.Framework.GameComponent
    {
        public Blast(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        public float Radius;
        public Vector2 Position;
        public float Power;
        public Vector2 Direction;
        public Circle Area;
        public Texture2D Sprite;
        public bool Ready;
        public TimeSpan blastTime = new TimeSpan(0,0,3);
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            Area = new Circle();
            Ready = true;

            Power = 10;
            Radius = 150;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Player Player, int index)
        {
            // TODO: Add your update code here3
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
                    blastTime = new TimeSpan(0, 0, 5);
                }
            }

            base.Update(gameTime);
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            if (!Ready)
            {
                sb.Draw(Sprite, Position, null, Color.White, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), Power/30, SpriteEffects.None, 0f);
            }
            sb.End();
        }
    }
}