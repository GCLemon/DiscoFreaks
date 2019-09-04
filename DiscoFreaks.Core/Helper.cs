namespace DiscoFreaks.Core
{
    /// <summary>
    /// 列挙型関係の操作
    /// </summary>
    public static class Enum
    {
        /// <summary>
        /// 文字列から列挙型に変換する
        /// </summary>
        public static T Parse<T> (string value) =>
            (T)System.Enum.Parse(typeof(T), value);

        /// <summary>
        /// 列挙型に含まれる値を全て列挙する
        /// </summary>
        public static System.Array GetValues<T>() =>
            System.Enum.GetValues(typeof(T));
    }

    /// <summary>
    /// 計算関係の操作
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// 最小非負剰余を求める
        /// </summary>
        public static dynamic Mod(dynamic x, dynamic y)
        {
            return ((x % y) + y) % y;
        }

        /// <summary>
        /// 累乗を求める
        /// </summary>
        public static double Pow(double x, double y)
        {
            return System.Math.Pow(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        public static dynamic Clamp(dynamic x, dynamic min, dynamic max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
    }
}
