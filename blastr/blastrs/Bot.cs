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
    public class Bot : Microsoft.Xna.Framework.GameComponent
    {
        public Bot(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public Vector2 Position;
        public Vector2 Speed;
        public float SpeedPower;
        public int Target;
        public Animation Sprite;
        //public Texture2D Sprite;
        public float Scale;
        public TimeSpan BlastTimer;
        public Color TintColor;
        public bool Dropped;
        public bool Blasted;
        public Texture2D Shadow;
        public Random randomsssss;
        public int BotIndex;
        public BotBlast botBlast;

        public void Initialize(Game1 game, int index)
        {
            // TODO: Add your initialization code here
            BotIndex = index;
            BlastTimer = new TimeSpan(0, 0, 6);
            Scale = 1f;
            TintColor = Color.White;
            randomsssss = new Random();
            Position.X = (float)(randomsssss.NextDouble() * game.graphics.PreferredBackBufferWidth);
            Position.Y = 500;
            Dropped = false;
            SpeedPower = 0.1f;

            //Speed = new Vector2(new Random().Next(-1, 1), new Random().Next(-1, 1));

            base.Initialize();
        }

        public void LoadBotAnimation(string directory, ContentManager content, Game1 game)
        {
            Sprite = new Animation(game);
            Sprite.LoadAnimationData(directory, content);
        }

        public void LoadBlastAnimation(string directory, ContentManager content, Game1 game)
        {
            botBlast = new BotBlast(game);
            botBlast.Initialize();
            botBlast.LoadAnimation(directory, content);
        }


        public void Update(GameTime gameTime, Game1 game, Player[] targets)//, Blast blast)
        {


            Target = 0;

            for (int r = 0; r < targets.Length; r++)
            {
                if (Vector2.Distance(Position, targets[Target].Position) > Vector2.Distance(Position, targets[r].Position))
                {
                    Target = r;
                }
            }

                Position = Vector2.SmoothStep(Position, targets[Target].Position, SpeedPower);

            
            //Speed.X = SpeedPower * (Vector2.Distance(Position, targets[Target].Position) / (targets[Target].Position.X - Position.X));
            //Speed.Y = SpeedPower * (Vector2.Distance(Position, targets[Target].Position) / (targets[Target].Position.Y - Position.Y));

            if (targets[Target].Position.X == Position.X) { Speed.X = 0; }
            if (targets[Target].Position.Y == Position.Y) { Speed.Y = 0; }

            //Position += Speed;

            BlastTimer -= gameTime.ElapsedGameTime;

            try { TintColor.R = (byte)(255 - BlastTimer.Milliseconds /10); } catch (Exception e) { }

            if (BlastTimer <= TimeSpan.Zero)
            {
                if (!Blasted)
                {
                    botBlast.Detonate(targets, Position);
                    Blasted = true;
                    Dropped = false;
                }
                if (BlastTimer <= TimeSpan.Zero)
                {
                    Blasted = false;
                    Initialize(game, BotIndex);
                }
            }

            base.Update(gameTime);
        }

        public void Drop(GameTime gameTime)
        {
            //Position.X += (pos.X - Position.X) / 20f;
            //Position.Y += (pos.Y - Position.Y) / 20f;

            if (Sprite.IsPlaying == false)
            {
                Sprite.Play();
            }

                Blasted = false;
                Dropped = true;
                //Position = pos; 
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Shadow, new Vector2(Position.X - 20 + Sprite.Position[0].X, Position.Y - 10 + Sprite.Position[0].Y), null, Color.Black, 0f, new Vector2(22, 23), Scale/1.12f, SpriteEffects.None, 1f);
            if (Sprite.IsPlaying == false)
            {
                sb.Draw(Sprite.Images[0], Position, null, Color.White, 0f, new Vector2(Sprite.Images[0].Width / 2, Sprite.Images[0].Height / 2), 1f, SpriteEffects.None, 1f);
            }
            Sprite.Draw(Position, sb);
            botBlast.Draw(sb);
            sb.End();
        }
    }
}