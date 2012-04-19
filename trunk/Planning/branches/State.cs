using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class State
    {
        public List<Proposition> Propositions { get; private set; }
        public State()
        {
            Propositions = new List<Proposition>();
        }
        public State(State s)
        {
            Propositions = new List<Proposition>(s.Propositions);
        }
        public bool Contains(Proposition p)
        {
            return Propositions.Contains(p);
        }

        public void AddProposition(Proposition p)
        {
            if( !Propositions.Contains(p) )
                Propositions.Add(p);
        }

        public void RemoveProposition(Proposition p)
        {
            Propositions.Remove(p);
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

        public bool Equals(State s2)
        {
            if (Object.ReferenceEquals(this, s2))
                return true;
            if (null == this || null == s2)
                return false;
            return CompareSets(Propositions, s2.Propositions);
        }

        public bool Contains(List<Proposition> lPropositions)
        {
            foreach (Proposition p in lPropositions)
                if (!Propositions.Contains(p))
                    return false;
            return true;
        }
        public override string ToString()
        {
            string s = "";
            foreach (Proposition p in Propositions)
                s += p + " ";
            return s;
        }
        public override bool Equals(object obj)
        {
            if (obj is State)
                return Equals((State)obj);
            return false;
        }
    }
}
