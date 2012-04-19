using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Planning
{
    class Program
    {

        static void Test1()
        {
            FileStream fs = new FileStream("Test.txt", FileMode.Create);
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener(fs));
            BlocksWorld bw = new BlocksWorld(10);
            Problem prob = bw.GenerateRandomProblem(0);
            BFSPlanner p1 = new BFSPlanner(bw);
            HeuristicFunction h = new HSPHeuristic(bw, prob.Goal, false);
            ForwardSearchPlanner p2 = new ForwardSearchPlanner(bw, h);
            List<Action> lPlan = p2.Plan(prob);
            Console.WriteLine("A*");
            foreach (Action a in lPlan)
                Console.WriteLine(a);
            Console.WriteLine("Computation cost " + p2.ComputationCost());
            lPlan = p1.Plan(prob);
            Console.WriteLine("BFS");
            foreach (Action a in lPlan)
                Console.WriteLine(a);
            Console.WriteLine("Computation cost " + p1.ComputationCost());
            Debug.Close();
        }

        static void Test2()
        {
            FileStream fs = new FileStream("Test.txt", FileMode.Create);
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener(fs));
            Logistics logistics = new Logistics(2, 2, 10, 0.0, 1111);
            Problem prob = logistics.GenerateRandomProblem(1111);
            BFSPlanner p1 = new BFSPlanner(logistics);
            HeuristicFunction h = new HSPHeuristic(logistics, prob.Goal, false);
            ForwardSearchPlanner p2 = new ForwardSearchPlanner(logistics, h);
            List<Action> lPlan = p2.Plan(prob);
            foreach (Action a in lPlan)
                Console.WriteLine(a);
            Console.WriteLine("Computation cost " + p2.ComputationCost());
            lPlan = p1.Plan(prob);
            foreach (Action a in lPlan)
                Console.WriteLine(a);
            Console.WriteLine("Computation cost " + p1.ComputationCost());
            
            Debug.Close();
        }

        static void Main(string[] args)
        {
            FileStream fs = new FileStream("Debug.txt", FileMode.Create);
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener(fs));
            //Test1();
            //myTest2();
            Test2();
            Debug.Close();
        }

        static void myTest() {
            PriorityQueue<string, double> q = new PriorityQueue<string, double>();
            q.Enqueue("etay", 4);
            q.Enqueue("vadi", 2);
            q.Enqueue("niv", 0);
            Console.Out.WriteLine("q.isEmpty: " + q.isEmpty());
            string ans1 = q.Dequeue();
            string ans2 = q.Dequeue();
            string ans3 = q.Dequeue();
            Console.Out.WriteLine("q.isEmpty: " + q.isEmpty());
            Console.Out.WriteLine("ans1: " + ans1);
            Console.Out.WriteLine("ans2: " + ans2);
            Console.Out.WriteLine("ans3: " + ans3);

        }

        static void myTest2() {
            List<StatePriorityItem> visited = new List<StatePriorityItem>();
            State s1 = new State();
            Proposition p1 = new Proposition("on");
            p1.Objects.Add("a");
            p1.Objects.Add("b");            
            s1.Propositions.Add(p1);

            Proposition p2 = new Proposition("on");
            p2.Objects.Add("c");
            p2.Objects.Add("d");
            s1.Propositions.Add(p2);

            List<Action> actions = new List<Action>();
            actions.Add(new Action("move1"));
            actions.Add(new Action("move2"));

            StatePriorityItem spi = new StatePriorityItem(s1, actions);


            State s2 = new State();
            s2.Propositions.Add(p2);
            s2.Propositions.Add(p1);

            List<Action> actions2 = new List<Action>();
            actions2.Add(new Action("move2"));
            actions2.Add(new Action("move1"));
            StatePriorityItem spi2 = new StatePriorityItem(s2, actions2);

            visited.Add(spi);
            bool aaa = spi.Equals(spi2);            
        }
    }
}
