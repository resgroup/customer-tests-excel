﻿using CustomerTestsExcel.CodeOutputWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.Assertions
{
    public interface IAssertion<T>
    {
        bool Passed(T sut);
        void Write(T sut, bool passed, ITestOutputWriter writer);
    }
}
