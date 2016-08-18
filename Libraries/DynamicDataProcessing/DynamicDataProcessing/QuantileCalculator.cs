using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDataProcessing
{
    class QuantileCalculator
    {
        private int numOfCategories;
        float[][] markers;        
        private Stack<float> standardSettingValues;
        float[] percentiles;
        private bool initialPhase;

        public QuantileCalculator()
        {
            numOfCategories = 10;
            if(numOfCategories%2 != 0)
            {
                numOfCategories++;
            }

            markers = new float[numOfCategories + 1][];
            standardSettingValues = new Stack<float>();
            percentiles = new float[1];
            
            initialPhase = true;
        }

        public void addObservation(float observation)
        {
            if (initialPhase)
            {
                phase1Controller(observation);
            }
            else
            {
                phase2Controller(observation);
            }
        }

        #region Controllers
        private void phase1Controller(float observation)
        {
            if (standardSettingValues.Count < numOfCategories + 1)
            {
                standardSettingValues.Push(observation);
            }
            else if (standardSettingValues.Count == (numOfCategories + 1))
            {
                float[] temp = new float[standardSettingValues.Count];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = standardSettingValues.Pop();
                }

                Array.Sort(temp);
                for (int i = 0; i < temp.Length; i++)
                {
                    markers[i] = new float[] { i, temp[i] };
                }
            }
            else
                return;
        }

        private void phase2Controller(float observation)
        {
            //Check min.
            if(markers[0][1] > observation)
            {
                markers[0][1] = observation;    //Set height to new min. No need to shift marker.
            }
            //Check max.
            else if (markers[markers.Length-1][1] < observation)
            {
                markers[markers.Length - 1][0] = markers[markers.Length - 1][0] + 1;    //Move marker over one.
                markers[markers.Length - 1][1] = observation;   //Set height to new max.
            }
            //Make internal adjustments if not an extreme value.
            else
            {
                int index = markers.Length-1;
                int category;
                //Find index of first marker with greater height than observation.
                for(int i = 0; i < markers.Length; i++)
                {
                    if(markers[i][1] > observation)
                    {
                        index = i;
                    }
                }

                category = index - 1;

                //Increment all markers to the right of observation by one unit EXCEPT for the max.
                for(int i = index; i < markers.Length-1; i++)
                {
                    markers[i][0] = markers[i][0] + 1;
                }

                checkParabolicTrendOfMarkers();

            }
        }
        #endregion


        #region Check Methods
        //All check methods return a bool indicating whether or not a correction was made.
        private void checkParabolicTrendOfMarkers()
        {
            for(int i = 1; i < markers.Length-3; i++)    //Start at the first internal value and move every three until the third from last internal value.
            {

            }
        }

        private void checkForOverlappingMarkers()
        {

        }
        #endregion
    }
}
