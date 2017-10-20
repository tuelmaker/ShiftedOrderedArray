using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.GSX.Demo.ShiftedOrderedArray;


namespace Microsoft.GSX.Demo.ShiftedOrderedArray
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                decimal split = 10M;
                //int[] array = CreateArray(93.1M, 1000000);
                int[] array = CreateArray(split, 10000);
                Console.WriteLine(array.Count());

                //Console.WriteLine(Helper.Finders.FindShiftPosition_n(new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 2, 3 }));
                //Console.WriteLine(Helper.Finders.FindShiftPosition_Logn(new int[] { 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35,36,37,38,39,40, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 ,19, 20,21, 22, 23}));
                Console.WriteLine(string.Format("Running array with {0} elements with a split at the {1} percent point", array.Count().ToString(), split.ToString()));
                Console.WriteLine(Helper.Finders.FindShiftPosition_Logn(array));
                Console.WriteLine(Helper.Finders.FindShiftPosition_n(array));
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Format("Exception: {0}\r\nStack: {1}", ex.Message, ex.StackTrace));
                Console.Read();
            }

            Console.Read();
        }

        internal static int[] CreateArray(decimal splitLocationPercent, int elementCount)
        {
            int overallArraySize = elementCount;

            /// This will take the first 10% of the records and move those to the end so the starting value with say 100K records would be 10K...100K,1..9999 so we would expect to see the break
            /// found at location 90001 which would be base 1 or base 0 would be 90000.
            decimal splitLocation = splitLocationPercent;
            int beginNumber = (int)System.Math.Ceiling((decimal)(1 - (splitLocation / 100)) * overallArraySize);
            int remainder = overallArraySize - beginNumber;
            int[] largeArray = new int[overallArraySize];

            for (int i = beginNumber; i < overallArraySize; i++)
            {
                largeArray[i - beginNumber] = i;
            }
            for (int i = 1; i < beginNumber; i++)
            {
                largeArray[remainder + i] = i;
            }

            return largeArray;
        }
    }
}
