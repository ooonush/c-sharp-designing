using System;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }
        public bool IsNan => Denominator == 0;
        
        public Rational(int numerator, int denominator = 1)
        {
            Numerator = numerator;
            Denominator = denominator;
            
            Simplify();
        }
        
        public static Rational Nan => new Rational(0, 0);
        
        private void Simplify()
        {
            Reduce();
            
            if (Denominator < 0)
            {
                Numerator *= -1;
                Denominator *= -1;
            }
            else if (IsNan)
            {
                Numerator = 0;
            }
        }
        
        private void Reduce()
        {
            int gcd = GetGreatestCommonDivisor(Numerator, Denominator);
            
            if (gcd == 0) return;
            
            Numerator /= gcd;
            Denominator /= gcd;
        }
        
        private static int GetGreatestCommonDivisor(int a, int b) 
        {
            return b == 0 ? a : GetGreatestCommonDivisor(b, a % b);
        }

        public static Rational operator+(Rational a, Rational b)
        {
            if (a.IsNan || b.IsNan)
            {
                return Nan;
            }
            
            Rational a1 = a * b.Denominator;
            Rational b1 = b * a.Denominator;
            
            return new Rational(a1.Numerator + b1.Numerator, b.Denominator);
        }

        public static Rational operator-(Rational a, Rational b)
        {
            return a + -1 * b;
        }

        public static Rational operator*(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Rational operator/(Rational a, Rational b)
        {
            return a * new Rational(b.Denominator, b.Numerator);
        }

        public static Rational operator*(int number, Rational rational)
        {
            return new Rational(rational.Numerator * number, rational.Denominator);
        }
        
        public static Rational operator*(Rational rational, int number)
        {
            return number * rational;
        }
        
        public static explicit operator int(Rational rational)
        {
            if (rational.Numerator % rational.Denominator != 0)
            {
                throw new InvalidCastException();
            }
            return rational.Numerator / rational.Denominator;
        }
        
        public static implicit operator double(Rational rational)
        {
            if (rational.IsNan)
            {
                return double.NaN;
            }
            return (double)rational.Numerator / rational.Denominator;
        }
        
        public static implicit operator Rational(int number)
        {
            return new Rational(number);
        }
    }
}
