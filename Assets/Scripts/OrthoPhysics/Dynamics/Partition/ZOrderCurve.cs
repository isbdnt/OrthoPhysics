
namespace OrthoPhysics
{
    public static class ZOrderCurve
    {
        public static ulong Encode(int x, int y)
        {
            return Interleave(MapIntToULong(x), MapIntToULong(y));
        }

        static ulong MapIntToULong(int v)
        {
            if (v >= 0)
            {
                return (uint)v + 0x80000000;
            }
            else
            {
                return (uint)v & 0x7FFFFFFF;
            }
        }

        static ulong Interleave(ulong x, ulong y)
        {
            // https://en.wikipedia.org/wiki/Z-order_curve
            // http://graphics.stanford.edu/~seander/bithacks.html#InterleaveBMN

            // See this post for interleaving 32-bit numbers into 64-bit result, and interesting comparisons with SIMD versions
            // https://lemire.me/blog/2018/01/08/how-fast-can-you-bit-interleave-32-bit-integers/

            x = (x | (x << 16)) & 0x0000FFFF0000FFFF;
            x = (x | (x << 8)) & 0x00FF00FF00FF00FF;
            x = (x | (x << 4)) & 0x0F0F0F0F0F0F0F0F;
            x = (x | (x << 2)) & 0x3333333333333333;
            x = (x | (x << 1)) & 0x5555555555555555;

            y = (y | (y << 16)) & 0x0000FFFF0000FFFF;
            y = (y | (y << 8)) & 0x00FF00FF00FF00FF;
            y = (y | (y << 4)) & 0x0F0F0F0F0F0F0F0F;
            y = (y | (y << 2)) & 0x3333333333333333;
            y = (y | (y << 1)) & 0x5555555555555555;

            return x | (y << 1);
        }
    }
}
