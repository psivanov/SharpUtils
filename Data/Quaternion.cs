using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Data
{
    public struct Quaternion
    {
        public double Re, ImI, ImJ, ImK;

        public static readonly Quaternion I = new Quaternion(0, 1, 0, 0);
        public static readonly Quaternion J = new Quaternion(0, 0, 1, 0);
        public static readonly Quaternion K = new Quaternion(0, 0, 0, 1);

        #region Constructor and factory methods
        public Quaternion(double re, double imi, double imj, double imk)
        {
            Re = re;
            ImI = imi;
            ImJ = imj;
            ImK = imk;
        }

        public Quaternion(double Re) : this(Re, 0.0, 0.0, 0.0) { }
        #endregion

        public void Scale(double scale)
        {
            Re *= scale;
            ImI *= scale;
            ImJ *= scale;
            ImK *= scale;
        }

        public double Norm()
        {
            return Math.Sqrt(Norm2());
        }

        public double Norm2()
        {
            return Re * Re + ImI * ImI + ImJ * ImJ + ImK * ImK;
        }

        #region Operators

        public static implicit operator Quaternion(double Re)
        {
            return new Quaternion(Re);
        }

        public static Quaternion operator -(Quaternion x)
        {
            return new Quaternion(-x.Re, -x.ImI, -x.ImJ, -x.ImK);
        }

        public static Quaternion operator ~(Quaternion x)
        {
            return new Quaternion(x.Re, -x.ImI, -x.ImJ, -x.ImK);
        }

        public static Quaternion operator +(Quaternion x, Quaternion y)
        {
            return new Quaternion(x.Re + y.Re, x.ImI + y.ImI, x.ImJ + y.ImJ, x.ImK + y.ImK);
        }

        public static Quaternion operator -(Quaternion x, Quaternion y)
        {
            return new Quaternion(x.Re - y.Re, x.ImI - y.ImI, x.ImJ - y.ImJ, x.ImK - y.ImK);
        }

        public static Quaternion operator *(Quaternion x, Quaternion y)
        {
            return new Quaternion(
                x.Re * y.Re - x.ImI * y.ImI - x.ImJ * y.ImJ - x.ImK * y.ImK,
                x.Re * y.ImI + x.ImI * y.Re + x.ImJ * y.ImK - x.ImK * y.ImJ,
                x.Re * y.ImJ + x.ImJ * y.Re + x.ImK * y.ImI - x.ImI * y.ImK,
                x.Re * y.ImK + x.ImK * y.Re + x.ImI * y.ImJ - x.ImJ * y.ImI
            );
        }

        public static Quaternion operator *(Quaternion x, double y)
        {
            Quaternion r = x;
            r.Scale(y);
            return r;
        }

        public static Quaternion operator *(double x, Quaternion y)
        {
            return y * x;
        }

        public static Quaternion operator /(Quaternion x, Quaternion y)
        {
            return x * ~y * (1.0 / y.Norm2());
        }

        public static bool operator ==(Quaternion x, Quaternion y)
        {
            return x.Re == y.Re && x.ImI == y.ImI && x.ImJ == y.ImJ && x.ImK == y.ImK;
        }

        public static bool operator !=(Quaternion x, Quaternion y)
        {
            return x.Re != y.Re || x.ImI != y.ImI || x.ImJ != y.ImJ || x.ImK != y.ImK;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is Quaternion && this == (Quaternion)obj;
        }

        public override int GetHashCode()
        {
            return Re.GetHashCode() ^ ImI.GetHashCode() ^ ImJ.GetHashCode() ^ ImK.GetHashCode();
        }

        public override string ToString()
        {
            string s = "";
            if (Re != 0) s += string.Format("{0:+0.##;-0.##}", Re);
            if (ImI == -1.0) s += "-i"; else if (ImI == 1.0) s += "i"; else if (ImI != 0) s += string.Format("{0:+0.##;-0.##}i", ImI);
            if (ImJ == -1.0) s += "-j"; else if (ImJ == 1.0) s += "j"; else if (ImJ != 0) s += string.Format("{0:+0.##;-0.##}j", ImJ);
            if (ImK == -1.0) s += "-k"; else if (ImK == 1.0) s += "k"; else if (ImK != 0) s += string.Format("{0:+0.##;-0.##}k", ImK);
            if (s.Length == 0) return "0";
            return s.TrimStart('+');
        }
    }
}