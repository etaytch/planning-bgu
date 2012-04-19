using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class Logistics : Domain
    {
        private int m_cPackages, m_cTrucks, m_cCities;
        public Logistics(int cPackages, int cTrucks, int cCities, double dRoadDensity, int iRandomSeed)
        {
            m_cPackages = cPackages;
            m_cTrucks = cTrucks;
            m_cCities = cCities;
            CreateLogistics(iRandomSeed, dRoadDensity);
        }

        private Proposition CreateAtProposition(string sObject, int iCity)
        {
            Proposition pAt = new Proposition("At");
            pAt.Objects.Add(sObject);
            pAt.Objects.Add("C"+iCity);
            return pAt;
        }
        private Proposition CreateOnProposition(int iPackage, int iTruck)
        {
            Proposition pOn = new Proposition("On");
            pOn.Objects.Add("P" + iPackage);
            pOn.Objects.Add("T" + iTruck);
            return pOn;
        }
 

        private void CreateLogistics(int iRandomSeed, double dRoadDensity)
        {
            Random rnd = new Random(iRandomSeed);
            int iPackage = 0, iCity = 0, iTruck = 0, iOtherCity = 0;
            /*
            for (iCity = 0; iCity < m_cCities; iCity++)
            {
                iTruck = (iCity * m_cTrucks) / m_cCities;
                Actions.Add(CreateDriveTruckAction(iTruck, iCity, (iCity + 1) % m_cCities));
                for (iOtherCity = 0; iOtherCity < m_cCities; iOtherCity++)
                {

                    if (iCity != iOtherCity && rnd.NextDouble() < dRoadDensity)
                    {
                        for(iTruck = 0 ; iTruck < m_cTrucks ; iTruck++)
                        {
                            Actions.Add(CreateDriveTruckAction(iTruck, iCity, iOtherCity));
                        }
                    }
                }
            }
            */
            for (iTruck = 0; iTruck < m_cTrucks; iTruck++)
            {
                for (iCity = (iTruck * m_cCities) / m_cTrucks; iCity < ((iTruck+1) * m_cCities) / m_cTrucks; iCity++)
                {
                    Actions.Add(CreateDriveTruckAction(iTruck, iCity, (iCity + 1)));
                    for (iOtherCity = (iTruck * m_cCities) / m_cTrucks; iOtherCity < ((iTruck + 1) * m_cCities) / m_cTrucks; iOtherCity++)
                    {
                        if (iCity != iOtherCity && rnd.NextDouble() < dRoadDensity)
                        {
                            Actions.Add(CreateDriveTruckAction(iTruck, iCity, iOtherCity));
                        }
                    }
                }
                Actions.Add(CreateDriveTruckAction(iTruck, ((iTruck + 1) * m_cCities) / m_cTrucks, (iTruck * m_cCities) / m_cTrucks));
            }

            //add load/unload actions
            for (iCity = 0; iCity < m_cCities; iCity++)
            {
                for (iTruck = 0; iTruck < m_cTrucks; iTruck++)
                {
                    for (iPackage = 0; iPackage < m_cPackages; iPackage++)
                    {
                        Actions.Add(CreateLoadAction(iPackage, iTruck, iCity));
                        Actions.Add(CreateUnLoadAction(iPackage, iTruck, iCity));
                    }
                }
            }
        }

        private Action CreateLoadAction(int iPackage, int iTruck, int iCity)
        {
            Action aLoad = new Action("Load(P" + iPackage + ", T" + iTruck + ", C" + iCity + ")");
            //preconditions - At(T,C), At(P,C)
            aLoad.Preconditions.Add(CreateAtProposition("T" + iTruck, iCity));
            aLoad.Preconditions.Add(CreateAtProposition("P" + iPackage, iCity));
            //Add list - On(A,C), if B is not the table clear(B) 
            aLoad.AddList.Add(CreateOnProposition(iPackage, iTruck));
             //Remove list - On(A,B), if C is not the table clear(C) 
            aLoad.RemoveList.Add(CreateAtProposition("P" + iPackage, iCity));
            return aLoad;
        }

        private Action CreateUnLoadAction(int iPackage, int iTruck, int iCity)
        {
            Action aUnLoad = new Action("Unload(P" + iPackage + ", T" + iTruck + ", C" + iCity + ")");
            //preconditions - At(T,C), At(P,C)
            aUnLoad.Preconditions.Add(CreateAtProposition("T" + iTruck, iCity));
            aUnLoad.Preconditions.Add(CreateOnProposition(iPackage, iTruck));
            //Add list - On(A,C), if B is not the table clear(B) 
            aUnLoad.AddList.Add(CreateAtProposition("P" + iPackage, iCity));
            //Remove list - On(A,B), if C is not the table clear(C) 
            aUnLoad.RemoveList.Add(CreateOnProposition(iPackage, iTruck));
            return aUnLoad;
        }

        private Action CreateDriveTruckAction(int iTruck, int iSourceCity, int iTargetCity)
        {
            Action aUnLoad = new Action("Drive(T" + iTruck + ", C" + iSourceCity + ", C" + iTargetCity + ")");
            //preconditions - At(T,C), At(P,C)
            aUnLoad.Preconditions.Add(CreateAtProposition("T" + iTruck, iSourceCity));
            //Add list - On(A,C), if B is not the table clear(B) 
            aUnLoad.AddList.Add(CreateAtProposition("T" + iTruck, iTargetCity));
            //Remove list - On(A,B), if C is not the table clear(C) 
            aUnLoad.RemoveList.Add(CreateAtProposition("T" + iTruck, iSourceCity));
            return aUnLoad;
        }

        public override Problem GenerateRandomProblem(int iSeed)
        {
            State s0 = new State();
            Random rnd = new Random(iSeed);
            List<Proposition> lGoal = new List<Proposition>();
            for (int iPackage = 0; iPackage < m_cPackages; iPackage++)
            {
                int iSourceCity = rnd.Next(m_cCities);
                s0.AddProposition(CreateAtProposition("P" + iPackage, iSourceCity));
                int iTargetCity = rnd.Next(m_cCities);
                lGoal.Add(CreateAtProposition("P" + iPackage, iTargetCity));
            }
            for (int iTruck = 0; iTruck < m_cTrucks; iTruck++)
            {
                int iCity = rnd.Next(m_cCities/m_cTrucks) + (m_cCities * iTruck) / m_cTrucks;
                s0.AddProposition(CreateAtProposition("T" + iTruck, iCity));
            }
            return new Problem(s0, lGoal);
        }
    }
}
