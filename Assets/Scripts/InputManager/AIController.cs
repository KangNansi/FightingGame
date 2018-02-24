using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class AIController : MonoBehaviour, IController {

        FighterController controller;
        FighterController opponent;
        float reflexTimer = 0.0f;
        float sens = 0.0f;
        float currentHorizontal = 0.0f;
        float moveTimer = 0.0f;
        public float AIReflex = 0.1f;
        bool reflex = false;

        bool getReflex()
        {
            return reflex;
        }

        float distanceToOpponent()
        {
            return Vector3.Distance(controller.transform.position, opponent.transform.position);
        }

        float GetRandom()
        {
            return UnityEngine.Random.Range(0f, 100f);
        }

        public bool GetBackDashDown()
        {
            return false;
        }

        public bool GetBlockDown()
        {
            if (!getReflex())
            {
                return false;
            }
            return GetRandom() > 20f;
        }

        public bool GetBlockUp()
        {
            if (!getReflex())
            {
                return false;
            }
            return GetRandom() > 20f;
        }

        public bool GetDashDown()
        {
            return false;
        }

        public bool GetDPDown()
        {
            return false;
        }

        public bool GetHKDown()
        {
            return false;
        }

        public float GetHorizontal()
        {
            if(moveTimer < 0.1f)
            {
                return currentHorizontal;
            }
            if(opponent.ComboOverLife * GetRandom() > 30f)
            {
                currentHorizontal = -sens;
            }
            else if (distanceToOpponent() > 0.7f)
            {
                currentHorizontal = sens;
            }
            else
            {
                currentHorizontal = 0.0f;
            }
            moveTimer = 0;
            return currentHorizontal;
        }

        public float GetHorizontalS()
        {
            return 0.0f;
        }

        public bool GetJumpDown()
        {
            if (!getReflex())
            {
                return false;
            }
            return distanceToOpponent() > 3 && GetRandom() < 20f;
        }

        public bool GetKeyDown(VirtualController.Keys k)
        {
            if (!getReflex())
            {
                return false;
            }

            switch (k)
            {
                case VirtualController.Keys.P:
                    return GetRandom() < 20f && distanceToOpponent() < 1;
                case VirtualController.Keys.Taunt:
                    return GetRandom() * opponent.ComboOverLife > 20f && distanceToOpponent() > 2;
                case VirtualController.Keys.FP:
                    return GetRandom() < 10f && distanceToOpponent() < 2;
                case VirtualController.Keys.DP:
                    return GetRandom() < 15f && distanceToOpponent() < 1;
                case VirtualController.Keys.UP:
                    return GetRandom() < 15f && distanceToOpponent() < 1;
                case VirtualController.Keys.BP:
                    return GetRandom() < 40f && distanceToOpponent() < 0.5f;
                case VirtualController.Keys.Dash:
                    return GetRandom()*(1-opponent.ComboOverLife) > 5f && distanceToOpponent() > 3;
                case VirtualController.Keys.BackDash:
                    return GetRandom() * opponent.ComboOverLife > 15f && distanceToOpponent() < 2;
            }
            return false;
        }

        public bool GetPDown()
        {
            return false;
        }

        public bool GetUPDown()
        {
            return false;
        }

        public float GetVertical()
        {
            return 0.0f;
        }

        public void SetSens(float s)
        {
            sens = s;
        }

        // Use this for initialization
        void Start () {
            controller = GetComponent<FighterController>();
            opponent = controller.opponent;
            controller.controller = this;
	    }
	
	    // Update is called once per frame
	    void Update () {
            reflex = false;
            reflexTimer += Time.deltaTime;
            if (reflexTimer > AIReflex)
            {
                reflex = true;
                reflexTimer = 0;
            }
            moveTimer += Time.deltaTime;
	    }
    }
}
