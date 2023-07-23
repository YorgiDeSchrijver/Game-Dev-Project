﻿using Microsoft.Xna.Framework;
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
        private bool hasCycleEnded = false;

        public Vector2 Origin { get; private set; }
        public AnimationFrame CurrentFrame { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Animation()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (currentCycle != null) 
            { 
                CurrentFrame = frames[currentCycle[counter]]; 
            } 
            else 
            { 
                CurrentFrame = frames[counter]; 
            }

            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
            int fps = 15;

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
            else { hasCycleEnded = false; }
        }

        public void SetTextureProperties(int width, int height, int numberOfWidthSprites, int numberOfHeightSprites, int numberOfSprites)
        {
            Width = width / numberOfWidthSprites;
            Height = height / numberOfHeightSprites;

            for (int y = 0; y < height; y += Height)
            {
                for (int x = 0; x < width; x += Width)
                {
                    if (frames.Count < numberOfSprites)
                    {
                        frames.Add(new AnimationFrame(new Rectangle(x, y, Width, Height)));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Origin = new Vector2(Width / 2.0f, Height);
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