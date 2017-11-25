using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [System.Serializable]
    public class Move
    {
        [SerializeField]
        public List<FighterState> frames = new List<FighterState>();
        public int currentFrame = 0;
        public string name = "Unnamed";

        private float time = 0;
        
        public bool Compute(float delta)
        {
            time += delta;
            float p = 0;
            for(int i = 0; i < frames.Count; i++)
            {
                p += frames[i].time;
                if (p > time)
                {
                    currentFrame = i;
                    return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            time = 0;
            currentFrame = 0;
        }
        
        public FighterState GetFrame()
        {
            return frames[currentFrame];
        }

        float GetTotalTime()
        {
            float time = 0;
            foreach(FighterState fs in frames)
            {
                time += fs.time;
            }
            return time;
        }

        public FighterState GetFrame(float time)
        {
            time %= GetTotalTime();
            foreach(FighterState fs in frames)
            {
                time -= fs.time;
                if (time < 0)
                {
                    return fs;
                }
            }
            return null;
        }

        public float GetMaxHeight()
        {
            float height = 0;
            foreach(FighterState fs in frames)
            {
                if (fs.sprite && height < fs.sprite.rect.height)
                {
                    height = fs.sprite.rect.height;
                }
            }
            return height;
        }

        public void Draw(Matrix4x4 position)
        {
            if (frames.Count > 0)
            {
                frames[currentFrame].Draw(position);
            }
        }
    }
}
