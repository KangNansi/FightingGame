using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LZFight;
using UnityEngine;

namespace InputManager {
    [CreateAssetMenu()]
    public class LZFightAI : LZFightPlayer{

        

        public override InputObject GetInput(LZFIGHTERINPUTEVENT ev) {
            return pairs.Find((p) => p.ev == ev).input;
        }

        public void MakeInput(LZFIGHTERINPUTEVENT ev, InputObject input)
        {
            EventInputPair newPair = new EventInputPair();
            newPair.ev = ev;
            newPair.input = input;
        }



    }
}
