using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class Problem
    {
        public State StartState { get; private set; }
        public List<Proposition> Goal { get; private set; }
        public Problem(State s0, List<Proposition> lGoal)
        {
            Goal = lGoal;
            StartState = s0;
        }
    }
}
