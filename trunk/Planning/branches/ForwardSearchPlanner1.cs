using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class ForwardSearchPlanner : Planner
    {
        private HeuristicFunction m_fHeuristic;
        private int cost;
        public ForwardSearchPlanner(Domain d, HeuristicFunction fHeuristic)
            : base(d)
        {
            m_fHeuristic = fHeuristic;
            cost = 0;
        }
        public override List<Action> Plan(Problem p)
        {
            PriorityQueue<StatePriorityItem, double> pq = new PriorityQueue<StatePriorityItem, double>();
            List<StatePriorityItem> visited = new List<StatePriorityItem>();
            StatePriorityItem spi = new StatePriorityItem(p.StartState);
            pq.Enqueue(spi, 0 + m_fHeuristic.h(p.StartState));
            //Dictionary<StatePriorityItem, double> closed = new Dictionary<StatePriorityItem, double>();
            //Dictionary<StatePriorityItem, int> g = new Dictionary<StatePriorityItem, int>();
            List<Action> plan = new List<Action>();
            int step=0;
            int visitPositiveCounter=0;
            int visitNegetiveCounter = 0;

            while(!pq.isEmpty())
            {
                StatePriorityItem s = pq.Dequeue();
                
                //g[s] = step;
                Boolean isGoalState = true;
                foreach (Proposition pro_g in p.Goal) { 
                    if(!s.state.Propositions.Contains(pro_g)){
                        isGoalState = false;
                        break;
                    }
                }
                if (isGoalState)
                {
                    return s.Actions;
                }
                visited.Add(s);
                foreach (Action a in m_dDomain.Actions) {
                    State s_tag = a.apply(s.state);
                    if (s_tag!=null)
                    {
                        cost++;
                        List<Action> newActions = new List<Action>(s.Actions);
                        newActions.Add(a);
                        StatePriorityItem s_tag_priority = new StatePriorityItem(s_tag, newActions);
                        bool visitedContainsItem = contains(visited, s_tag_priority);
                        //if ((!visited.Contains(s_tag_priority)) 
                        //        || (visited.Contains(s_tag_priority) && s_tag_priority.g() < s.g()))
                        //if (!visitedContainsItem){
                        //    visited.Add(s_tag_priority);
                        //}
                        if ((!visitedContainsItem)
                                || (visitedContainsItem && s_tag_priority.g() < s.g()))
                        {
                            pq.Enqueue(s_tag_priority, s.g() + m_fHeuristic.h(s.state));
                            //pq.Enqueue(s_tag_priority, s_tag_priority.g() + m_fHeuristic.h(s_tag_priority.state));
                            visitPositiveCounter++;                            
                        }
                        else {
                            visitNegetiveCounter++;
                        }
                    }
                }                
            }
            return plan;
        }

        private bool contains(List<StatePriorityItem> lst, StatePriorityItem itm){
            foreach (StatePriorityItem spi in lst) { 
                if(spi.Equals(itm)){
                    return true;
                }
            }
            return false;
        }

        public override int ComputationCost()
        {
            return cost;
        }
    }
} 
