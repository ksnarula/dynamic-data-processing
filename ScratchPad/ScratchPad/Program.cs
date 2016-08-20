using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            PSquare.PSquarePercentileCalculator p = new PSquare.PSquarePercentileCalculator(0.9);

            double[] arr = new double[] { 0.02, 0.5, 0.74, 3.39, 0.83, 22.37, 10.15, 15.43, 38.62, 15.92, 34.6, 10.28, 1.47, 0.4, 0.05, 11.39, 0.27, 0.42, 0.09, 11.37 };
            
            for (int i = 0; i< arr.Length; i++)
            {
                Console.WriteLine(p.addObservation(arr[i]));
            }
        }
    }
}
