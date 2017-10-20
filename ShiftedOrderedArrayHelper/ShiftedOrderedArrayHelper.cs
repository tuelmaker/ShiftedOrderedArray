using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Microsoft.GSX.Demo.ShiftedOrderedArray.Helper
{
    public class Finders
    {
        /// <summary>
        /// This contant is used to make sure we don't have a slide within the algorithm.
        /// An example would be incrementing by 1 for several loops...this shouldn't happen based on the 
        /// logic...if it does then there may be a strange issue with the data like having the same values 
        /// in multiple sequential cells possibly where the condition may not be satisfied for many loops
        /// as the lookup traverses sideways.
        /// </summary>
        internal const int UPPER_BOUND_MIN_BASE_RANGE_LOOPS = 5;

        /// <summary>
        /// This will go through all the elements of the array until it finds where [i].Value > [i+1].Value
        /// O(n)
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int FindShiftPosition_n(int[] array)
        {
            for(int i=0;i<array.Count()-2;i++)
            {
                if (array[i]>array[i+1])
                {
                    return i+1;
                }
            }
            return 0;
        }

        /// <summary>
        /// This will use an exponential reduction to determine the Indexor which has the number which is just 
        /// after the regular integer index
        /// O(log n)
        /// </summary>
        /// <param name="array"><seealso cref="Array"/></param>
        /// <returns></returns>
        public static double FindShiftPosition_Logn(int[] array)
        {
            int beginOffset = 0;
            int endOffset = 0;
            int loopCounter = 1;
            int baseOffsetRange = 0;
            int minBaseRangeLoopCounter = 0;
            bool continueCheck = true;

            /// initial checks since the array we pass in could be null, have a single element or no elements;
            /// 
            if (array == null)
            {
                throw new ArgumentNullException("The array passed in is null and therefore we are unable to check the order");
            }
            if (array.Count() == 1)
            {
                return array.Count();
            }
            if (array.Count() == 0)
            {
                return 0;
            }
            
            /// Set the initial baseOffsetRange and endOffset using 2 ^ 1 to get the middle
            baseOffsetRange = (int)System.Math.Ceiling((array.Count() - 1) / System.Math.Pow(2, 1));
            endOffset = baseOffsetRange;
            
            while (continueCheck)
            {
                loopCounter += 1;
                
                /// we need to check whether we have a non-zero baseOffsetRange
                /// if this is 0 then we need to reset to 1 so that we can move the lookup range
                /// accordingly
                if(baseOffsetRange ==0)
                {
                    baseOffsetRange = 1;
                }
                Console.WriteLine(string.Format("LoopCounter: {0} - BaseRange {1}", loopCounter, baseOffsetRange));

                /// for this we are using 2 ^ loopCounter in order to exponentially reduce the ranges we are looking
                /// up
                baseOffsetRange = (int)System.Math.Floor((array.Count() - 1) / System.Math.Pow(2, loopCounter));
                if (baseOffsetRange == 0)
                {
                    baseOffsetRange = 1;
                    if(minBaseRangeLoopCounter>UPPER_BOUND_MIN_BASE_RANGE_LOOPS)
                    {
                        continueCheck = false;
                        throw new ArgumentException("We have encountered what looks to be an endless loop which is causing a performance issue, and could be the result of a corrupt array" +
                            "being passed\r\nPlease escalate");
                    }
                }
                ///primary logic
                if (array[beginOffset] > array[endOffset])
                {
                    ///We are on the left-hand side of the evaluateion so we want to increment the endOffset based on the 
                    ///beginOffset by the baseOffsetRange
                    endOffset = beginOffset + baseOffsetRange;
                    beginOffset = endOffset - baseOffsetRange;

                    /// reset the endOffset since not doing so will lead to an out of range exception
                    if (endOffset > array.Count() - 1)
                    {
                        endOffset = array.Count() - 1;
                    }                    
                    
                }
                else
                {
                    /// right-hand side of evaluation so we want to increase the endOffset by the baseOffsetRange
                    beginOffset = endOffset;
                    endOffset = endOffset + baseOffsetRange;

                    /// reset the endOffset since not doing so will lead to an out of range exception
                    if (endOffset > array.Count() - 1)
                    {
                        endOffset = array.Count() - 1;
                    }                    
                }

                Console.WriteLine(string.Format("{0} - {1}", array[beginOffset].ToString(), array[endOffset].ToString()));

                if (endOffset - beginOffset <=1)
                {
                    if(array[beginOffset] > array[endOffset])
                    {
                        continueCheck = false;
                        Console.WriteLine(string.Format("Index just after break occurs: {0}", endOffset.ToString()));
                        return endOffset;
                    }                    
                }
            }

            return -1;
        }

        /// <summary>
        /// Used to create a split ordered array for testing 
        /// </summary>
        /// <remarks>This increments by 1 only...other increment patterns could be added, but that's all it does now</remarks>
        /// <param name="splitLocationPercent">This can be something like 10 or 10.1M for a decimal number</param>
        /// <param name="elementCount">This is the count of the elements in total...no cap on this, and have tried with 100000000</param>
        /// <returns></returns>
        public static int[] CreateArray(decimal splitLocationPercent, int elementCount)
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

        /// <summary>
        /// This method will use some statistics (heuristics or it could also use some ML datasets to score and train the model to output 
        /// and influence flag that we can leverage to increase the probability of the pattern being skewed one way or the other).
        /// Essentially if we can determine that say 90% of the time on Wednesdays @ 10 AM the pattern skews towards the end of the array with 
        /// higher deviations than the beginning then that means we should not necessarily halve the array and remainders, but instead do something like
        /// take the first deviation and make that as the endingOffset. For example, we would have the pattern [1000,1001,1..990] in increments of 10 which means that we have 
        /// the break at the 3rd position or index 2. In this case we have a total of 101 elements and it will take several iterations to determine that 
        /// the position is 3. Therefore, we should take the beginning and the end values and add them together and divide by the count or (1000+990)/101 ~ 19...if we know 
        /// that the skew is for higher deviations towards the 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int FindShiftPosition_Logn_Better(int[] array)
        {

            return 0;
        }
    }
}
