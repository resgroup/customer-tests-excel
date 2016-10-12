using System;
using System.Collections.Generic;

namespace RES.Specification
{
    public class CreateTableHeaders
    {
        private readonly IReportsSpecificationSetup _properties;

        public CreateTableHeaders(IReportsSpecificationSetup properties)
        {
            this._properties = properties;
        }

        public IEnumerable<ITableHeader> Calculate()
        {
            foreach (var valueProperty in _properties.ValueProperties)
            {
                yield return new PropertyTableHeader(valueProperty.PropertyName);
            }
        }
    }
}