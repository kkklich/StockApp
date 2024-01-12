using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.Resources.Helpers;

public static class ClassComparator
{
    /// <summary>
    ///     Compare two objects of different types by property names, returns the list of differences (property names), if
    ///     objects are the same returns empty list, also
    ///     ignores properties from one class that not exist in another. mapNames dictionary is for map different names that
    ///     refers to the same logical field, cannot
    ///     compare nested properties, only one level
    /// </summary>
    public static List<string> FindDifferences<TClass1, TClass2>(TClass1 class1, TClass2 class2,
        Dictionary<string, string> mapNames = null, List<string> ignoreProperties = null)
    {
        var result = new List<string>();

        foreach (var prop in class1.GetType().GetProperties())
        {
            if (ignoreProperties != null && ignoreProperties.Contains(prop.Name)) continue;

            var secondClassPropName = prop.Name;
            if (mapNames != null && mapNames.ContainsKey(prop.Name))
                secondClassPropName = mapNames.GetValueOrDefault(prop.Name);

            var secondClassProperty = class2.GetType().GetProperty(secondClassPropName);
            if (secondClassProperty == null) continue;

            if (prop.GetValue(class1)?.ToString() != secondClassProperty.GetValue(class2)?.ToString())
                result.Add(prop.Name);
        }

        return result;
    }
}