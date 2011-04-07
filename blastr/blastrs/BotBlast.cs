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
    public class BotBlast : Microsoft.Xna.Framework.GameComponent
    {
        public BotBlast(Game game)
        : base(game)
        {
            // TODO: Construct any child components here
        }

        public Vector2 Position;
        public Circle Area;
        public float Radius;
        // the power of the blast, in pixels per frame
        public float Power;
        public Animation BlastAnimation;
        public Boolean isDetonating;

        public void Initialize()
        {
            Radius = 150;
            Power = 400;
            Area = new Circle(Position, Radius);
        }

        public void LoadAnimation(String directory, ContentManager content)
        {
            BlastAnimation = new Animation(Game);
            BlastAnimation.LoadAnimationData(directory, content);
        }

        public void Detonate(Player[] players, Vector2 Origin)
        {
            Position = Origin;
            Area.Center = Position;

            
            for (int r = 0; r < players.Length; r++)
            {
               if (Area.Intersects(new Rectangle((int)players[r].Position.X, (int)players[r].Position.Y, 1, 1)) == true)
               {
                   if (Math.Abs(players[r].Speed.X) < 0.3f) //IF PLAYER IS NOT MOVING, BLAST FAILS
                   {
                       players[r].Speed.X = 2f * Math.Sign(players[r].Speed.X);
                   }
                   if (Math.Abs(players[r].Speed.Y) < 0.3f)
                   {
                       players[r].Speed.Y = 2f * Math.Sign(players[r].Speed.Y);
                   }
                   players[r].Speed.X += (Power * ((players[r].Position.X - Position.X) / (Vector2.Distance(Position, players[r].Position))));
                   players[r].Speed.Y += (Power * ((players[r].Position.Y - Position.Y) / (Vector2.Distance(Position, players[r].Position))));
               }
            }

            isDetonating = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDetonating == true)
            {
                if (BlastAnimation.IsPlaying == false)
                {
                    BlastAnimation.Play();
                }
                BlastAnimation.Draw(Position, spriteBatch);
            }
            if (BlastAnimation.IsPlaying == false)
            {
                isDetonating = false;
                //BlastAnimation.CurrentFrame = 0;
            }
        }
    }
}
