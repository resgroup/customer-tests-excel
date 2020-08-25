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

        readonly List<IVisitedGivenSimpleProperty> visitedGivenSimpleProperties;
        readonly List<IVisitedGivenComplexProperty> visitedGivenComplexProperties;
        readonly List<IVisitedGivenFunction> visitedGivenFunctions;
        readonly List<IVisitedGivenListProperty> visitedGivenListProperties;
        readonly List<IVisitedGivenTableProperty> visitedGivenTableProperties;
        readonly List<GivenClassSimpleProperty> givenSimpleProperties;
        readonly List<GivenClassComplexProperty> givenComplexProperties;
        readonly List<GivenClassFunction> givenFunctions;
        readonly List<GivenClassComplexListProperty> givenComplexListProperties;

        public GivenClassMutable(string name, bool isRootClass = false)
        {
            Name = name;
            IsRootClass = isRootClass;
            properties = new List<IGivenClassProperty>();

            visitedGivenSimpleProperties = new List<IVisitedGivenSimpleProperty>();
            visitedGivenComplexProperties = new List<IVisitedGivenComplexProperty>();
            visitedGivenFunctions = new List<IVisitedGivenFunction>();
            visitedGivenListProperties = new List<IVisitedGivenListProperty>();
            visitedGivenTableProperties = new List<IVisitedGivenTableProperty>();

            givenSimpleProperties = new List<GivenClassSimpleProperty>();
            givenComplexProperties = new List<GivenClassComplexProperty>();
            givenFunctions = new List<GivenClassFunction>();
            givenComplexListProperties = new List<GivenClassComplexListProperty>();
        }

        public void AddSimpleProperty(IVisitedGivenSimpleProperty givenSimpleProperty) =>
            visitedGivenSimpleProperties.Add(givenSimpleProperty);

        public void AddFunction(IVisitedGivenFunction givenFunction) =>
            visitedGivenFunctions.Add(givenFunction);

        public void AddComplexProperty(IVisitedGivenComplexProperty givenComplexProperty) =>
            visitedGivenComplexProperties.Add(givenComplexProperty);

        public void AddListProperty(IVisitedGivenListProperty givenListProperty) =>
            visitedGivenListProperties.Add(givenListProperty);

        public void AddTableProperty(IVisitedGivenTableProperty givenTableProperty) =>
            visitedGivenTableProperties.Add(givenTableProperty);

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

            // Functions should all have different names, but this is enforced
            // anyway in RationaliseProperties so not going to worry about it
            // for now
            var rationalisedFunctions = RationaliseFunctions();

            // complex properties with the same name but a different property type are invalid
            var rationalisedComplexProperties = RationaliseComplexProperties();

            // list / table properties are enumerables of a certain type
            // Properties with then same name but different types are invalid
            var rationalisedListProperties = RationaliseListProperties();
            var rationalisedTableProperties = RationaliseTableProperties();

            // Properties of all these varieties exist on this class, and  
            // properties of the same name but different type are invalid
            // (for example a list property and a function)

            // RationaliseProperties also set givenSimpleProperties and similar
            // in a very hacky way. We should refactor this.
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
                    givenSimpleProperties,
                    givenComplexProperties,
                    givenFunctions,
                    givenComplexListProperties,
                    IsRootClass);
        }

        List<IVisitedGivenListProperty> RationaliseListProperties()
        {
            var rationalisedListProperties = new List<IVisitedGivenListProperty>();
            foreach (var givenListProperty in visitedGivenListProperties)
            {
                if (rationalisedListProperties.Any(r => r.PropertyName == givenListProperty.PropertyName))
                    continue;

                var sameProperties = visitedGivenListProperties.Where(p => p.PropertyName == givenListProperty.PropertyName).Select(p => p.ClassName).ToList();
                var sameComplexProperties = visitedGivenComplexProperties.Where(p => p.PropertyName == givenListProperty.PropertyName).Select(p => p.ClassName);
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
            foreach (var givenTableProperty in visitedGivenTableProperties)
            {
                if (rationalisedTableProperties.Any(r => r.PropertyName == givenTableProperty.PropertyName))
                    continue;

                var sameProperties = visitedGivenListProperties.Where(p => p.PropertyName == givenTableProperty.PropertyName).Select(p => p.ClassName).ToList();
                var sameComplexProperties = visitedGivenComplexProperties.Where(p => p.PropertyName == givenTableProperty.PropertyName).Select(p => p.ClassName);
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

        List<IVisitedGivenFunction> RationaliseFunctions()
        {
            var rationalisedFunctions = new List<IVisitedGivenFunction>();
            foreach (var givenFunction in visitedGivenFunctions)
            {
                if (rationalisedFunctions.Any(r => r.PropertyOrFunctionName == givenFunction.PropertyOrFunctionName))
                    continue;

                rationalisedFunctions.Add(givenFunction);
            }

            return rationalisedFunctions;
        }

        List<IVisitedGivenComplexProperty> RationaliseComplexProperties()
        {
            var rationalisedComplexProperties = new List<IVisitedGivenComplexProperty>();
            foreach (var givenComplexProperty in visitedGivenComplexProperties)
            {
                if (rationalisedComplexProperties.Any(r => r.PropertyName == givenComplexProperty.PropertyName))
                    continue;

                var sameProperties = visitedGivenComplexProperties.Where(p => p.PropertyName == givenComplexProperty.PropertyName);

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
                        new VisitedGivenSimpleProperty(
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
            var simplePropertiesWithoutNullComplexProperties = visitedGivenSimpleProperties.Select(p => p).ToList();
            foreach (var givenComplexProperty in visitedGivenComplexProperties)
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
            givenSimpleProperties.Clear();

            foreach (var givenComplexProperty in rationalisedGivenComplexProperties)
            {
                var complexProperty = 
                    new GivenClassComplexProperty(
                        givenComplexProperty.PropertyName,
                        givenComplexProperty.ClassName);

                givenComplexProperties.Add(complexProperty);
                AddProperty(complexProperty);
            }

            foreach (var givenSimpleProperty in rationalisedGivenSimpleProperties)
            {
                var simpleProperty =
                    new GivenClassSimpleProperty(
                        givenSimpleProperty.PropertyOrFunctionName,
                        givenSimpleProperty.ExcelPropertyType,
                        givenSimpleProperty.CsharpCodeRepresentation,
                        givenSimpleProperty.Nullable
                    );

                givenSimpleProperties.Add(simpleProperty);
                AddProperty(simpleProperty);
            }

            foreach (var givenFunction in rationalisedGivenFunctions)
            {
                var function = new GivenClassFunction(givenFunction.PropertyOrFunctionName);

                givenFunctions.Add(function);
                AddProperty(function);
            }

            foreach (var givenListProperty in rationalisedGivenListProperties)
            {
                var complexListProperty = 
                    new GivenClassComplexListProperty(
                        givenListProperty.PropertyName,
                        givenListProperty.ClassName);

                givenComplexListProperties.Add(complexListProperty);
                AddProperty(complexListProperty);
            }

            foreach (var givenTableProperty in rationalisedGivenTableProperties)
            {
                var complexListProperty =
                    new GivenClassComplexListProperty(
                        givenTableProperty.PropertyName,
                        givenTableProperty.ClassName);

                givenComplexListProperties.Add(complexListProperty);
                AddProperty(complexListProperty);
            }

            return properties;
        }

        void AddProperty(IGivenClassProperty property)
        {
            if (properties.Any(p => p.Name == property.Name))
            {
                var allProperties = properties.ToList();
                allProperties.Add(property);
                var multipleTypes = string.Join(", ", allProperties.Where(p => p.Name == property.Name).Select(s => s.ToString()));

                throw new ExcelToCodeException($"Multiple different property types found for {property.Name}: {multipleTypes}");
            }

            properties.Add(property);
        }
    }
}
