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
    public class Stadium : Microsoft.Xna.Framework.GameComponent
    {
        public Stadium(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        Game1 game = new Game1();
        public Vector2 CameraPosition;
        public float Scale;
        public Texture2D Sprite;
        public Texture2D CollisionMap;
        public Color bgColor;
        public Color[] bgColorArr;
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(GraphicsDeviceManager graphics)
        {
            CollisionMap = new Texture2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            

            base.Update(gameTime);
        }

        public void CheckCollisionWithPlayer(Player Player, GameTime gameTime)
        {
            bgColorArr = new Color[1];
            CollisionMap.GetData<Color>(0, new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 1, 1), bgColorArr, 0, 1);
            bgColor = bgColorArr[0];

            if (bgColor == Color.Black) //FALLS OFFFFFFFFFFF
            {
                if (Player.Speed.X < 0)
                {
                    Player.Position.X = 960;
                }
                else
                {
                    Player.Position.X = 420;
                }
                if (Player.Speed.Y < 0)
                {
                    Player.Position.Y = 650;
                }
                else
                {
                    Player.Position.Y = 120;
                }
                Player.Score -= 100;
            }
            if (bgColor == Color.Cyan) //SCOREEEEE
            {
                Player.Score += gameTime.ElapsedGameTime.Milliseconds / 10;
            }
            else
            {
                if (Player.Score > 0)
                {
                    Player.Score -= gameTime.ElapsedGameTime.Milliseconds / 15;
                }
                else
                {
                    Player.Score = 0;
                }
            }
            
            Player.TintColour = bgColor;
        }

        public void CheckCollisionWithBots(Bot Bot, GameTime gameTime)
        {
            bgColorArr = new Color[1];
            CollisionMap.GetData<Color>(0, new Rectangle((int)Bot.Position.X, (int)Bot.Position.Y, 1, 1), bgColorArr, 0, 1);
            bgColor = bgColorArr[0];

            if (bgColor == Color.Black) //FALLS OFFFFFFFFFFF
            {
                Bot.Initialize(game);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Sprite, CameraPosition, null, Color.White, 0f, new Vector2(Sprite.Width/2, Sprite.Height/2), 1f, SpriteEffects.None, 1f);
            //sb.Draw(CollisionMap, Vector2.Zero, Color.White);

            sb.End();
        }
    }
}