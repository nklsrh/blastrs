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
    public class Bot : Microsoft.Xna.Framework.GameComponent
    {
        public Bot(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        public Vector2 Position;
        public Vector2 Speed;
        public Texture2D Sprite;
        public float Scale;
        public TimeSpan BlastTimer;
        public Color TintColor;
        public bool Dropped;
        public bool Blasted;
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Game1 game)
        {
            // TODO: Add your initialization code here
            BlastTimer = new TimeSpan(0, 0, 7);
            Scale = 1f;
            TintColor = Color.White;
            Position.X = new Random().Next(game.graphics.PreferredBackBufferWidth / 2 - 100, game.graphics.PreferredBackBufferWidth / 2 + 100);
            Position.Y = 0;
            Dropped = false;
            Speed = new Vector2(new Random().Next(-1, 1), new Random().Next(-1, 1));

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Game1 game)
        {
            // TODO: Add your update code here
            Position += Speed;

            BlastTimer -= gameTime.ElapsedGameTime;

           try { TintColor.R = (byte)(255 - BlastTimer.Milliseconds /10); }catch { }

            if (BlastTimer <= TimeSpan.Zero)
            {
                Blasted = true;

                if (BlastTimer <= -(new TimeSpan(0, 0, 2)))
                {
                    Dropped = false;
                    Initialize(game);
                }
            }

            base.Update(gameTime);
        }

        public void Drop(GameTime gameTime) //MUST FIX THE WIDTH AND HEIGHT THINGY IT WAS USING UP TOO MUCH CPU IVE SET IT TO DEFAULT 800 by 600
        {
            Position.Y += (600 / 2 - Position.Y) / 20f;
            if (Position.Y >= 600 / 2 - 4)
            {
                Dropped = true;
                Position = new Vector2(Position.X, 600 / 2);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Sprite, Position, null, TintColor, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.End();
        }
    }
}