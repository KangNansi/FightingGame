using UnityEngine;
using System.Collections;

using LZFight;
using LZFight.Scripting;
using FightingGame;
using System.Collections.Generic;

[RequireComponent(typeof(LZFighterAnimator))]
public class HitboxComponent : LZFighterComponent {

    HitboxHandler handler;

    public HitboxLayer selfLayer;
    public List<HitboxLayer> hitLayers = new List<HitboxLayer>();


    public List<HitBox> defense;

    public bool debug = false;

    private void Awake() {
        OnFighterSet += () => {
            handler = fighter.GetComponent<HitboxHandler>();
        };
    }

    public void Start() {
        selfLayer.Register(this);
    }

    public void ReceiveHit(HitBox hitbox) {
        foreach(var d in defense) {
            HitBox worldSpace = new HitBox(d, transform);
            if (hitbox.Hit(worldSpace)) {
                Debug.Log("Hit! ");
                Debug.DrawLine(worldSpace._position, worldSpace._position + worldSpace._size, Color.green, 15f);
                Debug.DrawLine(hitbox._position, hitbox._position + hitbox._size, Color.magenta, 15f);
                handler.ReceiveHit(hitbox);
            }
        }
    }

    private void SendHit(HitBox hit) {
        foreach(var l in hitLayers) {
            HitBox worldSpace = new HitBox(hit, transform);
            l.ApplyHitbox(worldSpace);
        }
    }

    public void SetDefense(List<HitBox> def) {
        defense = def;
    }

    public void SetAttack(List<HitBox> attack) {
        foreach(var hb in attack) {
            SendHit(hb);
        }
    }
}
