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
using System.IO;

namespace blastrs
{
    public class Animation : Microsoft.Xna.Framework.GameComponent
    {
        public Animation(Game game)
            : base(game)
        {
            //add any child dependencies here
        }

        /// <summary>
        /// The images we will be animating
        /// </summary>
        public List<Texture2D> Images = new List<Texture2D>();

        /// <summary>
        /// The Data for each Position keyframe
        /// </summary>
        public List<List<Vector2>> Position_Data = new List<List<Vector2>>();
        /// <summary>
        /// The timestamp (in frames) of each keyframe
        /// </summary>
        public List<List<Int32>> Position_KeyFrame = new List<List<Int32>>();

        public List<List<Vector2>> Scale_Data = new List<List<Vector2>>();
        public List<List<Int32>> Scale_KeyFrame = new List<List<Int32>>();

        public List<List<float>> Opacity_Data = new List<List<float>>();
        public List<List<Int32>> Opacity_KeyFrame = new List<List<Int32>>();

        public List<List<Vector2>> Pivot_Data = new List<List<Vector2>>();
        public List<List<Int32>> Pivot_KeyFrame = new List<List<Int32>>();

        public List<List<float>> Rotation_Data = new List<List<float>>();
        public List<List<Int32>> Rotation_KeyFrame = new List<List<Int32>>();

        public Int32 EndFrame;

        public Int32 CurrentFrame;
        public Int32 PlayCount;
        public Boolean IsPlaying;

        public List<Vector2> Position = new List<Vector2>();
        public List<Vector2> Pivot = new List<Vector2>();
        public List<float> Opacity = new List<float>();
        public List<Vector2> Scale = new List<Vector2>();
        public List<float> Rotation = new List<float>();

        public enum DataType
        {
            Position,
            Scale,
            Opacity,
            Pivot,
            Rotation
        }

