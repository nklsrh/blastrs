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

        public Player[] Player = new Player[2];
        Stadium Stadium;
        Input Input;
        Blast[] Blast = new Blast[10];
        Bot[] Bot = new Bot[3];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            for (int r = 0; r <= Player.Rank; r++)
            {
                Player[r] = new Player(this);
                Player[r].Position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                Player[r].CameraPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                Player[r].Speed = Vector2.Zero;
                Player[r].SpeedPower = 0.1f;
                Player[r].Score = 1000;
            }

            Stadium = new Stadium(this);
            Stadium.CameraPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2); //STILL CAMERA FOR NOW

            Input = new Input(this);
            Input.Initialize(Player[0], Player[1]);

            for (int r = 0; r <= Blast.Rank; r++)
            {
                Blast[r] = new Blast(this);
            }

                Bot[0] = new Bot(this);
                Bot[0].Initialize(this);
               

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Player[0].Sprite = Content.Load<Texture2D>("SamplePlayer");
            Player[0].StarImage = Content.Load<Texture2D>("star");
            Player[1].Sprite = Content.Load<Texture2D>("SamplePurple");
            Player[1].StarImage = Content.Load<Texture2D>("star");

            Bot[0].Sprite = Content.Load<Texture2D>("bombot");

            Stadium.Sprite = Content.Load<Texture2D>("stadiaHoley");
            Stadium.CollisionMap = Content.Load<Texture2D>("HoleyCollisionMap");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Input.Update(gameTime, Blast, spriteBatch);
            
            for (int r = 0; r <= Player.Rank; r++)
            {
                Player[r].Update(gameTime);
                Stadium.CheckCollisionWithPlayer(Player[r], gameTime);

                for (int b = 0; b <= Blast.Rank; b++)
                {
                        Blast[b].Update(gameTime, Player[r]); //THERE ARE MORE BLASTS THAN BOTS SO try catch LOLOL
                }
            }

            
                if (Bot[0].Dropped)
                {
                    Bot[0].Update(gameTime, this, Player);
                    try
                    {
                        Stadium.CheckCollisionWithBots(Bot[0], gameTime);
                    }
                    catch { }
                }
                else
                {
                    Bot[0].Drop(gameTime);
                }
            
 
        //if (Bot[0].Blasted)
        //            {
        //                Blast[0 + 3].Position = Bot[0].Position;
        //                Blast[0 + 3].Radius = 300;
        //                Blast[0 + 3].Power = 3000;
        //                Blast[0 + 3].Direction = Vector2.One;
        //            }

            Window.Title = Bot[0].BlastTimer.ToString();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Stadium.Draw(gameTime, spriteBatch);

            for (int r = 0; r <= Player.Rank; r++)
            {
                Player[r].Draw(gameTime, spriteBatch);
            }
            // TODO: Add your drawing code here
            Bot[0].Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
