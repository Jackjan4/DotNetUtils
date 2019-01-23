using System;

namespace De.JanRoslan.NETUtils.Collections
{
    public static class ArrayUtils
    {



        /// <summary>
        /// Creates a sub array out of an original array by setting start point and the offset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] SubArray<T>(T[] array, int start, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, start, result, 0, length);
            return result;
        }
    }
}
