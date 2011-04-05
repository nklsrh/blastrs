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
        public Animation SideSwipers;
        public Animation MenuToControls;
        public Animation ControlsToChars;
        public Animation ChannelLogoAnim;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Player[] Player;
        public int NumberOfPlayers;

        Stadium Stadium;
        Input Input;
        Blast[] Blast = new Blast[10];
        public Bot[] Bot = new Bot[2];
        public SpriteFont Font, BoldFont;
        public Menu Menu;
        public TimeSpan CountDownTime;

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
            SideSwipers = new Animation(this);
            MenuToControls = new Animation(this);
            ControlsToChars = new Animation(this);
            ChannelLogoAnim = new Animation(this);

            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            NumberOfPlayers = 4;
            Player = new Player[NumberOfPlayers];
            for (int r = 0; r < NumberOfPlayers; r++)
            {
                Player[r] = new Player(this);
            }
            for (int r = 0; r < 10; r++)
            {
                Blast[r] = new Blast(this);
            }
            for (int r = 0; r < 2; r++)
            {
                Bot[r] = new Bot(this);
            }
            Stadium = new Stadium(this);
            Input = new Input(this);
            NewGame();  
            
            Stadium.Initialize(graphics);
            base.Initialize();
        }

        public void NewGame()
        {
            Vector2 Position = new Vector2(420, 120);
            // TODO: Add your initialization logic here
            for (int r = 0; r < NumberOfPlayers; r++)
            {
                Player[r].Position = Position;
                Player[r].CameraPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                Player[r].Speed = new Vector2(0.01f,0.01f);
                Player[r].SpeedPower = 0.4f;
                Player[r].Score = 1000;
                Player[r].Blasting = false;

                if (Position.X == 920)
                {
                    Position.X = 420;
                    Position.Y = 650;
                }
                else
                {
                    Position.X += 500;
                }
            }
            for (int r = 0; r < 2; r++)
            {
                Bot[r].Initialize(this);
                Bot[r].LoadBlastAnimation("BotBlast_Animation", Content, this);
            }
            for (int r = 0; r < 10; r++)
            {
                Blast[r].Initialize();
            }
            Stadium.CameraPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2); //STILL CAMERA FOR NOW         
            Input.Initialize(this, Player[0], Player[1], Player[2]);
            randomsssss = new Random(917329);
            CountDownTime = new TimeSpan(0, 0, 30);
        }

        protected override void LoadContent()
        {
            SideSwipers.LoadAnimationData("SideSwipes", Content);
            MenuToControls.LoadAnimationData("MainToControls", Content);
            ControlsToChars.LoadAnimationData("ControlsToChars", Content);
            ChannelLogoAnim.LoadAnimationData("ChannelLogo", Content);

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

            Player[3].Sprite = Content.Load<Texture2D>("yellowPlayer");
            Player[3].StarImage = Content.Load<Texture2D>("star");
            Player[3].Shadow = Content.Load<Texture2D>("shadow");

            for (int r = 0; r < 2; r++)
            {
                Bot[r].LoadBotAnimation("BomBot", Content, this); //Content.Load<Texture2D>("bombot2");
                Bot[r].Shadow = Content.Load<Texture2D>("shadow");
                Bot[r].BotIndex = r;
            }  
            
            Stadium.Sprite = Content.Load<Texture2D>("arena");
            Stadium.CollisionMap = Content.Load<Texture2D>("arenaCollisionMap");
            
            for (int r = 0; r < 10; r++)
            {
                Blast[r].Initialize();
                Blast[r].Sprite = Content.Load<Texture2D>("star");
            }

            Font = Content.Load<SpriteFont>("font");
            BoldFont = Content.Load<SpriteFont>("BoldFont");
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
                for (int r = 0; r < NumberOfPlayers; r++)
                {
                    Player[r].Update(this, gameTime);
                    Stadium.CheckCollisionWithPlayer(Player[r], gameTime);

                    for (int b = 0; b < 10; b++)
                    {
                        Blast[b].Update(gameTime, Player[r]);
                    }
                }

                for (int r = 0; r < 2; r++)
                {
                    if (Bot[r].Dropped)
                    {
                        Bot[r].Update(gameTime, this, Player);//, Blast[r+3]);

                        try
                        {
                            Stadium.CheckCollisionWithBots(Bot[r], gameTime);
                        }
                        catch { }
                    }
                    else
                    {
                        randomsssss = new Random(123123);
                        Bot[r].Drop(gameTime, new Vector2(randomsssss.Next(550, 800), randomsssss.Next(200, 600)));
                    }
                }

                CountDownTime -= gameTime.ElapsedGameTime;
                if (CountDownTime <= TimeSpan.Zero)
                {
                    Menu.CurrentScreen = Menu.Card.Scoreboard;
                    Menu.Initialize(this, spriteBatch, Content);
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

            //Window.Title = Menu.CurrentScreen.ToString();
            // TODO: Add your update logic here

            Input.Update(gameTime, Blast, spriteBatch, Menu, this, Content);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Menu.Draw(this, gameTime, spriteBatch, videoTexture);

            if (Menu.CurrentScreen == Menu.Card.InGame)
            {
                Stadium.Draw(gameTime, spriteBatch);

                for (int r = 0; r < NumberOfPlayers; r++)
                {
                    Player[r].Draw(gameTime, spriteBatch);
                }

                for (int r = 0; r < 2; r++)
                {
                    Bot[r].Draw(gameTime, spriteBatch);
                }

                for (int r = 0; r < 3; r++)
                {
                    Blast[r].Draw(spriteBatch);
                }

                DrawScore();
            }
            // if (player.State != MediaState.Stopped)
            //     videoTexture = player.GetTexture();

            PlayAnimations(gameTime);

            base.Draw(gameTime);
        }

        public void PlayAnimations(GameTime gameTime)
        {
            //if (SideSwipers.IsPlaying == false)
            //{
            //    SideSwipers.Play();
            //}
            //if (Menu.CurrentScreen == Menu.Card.MainMenu)
            //{
            //    if (SideSwipers.IsPlaying == true)
            //    {
            //        SideSwipers.Draw(spriteBatch);
            //    }
            //}

            if (MenuToControls.IsPlaying == true)
            {
                MenuToControls.Draw(spriteBatch);
            }
            if (MenuToControls.CurrentFrame == MenuToControls.EndFrame)
            {
                Menu.CurrentScreen = Menu.Card.Controls;
                Menu.Initialize(this, spriteBatch, Content);
            }

            if (ControlsToChars.IsPlaying == true)
            {
                ControlsToChars.Draw(spriteBatch);
            }
            if (ControlsToChars.CurrentFrame == ControlsToChars.EndFrame)
            {
                Menu.CurrentScreen = Menu.Card.PlayerInformation;
                Menu.Initialize(this, spriteBatch, Content);
            }

            if (ChannelLogoAnim.IsPlaying == true)
            {
                ChannelLogoAnim.Draw(spriteBatch);
            }
            if (ChannelLogoAnim.CurrentFrame == ChannelLogoAnim.EndFrame)
            {
                if (Menu.CurrentScreen == Menu.Card.PlayerInformation || Menu.CurrentScreen == Menu.Card.Scoreboard)
                {
                    Menu.CurrentScreen = blastrs.Menu.Card.InGame;
                    Menu.Initialize(this, spriteBatch, Content);
                }
            }
        }

        public void DrawScore()
        {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font, Player[0].Score.ToString(), new Vector2(160, 460), new Color(232, 156, 54));
                spriteBatch.DrawString(Font, Player[1].Score.ToString(), new Vector2(1130, 460), new Color(179, 194, 219));
                spriteBatch.DrawString(Font, Player[2].Score.ToString(), new Vector2(80, 560), new Color(179, 219, 189));
                spriteBatch.DrawString(Font, Player[3].Score.ToString(), new Vector2(1200, 560), new Color(243, 237, 217));
                spriteBatch.DrawString(Font, CountDownTime.Minutes.ToString() + " minute", new Vector2(10, 5), new Color(150, 150, 150));
                spriteBatch.DrawString(BoldFont, CountDownTime.Seconds.ToString(), new Vector2(40, 60), new Color(222, 222, 222));
                spriteBatch.End();
        }
        public void DrawScoreboard()
        {
            for (int r = 0; r < NumberOfPlayers; r++)
            {
                if (Player[r].Score < 0)
                {
                    Player[r].Score = 0;
                }
            }
            spriteBatch.Begin();
            spriteBatch.Draw(Menu.Screen, Vector2.Zero, Color.White);
            spriteBatch.DrawString(Font, Player[0].Score.ToString(), new Vector2(875, 110), new Color(232, 156, 54));
            spriteBatch.DrawString(Font, Player[1].Score.ToString(), new Vector2(875, 195), new Color(179, 194, 219));
            spriteBatch.DrawString(Font, Player[2].Score.ToString(), new Vector2(875, 275), new Color(179, 219, 189));
            spriteBatch.DrawString(Font, Player[3].Score.ToString(), new Vector2(875, 375), new Color(243, 237, 217));
            //DRAW THE WINNER"S NAME HERE 
            //CHECK THE WINNER
            //OK?
            //OK
            //DRAW THE WINNER"S NAME HERE 
            //CHECK THE WINNER
            //OK?
            //OK
            //DRAW THE WINNER"S NAME HERE 
            //CHECK THE WINNER
            //OK?
            //OK
            //DRAW THE WINNER"S NAME HERE 
            //CHECK THE WINNER
            //OK?
            //OK
            spriteBatch.End();
        }
    }
}
