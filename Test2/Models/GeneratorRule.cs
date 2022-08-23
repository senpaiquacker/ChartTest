using System;
using System.Collections.Generic;
using System.Text;

namespace Test2.Models
{
    public interface GeneratorRule
    {
        public int GenerateValue(int MaxYValue, int MaxXBuffer, int XPointer);
    }
    public class GeneratorRand : GeneratorRule
    {
        private Random Rand;
        public GeneratorRand()
        {
            Rand = new Random();
        }

        public int GenerateValue(int MaxYValue, int MaxXBuffer, int XPointer)
        {
            return GenerateValue(MaxYValue);
        }
        public int GenerateValue(int MaxYValue)
        {
            return Rand.Next(0, MaxYValue);
        }
    }

    public class GeneratorBufferLine : GeneratorRule
    {
        private Random Rand;

        private int XBuffer;
        private int YBuffer;

        private double AMult;

        private int range;
        public GeneratorBufferLine(int HorizontalBuffer, int VerticalBuffer)
        {
            Rand = new Random();

            XBuffer = HorizontalBuffer;
            YBuffer = VerticalBuffer;
            var mult = YBuffer / XBuffer;
            var k = Rand.Next(2);
            if (k == 0)
                AMult = Rand.NextDouble();
            else
                AMult = Rand.NextDouble() + Rand.Next(mult);
            range = YBuffer / 8;
        }

        public int GenerateValue(int MaxYValue, int MaxXBuffer, int XPointer)
        {
            return GenerateValue(XPointer, MaxYValue);
        }

        public int GenerateValue(int XPointer, int MaxYValue)
        {
            var cycles = XPointer / XBuffer;
            int value;
            if(cycles % 2 == 0)
                value = (int)Math.Round((XPointer % XBuffer) * AMult + Rand.Next(-range, range));
            else 
                value = (int)Math.Round((XBuffer * AMult) - (XPointer % XBuffer) * AMult + Rand.Next(-range, range));
            if (value > MaxYValue)
                value = MaxYValue;
            if (value < 0)
                value = 0;
            return (int)value;
        }
    }

    public class GeneratorIn4Cycles : GeneratorRule
    {
        private Random Rand;

        private int XBuffer;
        private int YBuffer;

        private int MaxY;
        private int MinY;
        private int oldMinY;

        private double AMult;

        private int range;
        public GeneratorIn4Cycles(int HorizontalBuffer, int VerticalBuffer)
        {
            Rand = new Random();

            XBuffer = HorizontalBuffer;
            YBuffer = VerticalBuffer;
            range = YBuffer / 8;
            RegenerateLine();
            oldMinY = MinY;
        }
        private void RegenerateLine()
        {
            MaxY = Rand.Next(0, YBuffer);
            MinY = Rand.Next(0, MaxY);
            var mult = (MaxY - MinY) / XBuffer;
            var k = Rand.Next(2);
            if (k == 0)
                AMult = Rand.NextDouble();
            else
                AMult = Rand.NextDouble() + Rand.Next(mult);
        }
        public int GenerateValue(int MaxYValue, int MaxXBuffer, int XPointer)
        {
            return GenerateValue(XPointer, MaxYValue);
        }
        public int GenerateValue(int XPointer, int MaxYValue)
        {
            var CycleStep = XPointer % XBuffer;
            int value = 0;
            var Stage = CycleStep / (XBuffer / 4);
            if (CycleStep == 0)
            {
                oldMinY = MinY;
                RegenerateLine();
            }
            switch(Stage)
            {
                case 0:
                    value = (int)Math.Round(oldMinY + (CycleStep % (XBuffer / 4)) * AMult + Rand.Next(-range, range));
                    break;
                case 1:
                    value = (int)Math.Round((double)(MaxY + Rand.Next(-range, range)));
                    break;
                case 2:
                    value = (int)Math.Round(MaxY - CycleStep % (XBuffer / 4) * AMult + Rand.Next(-range, range));
                    break;
                case 3:
                    value = (int)Math.Round((double)(MinY + Rand.Next(-range, range)));
                    break;
            }
            if (value > MaxYValue)
                value = MaxYValue;
            if (value < 0)
                value = 0;
            return value;
        }
    }
}
