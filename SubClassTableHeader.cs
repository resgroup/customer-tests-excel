using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel
{
    public class SubClassTableHeader : TableHeader, ITableHeader
    {
        public string ClassName { get; }
        public IEnumerable<ITableHeader> Headers { get; }

        public SubClassTableHeader(string propertyName, string className, IEnumerable<ITableHeader> creationalHeaders, IEnumerable<ITableHeader> headers)
            : base(propertyName)
        {
            ClassName = className;
            Headers = headers;
        }

        public bool Equals(ITableHeader other)
        {
            if (GetType().Equals(other.GetType()) == false) return false;

            var subClassOther = other as SubClassTableHeader;

            if (subClassOther.PropertyName != PropertyName || subClassOther.ClassName != ClassName) return false;

            if (EqualHeaders(Headers, subClassOther.Headers) == false) return false;

            return true;
        }

        bool EqualHeaders(IEnumerable<ITableHeader> ours, IEnumerable<ITableHeader> theirs)
        {
            if (ours.Count() != theirs.Count()) return false;

            var oursEnumerator = ours.GetEnumerator();
            var theirsEnumerator = theirs.GetEnumerator();

            while (oursEnumerator.MoveNext())
            {
                theirsEnumerator.MoveNext();

                if (oursEnumerator.Current.Equals(theirsEnumerator.Current) == false) return false;
            }

            return true;
        }

    }
}
