using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    public static class ArrayHelper
    {
        public static T[] FixArrayLength<T>(T[] arr, int length, T filler = default(T))
        {
            if (arr.Length == length)
            {
                return arr;
            }
            var retval = new T[length];
            int i = 0;
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
