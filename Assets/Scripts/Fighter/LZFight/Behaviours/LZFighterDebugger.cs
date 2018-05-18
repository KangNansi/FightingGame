using UnityEngine;
using System.Collections;
using FightingGame;

using LZFight;
using System;
using InputManager;
using System.Collections.Generic;

[RequireComponent(typeof(LZFighterAnimator))]
public class LZFighterDebugger : LZFighterComponent {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public LZFighter GetFighter() {
        return fighter;
    }
}
