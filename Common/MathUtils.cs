using System.Numerics;

namespace Common;

public static class MathUtils
{
    // There are two ways to calculate modulo of negative numbers
    public static int Mod(int first, int second)
    {
        var c = first % second;
        return (c < 0) ? c + second : c;
    }

    public static long ModV1(long first, long second)
    {
        return ((first % second) + second) % second;
    }

    public static long ModV2(long first, long second)
    {
        var c = first % second;
        return (c < 0) ? c + second : c;
    }

    public static T GreatestCommonDivisor<T>(T first, T second) where T : INumber<T>
    {
        while (second != T.Zero)
        {
            var temp = second;
            second = first % second;
            first = temp;
        }

        return first;
    }

    public static T LeastCommonMultiple<T>(T first, T second) where T : INumber<T>
        => first / GreatestCommonDivisor(first, second) * second;

    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);
}
