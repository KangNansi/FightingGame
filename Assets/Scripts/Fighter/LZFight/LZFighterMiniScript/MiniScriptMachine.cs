using UnityEngine;
using System.Collections.Generic;
using System;

namespace LZFight {
    public class ScriptHandle
    {
        public Action Kill;
    }
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

        public ScriptHandle AddScript(MiniScript script) {
            MiniScript newScript = UnityEngine.Object.Instantiate(script);
            newScript.Initialize(fighter);
            toStart.Add(newScript);
            scripts.Add(newScript);
            return new ScriptHandle()
            {
                Kill = () => Remove(newScript)
            };
        }

        public ScriptHandle AddScript(List<MiniScript> scripts)
        {
            List<ScriptHandle> soloHandle = new List<ScriptHandle>();
            foreach(var s in scripts)
            {
                soloHandle.Add(AddScript(s));
            }
            return new ScriptHandle()
            {
                Kill = () => soloHandle.ForEach((h) => h.Kill())
            };
        }

        private void Remove(MiniScript script)
        {
            script.OnEnd();
            scripts.Remove(script);
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

