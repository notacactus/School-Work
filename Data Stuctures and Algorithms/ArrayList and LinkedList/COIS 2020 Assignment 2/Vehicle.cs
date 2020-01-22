using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_2
{
    class Vehicle : MobileObject
    {
        double length;
        double width;
        double height;
        double volume;

        // Constructor assigns parameters as given, throws exception if dimensions are not posative, then calculates volume
        public Vehicle(string name, int iD, double length, double width, double height) : base(name, iD)
        {
            if (length <= 0 || width <= 0 || height <= 0)
                throw new Exception("Invalid Value");
            this.length = length;
            this.width = width;
            this.height = height;
            calcVolume();
        }

        // setters for dimesions require posative values
        public double Length
        {
            get { return length; }
            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Input");
                length = value;
                calcVolume();
            }
        }
        public double Width
        {
            get { return width; }
            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Input");
                width = value;
                calcVolume();
            }
        }
        public double Height
        {
            get { return height; }
            set
            {
                if (value <= 0)
                    throw new Exception("Invalid Value");
                height = value;
                calcVolume();
            }
        }
        public double Volume { get { return volume; } }

        // calculates volume
        public void calcVolume()
        {
            volume = length * width * height;
        }

        // Returns string containing various properties, makes use of parent class print function
        public override string Print()
        {
            return base.Print()
                + "\nLength: " + length
                + "\nWidth: " + width
                + "\nHeight: " + height
                + "\nBounding Volume: " + volume;
        }
    }
}
