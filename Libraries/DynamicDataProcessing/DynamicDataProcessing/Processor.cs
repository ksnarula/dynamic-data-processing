using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDataProcessing
{
    public class Processor
    {
        private object[] markers;
        private Stack<object> initialDataValues;

        private bool sufficiemtDataAcquired;

        #region Constructor
        /// <summary>
        /// Creates an instance of the Processor class.
        /// </summary>
        /// <param name="categories">Number of categories/classifications for the data set. Each incoming data point will be categorized into one of these based on numerical value. </param>
        public Processor(int categories)
        {
            markers = new object[categories + 1];
            initialDataValues = new Stack<object>();

            sufficiemtDataAcquired = false;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Uses the passed data point to adjust markers and classify the current datum as a member of a category. 
        /// Returns -1 when an error occurred and the value could not be categorized.
        /// Return value will be -1 for the first 15 values, until there is sufficient data to establish the markers for each category.
        /// </summary>
        /// <param name="datum">The latest numerical data value</param>
        /// <returns></returns>
        public int addDataPoint(object datum)
        {
            int category = -1;

            if (sufficiemtDataAcquired)
            {
                //Insert processing calls
            }
            else
            {
                //Insert default action for first 15 data values
            }

            return category;
        }
        #endregion


        #region Private methods
        
        #endregion

    }
}
