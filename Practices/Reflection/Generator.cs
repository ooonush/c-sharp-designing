using System;
using System.Linq;
using System.Reflection;

namespace Reflection.Randomness
{
    public class Generator<T> where T : new()
    {
        private static readonly Tuple<PropertyInfo,FromDistribution>[] PropertiesAndDistributions;
        
        static Generator()
        {
            PropertiesAndDistributions = typeof(T)
                .GetProperties()
                .Where(info => info.GetCustomAttributes(typeof(FromDistribution), true).Length > 0)
                .Select(info => Tuple.Create(info, (FromDistribution)info.GetCustomAttribute(typeof(FromDistribution))))
                .ToArray();
        }

        public T Generate(Random rnd)
        {
            var result = new T();
            foreach ((PropertyInfo property, FromDistribution distributionAttribute) in PropertiesAndDistributions)
            {
                try
                {
                    var distribution = (IContinuousDistribution)Activator
                        .CreateInstance(distributionAttribute.DistributionType, distributionAttribute.Parameters);
                    property.SetValue(result, distribution.Generate(rnd));
                }
                catch
                {
                    throw new ArgumentException(distributionAttribute.DistributionType.ToString());
                }
            }

            return result;
        }
    }
    
    public class FromDistribution : Attribute
    {
        public readonly Type DistributionType;
        public readonly object[] Parameters;
        
        public FromDistribution(Type distributionType, params object[] parameters)
        {
            Parameters = parameters;
            DistributionType = distributionType;
        }
    }
}
