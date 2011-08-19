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

using Lidgren.Network;

namespace BlastrServer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        string Message;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        NetServer server;
        NetPeerConfiguration config;

        int NumberOfPlayers = 0;
        Player[] Players = new Player[4];

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
             config = new NetPeerConfiguration("xnaapp");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = 8001;
            // create and start server
             server = new NetServer(config);
            server.Start();

            for (int x = 0; x < 4; x++)
            {
                Players[x] = new Player(this);
            }
            

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

            font = Content.Load<SpriteFont>("add");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            server.Shutdown("kthxbai.");
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

            // schedule initial sending of position updates
            double nextSendUpdates = NetTime.Now;

                NetIncomingMessage msg;
                while ((msg = server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            //
                            // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                            //
                            server.SendDiscoveryResponse(null, msg.SenderEndpoint);
                            
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            //
                            // Just print diagnostic messages to console
                            //
                            
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                            if (status == NetConnectionStatus.Connected)
                            {
                                //
                                // A new player just connected!
                                //
                                Message = (NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " connected!");
                                NumberOfPlayers = 0;
                                NetOutgoingMessage om = server.CreateMessage();
                                foreach (NetConnection player in server.Connections)
                                {
                                    NumberOfPlayers = server.ConnectionsCount;
                                    Players[NumberOfPlayers - 1].Identifier = player.RemoteUniqueIdentifier;
                                }
                
                                Message = ("There are player: " + NumberOfPlayers.ToString());
                                
                                // randomize his position and store in connection tag
                                msg.SenderConnection.Tag = new int[] {
									NetRandom.Instance.Next(10, 100),
									NetRandom.Instance.Next(10, 100)
								};
                            }

                            break;
                        case NetIncomingMessageType.Data:
                            //
                            // The client sent input to the server
                            //
                            int IndexPlayer = msg.ReadInt32();
                            int xinput = msg.ReadInt32();
                            int yinput = msg.ReadInt32();

                            // fancy movement logic goes here; we just append input to position
                            Players[IndexPlayer].Position = new Vector2(xinput, yinput);

                            break;  
                    }

                    //
                    // send position updates 30 times per second
                    //
                    double now = NetTime.Now;
                    if (now > nextSendUpdates)
                    {
                        // Yes, it's time to send position updates
                        foreach (NetConnection player in server.Connections)
                        {
                            // ... send information about every other player (actually including self)
                            foreach (NetConnection otherPlayer in server.Connections)
                            {
                                // send position update about 'otherPlayer' to 'player'
                                NetOutgoingMessage om = server.CreateMessage();

                                // write who this position is for
                                om.Write((Int32)NumberOfPlayers);
                                int y;
                                for (y = 0; y < NumberOfPlayers; y++)
                                {
                                    if (player.RemoteUniqueIdentifier == Players[y].Identifier) { break; }
                                }
                                om.Write((Int32)y);
                                om.Write((Int32)Players[y].Position.X);
                                om.Write((Int32)Players[y].Position.Y);
                                // send message
                                server.SendMessage(om, player, NetDeliveryMethod.ReliableOrdered);
                            }
                        }
                        // schedule next update
                        nextSendUpdates += (1.0 / 10.0);
                    }
                }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.DrawString(font, Message + " ", new Vector2(30,30), Color.Green);
            try
            {
                for (int x = 0; x < 4; x++)
                {
                    spriteBatch.DrawString(font, "Player " + (x+1).ToString() + Players[x].Position.ToString(), new Vector2(30, 50 + (30 * x)), Color.White);
                }
            }
            catch { }
            spriteBatch.End();

            //  Window.Title = Message.ToString();
            base.Draw(gameTime);
        }
    }
}
