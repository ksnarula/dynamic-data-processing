using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSquare
{
    class PSquarePercentileCalculator
    {
        public double[][] markers;      //Array to keep track of marker position and height.
        private Stack<double> standardSettingValues;     //Initial stack to store the first 5 data values.
        private bool initialPhase;      //Switch toggling between the first phase (just collection) and second phase (marker adjustment).
        private double percentile;       //The user-defined percentile to calculate markers for.

        public PSquarePercentileCalculator(double percentile)
        {
            this.percentile = (double)percentile;

            markers = new double[5][];       //Default of 5 markers used when calculating quantiles as per the P-Square Algorithm.
            standardSettingValues = new Stack<double>();

            initialPhase = true;
        }

        /// <summary>
        /// Add the latest data value to the class and readjust all marker placements and heights.
        /// </summary>
        /// <param name="observation">double value of the latest data observation.</param>
        /// <returns>Height of the adjusted marker representing the percentile value for the dataset. -1 if there is not enough data to calculate.</returns>
        public double addObservation(double userObservation)
        {
            double observation = (double)userObservation;

            switch (initialPhase)
            {
                case true:      //Check whether the first 5 values have been added yet. If not, then initialPhase == true.
                    phase1Controller(observation);
                    break;

                case false:    //If initialPhase == false then call the controller method for Phase 2 (adjusting markers).
                    return phase2Controller(observation);
            }

            return -1;      //Return -1 if still in the initial phase and all markers have not yet been placed.
        }

        #region Controllers
        private void phase1Controller(double observation)
        {
            if (standardSettingValues.Count < markers.Length)
            {
                standardSettingValues.Push(observation);
            }
            else if (standardSettingValues.Count == (markers.Length))
            {
                initialPhase = !initialPhase;

                double[] temp = new double[standardSettingValues.Count];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = standardSettingValues.Pop();
                }

                Array.Sort(temp);

                for (int i = 0; i < temp.Length; i++)
                {
                    markers[i] = new double[] { i, temp[i] };
                }
            }
            else
                return;
        }

        private double phase2Controller(double observation)
        {
            //Check min.
            if (markers[0][1] > observation)
            {
                markers[0][1] = observation;    //Set height to new min. No need to shift marker.

                //Increment all marker positions to the right by one
                for (int i = 1; i <= markers.Length; i++)
                {
                    markers[i][0] = markers[i][0] + 1;
                }
            }

            //Check max.
            else if (markers[markers.Length - 1][1] <= observation)
            {
                markers[markers.Length - 1][0] = markers[markers.Length - 1][0] + 1;    //Move marker over one.
                markers[markers.Length - 1][1] = observation;   //Set height to new max.
            }

            //Make internal adjustments if not an extreme value.
            else
            {
                int index = markers.Length - 1;

                //Find index of first marker with greater height than observation.
                for (int i = 0; i < markers.Length; i++)
                {
                    if (markers[i][1] > observation)
                    {
                        index = i;
                        break;
                    }
                }

                //Increment all markers to the right of observation by one unit (start at index).
                for (int i = index; i < markers.Length; i++)
                {
                    markers[i][0] = markers[i][0] + 1;
                }

                //Check for ideal marker placement and adjust for parabolic trend if that is not present
                for (int i = 1; i < markers.Length - 1; i++)
                {
                    checkForIdealPlacement(i);
                }


            }

            return markers[markers.Length / 2][1];        //Return the value of the middle marker, representing the percentile value.
        }
        #endregion


        #region Check Methods
        //Check whether the middle three markers are within 1 space of their ideal marker positions 
        //If they are not, then adjust marker heights and placements so they conform to the assumed qudratic trend
        private void checkForIdealPlacement(int markerIndex)
        {
            double n = markers[markers.Length - 1][0];    //Total number of data points (placement of the final marker).
            double idealPlacement;

            //Calculate ideal positions for the three internal markers
            switch (markerIndex)
            {
                case 1:
                    idealPlacement = ((n - 1) * percentile / 2) + 1;
                    break;

                case 2:
                    idealPlacement = ((n - 1) * percentile) + 1;
                    break;

                case 3:
                    idealPlacement = ((n - 1) * (1 + percentile) / 2) + 1;
                    break;

                default:
                    return;
            }

            double d = idealPlacement - markers[markerIndex][0];     //Calculate delta between ideal placement and real placement of marker

            //If marker placement is not within one unit of ideal placement, then adjust its height value
            if ((d > 1) || (d < -1))
            {
                if (d > 1)
                {
                    d = 1;
                }
                else if (d < -1)
                {
                    d = -1;
                }
                else
                {
                    return;
                }
                adjustForNonidealPlacement(markerIndex, d);
            }

        }

        private void adjustForNonidealPlacement(int markerIndex, double d)
        {
            double height = markers[markerIndex][1];     //Get current marker height
            double forwardMarker = markers[markerIndex + 1][0], backwardMarker = markers[markerIndex - 1][0];      //Get adjacent marker placements

            double a = d / (forwardMarker - backwardMarker);
            double b = (markers[markerIndex][0] - markers[markerIndex - 1][0] + d) * (markers[markerIndex + 1][1] - height) / (markers[markerIndex + 1][0] - markers[markerIndex][0]);
            double c = (markers[markerIndex + 1][0] - markers[markerIndex][0] - d) * (height - markers[markerIndex - 1][1]) / (markers[markerIndex][0] - markers[markerIndex - 1][0]);

            markers[markerIndex][1] = height + (a * (b + c));
        }
        #endregion
    }
}
