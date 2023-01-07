using System.Collections.Generic;

namespace PhotoSync.Extensions;

public static class ValidationErrorExtensions
{
    public static void AddError(this IDictionary<string, IList<string>> errors, string fieldName, string error)
    {
        if (!errors.ContainsKey(fieldName))
        {
            errors.Add(fieldName, new List<string>());
        }

        errors[fieldName].Add(error);
    }
}
