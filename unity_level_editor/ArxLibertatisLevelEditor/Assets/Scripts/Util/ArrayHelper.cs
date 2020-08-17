using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    public static class ArrayHelper
    {
        /// <summary>
        /// changes an arrays length if necessary. filling up empty spaces with filler, or truncating extra items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="length"></param>
        /// <param name="filler"></param>
        /// <returns></returns>
        public static T[] FixArrayLength<T>(T[] arr, int length, T filler = default(T))
        {
            if (arr.Length == length)
            {
                return arr;
            }
            var retval = new T[length];
            int i;
            for (i = 0; i < length && i < arr.Length; i++)
            {
                retval[i] = arr[i];
            }
            for (; i < length; i++)
            {
                retval[i] = filler;
            }

            return retval;
        }

        /// <summary>
        /// trimming the last elements matching trim from the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        public static T[] TrimEnd<T>(T[] arr, T trim)
        {
            int end = arr.Length - 1;
            if (!Equals(arr[end], trim))
            {
                return arr;
            }
            while (end >= 0)
            {
                if (!Equals(arr[end], trim))
                {
                    break;
                }
                end--;
            }
            T[] retval = new T[end + 1];
            for (int i = 0; i < end + 1; i++)
            {
                retval[i] = arr[i];
            }
            return retval;
        }
    }
}
