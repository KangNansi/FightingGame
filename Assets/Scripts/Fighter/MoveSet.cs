using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [System.Serializable]
    public class Action
    {
        public VirtualController.Keys input;
        public int state;

        public Action()
        {

        }

        public Action(Action t)
        {
            input = t.input;
            state = t.state;
        }
    }
    [System.Serializable]
    public class Node
    {
        public int moveId = 0;
        [SerializeField]
        public List<Action> actions = new List<Action>();
    }

    [System.Serializable]
    public class MoveSet {
        [SerializeField]
        public List<Action> actions = new List<Action>();
        [SerializeField]
        public List<Node> nodes = new List<Node>();
	
        List<Action> GetInputs()
        {
            return actions;
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }
    }
}
