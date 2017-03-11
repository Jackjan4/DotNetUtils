using System;

namespace Collections
{
    public static class ArrayUtils
    {


        public static object[] Concat(params object[][] arrays)
        {

            int length = 0;

            foreach (object[] arr in arrays)
            {
                length += arr.Length;
            }

            object[] result = new object[length];

            return result;
        }


        public static byte[] Concat(params byte[][] arrays)
        {

            int length = 0;

            foreach (byte[] arr in arrays)
            {
                length += arr.Length;
            }

            byte[] result = new byte[length];

            int currentSize = 0;
            foreach (byte[] arr in arrays)
            {
                arr.CopyTo(result, currentSize);
                currentSize += arr.Length;
            }

            return result;
        }

    }
}
