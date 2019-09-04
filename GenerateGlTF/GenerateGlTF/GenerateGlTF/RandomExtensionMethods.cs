using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GenerateGlTF
{
    public static class RandomExtensionMethods
    {
        public static Vector3 NextVector3(this Random rnd)
        {
            return new Vector3(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        }

        public static Single NextSingle(this Random rnd)
        {
            return (Single)rnd.NextDouble();
        }
    }
}
