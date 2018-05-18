using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LZFight {
    [System.Serializable]
    public class LZFighterState {
        // Editor Part
#if UNITY_EDITOR
        public Rect nodeRect = new Rect(0, 0, 100, 100);
        public bool isDragged = false;
#endif
        public int ID = -1;

        public Vector2 velocity;

        public string stateName = "";

        public List<int> transitionID = new List<int>();
        public bool invert = false;
        public LZFighterStateData data = null;

        public LZFighterState(int ID, string stateName) {
            this.ID = ID;
            this.stateName = stateName;
        }


    }
}
