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

        GamePadState[] previousGamePadState, currentGamePadState;
        KeyboardState previousKeyboardState, currentKeyboardState;
        bool isDualShockPressed;
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Game1 game, Player[] player)
        {
            // TODO: Add your initialization code here
            previousGamePadState = new GamePadState[4];
            currentGamePadState = new GamePadState[4];

            for (int i = 0; i < player.Length; i++) //RESET PREVIOUS GAME STATES
            {
                previousGamePadState[i] = GamePad.GetState((PlayerIndex)(i));
                currentGamePadState[i] = GamePad.GetState((PlayerIndex)(i));
            }
            previousKeyboardState = Keyboard.GetState(PlayerIndex.One);
            isDualShockPressed = false;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Blast[] blast, SpriteBatch spriteBatch, Menu menu, Game1 game, ContentManager content, Player[] player)
        {
            //------------------------------------------------------DUALSHOCK3
            game.u1 = game.users.GetUser(1);
            if (!game.u1.PressedA() && isDualShockPressed)
            {
                if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
                {
                    game.ControlsToChars.Play();
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
                {
                    game.MenuToControls.Play();
                    //game.ChannelLogoAnim.Play();
                }
                isDualShockPressed = false;
            }
            if (game.u1.PressedA())
            {
                isDualShockPressed = true;
            }
            if (game.u1.PressedStart())
            {
                if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard)
                {
                    game.ChannelLogoAnim.Play();
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
                {
                    game.ChannelLogoAnim.Play();
                }
            }

            //------------------------------------------------------KEYBOARD
            currentKeyboardState = Keyboard.GetState(PlayerIndex.One);
            if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
            {
                if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
                {
                    game.ControlsToChars.Play();
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
                {
                    game.MenuToControls.Play();
                    //game.ChannelLogoAnim.Play();
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard)
                {
                    game.ChannelLogoAnim.Play();
                }
                if (menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
                {
                    game.ChannelLogoAnim.Play();
                }
            }

            //------------------------------------------------------XBOX360
            for (int i = 0; i < player.Length; i++)
            {
                currentGamePadState[i] = GamePad.GetState((PlayerIndex)(i));
                if (currentGamePadState[i].Buttons.A == ButtonState.Pressed && previousGamePadState[i].Buttons.A == ButtonState.Released)
                {
                    if (menu.CurrentScreen == blastrs.Menu.Card.Controls)
                    {
                        game.ControlsToChars.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.PlayerInformation)
                    {
                        game.ChannelLogoAnim.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.MainMenu)
                    {
                        game.MenuToControls.Play();
                        //game.ChannelLogoAnim.Play();
                    }
                    if (menu.CurrentScreen == blastrs.Menu.Card.Scoreboard)
                    {
                        game.ChannelLogoAnim.Play();
                    }
                }
            }

            
            #region GameControls
            if (menu.CurrentScreen == blastrs.Menu.Card.InGame)
            {
                //------------------------------------------------------DUALSHOCK3
                if (game.u1.GetLeftStick().X > 0)
                {
                    player[1].Speed.X += player[1].SpeedPower;
                }
                if (game.u1.GetLeftStick().X < 0)
                {
                    player[1].Speed.X -= player[1].SpeedPower;
                }
                if (game.u1.GetLeftStick().Y > 0)
                {
                    player[1].Speed.Y -= player[1].SpeedPower;
                }
                if (game.u1.GetLeftStick().Y < 0)
                {
                    player[1].Speed.Y += player[1].SpeedPower;
                }

                if (!game.u1.PressedRightBumper())
                {
                    if (!player[1].Blasting)
                    {
                        blast[1].Position = player[1].Position + Vector2.Multiply(player[1].Speed, 1.5f);
                        blast[1].Direction = player[1].Speed;
                        player[1].Speed = Vector2.Multiply(blast[1].Direction, -0.8f);
                        player[1].Blasting = true;
                    }
                }
                else
                {
                    if (blast[1].Ready)
                    {
                        player[1].Blasting = false;
                        blast[1].Ready = false;
                    }
                }


                //------------------------------------------------------KEGBOARD
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

                //------------------------------------------------------KEGBOARD2
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
                //------------------------------------------------------XBOX360
                for (int i = 0; i < player.Length; i++)
                {
                    if (currentGamePadState[i].ThumbSticks.Left.X > 0)
                    {
                        player[i].Speed.X += player[i].SpeedPower;
                    }
                    if (currentGamePadState[i].ThumbSticks.Left.X < 0)
                    {
                        player[i].Speed.X -= player[i].SpeedPower;
                    }
                    if (currentGamePadState[i].ThumbSticks.Left.Y > 0)
                    {
                        player[i].Speed.Y -= player[i].SpeedPower;
                    }
                    if (currentGamePadState[i].ThumbSticks.Left.Y < 0)
                    {
                        player[i].Speed.Y += player[i].SpeedPower;
                    }

                    if (currentGamePadState[i].Triggers.Right < 0.5)
                    {
                        if (!player[i].Blasting)
                        {
                            blast[i].Position = player[1].Position + Vector2.Multiply(player[1].Speed, 1.5f);
                            blast[i].Direction = Vector2.Multiply(player[i].Speed, 5f);
                            player[i].Speed = Vector2.Multiply(blast[i].Direction, -0.8f);
                            player[i].Blasting = true;
                        }
                    }
                    if (currentGamePadState[i].Triggers.Right > 0.5)
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

            previousKeyboardState = currentKeyboardState;
            for (int i = 0; i < player.Length; i++) 
            {
                previousGamePadState[i] = currentGamePadState[i];
            }
            

            base.Update(gameTime);
        }
    }
}