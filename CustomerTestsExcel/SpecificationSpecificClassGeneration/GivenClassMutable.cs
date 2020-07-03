using CustomerTestsExcel.ExcelToCode;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassMutable
    {
        public bool IsRootClass { get; }
        public string Name { get; set; }

        readonly List<IGivenClassProperty> properties;
        public IReadOnlyList<IGivenClassProperty> Properties => properties;

        readonly List<IGivenSimpleProperty> givenSimpleProperties;
        readonly List<IGivenComplexProperty> givenComplexProperties;
        readonly List<IGivenFunction> givenFunctions;
        readonly List<IGivenListProperty> givenListProperties;
        readonly List<IGivenTableProperty> givenTableProperties;

        public GivenClassMutable(string name, bool isRootClass = false)
        {
            Name = name;
            IsRootClass = isRootClass;
            properties = new List<IGivenClassProperty>();

            givenSimpleProperties = new List<IGivenSimpleProperty>();
            givenComplexProperties = new List<IGivenComplexProperty>();
            givenFunctions = new List<IGivenFunction>();
            givenListProperties = new List<IGivenListProperty>();
            givenTableProperties = new List<IGivenTableProperty>();
        }

        public GivenClass CreateGivenClass()
        {
            // rationalise simple and complex propertires
            //  if a property is in simple and complex, and the simple one has only null values, then use the complex one
            var newGivenSimpleProperties = givenSimpleProperties.Select(p => p).ToList();
            foreach (var givenComplexProperty in givenComplexProperties)
            {
                if (newGivenSimpleProperties.Any(p => p.PropertyOrFunctionName == givenComplexProperty.PropertyName)
                    && newGivenSimpleProperties.Where(p => p.PropertyOrFunctionName == givenComplexProperty.PropertyName).All(p => p.ExcelPropertyType == ExcelPropertyType.Null))
                {
                    newGivenSimpleProperties = newGivenSimpleProperties.Where(p => p.PropertyOrFunctionName != givenComplexProperty.PropertyName).ToList();
                }
            }

            // simple properties need to check their values
            //  if there are only null values then the value type is null
            //  if there are null and primtive then nullable primtive
            //  if there are different types of primitive then error
            foreach (var givenSimpleProperty in givenSimpleProperties.Select(p => p.PropertyOrFunctionName).Distinct())
            {
            }


                // aggregating functions can just take the first one
                // aggregating list and table properties can also take the first one
                // complex properties can just take first one
                // could check for incompatible things at end
                //  same name but different type is an error


                return
                    new GivenClass(
                    Name,
                    AggregateProperties(),
                    givenSimpleProperties,
                    givenComplexProperties,
                    givenFunctions,
                    givenListProperties,
                    givenTableProperties,
                    IsRootClass);
        }

        public IReadOnlyList<IGivenClassProperty> AggregateProperties()
        {
            foreach (var givenComplexProperty in givenComplexProperties)
            {
                AddProperty(
                    new GivenClassComplexProperty(
                        givenComplexProperty.PropertyName,
                        givenComplexProperty.ClassName));
            }

            foreach (var givenSimpleProperty in givenSimpleProperties)
            {
                AddProperty(
                    new GivenClassSimpleProperty(
                        givenSimpleProperty.PropertyOrFunctionName,
                        givenSimpleProperty.ExcelPropertyType,
                        givenSimpleProperty.CsharpCodeRepresentation
                    )
                );
            }

            foreach (var givenFunction in givenFunctions)
            {
                AddProperty(
                    new GivenClassFunction(givenFunction.PropertyOrFunctionName));
            }

            foreach (var givenListProperty in givenListProperties)
            {
                AddProperty(
                    new GivenClassComplexListProperty(
                        givenListProperty.PropertyName,
                        givenListProperty.ClassName));
            }

            foreach (var givenTableProperty in givenTableProperties)
            {
                AddProperty(
                    new GivenClassComplexListProperty(
                        givenTableProperty.PropertyName,
                        givenTableProperty.ClassName));
            }

            return properties;
        }

        public void AddSimpleProperty(IGivenSimpleProperty givenSimpleProperty)
        {
            givenSimpleProperties.Add(givenSimpleProperty);

            //AddProperty(
            //    new GivenClassSimpleProperty(
            //        givenSimpleProperty.PropertyOrFunctionName,
            //        givenSimpleProperty.ExcelPropertyType,
            //        givenSimpleProperty.CsharpCodeRepresentation
            //    )
            //);
        }

        public void AddFunction(IGivenFunction givenFunction)
        {
            givenFunctions.Add(givenFunction);
            //AddProperty(
            //    new GivenClassFunction(givenSimpleProperty.PropertyOrFunctionName));
        }

        public void AddComplexProperty(IGivenComplexProperty givenComplexProperty)
        {
            givenComplexProperties.Add(givenComplexProperty);

            //AddProperty(
            //    new GivenClassComplexProperty(
            //        givenComplexProperty.PropertyName,
            //        givenComplexProperty.ClassName));
        }

        public void AddListProperty(IGivenListProperty givenListProperty)
        {
            givenListProperties.Add(givenListProperty);
            //AddProperty(
            //    new GivenClassComplexListProperty(
            //        givenListProperty.PropertyName,
            //        givenListProperty.ClassName));
        }

        public void AddTableProperty(IGivenTableProperty givenTableProperty)
        {
            givenTableProperties.Add(givenTableProperty);
            //AddProperty(
            //    new GivenClassComplexListProperty(
            //        givenTableProperty.PropertyName,
            //        givenTableProperty.ClassName));
        }

        void AddProperty(IGivenClassProperty property)
        {
            // could think about raising an exception if the property name already
            // exists but with a different type
            if (properties.Any(p => p.Name == property.Name) == false)
                properties.Add(property);
        }
    }
}
