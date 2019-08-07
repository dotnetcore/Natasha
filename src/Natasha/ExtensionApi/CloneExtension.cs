using Natasha.Operator;

namespace Natasha.CloneExtension
{
    public static class CloneExtension
    {
        public static T Clone<T>(this T instance)
        {
            return CloneOperator.Clone(instance);
        }
    }
}
