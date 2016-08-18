using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDataProcessing
{
    public class Processor
    {
        private object[] markers;

        public Processor(int categories)
        {
            markers = new object[categories + 1];
        }
    }
}
