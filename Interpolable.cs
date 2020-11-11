namespace SyntheticData
{
    public class Interpolable
    {
        public static double LerpDouble(double a, double b, double t)
            => a + (b - a) * t;
        public interface IInterpolable<T>
        {
            T Lerp(T b, double t);
        }
    }
}
