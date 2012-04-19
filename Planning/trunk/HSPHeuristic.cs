using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class HSPHeuristic : HeuristicFunction
    {
        private Domain m_dDomain;
        private List<Proposition> m_lGoal;
        private bool m_bMax;
        

        //the bMax flag is used to indicate using max or sum when computing a value for a set of propositions
        public HSPHeuristic(Domain d, List<Proposition> lGoal, bool bMax)
        {
            m_dDomain = d;
            m_lGoal = lGoal;
            m_bMax = bMax;            
        }

        public override double h(State s)
        {
            Dictionary<Proposition, double> observed = new Dictionary<Proposition, double>();
            foreach(Proposition p in s.Propositions){
                observed[p] = 0;
            }
            int d = 1;
            Boolean observedHasChaned = false;
            do
            {
                observedHasChaned = false;
                foreach (Action a in m_dDomain.Actions) { 
                    Boolean inObserved = true;
                    foreach(Proposition pre in a.Preconditions){
                        if(!observed.ContainsKey(pre)){
                            inObserved = false;
                            break;
                        }
                    }
                    if (inObserved)
                    {
                        foreach (Proposition q in a.AddList) 
                        {
                            if (!observed.ContainsKey(q))
                            {
                                observed[q] = d;
                                observedHasChaned = true;
                            }
                        }
                    }
                    
                }
                d++;
            } while (observedHasChaned);
            foreach (Proposition pro in m_lGoal) {
                if (!observed.ContainsKey(pro)) {
                    return Double.NegativeInfinity;
                }
            }
            if (m_bMax)
            {
                double max = Double.MinValue;
                foreach(Proposition p in observed.Keys){
                    if(m_lGoal.Contains(p)){
                        if(observed[p]>max){
                            max = observed[p];
                        }
                    }
                }
                return max;
            }
            else
            {
                double sum = 0.0;
                foreach (Proposition p in observed.Keys)
                {
                    if (m_lGoal.Contains(p))
                    {                        
                        sum += observed[p];                     
                    }
                }
                return sum;
            }
        }
    }
}
