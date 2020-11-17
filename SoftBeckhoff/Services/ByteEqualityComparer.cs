using System.Collections.Generic;

namespace SoftBeckhoff.Services
{
    internal class ByteEqualityComparer : IEqualityComparer<byte[]>
    {
        //partially copied from https://searchcode.com/codesearch/view/561886/
        
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public static bool AreEqual(byte[] x, byte[] y)
        {
            if (x == y)
                return true;

            if (x == null || y == null || x.Length != y.Length)
                return false;

            // Simple (slower) version:
            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }



        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and
        /// <paramref name="obj"/> is null.</exception>
        public static int GetHashCode(byte[] obj)
        {
            // FNV-style hash
            // http://bretm.home.comcast.net/~bretm/hash/6.html
            unchecked
            {
                const int p = 16777619;
                var hash = (int)2166136261;

                for (var i = 0; i < obj.Length; i++)
                {
                    hash = (hash ^ obj[i]) * p;
                }

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        bool IEqualityComparer<byte[]>.Equals(byte[] x, byte[] y)
        {
            return AreEqual(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and
        /// <paramref name="obj"/> is null.</exception>
        int IEqualityComparer<byte[]>.GetHashCode(byte[] obj)
        {
            return GetHashCode(obj);
        }
    }
}