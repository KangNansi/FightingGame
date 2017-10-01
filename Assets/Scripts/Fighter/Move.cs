using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [System.Serializable]
    public class Move : ScriptableObject
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

        public void Draw(Matrix4x4 position)
        {
            if (frames.Count > 0)
            {
                frames[currentFrame].Draw(position);
            }
        }
    }
}
