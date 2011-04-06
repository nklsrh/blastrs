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
    public class Input : Microsoft.Xna.Framework.GameComponent
    {
        public Input(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        bool KeyPressed;
        //Player[] Player;
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Game1 game)
        {
            // TODO: Add your initialization code here
            //Player = new Player[3];
            //Player[0] = p1;
            //Player[1] = p2;
            //Player[2] = p3;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Blast[] blast, SpriteBatch spriteBatch, Menu menu, Game1 game, ContentManager content, Player[] player)
        {
            // TODO: Add your update code here
            #region GameControls
            if (menu.CurrentScreen == blastrs.Menu.Card.InGame)
            {
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
                {
                    player[0].Speed.X += player[0].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
                {
                    player[0].Speed.X -= player[0].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
                {
                    player[0].Speed.Y -= player[0].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S))
                {
                    player[0].Speed.Y += player[0].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.LeftShift))
                {
                    if (!player[0].Blasting)
                    {
                        blast[0].Position = player[0].Position + Vector2.Multiply(player[0].Speed, 1.5f);
                        blast[0].Direction = player[0].Speed;
                        player[0].Speed = Vector2.Multiply(blast[0].Direction, -0.8f);
                        player[0].Blasting = true;
                    }
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.LeftShift))
                {
                    if (blast[0].Ready)
                    {
                        player[0].Blasting = false;
                        blast[0].Ready = false;
                    }
                }


                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
                {
                    player[1].Speed.X += player[1].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
                {
                    player[1].Speed.X -= player[1].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                {
                    player[1].Speed.Y -= player[1].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                {
                    player[1].Speed.Y += player[1].SpeedPower;
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.RightShift))
                {
                    if (!player[1].Blasting)
                    {
                        blast[1].Position = player[1].Position + Vector2.Multiply(player[1].Speed, 1.5f);
                        blast[1].Direction = Vector2.Multiply(player[1].Speed, 5f);
                        player[1].Speed = Vector2.Multiply(blast[1].Direction, -0.8f);
                        player[1].Blasting = true;
                    }
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.RightShift))
                {
                    if (blast[1].Ready)
                    {
                        player[1].Blasting = false;
                        blast[1].Ready = false;
                    }
                }

                for (int i = 0; i < player.Length; i++)
                {
                    if (GamePad.GetState((PlayerIndex)(i)).ThumbSticks.Left.X > 0)
                    {
                        player[i].Speed.X += player[i].SpeedPower;
                    }
                    if (GamePad.GetState((PlayerIndex)(i)).ThumbSticks.Left.X < 0)
                    {
                        player[i].Speed.X -= player[i].SpeedPower;
                    }
                    if (GamePad.GetState((PlayerIndex)(i)).ThumbSticks.Left.Y > 0)
                    {
                        player[i].Speed.Y -= player[i].SpeedPower;
                    }
                    if (GamePad.GetState((PlayerIndex)(i)).ThumbSticks.Left.Y < 0)
                    {
                        player[i].Speed.Y += player[i].SpeedPower;
                    }

                    if (GamePad.GetState((PlayerIndex)(i)).Triggers.Right < 0.5)
                    {
                        if (!player[i].Blasting)
                        {
                            blast[i].Position = player[1].Position + Vector2.Multiply(player[1].Speed, 1.5f);
                            blast[i].Direction = Vector2.Multiply(player[i].Speed, 5f);
                            player[i].Speed = Vector2.Multiply(blast[i].Direction, -0.8f);
                            player[i].Blasting = true;
                        }
                    }
                    if (GamePad.GetState((PlayerIndex)(i)).Triggers.Right > 0.5)
                    {
                        if (blast[i].Ready)
                        {
                            player[i].Blasting = false;
                            blast[i].Ready = false;
                        }
                    }
                }

            }
            #endregion GameControls
            if (KeyPressed)
            {
                if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.A))
                    {
                        game.ControlsToChars.Play();
                    }
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.Enter))
                    {
                        game.ChannelLogoAnim.Play();
                    }
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.A))
                    {
                        game.MenuToControls.Play();
                        //game.ChannelLogoAnim.Play();
                    }
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.Enter))
                    {
                        game.ChannelLogoAnim.Play();
                    }
                }
                KeyPressed = false;
            }
            else
            {
                 if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
                    {
                        KeyPressed = true;
                    }
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                    {
                        KeyPressed = true;
                    }
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
                    {
                        KeyPressed = true;
                    }
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                    {
                        KeyPressed = true;
                    }
                }
            }
            

            base.Update(gameTime);
        }
    }
}