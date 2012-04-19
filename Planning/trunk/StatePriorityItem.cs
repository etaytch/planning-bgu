using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class StatePriorityItem
    {
        public StatePriorityItem(State s) {
            state = s;
            Actions = new List<Action>();
        }
        public StatePriorityItem(State s,List<Action> actions)
        {
            state = s;
            Actions = actions;
        }
        public List<Action> Actions { get; private set; }
        public State state { get; private set; }
        public int g() {
            return Actions.Count;
        }

        public void addAction(Action a) {
            Actions.Add(a);
        }

        public bool Equals(StatePriorityItem s2) {
            if (!state.Equals(s2.state))
            {
                return false;
            }            
            //if(Actions.Count!=s2.Actions.Count)
            //{
            //    return false;
            //}
            //foreach (Action a in s2.Actions) { 
            //    if (!Actions.Contains(a)){
            //        return false;
            //    }
            //}            
            return true;
        }
    }
}
