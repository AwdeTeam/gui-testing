using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgGui
{
    public class Datatype
    {
        public enum Type { Null, Boolean, Integer, BoundedReal, Real, Complex, String, Image, Aggregate }

        private Type type = Type.Null;
        private String name = "";
        private int rank = -1;
        private Datatype[] bundle = null;

        private static Datatype[] Directory = null; //This is a VERY temporary solution; will need a more effecient structure later

        private Datatype(){}

        private static Boolean appendType(Datatype dt)
        {
            if (Directory == null)
            {
                Directory = new Datatype[] { dt };
                return true;
            }
            
            if (Directory.Contains(dt))
                return false;

            Datatype[] n = new Datatype[Directory.Length + 1];
            for (int i = 0; i < Directory.Length; i++)
                n[i] = Directory[i];
            n[Directory.Length] = dt;
            Directory = n;
            return true;
        }

        public static Datatype aggregateTypes(String name, Datatype[] agg)
        {
            Datatype r = new Datatype();
            r.type = Type.Aggregate;
            r.name = name;
            r.rank = 0;
            Console.Out.WriteLine(agg.Length);
            for (int i = 0; i < agg.Length; i++)
                r.rank += agg[i].rank;
            r.bundle = agg;
            if (!appendType(r))
            {
             //   throw new UnauthorizedAccessException(); //Probably not the right exception to use...
            }
            return r;
        }

        public static Datatype aggregateTypes(String name, Datatype a, Datatype b)
        {
            return aggregateTypes(name, new Datatype[] { a, b });
        }

        public static Datatype aggregateTypes(String name, Datatype a, Datatype b, Datatype c)
        {
            return aggregateTypes(name, new Datatype[] { a, b, c });
        }

        public static Datatype defineDatatype(String name, Type type, int rank)
        {
            Datatype r = new Datatype();
            r.type = type;
            r.name = name;
            r.rank = rank;
            appendType(r);
            return r;
        }

        public Boolean equals(Datatype compare) //Datatype names SHOULD be unique!
        {
            return compare.name.Equals(name) && fits(compare);
        }

        public Boolean fits(Datatype compare)
        {
            return compare.rank == rank && compare.type == type;
        }

        public String getName() { return name; }
        public Type getType() { return type; }
        public int getRank() { return rank; }

        public static void testingTypes()
        {
            defineDatatype("32x32 Grayscale", Type.Image, 32 * 32);
            defineDatatype("Flower Properties", Type.Real, 15);
            defineDatatype("Chartered Accountants", Type.Integer, 1);
        }

        public static Datatype findType(String name)
        {
            foreach(Datatype d in Directory)
                if(d.name.Equals(name))
                    return d;
            return null;
        }

        public static Datatype getType(int dex) 
        {
            try
            {
                return Directory[dex];
            }catch(Exception e)
            {
                return null;
            }
        }
    }
}
