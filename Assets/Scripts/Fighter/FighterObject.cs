using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FightingGame
{
    public class FighterObject : ScriptableObject {

        [SerializeField]
        public MoveSet moveSet = new MoveSet();
        [SerializeField]
        public List<Move> moves = new List<Move>();

        public float speed;
        public int currentState = 0;
        public bool drawHitbox = true;
        public bool Grounded = false;

        public int Stand = 0;
        public int Crouch = 0;
        public int Walk = 0;

        public bool running = false;

        public Vector3 velocity = new Vector3();
        public float jumpStrength = 30;
        public bool jumped = false;

        public bool Standing()
        {
            if (currentState == Stand || currentState == Walk) return true;
            return false;
        }

        public void SetMove(int newMove)
        {
            currentState = newMove;
            moves[currentState].Reset();
        }

        public FighterState GetFrame()
        {
            return moves[currentState].GetFrame();
        }

        public Move GetMove()
        {
            return moves[currentState];
        }
    }
}
