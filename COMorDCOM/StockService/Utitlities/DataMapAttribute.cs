namespace System.Reflection.Utitlities
{
    public class DataMapAttribute: Attribute
    {
        public DataMapAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public DataMapAttribute()
        {
        }
        public static DataMapAttribute GetAttriubute(PropertyInfo fi)
        {
            var atr = fi.GetCustomAttributes(typeof(DataMapAttribute), false);
            if (atr.Length > 0)
                return atr[0] as DataMapAttribute;
            return null;
        }
    }
}
