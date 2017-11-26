using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [System.Serializable]
    public class FighterState {
        public string name = "";
        public enum Type
        {
            StartingFrame,
            AttackFrame,
            RecoveryFrame
        }
        public Type frameType = Type.RecoveryFrame;
        [SerializeField]
        public List<HitBox> hitboxes = new List<HitBox>();
        public Sprite sprite;
        public float time = 0.08f;
        public Vector2 velocity = new Vector2();

        public FighterState()
        {
            //hitboxes.Add(new HitBox(HitBox.Type.Body, Vector2.zero, Vector2.one));
        }

        public void Hit()
        {

        }

        public List<HitBox> GetHitbox(HitBox.Type type)
        {
            return hitboxes.FindAll((HitBox h) => h._type == type);
        }

        public void Draw(Matrix4x4 position)
        {
            foreach(HitBox h in hitboxes)
            {
                h.Draw(position);
            }
        }

        public void CopyFrom(FighterState fs)
        {
            foreach(HitBox hb in fs.hitboxes)
            {
                hitboxes.Add(new HitBox(hb));
            }
            sprite = fs.sprite;
            time = fs.time;
        }
    }
}
