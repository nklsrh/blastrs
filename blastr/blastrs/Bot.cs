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
        public Texture2D Sprite;
        public float Scale;
        public TimeSpan BlastTimer;
        public Color TintColor;
        public bool Dropped;
        public bool Blasted;
        public Texture2D Shadow;
        public Random randomsssss;

        public BotBlast botBlast;

        public void Initialize(Game1 game)
        {
            // TODO: Add your initialization code here
            BlastTimer = new TimeSpan(0, 0, 2);
            Scale = 1f;
            TintColor = Color.White;
            randomsssss = new Random(123123);
            Position.X = randomsssss.Next(game.graphics.PreferredBackBufferWidth / 2 - 200, game.graphics.PreferredBackBufferWidth / 2 + 200);
            Position.Y = 0;
            Dropped = false;
            SpeedPower = 0.1f;

            //Speed = new Vector2(new Random().Next(-1, 1), new Random().Next(-1, 1));

            base.Initialize();
        }

        public void LoadBlastAnimation(string directory, ContentManager content, Game1 game)
        {
            botBlast = new BotBlast(game); ;
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

            Position = Vector2.SmoothStep(Position, targets[Target].Position, SpeedPower); //WTF HAX?

            //Speed.X = SpeedPower * (Vector2.Distance(Position, targets[Target].Position) / (targets[Target].Position.X - Position.X));
            //Speed.Y = SpeedPower * (Vector2.Distance(Position, targets[Target].Position) / (targets[Target].Position.Y - Position.Y));

            if (targets[Target].Position.X == Position.X) { Speed.X = 0; }
            if (targets[Target].Position.Y == Position.Y) { Speed.Y = 0; }

            //Position += Speed;

            BlastTimer -= gameTime.ElapsedGameTime;

            try { TintColor.R = (byte)(255 - BlastTimer.Milliseconds /10); } catch { }

            if (BlastTimer <= TimeSpan.Zero)
            {
                if (!Blasted)
                {
                    botBlast.Detonate(targets, Position);
                    //blast.Position = Position;
                    //blast.Direction = Vector2.Multiply(Speed, 100f);
                    //blast.blastTime = new TimeSpan(0, 0, 1);
                    //blast.Power = 1000;
                    //blast.Ready = false;
                 
                }
                Blasted = true;
                

                if (BlastTimer <= -(new TimeSpan(0, 0, 2)))
                {
                    Dropped = false;
                    Initialize(game);
                }
            }

            base.Update(gameTime);
        }

        public void Drop(GameTime gameTime, Vector2 pos) //MUST FIX THE WIDTH AND HEIGHT THINGY IT WAS USING UP TOO MUCH CPU IVE SET IT TO DEFAULT 1366 by 768
        {
            Position.X += (pos.X - Position.X) / 20f;
            Position.Y += (pos.Y - Position.Y) / 20f;
            if (Position.Y >= pos.Y - 4)
            {
                Dropped = true;
                Position = pos; 
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Shadow, new Vector2(Position.X - 20, Position.Y - 10), null, Color.Black, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), Scale/1.12f, SpriteEffects.None, 1f);
            sb.Draw(Sprite, Position, null, Color.White, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            botBlast.Draw(sb);
            sb.End();
        }
    }
}