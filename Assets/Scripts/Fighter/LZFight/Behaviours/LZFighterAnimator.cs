using UnityEngine;
using System.Collections;
using FightingGame;

using LZFight;
using System;
using InputManager;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class LZFighterAnimator : MonoBehaviour {
    public LZFighter fighterObject;
    [NonSerialized]
    public LZFighter fighter;
    public LZFightPlayer controller;
    public bool debug = false;
    public bool allowInput = true;

    private Vector3 baseScale;

    private LZFighterComponent[] components;

    public event System.Action onInitialize;

    // Use this for initialization
    void Start() {
        baseScale = transform.localScale;
        Initialize();
    }

    // Update is called once per frame
    void Update() {
        fighter.blockInput = !allowInput;
        fighter.debug = debug;
        if (fighter.invertHorizontal) {
            transform.localScale = new Vector3(-baseScale.x, baseScale.y, baseScale.z);
        }
        else {
            transform.localScale = baseScale;
        }
        fighter.Refresh(Time.deltaTime);
        //fighter.stateMachine.LogPath();
    }

    public void Initialize() {
        fighter = Instantiate(fighterObject);
        fighter.Initialize(controller, gameObject);
        components = GetComponents<LZFighterComponent>();
        foreach (var c in components) {
            c.SetFighter(fighter);
        }
        if(onInitialize != null) {
            onInitialize.Invoke();
        }
    }

    public Vector2 GetVelocity() {
        return fighter.GetVelocity();
    }

    private void OnRenderObject() {
        LZFighterFrame frame = fighter.CurrentFrame;
        GetComponent<SpriteRenderer>().sprite = frame.sprite;
        if (debug) {
            Debug.DrawLine(transform.position, transform.position + transform.up * 3, fighter.stateMachine.allowInput ? Color.blue : Color.red);
            foreach (HitBox hb in frame.hitboxes) {
                hb.Draw(transform.localToWorldMatrix);
            }
        }
    }

    private void OnDrawGizmos() {
        //OnRenderObject();
    }
}
