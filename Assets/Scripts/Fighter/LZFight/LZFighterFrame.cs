using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FightingGame;
using UnityEngine;

namespace LZFight {
    [System.Serializable]
    public class LZFighterFrame {

        public enum Type {
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
        bool computed = false;
        public bool Computed {
            get {
                return computed;
            }
        }

        [SerializeField]
        public List<MiniScript> scripts = new List<MiniScript>(); 

        public LZFighterFrame() {

        }

        public LZFighterFrame(LZFighterFrame source) {
            foreach (HitBox hb in source.hitboxes) {
                hitboxes.Add(new HitBox(hb));
            }
            sprite = source.sprite;
            time = source.time;
            velocity = source.velocity;
        }
    }
}