        /// <summary>
        /// Loads the animation into the game based on the animation data file (named anim.dat) given in the specified directory
        /// </summary>
        /// <param name="Directory">The location of the folder of the data file (e.g: C:\\TestProject)</param>
        /// <param name="content">The content manager to do the loading</param>
        public void LoadAnimationData(string Directory, ContentManager content)
        {
            StreamReader reader = new StreamReader(content.RootDirectory + "\\" + Directory + "\\" + "anim.dat");
            String line;
            DataType dataType = new DataType();
            
            //The index of our current image, is incremented by 1 every time a new image is found
            Int32 index = -1;

            while (reader.EndOfStream == false)
            {
                line = reader.ReadLine();
                if (line.StartsWith("##"))
                {
                    line = line.Remove(0, 2);
                    Images.Add(content.Load<Texture2D>(Directory + "\\" + line));

                    Position_Data.Add(new List<Vector2>());
                    Position_KeyFrame.Add(new List<Int32>());

                    Scale_Data.Add(new List<Vector2>());
                    Scale_KeyFrame.Add(new List<Int32>());

                    Opacity_Data.Add(new List<float>());
                    Opacity_KeyFrame.Add(new List<Int32>());

                    Pivot_Data.Add(new List<Vector2>());
                    Pivot_KeyFrame.Add(new List<Int32>());

                    Rotation_Data.Add(new List<float>());
                    Rotation_KeyFrame.Add(new List<Int32>());

                    Position.Add(Vector2.Zero);
                    Opacity.Add(1);
                    Scale.Add(Vector2.One);
                    Pivot.Add(Vector2.Zero);
                    Rotation.Add(0);

                    index += 1;
                }
                else if (line == "BEGIN " + DataType.Position.ToString())
                {
                    dataType = DataType.Position;
                }
                else if (line == "BEGIN " + DataType.Scale.ToString())
                {
                    dataType = DataType.Scale;
                }
                else if (line == "BEGIN " + DataType.Opacity.ToString())
                {
                    dataType = DataType.Opacity;
                }
                else if (line == "BEGIN " + DataType.Pivot.ToString())
                {
                    dataType = DataType.Pivot;
                }
                else if (line == "BEGIN " + DataType.Rotation.ToString())
                {
                    dataType = DataType.Rotation;
                }
                else if (line.StartsWith("<"))
                {
                    if (dataType == DataType.Position)
                    {
                        Position_KeyFrame[index].Add(Convert.ToInt32(line.Split('>').First().Split('<').Last()));
                        Position_Data[index].Add(new Vector2((float)Convert.ToDouble(line.Split(' ').Last().Split(',').First()), (float)Convert.ToDouble(line.Split(' ').Last().Split(',').Last())));

                        if (Position_KeyFrame[index][Position_KeyFrame[index].Count - 1] > EndFrame)
                        {
                            EndFrame = Position_KeyFrame[index][Position_KeyFrame[index].Count - 1];
                        }
                        if (Position_KeyFrame[index][0] != 0)
                        {
                            Position_KeyFrame[index].Add(Position_KeyFrame[index][0]);
                            Position_KeyFrame[index][0] = 0;
                            Position_Data[index].Add(Position_Data[index][0]);
                        }
                    }
                    else if (dataType == DataType.Opacity)
                    {
                        Opacity_KeyFrame[index].Add(Convert.ToInt32(line.Split('>').First().Split('<').Last()));
                        Opacity_Data[index].Add((float)Convert.ToDouble(line.Split(' ').Last()));

                        if (Opacity_KeyFrame[index][Opacity_KeyFrame[index].Count - 1] > EndFrame)
                        {
                            EndFrame = Opacity_KeyFrame[index][Opacity_KeyFrame[index].Count - 1];
                        }
                        if (Opacity_KeyFrame[index][0] != 0)
                        {
                            Opacity_KeyFrame[index].Add(Opacity_KeyFrame[index][0]);
                            Opacity_KeyFrame[index][0] = 0;
                            Opacity_Data[index].Add(Opacity_Data[index][0]);
                        }
                    }
                    else if (dataType == DataType.Scale)
                    {
                        Scale_KeyFrame[index].Add(Convert.ToInt32(line.Split('>').First().Split('<').Last()));
                        Scale_Data[index].Add(new Vector2((float)Convert.ToDouble(line.Split(' ').Last().Split(',').First()), (float)Convert.ToDouble(line.Split(' ').Last().Split(',').Last())));

                        if (Scale_KeyFrame[index][Scale_KeyFrame[index].Count - 1] > EndFrame)
                        {
                            EndFrame = Scale_KeyFrame[index][Scale_KeyFrame[index].Count - 1];
                        }
                        if (Scale_KeyFrame[index][0] != 0)
                        {
                            Scale_KeyFrame[index].Add(Scale_KeyFrame[index][0]);
                            Scale_KeyFrame[index][0] = 0;
                            Scale_Data[index].Add(Scale_Data[index][0]);
                        }
                    }
                    else if (dataType == DataType.Pivot)
                    {
                        Pivot_KeyFrame[index].Add(Convert.ToInt32(line.Split('>').First().Split('<').Last()));
                        Pivot_Data[index].Add(new Vector2((float)Convert.ToDouble(line.Split(' ').Last().Split(',').First()), (float)Convert.ToDouble(line.Split(' ').Last().Split(',').Last())));

                        if (Pivot_KeyFrame[index][Pivot_KeyFrame[index].Count - 1] > EndFrame)
                        {
                            EndFrame = Pivot_KeyFrame[index][Pivot_KeyFrame[index].Count - 1];
                        }
                        if (Pivot_KeyFrame[index][0] != 0)
                        {
                            Pivot_KeyFrame[index].Add(Pivot_KeyFrame[index][0]);
                            Pivot_KeyFrame[index][0] = 0;
                            Pivot_Data[index].Add(Pivot_Data[index][0]);
                        }
                    }
                    else if (dataType == DataType.Rotation)
                    {
                        Rotation_KeyFrame[index].Add(Convert.ToInt32(line.Split('>').First().Split('<').Last()));
                        Rotation_Data[index].Add((float)Convert.ToDouble(line.Split(' ').Last()));

                        if (Rotation_KeyFrame[index][Rotation_KeyFrame[index].Count - 1] > EndFrame)
                        {
                            EndFrame = Rotation_KeyFrame[index][Rotation_KeyFrame[index].Count - 1];
                        }
                        if (Rotation_KeyFrame[index][0] != 0)
                        {
                            Rotation_KeyFrame[index].Add(Rotation_KeyFrame[index][0]);
                            Rotation_KeyFrame[index][0] = 0;
                            Rotation_Data[index].Add(Rotation_Data[index][0]);
                        }
                    }
                }
            }
            reader.Close();

            for (int i = 0; i < Images.Count; i++)
            {
                try
                {
                    if (Position_KeyFrame[i].Last() != EndFrame)
                    {
                        Position_KeyFrame[i].Add(EndFrame);
                        Position_Data[i].Add(Position_Data[i].Last());
                    }
                }
                catch (Exception e)
                {
                    Position_KeyFrame[i].Add(0);
                    Position_KeyFrame[i].Add(EndFrame);
                    Position_Data[i].Add(new Vector2(0,0));
                    Position_Data[i].Add(new Vector2(0,0));
                }

                try
                {
                    if (Opacity_KeyFrame[i].Last() != EndFrame)
                    {
                        Opacity_KeyFrame[i].Add(EndFrame);
                        Opacity_Data[i].Add(Opacity_Data[i].Last());
                    }
                }
                catch (Exception e)
                {
                    Opacity_KeyFrame[i].Add(0);
                    Opacity_KeyFrame[i].Add(EndFrame);
                    Opacity_Data[i].Add(1);
                    Opacity_Data[i].Add(1);
                }

                try
                {
                    if (Scale_KeyFrame[i].Last() != EndFrame)
                    {
                        Scale_KeyFrame[i].Add(EndFrame);
                        Scale_Data[i].Add(Scale_Data[i].Last());
                    }
                }
                catch (Exception e)
                {
                    Scale_KeyFrame[i].Add(0);
                    Scale_KeyFrame[i].Add(EndFrame);
                    Scale_Data[i].Add(new Vector2(1, 1));
                    Scale_Data[i].Add(new Vector2(1,1));
                }

                try
                {
                    if (Pivot_KeyFrame[i].Last() != EndFrame)
                    {
                        Pivot_KeyFrame[i].Add(EndFrame);
                        Pivot_Data[i].Add(Pivot_Data[i].Last());
                    }
                }
                catch (Exception e)
                {
                    Pivot_KeyFrame[i].Add(0);
                    Pivot_KeyFrame[i].Add(EndFrame);
                    Pivot_Data[i].Add(new Vector2(0, 0));
                    Pivot_Data[i].Add(new Vector2(0,0));
                }

                try
                {
                    if (Rotation_KeyFrame[i].Last() != EndFrame)
                    {
                        Rotation_KeyFrame[i].Add(EndFrame);
                        Rotation_Data[i].Add(Rotation_Data[i].Last());
                    }
                }
                catch (Exception e)
                {
                    Rotation_KeyFrame[i].Add(0);
                    Rotation_KeyFrame[i].Add(EndFrame);
                    Rotation_Data[i].Add(0);
                    Rotation_Data[i].Add(0);
                }
            }
        }

