using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class BlocksWorld : Domain
    {
        private int m_cBlocks;
        private string[] m_aBlocks;
        public BlocksWorld( int cBlocks )
        {
            m_cBlocks = cBlocks;
            m_aBlocks = new string[cBlocks];
            CreateBlocksWorld(m_cBlocks, Actions);
        }
        
        private Proposition CreateOnProposition(string sOn, string sUnder)
        {
            Proposition pOn = new Proposition("On");
            pOn.Objects.Add(sOn);
            pOn.Objects.Add(sUnder);
            return pOn;
        }

        private Proposition CreateClearProposition(string sCube)
        {
            Proposition pClear = new Proposition("Clear");
            pClear.Objects.Add(sCube);
            return pClear;
        }


        private Action CreateMoveAction(string sMovingCube, string sFrom, string sTo)
        {
            Action aMove = new Action("Move(" + sMovingCube + ", " + sFrom + ", " + sTo + ")");
            //preconditions - On(A,B), Clear(A), if C is not the table Clear(C)
            aMove.Preconditions.Add(CreateOnProposition(sMovingCube,sFrom));
            aMove.Preconditions.Add(CreateClearProposition(sMovingCube));
            if (sTo != "Table")
            {
                aMove.Preconditions.Add(CreateClearProposition(sTo));
            }
            //Add list - On(A,C), if B is not the table clear(B) 
            aMove.AddList.Add(CreateOnProposition(sMovingCube, sTo));
            if (sFrom != "Table")
            {
                aMove.AddList.Add(CreateClearProposition(sFrom));
            }
            //Remove list - On(A,B), if C is not the table clear(C) 
            aMove.RemoveList.Add(CreateOnProposition(sMovingCube, sFrom));
            if (sTo != "Table")
            {
                aMove.RemoveList.Add(CreateClearProposition(sTo));
            }
            return aMove;
        }

        private void CreateBlocksWorld(int cBlocks, List<Action> lActions)
        {
            for( int i = 0 ; i < cBlocks ; i++ )
            {
                m_aBlocks[i] = "" + (char)('A' + i);
            }
            foreach (string sMovingBlock in m_aBlocks)
            {
                foreach (string sFromBlock in m_aBlocks)
                {
                    if (sFromBlock != sMovingBlock)
                    {
                        foreach (string sToBlock in m_aBlocks)
                        {
                            if (sToBlock != sFromBlock && sToBlock != sMovingBlock)
                            {
                                lActions.Add(CreateMoveAction(sMovingBlock, sFromBlock, sToBlock));
                            }
                        }
                        lActions.Add(CreateMoveAction(sMovingBlock, "Table", sFromBlock));
                        lActions.Add(CreateMoveAction(sMovingBlock, sFromBlock, "Table"));
                    }
                }
            }
            /*
            s0.AddProposition(CreateOnProposition("A", "B"));
            s0.AddProposition(CreateOnProposition("B", "Table"));
            s0.AddProposition(CreateOnProposition("C", "D"));
            s0.AddProposition(CreateOnProposition("D", "Table"));
            s0.AddProposition(CreateClearProposition("A"));
            s0.AddProposition(CreateClearProposition("C"));
            lGoal.Add(CreateOnProposition("C", "D"));
            lGoal.Add(CreateOnProposition("D", "B"));
            lGoal.Add(CreateOnProposition("A", "C"));
             * 
             * 
            for (int i = 0; i < cBlocks - 1; i++)
            {
                s0.AddProposition(CreateOnProposition(aBlocks[i], aBlocks[i + 1]));
                lGoal.Add(CreateOnProposition(aBlocks[i + 1], aBlocks[i]));
            }
            s0.AddProposition(CreateOnProposition(aBlocks[cBlocks - 1], "Table"));
            s0.AddProposition(CreateClearProposition(aBlocks[0]));
             * */
        }
        public override Problem GenerateRandomProblem(int iSeed)
        {
            State s0 = new State();
            Random rnd = new Random(iSeed);
            int cBlocksPerTowers = rnd.Next(m_cBlocks) + 1;
            int iTower = 0, iLastTower = -1;
            for (int i = 0; i < m_cBlocks; i++)
            {
                iTower = i / cBlocksPerTowers;
                if (iLastTower != iTower)
                {
                    if (iTower > 0)
                        s0.AddProposition(CreateClearProposition(m_aBlocks[i - 1]));
                    s0.AddProposition(CreateOnProposition(m_aBlocks[i], "Table"));
                }
                else
                {
                    s0.AddProposition(CreateOnProposition(m_aBlocks[i], m_aBlocks[i - 1]));
                }
                iLastTower = iTower;                
            }
            s0.AddProposition(CreateClearProposition(m_aBlocks[m_cBlocks - 1]));

            List<Proposition> lGoal = new List<Proposition>();
            State sCurrent = s0;
            for (int i = 0; i < 2000; i++)
            {
                Action a = Actions[rnd.Next(Actions.Count)];
                if (a.apply(sCurrent) != null)
                    sCurrent = a.apply(sCurrent);
            }
            foreach (Proposition p in sCurrent.Propositions)
                if (p.Name == "On")
                    lGoal.Add(p);
            return new Problem(s0, lGoal);
        }
    }
}
