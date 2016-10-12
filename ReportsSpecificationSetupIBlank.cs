using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification
{
    // this class does most of the hard work in implementing IReportsSpecificationSetup. Our generated specificationspericicsetup classes can't use it, as they need to inherit the concrete business object, but custom setup classes can use it if they are implementing an interface rather than inheriting.
    public class ReportsSpecificationSetupIBlank : ReportsSpecificationSetup, IBlank
    {
        public ReportsSpecificationSetupIBlank() 
            : base () { }

        public ReportsSpecificationSetupIBlank(IReportsSpecificationSetup creationalProperties)
            : base(creationalProperties){}

        public IBlank BusinessInterface
        {
            get { return this; }
        }
    }
}
