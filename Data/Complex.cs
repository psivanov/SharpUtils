using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Data
{
    public struct Complex
    {
        public double Re;
        public double Im;

        public static readonly Complex I = new Complex(0, 1);

        #region Constructor and factory methods 
        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }

        public Complex(double Re) : this(Re, 0.0) { }

        public static Complex FromPolar(double radius, double angle)
        {
            return new Complex(radius * Math.Cos(angle), radius * Math.Sin(angle));
        }

        #endregion

        public double Norm()
        {
            return Math.Sqrt(Norm2());
        }

        public double Norm2()
        {
            return Re * Re + Im * Im;
        }

        public Complex Exp()
        {
            return FromPolar(Math.Exp(Re), Im);
        }

        #region Operators

        public static implicit operator Complex(double Re)
        {
            return new Complex(Re);
        }

        public static Complex operator -(Complex x)
        {
            return new Complex(-x.Re, -x.Im);
        }

        public static Complex operator ~(Complex x)
        {
            return new Complex(x.Re, -x.Im);
        }

        public static Complex operator +(Complex x, Complex y)
        {
            return new Complex(x.Re + y.Re, x.Im + y.Im);
        }

        public static Complex operator -(Complex x, Complex y)
        {
            return new Complex(x.Re - y.Re, x.Im - y.Im);
        }

        public static Complex operator *(Complex x, Complex y)
        {
            return new Complex(x.Re * y.Re - x.Im * y.Im, x.Re * y.Im + x.Im * y.Re);
        }

        public static Complex operator /(Complex x, Complex y)
        {
            double n2 = y.Norm2();
            return new Complex((x.Re * y.Re + x.Im * y.Im) / n2, (x.Im * y.Re - x.Re * y.Im) / n2);
        }

        public static bool operator ==(Complex x, Complex y)
        {
            return x.Re == y.Re && x.Im == y.Im;
        }

        public static bool operator !=(Complex x, Complex y)
        {
            return x.Re != y.Re || x.Im != y.Im;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is Complex && this == (Complex)obj;
        }

        public override int GetHashCode()
        {
            return Re.GetHashCode() ^ Im.GetHashCode();
        }

        public override string ToString()
        {
            if (Im < 0) return string.Format("{0}{1}i", Re, Im);
            if (Im > 0) return string.Format("{0}+{1}i", Re, Im);
            return Re.ToString();
        }
    }
}
