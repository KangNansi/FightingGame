using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [System.Serializable]
    public class FighterState {
        public string name = "";
        public List<HitBox> hitboxes = new List<HitBox>();

        public FighterState()
        {
            hitboxes.Add(new HitBox(HitBox.Type.Body, Vector2.zero, Vector2.one));
        }

        public void Draw(Matrix4x4 position)
        {
            foreach(HitBox h in hitboxes)
            {
                h.Draw(position);
            }
        }
    }
}
