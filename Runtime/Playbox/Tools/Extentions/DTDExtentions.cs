using System.Collections.Generic;
using DevToDev.Analytics;

namespace Playbox.Tools.Extentions
{
    public static class DtdExtentions
    {
        public static DTDCustomEventParameters ToCustomParameters(
            this List<KeyValuePair<string,string>> parametersList)
        {
            
            DTDCustomEventParameters parameters = new DTDCustomEventParameters();
            
            foreach (var item in parametersList)
            {
                parameters.Add(item.Key, item.Value);
            }
            
            return parameters;
        }
    }
}