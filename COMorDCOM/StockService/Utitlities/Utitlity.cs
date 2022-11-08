using StockService.DataSourceHandler;

namespace System.Reflection.Utitlities
{
    public static class Utitlity
    {
        public static object ToValue<T>(this T t, string topicType)
        {
            if (t == null)
                return null;
            string allValue = string.Empty;
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var v1 = DataMapAttribute.GetAttriubute(prop);
                if ((v1 != null && v1.Name.ToUpper() == topicType.ToUpper()) || prop.Name.ToUpper() == topicType.ToUpper())
                    return prop.GetValue(t);
                if (v1 != null && topicType.ToUpper() == "ALL")
                {
                    allValue += prop.GetValue(t) + "|";
                }
            }
            return allValue;
        }
    }
}
