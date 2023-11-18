using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JUtils
{
    /// <summary>
    /// Utilities class working with assemblies
    /// </summary>
    public static class AssemblyJUtils
    {
        /// <summary>
        /// Get all types in all assemblies
        /// </summary>
        public static IEnumerable<Type> GetAllTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
        }


        public static Type GetTypeFromCode(int typeHashCode, int assemblyHashCode)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetHashCode() == assemblyHashCode);
            return assembly == null ? null : assembly.GetTypes().FirstOrDefault(x => x.GetHashCode() == typeHashCode);
        }


        /// <summary>
        /// Get the type from a name
        /// </summary>
        /// <param name="typeName">Assembly type name</param>
        /// <returns>The type</returns>
        public static Type GetType(string typeName)
        {
            string[] splitted = typeName.Split(' ');
            string assemblyName = splitted[0];
            string name = splitted[^1];

            return Assembly.Load(assemblyName).GetType(name);
        }


        /// <summary>
        /// Check if the given type extends the target type
        /// </summary>
        /// <param name="current">The type to check</param>
        /// <param name="target">The desired type</param>
        /// <returns>True if current extends the target</returns>
        public static bool ExtendsClassOrInterface(Type current, Type target)
        {
            if (target.IsInterface) return current.GetInterface(target.FullName) is not null;
            return current.IsSubclassOf(target);
        }
    }
}
