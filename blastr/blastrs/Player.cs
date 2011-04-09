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
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        public Player(Game game)
            : base(game)
        {
        }

        public Vector2 Position;
        public Vector2 Speed;
        public Texture2D Sprite;
        public float SpeedPower;
        public int Score;
        public Color TintColour;
        public Texture2D StarImage;
        public bool Blasting;
        public Texture2D ShadowSprite;
        public SpriteFont tag;
        public float tagOpacity = 6;

        public override void Initialize()
        {
            Blasting = false;
            Score = 50;
            SpeedPower = 0.4f;
            Speed = new Vector2(0.01f, 0.01f);
            tagOpacity = 6;

            base.Initialize();
        }

        public void Update(Game1 game, GameTime gameTime)
        {
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
            sb.Draw(ShadowSprite, new Vector2(Position.X - 20, Position.Y), null, TintColour, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.Draw(Sprite, Position, null, TintColour, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.End();
        }

        public void DrawTag(GameTime gametime, SpriteBatch sb, int playerNum, Vector3 Colour)
        {
            sb.Begin();
            sb.DrawString(tag, "Player: " + playerNum.ToString(), new Vector2(Position.X + 40, Position.Y - 30), new Color(new Vector4(Colour, tagOpacity)));
            sb.End();

            tagOpacity -= 0.03f;
        }
    }
}