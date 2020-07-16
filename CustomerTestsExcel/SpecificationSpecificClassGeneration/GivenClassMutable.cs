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

        readonly List<IVisitedGivenSimpleProperty> givenSimpleProperties;
        readonly List<IVisitedGivenComplexProperty> givenComplexProperties;
        readonly List<IVisitedGivenFunction> givenFunctions;
        readonly List<IVisitedGivenListProperty> givenListProperties;
        readonly List<IVisitedGivenTableProperty> givenTableProperties;

        public GivenClassMutable(string name, bool isRootClass = false)
        {
            Name = name;
            IsRootClass = isRootClass;
            properties = new List<IGivenClassProperty>();

            givenSimpleProperties = new List<IVisitedGivenSimpleProperty>();
            givenComplexProperties = new List<IVisitedGivenComplexProperty>();
            givenFunctions = new List<IVisitedGivenFunction>();
            givenListProperties = new List<IVisitedGivenListProperty>();
            givenTableProperties = new List<IVisitedGivenTableProperty>();
        }

        public void AddSimpleProperty(IVisitedGivenSimpleProperty givenSimpleProperty)
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

        public void AddFunction(IVisitedGivenFunction givenFunction)
        {
            givenFunctions.Add(givenFunction);
            //AddProperty(
            //    new GivenClassFunction(givenSimpleProperty.PropertyOrFunctionName));
        }

        public void AddComplexProperty(IVisitedGivenComplexProperty givenComplexProperty)
        {
            givenComplexProperties.Add(givenComplexProperty);

            //AddProperty(
            //    new GivenClassComplexProperty(
            //        givenComplexProperty.PropertyName,
            //        givenComplexProperty.ClassName));
        }

        public void AddListProperty(IVisitedGivenListProperty givenListProperty)
        {
            givenListProperties.Add(givenListProperty);
            //AddProperty(
            //    new GivenClassComplexListProperty(
            //        givenListProperty.PropertyName,
            //        givenListProperty.ClassName));
        }

        public void AddTableProperty(IVisitedGivenTableProperty givenTableProperty)
        {
            givenTableProperties.Add(givenTableProperty);
            //AddProperty(
            //    new GivenClassComplexListProperty(
            //        givenTableProperty.PropertyName,
            //        givenTableProperty.ClassName));
        }

        public GivenClass CreateGivenClass()
        {
            // Rationalise simple and complex propertires
            // If a property is in simple and complex, and the simple one has only null values, then use the complex one
            var simplePropertiesWithoutNullComplexProperties = SimplePropertiesWithoutNullComplexProperties();

            // simple properties need to check their values
            //  if there are only null values then the value type is null
            //  if there are null and primitive then nullable primtive
            //  if there are different types of primitive then oops
            var rationalisedSimpleProperties = RationaliseSimpleProperties(simplePropertiesWithoutNullComplexProperties);

            // functions can be overloaded, so I think we can just use all of them
            var rationalisedFunctions = givenFunctions;

            // complex properties with the same name but a different property type are invalid
            var rationalisedComplexProperties = RationaliseComplexProperties();

            // list / table properties are enumerables of a certain type
            // Properties with then same name but different types are invalid
            var rationalisedListProperties = RationaliseListProperties();
            var rationalisedTableProperties = RationaliseTableProperties();

            // Properties of all these varieties exist on this class, and  
            // properties of the same name but different type are invalid
            // (for example a list property and a function)
            var rationalisedCombinedProperties = RationaliseProperties(
                rationalisedSimpleProperties,
                rationalisedComplexProperties,
                rationalisedFunctions,
                rationalisedListProperties,
                rationalisedTableProperties
            );

            return
                new GivenClass(
                    Name,
                    rationalisedCombinedProperties,
                    rationalisedSimpleProperties,
                    rationalisedComplexProperties,
                    rationalisedFunctions,
                    rationalisedListProperties,
                    rationalisedTableProperties,
                    IsRootClass);
        }

        List<IVisitedGivenListProperty> RationaliseListProperties()
        {
            var rationalisedListProperties = new List<IVisitedGivenListProperty>();
            foreach (var givenListProperty in givenListProperties)
            {
                if (rationalisedListProperties.Any(r => r.PropertyName == givenListProperty.PropertyName))
                    continue;

                var sameProperties = givenListProperties.Where(p => p.PropertyName == givenListProperty.PropertyName).Select(p => p.ClassName).ToList();
                var sameComplexProperties = givenComplexProperties.Where(p => p.PropertyName == givenListProperty.PropertyName).Select(p => p.ClassName);
                sameProperties.AddRange(sameComplexProperties);
                var uniqueClassNames = sameProperties.Distinct();

                if (uniqueClassNames.Count() > 1)
                {
                    var distinctTypes = string.Join(", ", uniqueClassNames);
                    throw new ExcelToCodeException($"Multiple different types found for Enumerable {givenListProperty.PropertyName}: {distinctTypes}");
                }

                rationalisedListProperties.Add(givenListProperty);
            }

            return rationalisedListProperties;
        }

        List<IVisitedGivenTableProperty> RationaliseTableProperties()
        {
            var rationalisedTableProperties = new List<IVisitedGivenTableProperty>();
            foreach (var givenTableProperty in givenTableProperties)
            {
                if (rationalisedTableProperties.Any(r => r.PropertyName == givenTableProperty.PropertyName))
                    continue;

                var sameProperties = givenListProperties.Where(p => p.PropertyName == givenTableProperty.PropertyName).Select(p => p.ClassName).ToList();
                var sameComplexProperties = givenComplexProperties.Where(p => p.PropertyName == givenTableProperty.PropertyName).Select(p => p.ClassName);
                sameProperties.AddRange(sameComplexProperties);
                var uniqueClassNames = sameProperties.Distinct();

                if (uniqueClassNames.Count() > 1)
                {
                    var distinctTypes = string.Join(", ", uniqueClassNames);
                    throw new ExcelToCodeException($"Multiple different types found for Enumerable {givenTableProperty.PropertyName}: {distinctTypes}");
                }

                rationalisedTableProperties.Add(givenTableProperty);
            }

            return rationalisedTableProperties;
        }

        List<IVisitedGivenComplexProperty> RationaliseComplexProperties()
        {
            var rationalisedComplexProperties = new List<IVisitedGivenComplexProperty>();
            foreach (var givenComplexProperty in givenComplexProperties)
            {
                if (rationalisedComplexProperties.Any(r => r.PropertyName == givenComplexProperty.PropertyName))
                    continue;

                var sameProperties = givenComplexProperties.Where(p => p.PropertyName == givenComplexProperty.PropertyName);

                var numberOfDistinctTypes = sameProperties.Select(p => p.ClassName).Distinct().Count();

                if (numberOfDistinctTypes > 1)
                {
                    var distinctTypes = string.Join(", ", sameProperties.Select(p => p.ClassName).Distinct());
                    throw new ExcelToCodeException($"Multiple different types found for {givenComplexProperty.PropertyName}: {distinctTypes}");
                }

                rationalisedComplexProperties.Add(givenComplexProperty);
            }

            return rationalisedComplexProperties;
        }

        static List<IVisitedGivenSimpleProperty> RationaliseSimpleProperties(List<IVisitedGivenSimpleProperty> simplePropertiesWithoutNullComplexProperties)
        {
            var rationalisedSimpleProperties = new List<IVisitedGivenSimpleProperty>();
            foreach (var givenSimpleProperty in simplePropertiesWithoutNullComplexProperties)
            {
                if (rationalisedSimpleProperties.Any(r => r.PropertyOrFunctionName == givenSimpleProperty.PropertyOrFunctionName))
                    continue;

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
                    rationalisedSimpleProperties.Add(
                        new GivenSimpleProperty(
                            givenSimpleProperty.PropertyOrFunctionName,
                            givenSimpleProperty.CsharpCodeRepresentation,
                            givenSimpleProperty.ExcelPropertyType,
                            true
                        )
                    );
                }
                // primitve
                else if (numberOfPrimitiveTypes == 1 && hasNullValues == false)
                {
                    rationalisedSimpleProperties.Add(givenSimpleProperty);
                }
                else
                {
                    // null 
                    rationalisedSimpleProperties.Add(givenSimpleProperty);
                }
            }

            return rationalisedSimpleProperties;
        }

        List<IVisitedGivenSimpleProperty> SimplePropertiesWithoutNullComplexProperties()
        {
            var simplePropertiesWithoutNullComplexProperties = givenSimpleProperties.Select(p => p).ToList();
            foreach (var givenComplexProperty in givenComplexProperties)
            {
                if (simplePropertiesWithoutNullComplexProperties.Any(p => p.PropertyOrFunctionName == givenComplexProperty.PropertyName)
                    && simplePropertiesWithoutNullComplexProperties.Where(p => p.PropertyOrFunctionName == givenComplexProperty.PropertyName).All(p => p.ExcelPropertyType == ExcelPropertyType.Null))
                {
                    simplePropertiesWithoutNullComplexProperties = simplePropertiesWithoutNullComplexProperties.Where(p => p.PropertyOrFunctionName != givenComplexProperty.PropertyName).ToList();
                }
            }

            return simplePropertiesWithoutNullComplexProperties;
        }

        IReadOnlyList<IGivenClassProperty> RationaliseProperties(
            IReadOnlyList<IVisitedGivenSimpleProperty> rationalisedGivenSimpleProperties,
            IReadOnlyList<IVisitedGivenComplexProperty> rationalisedGivenComplexProperties,
            IReadOnlyList<IVisitedGivenFunction> rationalisedGivenFunctions,
            IReadOnlyList<IVisitedGivenListProperty> rationalisedGivenListProperties,
            IReadOnlyList<IVisitedGivenTableProperty> rationalisedGivenTableProperties
        )
        {
            properties.Clear();

            foreach (var givenComplexProperty in rationalisedGivenComplexProperties)
            {
                AddProperty(
                    new GivenClassComplexProperty(
                        givenComplexProperty.PropertyName,
                        givenComplexProperty.ClassName));
            }

            foreach (var givenSimpleProperty in rationalisedGivenSimpleProperties)
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

            foreach (var givenFunction in rationalisedGivenFunctions)
            {
                AddProperty(
                    new GivenClassFunction(givenFunction.PropertyOrFunctionName));
            }

            foreach (var givenListProperty in rationalisedGivenListProperties)
            {
                AddProperty(
                    new GivenClassComplexListProperty(
                        givenListProperty.PropertyName,
                        givenListProperty.ClassName));
            }

            foreach (var givenTableProperty in rationalisedGivenTableProperties)
            {
                AddProperty(
                    new GivenClassComplexListProperty(
                        givenTableProperty.PropertyName,
                        givenTableProperty.ClassName));
            }

            return properties;
        }

        void AddProperty(IGivenClassProperty property)
        {
            if (properties.Any(p => p.Name == property.Name))
            {
                var multipleTypes = string.Join(", ", properties.Where(p => p.Name == property.Name).Select(s => s.ToString()));

                throw new ExcelToCodeException($"Multiple different property types found for {property.Name}: {multipleTypes}");
            }

            properties.Add(property);
        }
    }
}
