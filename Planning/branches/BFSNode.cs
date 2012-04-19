using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class BFSNode
    {
        public BFSNode m_parent;
        public State m_state;
        public Action m_action;
        

        public BFSNode(BFSNode parent, State state, Action action)
        {
            m_parent = parent;
            m_state = state;
            m_action = action;
        }
        public BFSNode(State state)
        {
            m_parent = null;
            m_state = state;
            m_action = null;
        }

        public bool Equals(BFSNode other) {
            if ((m_action != null) && (other.m_action!=null) && (!m_action.Equals(other.m_action)))
            {
                return false;
            }
            if ((m_action != null) && (other.m_action == null))
            {
                return false;
            }
            if ((m_action == null) && (other.m_action != null))
            {
                return false;
            }
            //if ((m_parent != null) && (other.m_parent != null) && (!m_parent.Equals(other.m_parent)))
            //{
            //    return false;
            //}
            //if ((m_parent != null) && (other.m_parent == null))
            //{
            //    return false;
            //}
            //if ((m_parent == null) && (other.m_parent != null))
            //{
            //    return false;
            //}
            if ((m_state != null) && (other.m_state != null) && (!m_state.Equals(other.m_state)))
            {
                return false;
            }
            if ((m_state != null) && (other.m_state == null))
            {
                return false;
            }
            if ((m_state == null) && (other.m_state != null))
            {
                return false;
            }
            return true;
        }

        public List<Action> collectActions()
        {
            List<Action> ans = new List<Action>();
            if (m_action != null)
            {
                ans.Add(m_action);
                if (m_parent != null)
                {
                    ans.AddRange(m_parent.collectActions());
                }
            }
            return ans;            
        }
    }
}
