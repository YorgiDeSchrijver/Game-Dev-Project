using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Animations
{
    public class Animation
    {
        private readonly List<AnimationFrame> frames;
        private List<AnimationCycle> cycles;
        private int[] currentCycle;
        private int counter = 0;
        private double secondCounter = 0;
        private bool isLooping = false;
        public bool hasCycleEnded { get; private set;} = false;

        public Vector2 Origin { get; private set; }
        public AnimationFrame CurrentFrame { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Animation()
        {
            frames = new List<AnimationFrame>();
            cycles = new List<AnimationCycle>();
            CurrentFrame = new AnimationFrame(new Rectangle(0,0,0,0));
        }

        public void Update(GameTime gameTime)
        {
            int fps = 10;
            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (currentCycle != null) 
            { 
                CurrentFrame = frames[currentCycle[counter]];
                if (secondCounter >= 1d / fps)
                {
                    counter++;
                    secondCounter = 0;
                }
                if (counter == currentCycle.Length)
                {
                    if (isLooping)
                    {
                        counter = 0;
                    }
                    else
                    {
                        counter = currentCycle.Length - 1;
                        hasCycleEnded = true;
                    }
                }
                else 
                { 
                    hasCycleEnded = false; 
                }
            } 
            else 
            { 
                CurrentFrame = frames[counter]; 
            }
        }

        public void SetTextureProperties(int width, int height, int spriteSize, int numberOfSprites)
        {
            
            for (int y = 0; y < height; y += spriteSize)
            {
                for (int x = 0; x < width; x += spriteSize)
                {
                    if (frames.Count < numberOfSprites)
                    {
                        frames.Add(new AnimationFrame(new Rectangle(x, y, spriteSize, spriteSize)));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Origin = new Vector2(spriteSize / 2.0f, spriteSize);
        }

        public void SetAnimationCycles(string name)
        {
            using StreamReader r = new(name);
            string json = r.ReadToEnd();
            cycles = JsonConvert.DeserializeObject<List<AnimationCycle>>(json);
        }

        public void Play(string cycle)
        {
            if (isLooping || hasCycleEnded || this.currentCycle == null)
            {
                foreach (AnimationCycle item in cycles)
                {
                    if (item.Cycle == cycle)
                    {
                        if (this.currentCycle != item.Frames)
                        {
                            this.currentCycle = item.Frames;
                            this.isLooping = item.Looping;
                            hasCycleEnded = false;
                            counter = 0;
                        }
                    }
                }
            }
        }
    }
}
