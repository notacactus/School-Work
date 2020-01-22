using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_4
{
    class NPC : MobileObject
    {
        private double legLength;
        private double torsoHeight;
        private double headHeight;
        private double totalHeight;

        // Constructor assigns parameters as given, throws exception if dimensions are not posative, then calculates total height
        public NPC(Random rnd, string name, int iD, double legLength, double torsoHeight, double headHeight) : base(rnd, name, iD)
        {
            if (legLength <= 0 || torsoHeight <= 0 || headHeight <= 0)
                throw new Exception("Invalid Value");
            this.legLength = legLength;
            this.torsoHeight = torsoHeight;
            this.headHeight = headHeight;
            calcTotal();
        }

        // setters for dimensions require posative values
        public double LegLength
        {
            get { return legLength; }
            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Value");
                legLength = value;
                calcTotal();
            }
        }
        public double TorsoHeight
        {
            get { return torsoHeight; }
            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Value");
                torsoHeight = value;
                calcTotal();
            }
        }
        public double HeadHeight
        {
            get { return headHeight; }
            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Value");
                headHeight = value;
                calcTotal();
            }
        }

        public double TotalHeight { get { return totalHeight; } }

        // calculates total height
        private void calcTotal()
        {
            totalHeight = legLength + torsoHeight + headHeight;
        }

        // Returns string containing various properties, makes use of parent class print function
        public override string Print()
        {
            return base.Print()
                + "\nLeg Length: " + legLength
                + "\nTorso Height: " + torsoHeight
                + "\nHead Height: " + headHeight
                + "\nTotal Height: " + totalHeight;
        }
    }
}
