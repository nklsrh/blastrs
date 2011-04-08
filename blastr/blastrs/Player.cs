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
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        public Player(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        public Vector2 Position;
        public Vector2 Speed;
        public Texture2D Sprite;
        public float SpeedPower;
        public int Score;
        public Color TintColour;
        public Texture2D StarImage;
        public bool Blasting;
        public Texture2D Shadow;
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            TintColour = Color.White;
            Blasting = false;
            Score = 50;
            SpeedPower = 0.4f;
            Speed = new Vector2(0.01f, 0.01f);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(Game1 game, GameTime gameTime)
        {
            // TODO: Add your update code here
            ClampSpeed();
            Position += Speed;

            if (Score >= 1000)
            {
                game.Menu.CurrentScreen = Menu.Card.Scoreboard;
                game.Menu.Initialize(game, game.spriteBatch, game.Content);
            }

            base.Update(gameTime);
        }

        void ClampSpeed()
        {
            if (Speed.X > SpeedPower * 10)
            {
                Speed.X = SpeedPower * 10;
            }
            if (Speed.X < -SpeedPower * 10)
            {
                Speed.X = -SpeedPower * 10;
            }
            if (Speed.Y > SpeedPower * 10)
            {
                Speed.Y = SpeedPower * 10;
            }
            if (Speed.Y < -SpeedPower * 10)
            {
                Speed.Y = -SpeedPower * 10;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Shadow, new Vector2(Position.X - 20, Position.Y), null, TintColour, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.Draw(Sprite, Position, null, Color.White, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.End();
        }
    }
}