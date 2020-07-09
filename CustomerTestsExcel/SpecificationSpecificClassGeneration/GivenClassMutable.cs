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
            // Rationalise simple and complex propertires
            // If a property is in simple and complex, and the simple one has only null values, then use the complex one
            var simplePropertiesWithoutNullComplexProperties = givenSimpleProperties.Select(p => p).ToList();
            foreach (var givenComplexProperty in givenComplexProperties)
            {
                if (simplePropertiesWithoutNullComplexProperties.Any(p => p.PropertyOrFunctionName == givenComplexProperty.PropertyName)
                    && simplePropertiesWithoutNullComplexProperties.Where(p => p.PropertyOrFunctionName == givenComplexProperty.PropertyName).All(p => p.ExcelPropertyType == ExcelPropertyType.Null))
                {
                    simplePropertiesWithoutNullComplexProperties = simplePropertiesWithoutNullComplexProperties.Where(p => p.PropertyOrFunctionName != givenComplexProperty.PropertyName).ToList();
                }
            }

            // simple properties need to check their values
            //  if there are only null values then the value type is null
            //  if there are null and primitive then nullable primtive
            //  if there are different types of primitive then oops
            var aggregatedSimpleProperties = new List<IGivenSimpleProperty>();
            foreach (var givenSimpleProperty in simplePropertiesWithoutNullComplexProperties)
            {
                var sameProperties = simplePropertiesWithoutNullComplexProperties.Where(p => p.PropertyOrFunctionName == givenSimpleProperty.PropertyOrFunctionName);

                var hasNullValues = sameProperties.Any(p => p.ExcelPropertyType == ExcelPropertyType.Null);

                // this wont work properly with StringNull at the moment, 
                // need to just stop using this I think
                var numberOfPrimitiveTypes = sameProperties.Select(p => p.ExcelPropertyType).Where(p => p.IsPrimitive()).Distinct().Count();

                if (numberOfPrimitiveTypes > 1)
                    throw new ExcelToCodeException($"Multiple different property types found for {givenSimpleProperty.PropertyOrFunctionName}");

                // nullable primitive
                if (numberOfPrimitiveTypes == 1 && hasNullValues == true)
                {
                    aggregatedSimpleProperties.Add(
                        new GivenSimpleProperty(
                            givenSimpleProperty.PropertyOrFunctionName,
                            givenSimpleProperty.CsharpCodeRepresentation,
                            givenSimpleProperty.ExcelPropertyType,
                            true
                        )
                    );
                }

                // primitve
                if (numberOfPrimitiveTypes == 1 && hasNullValues == false)
                {
                    aggregatedSimpleProperties.Add(givenSimpleProperty);
                }

                // null 
                aggregatedSimpleProperties.Add(givenSimpleProperty);
            }

            // aggregating functions can just take the first one
            // aggregating list and table properties can also take the first one
            // complex properties can just take first one
            // could check for incompatible things at end
            //  same name but different type is an error


            return
                new GivenClass(
                    Name,
                    AggregateProperties(aggregatedSimpleProperties),
                    givenSimpleProperties,
                    givenComplexProperties,
                    givenFunctions,
                    givenListProperties,
                    givenTableProperties,
                    IsRootClass);
        }

        public IReadOnlyList<IGivenClassProperty> AggregateProperties(
            IReadOnlyList<IGivenSimpleProperty> aggregatedGivenSimpleProperties)
        {
            foreach (var givenComplexProperty in givenComplexProperties)
            {
                AddProperty(
                    new GivenClassComplexProperty(
                        givenComplexProperty.PropertyName,
                        givenComplexProperty.ClassName));
            }

            // the simple properties are already aggreated, and shouldn't
            // conflict with any complex properties (as this is already
            // checked for), but they could conflict with other things
            foreach (var givenSimpleProperty in aggregatedGivenSimpleProperties)
            {
                AddProperty(
                    new GivenClassSimpleProperty(
                        givenSimpleProperty.PropertyOrFunctionName,
                        givenSimpleProperty.ExcelPropertyType,
                        givenSimpleProperty.CsharpCodeRepresentation,
                        givenSimpleProperty.Nullable
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
