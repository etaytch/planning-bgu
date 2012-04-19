using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class BFSPlanner : Planner
    {
        private int cost;
        List<BFSNode> visited;
        public BFSPlanner(Domain d)
            : base(d)
        {
            cost = 0;
        }
        public override List<Action> Plan(Problem p)
        {

            visited = new List<BFSNode>();
            Queue<BFSNode> theQueue = initQueue(p);
            BFSNode tmpNode = null;
            do
            {                
                tmpNode = theQueue.Dequeue();
                insertChilds(p, theQueue, tmpNode, visited);
            } while (!((theQueue.Count == 0) || (isGoal(p, tmpNode))));
            List<Action> actions = tmpNode.collectActions();
            actions.Reverse(0, actions.Count);
            return actions;
        }

        private void insertChilds(Problem problem, Queue<BFSNode> queue, BFSNode tmpNode,List<BFSNode> visited)
        {            
            foreach (Action a in this.m_dDomain.Actions){
                State res;
                res = a.apply(tmpNode.m_state);
                if(res!=null){
                    //actions.Add(a);
                    BFSNode newNode = new BFSNode(tmpNode, res, a);
                    if (contains(visited,newNode)) {
                        break;
                    }
                    visited.Add(newNode);
                    queue.Enqueue(newNode);
                    cost++;
                }
            }
        }

        private bool contains(List<BFSNode> visited, BFSNode node) { 
            foreach(BFSNode b in visited){
                if(b.Equals(node)){
                    return true;
                }
            }
            return false;
        }

        private bool isGoal(Problem p, BFSNode node) { 
            foreach(Proposition pro in p.Goal){
                if (!node.m_state.Propositions.Contains(pro))
                {
                    return false;
                }
            }
            return true;
        }
        private Queue<BFSNode> initQueue(Problem p)
        {
            Queue<BFSNode> ans = new Queue<BFSNode>();
            BFSNode node = new BFSNode(p.StartState);
            ans.Enqueue(node);
            return ans;
        }
        private Boolean isSolution(Problem p) {
            foreach (Proposition g in p.Goal)
            {
                if (!p.StartState.Contains(g))
                {
                    return false;
                }
            }
            return true;
        }

        public override int ComputationCost()
        {
            //your implementaiton here
            return cost;
        }
    }
}
