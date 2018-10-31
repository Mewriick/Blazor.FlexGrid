using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class LazyRequestParams
    {
        private readonly Dictionary<string, string> requestParams;

        public bool IsEmpty => requestParams.Count == 0;

        public string this[string key] => requestParams[key];

        public LazyRequestParams()
        {
            this.requestParams = new Dictionary<string, string>();
        }

        public LazyRequestParams AddParam(string key, string value)
        {
            var lowerKey = key.ToLower();
            if (requestParams.ContainsKey(lowerKey))
            {
                requestParams[lowerKey] = value;
            }
            else
            {
                requestParams.Add(lowerKey, value);
            }

            return this;
        }

        public override string ToString()
        {
            if (IsEmpty)
            {
                return string.Empty;
            }

            if (requestParams.Count == 1)
            {
                var firstEntry = requestParams.First();
                return $"{firstEntry.Key}={firstEntry.Value}&";
            }

            var sb = new StringBuilder();
            foreach (var requestParam in requestParams)
            {
                sb.AppendFormat($"{requestParam.Key}={requestParam.Value}&");
            }

            return sb.ToString();
        }
    }
}
