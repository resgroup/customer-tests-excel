using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest.AnovaCalculator
{
    public class AnovaResult : IAnovaResult
    {
        public string VariableDescription { get; set;  }
        public double SS_Between { get; set;  }
        public int DF_Between { get; set; }
        public double MS_Between { get; set; }
        public double SS_Within { get; set; }
        public int DF_Within { get; set; }
        public double MS_Within { get; set; }
        public double F { get; set; }
        public double StatisticalSignificance { get; set; }
        public double EffectSize { get; set; }
    }
}
