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
    public class Menu : Microsoft.Xna.Framework.GameComponent
    {
        public Menu(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public enum Card{
            MainMenu,
            Controls,
            PlayerInformation,
            InGame,
            Scoreboard,
            Intro
        }

        public Texture2D Screen;
        public Card CurrentScreen;

        public void Initialize(Game1 game, SpriteBatch sb, ContentManager content)
        {
            if (CurrentScreen != Card.InGame)
            {
                Screen = content.Load<Texture2D>(CurrentScreen.ToString());
            }
         
            base.Initialize();
        }

        public void Update(GameTime gameTime, SpriteBatch sb, Texture2D videoTexture)
        {
            base.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch sb, Texture2D videoTexture)
        {
            sb.Begin();
            if (CurrentScreen != Card.InGame)
            {
                if (CurrentScreen == Card.Intro)
                {
                    if (videoTexture != null)
                    {
                        sb.Begin();
                        sb.Draw(videoTexture, new Rectangle(sb.GraphicsDevice.Viewport.X, sb.GraphicsDevice.Viewport.Y, sb.GraphicsDevice.Viewport.Width, sb.GraphicsDevice.Viewport.Height), Color.White);
                        sb.End();
                    }
                }
                else { sb.Draw(Screen, Vector2.Zero, Color.White); }
            }
            sb.End();
        }
    }
}