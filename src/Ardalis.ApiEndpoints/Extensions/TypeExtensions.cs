using System.Collections.Generic;

namespace System
{
    internal static class TypeExtensions
    {
        public static IEnumerable<Type> GetBaseTypesAndThis(this Type type)
        {
            Type current = type;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }
    }
}
