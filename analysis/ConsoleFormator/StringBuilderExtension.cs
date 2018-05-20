using System.Text;

namespace ConsoleFormator
{
    public static class StringBuilderExtension
    {
        public static void RemoveLastest(this StringBuilder builder, int length)
        {
            builder.Remove(builder.Length - length, length);
        }
        public static int OutputLength(this StringBuilder builder)
        {
            int length = 0;
            for (int i = 0; i < builder.Length; i += 1)
            {
                if (builder[i] >= 0 && builder[i] <= 127)
                {
                    length += 1;
                }
            }
            return builder.Length * 2 - length;
        }

        public static int OutputLength(this string instance)
        {
            int length = 0;
            for (int i = 0; i < instance.Length; i += 1)
            {
                if (instance[i] >= 0 && instance[i] <= 127)
                {
                    length += 1;
                }
            }
            return instance.Length * 2 - length;
        }
    }
}
