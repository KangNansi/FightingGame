using System;
using UnityEngine;

namespace LZFight.Scripting {
    public abstract class State : MiniScript {

        public abstract LZFighterFrame GetFrame();

    }
}
