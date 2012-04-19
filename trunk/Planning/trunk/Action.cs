using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class Action
    {
        public List<Proposition> Preconditions { get; private set; }
        public List<Proposition> AddList { get; private set; }
        public List<Proposition> RemoveList { get; private set; }
        public string Name { get; private set; }
        public Action( string sName )
        {
            Preconditions = new List<Proposition>();
            AddList = new List<Proposition>();
            RemoveList = new List<Proposition>();
            Name = sName;
            //Move(I, B, G)
        }

        public State apply(State s)
        {
            State tmpState = new State(s);
                       
            foreach (Proposition pre in Preconditions)
            {
                if (!tmpState.Propositions.Contains(pre))
                {
                   // Console.Out.Write("Precondition: "+ pre.ToString()+" is false");
                    return null;
                }
            }
            foreach (Proposition pre in RemoveList)
            {
                tmpState.Propositions.Remove(pre);
            }
            foreach (Proposition pre in AddList)
            {
                tmpState.Propositions.Add(pre);
            }
            return tmpState;
        }

     
      
        private static bool CompareSets(List<Proposition> l1, List<Proposition> l2)
        {
            if (l1.Count != l2.Count)
                return false;
            foreach (Proposition p in l1)
                if (!l2.Contains(p))
                    return false;
            return true;
        }


        public bool Equals(Action a2)
        {
            if (Object.ReferenceEquals(this, a2))
                return true;
            if (this == null || a2 == null)
                return false;
            if (this.Name != a2.Name)
                return false;
            if (!CompareSets(this.Preconditions, a2.Preconditions))
                return false;
            if (!CompareSets(this.AddList, a2.AddList))
                return false;
            if (!CompareSets(this.RemoveList, a2.RemoveList))
                return false;
            
            return true;
        }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            if (obj is Action)
                return Equals((Action)obj);
            return false;
        }
    }
}
