using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    abstract class Domain
    {
        public List<Action> Actions{ get; private set; }

        public Domain()
        {
            Actions = new List<Action>();
        }

        public abstract Problem GenerateRandomProblem(int iSeed);
    }
}
