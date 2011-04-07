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
using Microsoft.DirectX.DirectInput;
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
        public Animation CountDown;
        public Animation ControlsToMain;
        public Animation CharsToControls;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Player[] Player;
        public int NumberOfPlayers;
        Texture2D[] ScoreBar = new Texture2D[4];

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
        Song[] Song;

        public Device device;
        public bool loaded = false;

        public Vector2 psLeftThumbStick;
        public Vector2 psRightThumbStick;
        public Vector2 LeftThumbStick;
        public Vector2 RightThumbStick;
        public bool HasLeft;
        public bool HasRight;
        const float center = 32767.5f;

        public GamePadConfig conf;
        public User[] localUsers;
        public IUserInterface users;
        public User u1;

        public int[] myint;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            new UserControl(this);
        }

        void SetupDirectInputGamepad(Guid gamepadInstanceGuid)
        {
            device = new Device(gamepadInstanceGuid);
            device.SetDataFormat(DeviceDataFormat.Joystick);
            device.Acquire();
        }

        protected override void Initialize()
        {
            SideSwipers = new Animation(this);
            MenuToControls = new Animation(this);
            ControlsToChars = new Animation(this);
            ChannelLogoAnim = new Animation(this);
            CountDown = new Animation(this);
            ControlsToMain = new Animation(this);
            CharsToControls = new Animation(this);

            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            NumberOfPlayers = 2;

            for (int i = 2; i < 4; i++)
            {
                if (GamePad.GetCapabilities((PlayerIndex)(i)).IsConnected == true)
                {
                    NumberOfPlayers += 1;
                }
            }

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
                Player[r].Initialize();
                Player[r].Position = Position;
                
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
            Input.Initialize(this, Player);
            randomsssss = new Random(917329);
            CountDownTime = new TimeSpan(0, 1, 0);
        }

        protected override void LoadContent()
        {
            SideSwipers.LoadAnimationData("SideSwipes", Content);
            MenuToControls.LoadAnimationData("MainToControls", Content);
            ControlsToChars.LoadAnimationData("ControlsToChars", Content);
            ChannelLogoAnim.LoadAnimationData("ChannelLogo", Content);
            CountDown.LoadAnimationData("CountDown", Content);
            ControlsToMain.LoadAnimationData("ControlsToMain", Content);
            CharsToControls.LoadAnimationData("CharsToControls", Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            try
            {
                Player[0].Sprite = Content.Load<Texture2D>("redPlayer");
                Player[0].StarImage = Content.Load<Texture2D>("star");
                Player[0].Shadow = Content.Load<Texture2D>("shadow");
            }
            catch (Exception e)
            {
            }

            try
            {
                Player[1].Sprite = Content.Load<Texture2D>("bluePlayer");
                Player[1].StarImage = Content.Load<Texture2D>("star");
                Player[1].Shadow = Content.Load<Texture2D>("shadow");
            }
            catch (Exception e)
            {
            }

            try
            {
                Player[2].Sprite = Content.Load<Texture2D>("greenPlayer");
                Player[2].StarImage = Content.Load<Texture2D>("star");
                Player[2].Shadow = Content.Load<Texture2D>("shadow");
            }
            catch (Exception e) { }

            try
            {
                Player[3].Sprite = Content.Load<Texture2D>("yellowPlayer");
                Player[3].StarImage = Content.Load<Texture2D>("star");
                Player[3].Shadow = Content.Load<Texture2D>("shadow");
            }
            catch (Exception e) { }

            ScoreBar[0] = Content.Load<Texture2D>("redBar");
            ScoreBar[1] = Content.Load<Texture2D>("blueBar");
            ScoreBar[2] = Content.Load<Texture2D>("greenBar");
            ScoreBar[3] = Content.Load<Texture2D>("yellowBar");

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

            ChannelLogoAnim.Play();

            Song = new Song[4];
            for (int i = 0; i < 4; i++)
            {
                Song[i] = Content.Load<Song>("Audio\\Music\\" + i);
            }
            MediaPlayer.Stop();
            MediaPlayer.Play(Song[1]);
            //video = Content.Load<Video>("smallIntro2");
            //player = new VideoPlayer();

            users = (IUserInterface)Services.GetService(typeof(IUserInterface));
            
        //--------------------------------------------------------------------------------MENU SELECTLOLOLOL
            Menu = new Menu(this);
            Menu.CurrentScreen = Menu.Card.InGame;
            Menu.Initialize(this, spriteBatch, Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            //for (int i = 0; i < 100; i++)
            //{
            //    myint = new Int32[1000];
            //}

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
                        Bot[r].Drop(gameTime, new Vector2(500 + 300 * r, 500)); //new Vector2(randomsssss.Next(550, 800), randomsssss.Next(200, 600)));
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

            //Window.Title = MediaPlayer.Queue.ActiveSong.Name.ToString();

            Input.Update(gameTime, Blast, spriteBatch, Menu, this, Content, Player);

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
            if (MenuToControls.IsPlaying == true)
            {
                MenuToControls.Draw(spriteBatch);
            }
            if (MenuToControls.CurrentFrame == MenuToControls.EndFrame)
            {
                Menu.CurrentScreen = Menu.Card.Controls;
                Menu.Initialize(this, spriteBatch, Content);
            }

            if (ControlsToMain.IsPlaying == true)
            {
                ControlsToMain.Draw(spriteBatch);
            }
            if (ControlsToMain.CurrentFrame == ControlsToMain.EndFrame)
            {
                Menu.CurrentScreen = Menu.Card.MainMenu;
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

            if (CharsToControls.IsPlaying == true)
            {
                CharsToControls.Draw(spriteBatch);
            }
            if (CharsToControls.CurrentFrame == CharsToControls.EndFrame)
            {
                Menu.CurrentScreen = Menu.Card.Controls;
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
                    ShuffleSongs();
                    CountDown.Play();
                }
            }

            if (CountDown.IsPlaying)
            {
                CountDown.Draw(spriteBatch);
            }
            if (CountDown.CurrentFrame == CountDown.EndFrame)
            {
                if (Menu.CurrentScreen == Menu.Card.PlayerInformation || Menu.CurrentScreen == Menu.Card.Scoreboard)
                {
                    Menu.CurrentScreen = blastrs.Menu.Card.InGame;
                    Menu.Initialize(this, spriteBatch, Content);
                }
            }
        }
        public void ShuffleSongs()
        {
            if (MediaPlayer.Queue.ActiveSong.Name == "Audio\\Music\\0")
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(Song[2]);
            }
            if (MediaPlayer.Queue.ActiveSong.Name == "Audio\\Music\\2")
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(Song[1]);
            }
            if (MediaPlayer.Queue.ActiveSong.Name == "Audio\\Music\\3")
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(Song[0]);
            }
            if (MediaPlayer.Queue.ActiveSong.Name == "Audio\\Music\\1")
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(Song[3]);
            }
        }
        public void DrawScore()
        {
                spriteBatch.Begin();
                try
                {
                    spriteBatch.Draw(ScoreBar[0], new Rectangle(42, (int)(622.5f - ((float)(Player[0].Score / 1000f) * 379f)), 85, (int)(((float)(Player[0].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[0].Score.ToString(), new Vector2(160, 460), new Color(232, 156, 54));
                }
                catch (Exception e) { }
                try
                {
                    spriteBatch.Draw(ScoreBar[1], new Rectangle(165, (int)(622.5f - ((float)(Player[1].Score / 1000f) * 379f)), 85, (int)(((float)(Player[1].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[1].Score.ToString(), new Vector2(1130, 460), new Color(179, 194, 219));
                } 
                catch (Exception e) { }
                try
                {
                    spriteBatch.Draw(ScoreBar[2], new Rectangle(1087, (int)(622.5f - ((float)(Player[2].Score / 1000f) * 379f)), 85, (int)(((float)(Player[2].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[2].Score.ToString(), new Vector2(80, 560), new Color(179, 219, 189));
                }
                catch (Exception e) { }
                try
                {
                    spriteBatch.Draw(ScoreBar[3], new Rectangle(1223, (int)(622.5f - ((float)(Player[3].Score / 1000f) * 379f)), 85, (int)(((float)(Player[3].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[3].Score.ToString(), new Vector2(1200, 560), new Color(243, 237, 217));
                }
                catch (Exception e) { }

                spriteBatch.DrawString(Font, "seconds", new Vector2(40, 140), new Color(150, 150, 150));
                spriteBatch.DrawString(BoldFont, CountDownTime.Seconds.ToString(), new Vector2(40, 0), new Color(222, 222, 222));
                spriteBatch.End();
        }
        public void DrawScoreboard()
        {
            for (int r = 0; r < NumberOfPlayers; r++)
            {
                if (Player[r].Score > 1000)
                {
                    Player[r].Score = 1000;
                }
            }
            spriteBatch.Begin();
            spriteBatch.Draw(Menu.Screen, Vector2.Zero, Color.White);
            try
            {
                spriteBatch.Draw(ScoreBar[0], new Rectangle(442, (int)(554f - ((float)(Player[0].Score / 1000f) * 324f)), 85, (int)(((float)(Player[0].Score / 1000f) * 324f))), Color.White);
            }
            catch (Exception e) { }
            try
            {
                spriteBatch.Draw(ScoreBar[1], new Rectangle(570, (int)(554f - ((float)(Player[1].Score / 1000f) * 324f)), 85, (int)(((float)(Player[1].Score / 1000f) * 324f))), Color.White);   
            }
            catch (Exception e) { }
            try
            {
                spriteBatch.Draw(ScoreBar[2], new Rectangle(724, (int)(554f - ((float)(Player[2].Score / 1000f) * 324f)), 85, (int)(((float)(Player[2].Score / 1000f) * 324f))), Color.White);   
            }
            catch (Exception e) { }
            try
            {
                spriteBatch.Draw(ScoreBar[3], new Rectangle(860, (int)(554f - ((float)(Player[3].Score / 1000f) * 324f)), 85, (int)(((float)(Player[3].Score / 1000f) * 324f))), Color.White);
            }
            catch (Exception e) { }
            spriteBatch.End();
        }
    }
}
