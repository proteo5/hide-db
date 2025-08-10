using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Proteo5.HideDB.Lib.Utils
{
    public static class SqlParameterExtractor
    {
        public static List<string> ExtractParameters(string sql)
        {
            var parameters = new List<string>();
            var parameterPattern = @"@(\w+)";
            var matches = Regex.Matches(sql, parameterPattern);
            
            foreach (Match match in matches)
            {
                var param = match.Groups[1].Value;
                if (!parameters.Contains(param))
                    parameters.Add(param);
            }
            
            return parameters;
        }
    }
}