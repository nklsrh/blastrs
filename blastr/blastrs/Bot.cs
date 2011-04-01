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
        /// <summary>
        /// where the bot is
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// how fast it is moving, in x and y components I THINK I KNOW WHAT U MEAN BY POSITION KTHX
        /// </summary>
        public Vector2 Speed;
        /// <summary>
        /// how many pixels it can travel in a frame
        /// </summary>
        public float SpeedPower;
        /// <summary>
        /// where the bot wants to move
        /// </summary>
        public int Target;
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
            SpeedPower = 0.1f;

            
            //Speed = new Vector2(new Random().Next(-1, 1), new Random().Next(-1, 1));

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Game1 game, Player[] targets)
        {

            Target = 0;

            for (int r = 0; r < targets.Length; r++)
            {
                if (Vector2.Distance(Position, targets[Target].Position) > Vector2.Distance(Position, targets[r].Position))
                {
                    Target = r;
                }
            }

            Position = Vector2.SmoothStep(Position, targets[Target].Position, 0.05f);

            //Speed.X = SpeedPower * (Vector2.Distance(Position, targets[Target].Position) / (targets[Target].Position.X - Position.X));
            //Speed.Y = SpeedPower * (Vector2.Distance(Position, targets[Target].Position) / (targets[Target].Position.Y - Position.Y));

            if (targets[Target].Position.X == Position.X) { Speed.X = 0; }
            if (targets[Target].Position.Y == Position.Y) { Speed.Y = 0; }

            //Position += Speed;

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

        public void Drop(GameTime gameTime) //MUST FIX THE WIDTH AND HEIGHT THINGY IT WAS USING UP TOO MUCH CPU IVE SET IT TO DEFAULT 1366 by 768
        {
            Position.Y += (768 / 2 - Position.Y) / 20f;
            if (Position.Y >= 768 / 2 - 4)
            {
                Dropped = true;
                Position = new Vector2(Position.X, 768 / 2);
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