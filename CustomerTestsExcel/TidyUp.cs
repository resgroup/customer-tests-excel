using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class TidyUp : IDisposable
    {
        private readonly Action tidyUp;

        public TidyUp(Action tidyUp)
        {
            this.tidyUp = tidyUp ?? throw new ArgumentNullException("tidyUp");
        }

        public TidyUp(Action makeMess, Action tidyUp) : this(tidyUp)
        {
            if (makeMess == null) throw new ArgumentNullException("makeMess");

            makeMess();
        }

        public void Dispose()
        {
            tidyUp();
        }
    }

    public class TidyUpWithRemember<T> : IDisposable
    {
        readonly Action<T> tidyUp;
        public T RememberedThing { get; }

        public TidyUpWithRemember(Func<T> makeMess, Action<T> tidyUp) 
        {
            if (makeMess == null) throw new ArgumentNullException("makeMess");
            this.tidyUp = tidyUp;

            RememberedThing = makeMess();
        }

        public void Dispose()
        {
            tidyUp(RememberedThing);
        }
    }
}
