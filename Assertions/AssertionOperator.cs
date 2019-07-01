using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CustomerTestsExcel.Assertions
{
    public enum AssertionOperator
    {
        Equality
    }

    public static class AssertionOperatorExtensionMethods
    {
        // can we replace this something clever that uses the description attribute on the enum itself?
        public static string ToDescription(this AssertionOperator en)
        {
            switch (en)
            {
                case AssertionOperator.Equality: return "=";
                default: return en.ToString();
            }
        }

        public static string ToDescription(this AssertionOperator? en)
        {
            return en.HasValue ? en.Value.ToDescription() : string.Empty;
        }

        public static int CompareTo(this AssertionOperator? a, AssertionOperator? b)
        {
            if (!a.HasValue && !b.HasValue)
                return 0;
            else if (!a.HasValue)
                return int.MinValue;
            else if (!b.HasValue)
                return int.MaxValue;
            else
                return a.Value.CompareTo(b.Value);
        }

        public static AssertionOperator ToAssertionOperator(this string AssertionOperatorText)
        {
            return (AssertionOperator)Enum.Parse(typeof(AssertionOperator), AssertionOperatorText);
        }
    }

}
