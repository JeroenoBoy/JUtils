namespace JUtils
{
    /// <summary>
    /// Contains helper functions for working with Weighted Randomness
    /// </summary>
    public static class WeightedExtensions
    {
        /// <summary>
        /// Get a random weighted value using UnityEngine.Random
        /// </summary>
        public static T Random<T>(this Weighted<T>[] weightedArray)
        {
            float sum = weightedArray.WeightedSum();
            float index = UnityEngine.Random.Range(0, sum);
            return weightedArray.GetWeightedValue(index).value;
        }


        /// <summary>
        /// Get a random weighted value using System.Random
        /// </summary>
        public static T Random<T>(this Weighted<T>[] weightedArray, System.Random random)
        {
            float sum = weightedArray.WeightedSum();
            float index = (float)(random.NextDouble() * sum);
            return weightedArray.GetWeightedValue(index).value;
        }


        /// <summary>
        /// Get a random weighted value using UnityEngine.Random
        /// </summary>
        public static T Random<T>(this T[] weightedArray) where T : IWeighted
        {
            float sum = weightedArray.WeightedSum();
            float index = UnityEngine.Random.Range(0, sum);
            return weightedArray.GetWeightedValue(index);
        }
        
        
        /// <summary>
        /// Get a random weighted value using System.Random
        /// </summary>
        public static T Random<T>(this T[] weightedArray, System.Random random) where T : IWeighted
        {
            float sum = weightedArray.WeightedSum();
            float index = (float)(random.NextDouble() * sum);
            return weightedArray.GetWeightedValue(index);
        }


        /// <summary>
        /// Get a weighted value at a certain index
        /// </summary>
        public static T GetWeightedValue<T>(this T[] weightedArray, float weightedIndex) where T : IWeighted
        { 
            T value = default;
            
            for (int i = 0; weightedIndex > 0; i++) {
                value = weightedArray[i];
                weightedIndex -= weightedArray[i].weight;
            }

            return value;
        }

        
        /// <summary>
        /// Get the sum of a weighted array
        /// </summary>
        public static float WeightedSum<T>(this T[] weightedArray) where T : IWeighted
        {
            float sum = 0;
            for (int i = weightedArray.Length; i --> 0;) {
                sum += weightedArray[i].weight;
            }

            return sum;
        }
    }
}