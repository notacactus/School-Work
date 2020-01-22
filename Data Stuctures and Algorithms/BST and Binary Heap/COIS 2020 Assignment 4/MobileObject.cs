using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_4
{
    public abstract class MobileObject : IComparable<MobileObject>
    {
        private string name;
        private int iD;
        private double posX;
        private double posY;
        private double posZ;
        private double disOrigin;

        // Constructor assigns name and ID based on parameters passed and assigns position randomly
        public MobileObject(Random rnd, string name, int iD)
        {
            this.name = name;
            this.iD = iD;
            posX = rnd.NextDouble()*10;
            posY = rnd.NextDouble()*10;
            posZ = rnd.NextDouble()*10;
            CalcDisOrigin();
        }

        public string Name
        {
            get { return name; }
        }
        public int ID
        {
            get { return iD; }
        }
        // setters for positions require non-negative values
        public double PosX
        {
            get { return posX; }
            set
            {
                if (value < 0) 
                    throw new Exception("Invalid Value");
                posX = value;
                CalcDisOrigin();
            }
        }
        public double PosY
        {
            get { return posY; }
            set
            {
                if (value < 0)
                    throw new Exception("Invalid Value");
                posY = value;
                CalcDisOrigin();
            }
        }
        public double PosZ
        {
            get { return posZ; }
            set
            {
                if (value < 0)
                    throw new Exception("Invalid Value");
                posZ = value;
                CalcDisOrigin();
            }
        }
        public double DisOrigin
        {
            get { return disOrigin; }
        }

        // calculates the distance to the origin
        private void CalcDisOrigin()
        {
            disOrigin = Math.Sqrt(posX * posX + posY * posY + posZ * posZ);
        }

        // returns various properties of MobileObject as a string
        public virtual string Print()
        {
            return "Name: " + name
                + "\nID: " + iD
                + "\nPosition (x, y, z): (" + posX + ", " + posY + ", " + posZ + ")";
        }

        // overrides ToString to act as print
        public override string ToString()
        {
            return Print();
        }

        // compares mobileobjects based on distance from origin
        public int CompareTo(MobileObject obj)
        {
            if (disOrigin == obj.disOrigin)
                return 0;
            if (disOrigin > obj.disOrigin)
                return 1;
            return -1;
        }

    }
}
