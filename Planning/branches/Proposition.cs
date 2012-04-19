using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    class Proposition
    {
        public string Name { get; private set; }
        public List<string> Objects{ get; private set; }
        public Proposition(string sName)
        {
            Name = sName;
            Objects = new List<string>();
        }

        public bool Equals(Proposition p2)
        {
            if( Object.ReferenceEquals(this,p2))
                return true;
            if (null== p2)
                return false;
            if (Name != p2.Name)
                return false;
            if (Objects.Count != p2.Objects.Count)
                return false;
            int i = 0;
            for( i = 0 ; i < Objects.Count ; i++ )
                if( Objects[i] != p2.Objects[i] )
                    return false;
            return true;
        }

        public override string ToString()
        {
            string s = Name + "(";
            foreach (string sObj in Objects)
                s += sObj + ", ";
            s = s.Substring(0, s.Length - 2) + ")";
            return s;
        }
        public override bool Equals(object obj)
        {
            if (obj is Proposition)
                return Equals((Proposition)obj);
            return false;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
