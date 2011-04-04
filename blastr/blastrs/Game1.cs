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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Player[] Player = new Player[3];
        Stadium Stadium;
        Input Input;
        Blast[] Blast = new Blast[10];
        Bot[] Bot = new Bot[3];
        public SpriteFont Font;
        Menu Menu;

        public Random randomsssss;

        //Video video;
        //VideoPlayer player;
        Texture2D videoTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            for (int r = 0; r < 3; r++)
            {
                Player[r] = new Player(this);
                Player[r].Position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                Player[r].CameraPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                Player[r].Speed = Vector2.Zero;
                Player[r].SpeedPower = 0.4f;
                Player[r].Score = 1000;
                Player[r].Blasting = false;
            }

            Stadium = new Stadium(this);
            Stadium.CameraPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2); //STILL CAMERA FOR NOW

            Input = new Input(this);
            Input.Initialize(Player[0], Player[1], Player[2]);

            for (int r = 0; r < 10; r++)
            {
                Blast[r] = new Blast(this);
            }

            for (int r = 0; r < 3; r++)
            {
                Bot[r] = new Bot(this);
                Bot[r].Initialize(this);
            }

            randomsssss = new Random(917329);

            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Player[0].Sprite = Content.Load<Texture2D>("redPlayer");
            Player[0].StarImage = Content.Load<Texture2D>("star");
            Player[0].Shadow = Content.Load<Texture2D>("shadow");

            Player[1].Sprite = Content.Load<Texture2D>("bluePlayer");
            Player[1].StarImage = Content.Load<Texture2D>("star");
            Player[1].Shadow = Content.Load<Texture2D>("shadow");

            Player[2].Sprite = Content.Load<Texture2D>("greenPlayer");
            Player[2].StarImage = Content.Load<Texture2D>("star");
            Player[2].Shadow = Content.Load<Texture2D>("shadow");

            for (int r = 0; r < 3; r++)
            {
                Bot[r].Sprite = Content.Load<Texture2D>("bombot2");
                Bot[r].Shadow = Content.Load<Texture2D>("shadow");
            }  
            
            Stadium.Sprite = Content.Load<Texture2D>("arena");
            Stadium.CollisionMap = Content.Load<Texture2D>("arenaCollisionMap");
            
            for (int r = 0; r < 10; r++)
            {
                Blast[r].Initialize();
                Blast[r].Sprite = Content.Load<Texture2D>("star");
            }

            Font = Content.Load<SpriteFont>("font");

            //video = Content.Load<Video>("smallIntro2");
            //player = new VideoPlayer();

        //--------------------------------------------------------------------------------MENU SELECTLOLOLOL
            Menu = new Menu(this);
            Menu.CurrentScreen = Menu.Card.MainMenu;
            Menu.Initialize(this, spriteBatch, Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            if (Menu.CurrentScreen == Menu.Card.InGame)
            {
                for (int r = 0; r < 3; r++)
                {
                    Player[r].Update(gameTime);
                    Stadium.CheckCollisionWithPlayer(Player[r], gameTime);

                    for (int b = 0; b < 10; b++)
                    {
                        Blast[b].Update(gameTime, Player[r]);
                    }
                }

                for (int r = 0; r < 3; r++)
                {
                    if (Bot[r].Dropped)
                    {
                        Bot[r].Update(gameTime, this, Player, Blast[r+3]);

                        try
                        {
                            Stadium.CheckCollisionWithBots(Bot[r], gameTime);
                        }
                        catch { }
                    }
                    else
                    {
                        randomsssss = new Random(123123);
                        Bot[r].Drop(gameTime, new Vector2(randomsssss.Next(500, 800), randomsssss.Next(200, 600)));
                    }
                }
            }
            

            //if (Menu.CurrentScreen = Menu.Card.Intro)
            //{
            //    if (player.State == MediaState.Stopped && player.IsLooped = false)
            //    {
            //        player.IsLooped = true;
            //        player.Play(video);
            //    }
            //}

            //Window.Title = Bot[0].BlastTimer.ToString();
            // TODO: Add your update logic here

            Input.Update(gameTime, Blast, spriteBatch, Menu, this, Content);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Stadium.Draw(gameTime, spriteBatch);

            for (int r = 0; r < 3; r++)
            {
                Player[r].Draw(gameTime, spriteBatch);
            }

            for (int r = 0; r < 3; r++)
            {
                Bot[r].Draw(gameTime, spriteBatch);
            } 

            for (int r = 0; r < 3; r++)
            {
                Blast[r].Draw(spriteBatch);
            }

           // if (player.State != MediaState.Stopped)
           //     videoTexture = player.GetTexture();
            Menu.Draw(gameTime, spriteBatch, videoTexture);

            DrawScore();

            base.Draw(gameTime);
        }

        public void DrawScore()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, Player[0].Score.ToString(), new Vector2(25, 25), Color.Orange);
            spriteBatch.End();
        }
    }
}
