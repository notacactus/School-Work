using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_3
{
    public abstract class MobileObject
    {
        private string name;
        private int iD;
        private double posX;
        private double posY;
        private double posZ;

        // Constructor assigns name and ID based on parameters passed and assigns position randomly
        public MobileObject(string name, int iD)
        {
            this.name = name;
            this.iD = iD;
            Random rnd = new Random();
            posX = rnd.NextDouble();
            posY = rnd.NextDouble();
            posZ = rnd.NextDouble();
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
            }
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

    }
}
