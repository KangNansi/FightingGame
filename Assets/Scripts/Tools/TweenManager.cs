using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class TweenExtensions {
    public static void TweenPosition(this Transform t, Vector3 target, float duration) {
        Vector3 start = t.position;
        float time = 0f;
        TweenManager.Instance.Add(() => {
            time += Time.deltaTime;
            float l = time / duration;
            t.position = Vector3.Lerp(start, target, l);
            return time > duration;
        });
    }
}

public class TweenManager : MonoBehaviour {
    private static TweenManager instance = null;
    public static TweenManager Instance {
        get {
            if(instance == null) {
                instance = new GameObject().AddComponent<TweenManager>();
            }
            return instance;
        }
    }

    List<Func<bool>> tweens = new List<Func<bool>>();

    // Use this for initialization
    void Start() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update() {
        for(int i = 0; i < tweens.Count; i++) {
            if (tweens[i].Invoke()) {
                tweens.RemoveAt(i);
                i--;
            }
        }
    }

    public void Add(Func<bool> tween) {
        tweens.Add(tween);
    }
}
