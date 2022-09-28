namespace TestingApp
{
    public static class Enums
    {
        public enum ArtType
        {
            Food = 0,
            Drink = 1,
            Else = 2
        }

        public enum Roles
        {
            SuperAdmin,
            Admin,
            Moderator,
            User
        }

        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(
                                                            IEnumerable<TSource> items,
                                                            Converter<TSource, TKey> keySelector,
                                                            Converter<TSource, TValue> valueSelector)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            foreach (TSource item in items)
            {
                result.Add(keySelector(item), valueSelector(item));
            }
            return result;
        }
    }        
}