        /// <summary>
        /// plays the given animation and draws it with respect to the parent object
        /// </summary>
        /// <param name="parent"></param>
        public void Draw(Player parent, SpriteBatch spriteBatch)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                for (int j = 0; j < Position_KeyFrame[i].Count; j++)
                {
                    if (Position_KeyFrame[i][j] > CurrentFrame)
                    {
                        Position[i] = Vector2.Lerp(Position_Data[i][j - 1], Position_Data[i][j], (float)(Convert.ToDouble(CurrentFrame - (Position_KeyFrame[i][j - 1])) / Convert.ToDouble(Position_KeyFrame[i][j] - Position_KeyFrame[i][j - 1])));
                        break;
                    }
                }
                for (int j = 0; j < Scale_KeyFrame[i].Count; j++)
                {
                    if (Scale_KeyFrame[i][j] > CurrentFrame)
                    {
                        Scale[i] = Vector2.Lerp(Scale_Data[i][j - 1], Scale_Data[i][j], (float)(Convert.ToDouble(CurrentFrame - (Scale_KeyFrame[i][j - 1])) / Convert.ToDouble(Scale_KeyFrame[i][j] - Scale_KeyFrame[i][j - 1])));
                        break;
                    }
                }
                for (int j = 0; j < Pivot_KeyFrame[i].Count; j++)
                {
                    if (Pivot_KeyFrame[i][j] > CurrentFrame)
                    {
                        Pivot[i] = Vector2.Lerp(Pivot_Data[i][j - 1], Pivot_Data[i][j], (float)(Convert.ToDouble(CurrentFrame - (Pivot_KeyFrame[i][j - 1])) / Convert.ToDouble(Pivot_KeyFrame[i][j] - Pivot_KeyFrame[i][j - 1])));
                        break;
                    }
                }
                for (int j = 0; j < Opacity_KeyFrame[i].Count; j++)
                {
                    if (Opacity_KeyFrame[i][j] > CurrentFrame)
                    {
                        Opacity[i] = MathHelper.Lerp(Opacity_Data[i][j - 1], Opacity_Data[i][j], (float)(Convert.ToDouble(CurrentFrame - (Opacity_KeyFrame[i][j - 1])) / Convert.ToDouble(Opacity_KeyFrame[i][j] - Opacity_KeyFrame[i][j - 1])));
                        break;
                    }
                }
                for (int j = 0; j < Rotation_KeyFrame[i].Count; j++)
                {
                    if (Rotation_KeyFrame[i][j] > CurrentFrame)
                    {
                        Rotation[i] = MathHelper.Lerp(Rotation_Data[i][j - 1], Rotation_Data[i][j], (float)(Convert.ToDouble(CurrentFrame - (Rotation_KeyFrame[i][j - 1])) / Convert.ToDouble(Rotation_KeyFrame[i][j] - Rotation_KeyFrame[i][j - 1])));
                        break;
                    }
                }

                spriteBatch.Begin();
                spriteBatch.Draw(Images[i], Position[i], null, new Color(1, 1, 1, Opacity[i]), MathHelper.ToRadians(Rotation[i]), Pivot[i], Scale[i], SpriteEffects.None, 1);
                spriteBatch.End();
            }

            CurrentFrame += 1;

            if (CurrentFrame > EndFrame)
            {
                IsPlaying = false;
                CurrentFrame = 0;
            }
        }

        public void Draw(Vector2 parent, SpriteBatch spriteBatch)
        {

        }

        public void Play()
        {
            if (IsPlaying == false)
            {
                IsPlaying = true;
            }
        }
    }
}
