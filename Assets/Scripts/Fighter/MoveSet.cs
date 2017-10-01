using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class Action
    {
        public string input;
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
    [CreateAssetMenu(fileName = "MoveSet", menuName = "MoveSet", order = 42)]
    public class MoveSet : ScriptableObject {
        List<Action> actions = new List<Action>();
	
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
