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
    public class Input : Microsoft.Xna.Framework.GameComponent
    {
        public Input(Game game)
            : base(game)
        {
        }
        GamePadState[] previousGamePadState, currentGamePadState;

        public void Initialize(Game1 game, Player[] player)
        {
            previousGamePadState = new GamePadState[4];
            currentGamePadState = new GamePadState[4];

            for (int i = 0; i < player.Length; i++) //RESET PREVIOUS GAME STATES
            {
                previousGamePadState[i] = GamePad.GetState((PlayerIndex)(i));
                currentGamePadState[i] = GamePad.GetState((PlayerIndex)(i));
            }

            base.Initialize();
        }

        public void Update(GameTime gameTime, Blast[] blast, SpriteBatch spriteBatch, Menu menu, Game1 game, ContentManager content, Player[] player)
        {
            #region Menus
            for (int i = 0; i < player.Length; i++)
            {
                currentGamePadState[i] = GamePad.GetState((PlayerIndex)(i));
                if (currentGamePadState[i].Buttons.A == ButtonState.Pressed && previousGamePadState[i].Buttons.A == ButtonState.Released)
                {
                    GamePad.SetVibration((PlayerIndex)(i), 0.5f, 0.5f);
                    if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
                    {
                        game.ControlsToChars.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
                    {
                        game.MenuToControls.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard || menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
                    {
                        if (!game.ChannelLogoAnim.IsPlaying)
                        {
                            game.ChannelLogoAnim.Play();
                        }
                    }
                }
                else
                {
                    GamePad.SetVibration((PlayerIndex)(i), 0f, 0f);
                }

                if (currentGamePadState[i].Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released)
                {
                    if (menu.CurrentScreen == blastrs.Menu.Card.Intro)
                    {
                        game.player.Stop();
                        MediaPlayer.Play(game.Song[0]);
                        game.Menu.CurrentScreen = Menu.Card.MainMenu;
                    }
                }

                if (currentGamePadState[i].Buttons.B == ButtonState.Pressed && previousGamePadState[i].Buttons.B == ButtonState.Released)
                {
                    GamePad.SetVibration((PlayerIndex)(i), 0.5f, 0.5f);
                    if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
                    {
                        game.ControlsToMain.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
                    {
                        game.CharsToControls.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard)
                    {
                        game.ControlsToMain.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
                    {
                        menu.CurrentScreen = Menu.Card.Intro;
                        MediaPlayer.Stop();
                        game.player.Play(game.video);
                    }
                }
                else
                {
                    GamePad.SetVibration((PlayerIndex)(i), 0f, 0f);
                }
            }
            #endregion Menus

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.R))
            {
                game.GameInitialize();
            }

            #region GameControls
            for (int i = 0; i < player.Length; i++)
            {
                if (menu.CurrentScreen == blastrs.Menu.Card.InGame)
                {
                    if (currentGamePadState[i].ThumbSticks.Left.X != 0)
                    {
                        player[i].Speed.X += player[i].SpeedPower * currentGamePadState[i].ThumbSticks.Left.X;
                    }
                    if (currentGamePadState[i].ThumbSticks.Left.Y != 0)
                    {
                        player[i].Speed.Y -= player[i].SpeedPower * currentGamePadState[i].ThumbSticks.Left.Y;
                    }
                    if (currentGamePadState[i].IsButtonUp(Buttons.Start) && previousGamePadState[i].IsButtonDown(Buttons.Start))
                    {
                        game.Menu.CurrentScreen = Menu.Card.Paused;
                    }

                    if (currentGamePadState[i].Triggers.Right < 0.3)
                    {
                        if (!player[i].Blasting && currentGamePadState[i].ThumbSticks.Right != Vector2.Zero && blast[i].Ready == false)
                        {
                            blast[i].Position = player[i].Position + Vector2.Multiply(player[i].Speed, 1.5f);

                            try
                            {
                                blast[i].Direction = Vector2.Multiply(Vector2.Normalize(new Vector2(currentGamePadState[i].ThumbSticks.Right.X, -(currentGamePadState[i].ThumbSticks.Right.Y))), blast[i].Power);
                            }
                            catch { }

                            player[i].Speed = Vector2.Multiply(blast[i].Direction, -35f);
                            player[i].Blasting = true;
                        }
                    }
                    else
                    {
                        if (blast[i].Ready)
                        {
                            player[i].Blasting = false;
                            blast[i].Ready = false;
                        }
                    }
                }
                else
                {
                    if (game.Menu.CurrentScreen == Menu.Card.Paused)
                    {
                        if (currentGamePadState[i].IsButtonUp(Buttons.Start) && previousGamePadState[i].IsButtonDown(Buttons.Start))
                        {
                            game.Menu.CurrentScreen = Menu.Card.InGame;
                        }
                    }
                }
            }
            #endregion GameControls

          
                
            for (int i = 0; i < player.Length; i++) 
            {
                previousGamePadState[i] = currentGamePadState[i];
            }

#region KeyboardInput
            //------------------------------------------------------KEGBOARD
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
            //{
            //    player[0].Speed.X += player[0].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
            //{
            //    player[0].Speed.X -= player[0].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
            //{
            //    player[0].Speed.Y -= player[0].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S))
            //{
            //    player[0].Speed.Y += player[0].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.LeftShift))
            //{
            //    if (!player[0].Blasting)
            //    {
            //        blast[0].Position = player[0].Position + Vector2.Multiply(player[0].Speed, 1.5f);
            //        blast[0].Direction = player[0].Speed;
            //        player[0].Speed = Vector2.Multiply(blast[0].Direction, -0.8f);
            //        player[0].Blasting = true;
            //    }
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.LeftShift))
            //{
            //    if (blast[0].Ready)
            //    {
            //        player[0].Blasting = false;
            //        blast[0].Ready = false;
            //    }
            //}

            ////------------------------------------------------------KEGBOARD2
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
            //{
            //    player[1].Speed.X += player[1].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
            //{
            //    player[1].Speed.X -= player[1].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
            //{
            //    player[1].Speed.Y -= player[1].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
            //{
            //    player[1].Speed.Y += player[1].SpeedPower;
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyUp(Keys.RightShift))
            //{
            //    if (!player[1].Blasting)
            //    {
            //        blast[1].Position = player[1].Position + Vector2.Multiply(player[1].Speed, 1.5f);
            //        blast[1].Direction = Vector2.Multiply(player[1].Speed, 5f);
            //        player[1].Speed = Vector2.Multiply(blast[1].Direction, -0.8f);
            //        player[1].Blasting = true;
            //    }
            //}
            //if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.RightShift))
            //{
            //    if (blast[1].Ready)
            //    {
            //        player[1].Blasting = false;
            //        blast[1].Ready = false;
            //    }
            //}
#endregion KeyboardInput
#region KeyboardMenus
            //------------------------------------------------------KEYBOARD
            //currentKeyboardState = Keyboard.GetState(PlayerIndex.One);
            //if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
            //{
            //    if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
            //    {
            //        game.ControlsToChars.Play();
            //    }
            //    if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
            //    {
            //        game.MenuToControls.Play();
            //        //game.ChannelLogoAnim.Play();
            //    }
            //}
            //if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            //{
            //    if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard)
            //    {
            //        game.ChannelLogoAnim.Play();
            //    }
            //    if (menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
            //    {
            //        game.ChannelLogoAnim.Play();
            //    }
            //}
            #endregion KeyboardMenus

            base.Update(gameTime);
        }
    }
}