using System;

namespace MyPhotoshop
{
    public struct Pixel
    {
        private double _r;
        private double _g;
        private double _b;
        
        public double R
        {
            get => _r;
            set => _r = Check(value);
        }
        
        public double G
        {
            get => _g;
            set => _g = Check(value);
        }
        
        public double B
        {
            get => _b;
            set => _b = Check(value);
        }
        
        public Pixel(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }
        
        public static Pixel operator *(Pixel pixel, double number)
        {
            return new Pixel(Clamp01(pixel.R * number), Clamp01(pixel.G * number),  Clamp01(pixel.B * number));
        }
        
        public static Pixel operator *(double number, Pixel pixel)
        {
            return pixel * number;
        } 
        
        public static double Clamp01(double value)
        {
            if (value < 0)
            {
                return 0;
            }
            
            if (value > 1)
            {
                return 1;
            }
            
            return value;
        }

        public static double Check(double value)
        {
            if (value is < 0 or > 1)
            {
                throw new ArgumentException();
            }
            
            return value;
        }
    }
}