using UnityEngine;
using System.Collections;
using FightingGame;

using LZFight;
using InputManager;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(LZFighterAnimator))]
public class LZFighterController : MonoBehaviour {
    public LZFightPlayer controller;
    private LZFighterAnimator animator;

    // Use this for initialization
    void Start() {
        animator = GetComponent<LZFighterAnimator>();
    }

    // Update is called once per frame
    void Update() {
        LZFIGHTERINPUTEVENT ev;
        if (controller.GetEvent(Time.deltaTime, out ev)) {
           // animator.ApplyEvent(ev);
        }
        else {
            //animator.ApplyEvent(LZFIGHTEREVENT.NONE);
        }
    }
}
