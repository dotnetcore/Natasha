namespace Natasha.Caller
{
    public static class EntityExtension
    {
        public static DynamicBase<T> Caller<T>(this T value)
        {
            var caller = (DynamicBase<T>)EntityOperator<T>.Create();
            caller.SetInstance(value);
            return caller;
        }
    }
}
