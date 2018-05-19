using UnityEngine;
using System.Collections.Generic;

namespace LZFight {
    public class MiniScriptMachine {
        private LZFighter fighter;
        private List<MiniScript> scripts = new List<LZFight.MiniScript>();
        private List<MiniScript> toStart = new List<MiniScript>();

        public MiniScriptMachine(LZFighter fighter) {
            this.fighter = fighter;
        }

        public void Update(float deltaTime) {
            for(int i = 0; i < toStart.Count; i++) {
                toStart[i].OnStart();
            }
            toStart.Clear();
            for(int i = 0; i < scripts.Count; i++) {
                if (scripts[i].OnUpdate()) {
                    scripts[i].OnEnd();
                    scripts.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddScript(MiniScript script) {
            MiniScript newScript = Object.Instantiate(script);
            newScript.Initialize(fighter);
            toStart.Add(newScript);
            scripts.Add(newScript);
        }

        public T GetComponent<T>() where T : MiniScript {
            foreach(var s in scripts) {
                if(s is T) {
                    return s as T;
                }
            }
            return null;
        }
    }
}

