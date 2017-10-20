using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.GSX.Demo.ShiftedOrderedArray.Helper;

namespace Microsoft.GSX.Demo.ShiftedOrderedArrayUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SmallSuccessfulArray_Logn()
        {
            int[] array = Finders.CreateArray(10M, 100);            
            Assert.AreEqual(10, Finders.FindShiftPosition_Logn(array));
        }

        [TestMethod]
        public void MediumSuccessfulArray_Logn()
        {
            int[] array = Finders.CreateArray(10M, 10000);
            Assert.AreEqual(1000, Finders.FindShiftPosition_Logn(array));
        }

        [TestMethod]
        public void LargeSuccessfulArray_Logn()
        {
            int[] array = Finders.CreateArray(93.1M, 1000000);
            Assert.AreEqual(931000, Finders.FindShiftPosition_Logn(array));
        }

        [TestMethod]
        public void VeryLargeSuccessfulArray_Logn()
        {
            int[] array = Finders.CreateArray(93.1M, 100000000);
            Assert.AreEqual(93100000, Finders.FindShiftPosition_Logn(array));
        }

        [TestMethod]
        public void VeryLargeSuccessfulArrayCrazyLocation_Logn()
        {
            int[] array = Finders.CreateArray(42.18754M, 100000000);
            Assert.AreEqual(42187540, Finders.FindShiftPosition_Logn(array));
        }

        [TestMethod]
        public void SmallSuccessfulArray_n()
        {
            int[] array = Finders.CreateArray(10M, 100);
            Assert.AreEqual(10, Finders.FindShiftPosition_n(array));
        }

        [TestMethod]
        public void MediumSuccessfulArray_n()
        {
            int[] array = Finders.CreateArray(10M, 10000);
            Assert.AreEqual(1000, Finders.FindShiftPosition_n(array));
        }

        [TestMethod]
        public void LargeSuccessfulArray_n()
        {
            int[] array = Finders.CreateArray(93.1M, 1000000);
            Assert.AreEqual(931000, Finders.FindShiftPosition_n(array));
        }

        [TestMethod]
        public void VeryLargeSuccessfulArray_n()
        {
            int[] array = Finders.CreateArray(93.1M, 100000000);
            Assert.AreEqual(93100000, Finders.FindShiftPosition_n(array));
        }


        //internal int[] CreateArray(decimal splitLocationPercent,int elementCount)
        //{
        //    int overallArraySize = elementCount;

        //    /// This will take the first 10% of the records and move those to the end so the starting value with say 100K records would be 10K...100K,1..9999 so we would expect to see the break
        //    /// found at location 90001 which would be base 1 or base 0 would be 90000.
        //    decimal splitLocation = splitLocationPercent;
        //    int beginNumber = (int)System.Math.Ceiling((decimal)(1 - (splitLocation / 100)) * overallArraySize);
        //    int remainder = overallArraySize - beginNumber;
        //    int[] largeArray = new int[overallArraySize];

        //    for (int i = beginNumber; i < overallArraySize; i++)
        //    {
        //        largeArray[i - beginNumber] = i;
        //    }
        //    for (int i = 1; i < beginNumber; i++)
        //    {
        //        largeArray[remainder + i] = i;
        //    }

        //    return largeArray;
        //}
    }
}
