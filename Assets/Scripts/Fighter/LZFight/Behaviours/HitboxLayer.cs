using UnityEngine;
using System.Collections.Generic;
using FightingGame;

[CreateAssetMenu()]
public class HitboxLayer : ScriptableObject {

    List<HitboxComponent> registered = new List<HitboxComponent>();

    private void Awake() {
        registered.Clear();
        Debug.Log("Layer cleared");
    }

    private void OnEnable() {
        registered.Clear();
        Debug.Log("Layer cleared");
    }

    public void Register(HitboxComponent component) {
        registered.Add(component);
    }

    public void ApplyHitbox(HitBox hitbox) {
        for(int i = 0; i < registered.Count; i++) {
            if(registered[i] == null)
            {
                registered.RemoveAt(i);
                i--;
            }
            else
            {
                registered[i].ReceiveHit(hitbox);
            }
        }
    }
}