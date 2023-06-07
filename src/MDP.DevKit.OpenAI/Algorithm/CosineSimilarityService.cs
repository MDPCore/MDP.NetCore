using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class CosineSimilarityService
    {
        // Methods
        public double Calculate(double[] vector1, double[] vector2)
        {
            #region Contracts

            if (vector1 == null) throw new ArgumentException($"{nameof(vector1)}=null");
            if (vector2 == null) throw new ArgumentException($"{nameof(vector2)}=null");

            #endregion

            // Require
            if (vector1.Length != vector2.Length) throw new ArgumentException("Vectors are not the same length!");

            // Calculate
            var dotProduct = DotProduct(vector1, vector2);
            var magnitudeOfVector1 = Magnitude(vector1);
            var magnitudeOfVector2 = Magnitude(vector2);
            var similarityScore = dotProduct / (magnitudeOfVector1 * magnitudeOfVector2);

            // Return
            return similarityScore;
        }

        private static double DotProduct(double[] vector1, double[] vector2)
        {
            #region Contracts

            if (vector1 == null) throw new ArgumentException($"{nameof(vector1)}=null");
            if (vector2 == null) throw new ArgumentException($"{nameof(vector2)}=null");

            #endregion

            // Require
            if (vector1.Length != vector2.Length) throw new ArgumentException("Vectors are not the same length!");

            // Calculate
            double result = 0.0;
            for (int i = 0; i < vector1.Length; i++)
            {
                result += vector1[i] * vector2[i];
            }

            // Return
            return result;
        }

        private static double Magnitude(double[] vector)
        {
            #region Contracts

            if (vector == null) throw new ArgumentException($"{nameof(vector)}=null");

            #endregion

            // Calculate
            double sum = 0.0;
            for (int i = 0; i < vector.Length; i++)
            {
                sum += vector[i] * vector[i];
            }

            // Return
            return Math.Sqrt(sum);
        }
    }
}
