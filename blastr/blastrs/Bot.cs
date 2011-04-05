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
        public int BotIndex;
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

                if (BotIndex == 1)
                {
                    Target = 3 - game.Bot[0].Target; //other bombot chases the opposite color; if blue then yellow
                }

                Position = Vector2.SmoothStep(Position, targets[Target].Position, SpeedPower);

            
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
                    Blasted = true;
                }
                if (BlastTimer <= -(new TimeSpan(0, 0, 0)))
                {
                    Blasted = false;
                    Initialize(game);
                }
            }

            base.Update(gameTime);
        }

        public void Drop(GameTime gameTime, Vector2 pos)
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