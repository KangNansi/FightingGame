using UnityEngine;
using System.Collections;
using LZFight;
using InputManager;

public class AIResolver : GlobalScript
{
    LZFighterAnimator opponent;
    LZFighterAnimator self;
    LZFightAI controller;

    public InputObject Get(LZFIGHTERINPUTEVENT e)
    {
        if(controller.pairs.Find((p ) => p.ev == e) == null)
        {

        }
        return null;
    }
}
