using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GlobalScriptMachine : MonoBehaviour {
    private static GlobalScriptMachine instance;
    public static GlobalScriptMachine Instance {
        get {
            if(instance == null) {
                instance = new GameObject().AddComponent<GlobalScriptMachine>();
            }
            return instance;
        }
    }

    List<IScript> running = new List<IScript>();
    List<IScript> starting = new List<IScript>();
    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update() {
        for(int i = 0; i < starting.Count; i++) {
            starting[i].Start();
        }
        starting.Clear();

        Debug.Log(running.Count);
        for(int i = 0; i < running.Count; i++) {
            if (running[i].Update()) {
                running[i].End();
                running.RemoveAt(i);
                i--;
            }
        }
    }

    public static GlobalScript Launch(GlobalScript script) {
        Instance.Run(script);
        return script;
    }

    public static void Launch(ScriptableScript script) {
        Instance.Run(script.Get());
    }

    public static void Launch(List<ScriptableScript> scripts) {
        foreach (var script in scripts) {
            Instance.Run(script.Get());
        }
    }

    public static void Launch(List<GlobalScript> scripts) {
        foreach (var script in scripts) {
            Instance.Run(script);
        }
    }


    private void Run(IScript script) {
        Debug.Log("Adding");
        starting.Add(script);
        running.Add(script);
        running.Sort((a, b) => a.GetPriority() - b.GetPriority());
    }
}
