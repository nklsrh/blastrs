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

        public Int32 EndFrame;

        public enum DataType
        {
            Position,
            Scale,
            Opacity
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
                }
            }
        }
    }
}
