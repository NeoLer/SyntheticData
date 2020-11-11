using System;
using static SyntheticData.Interpolable;

namespace SyntheticData
{
	public class Angle
	{
		public static double LerpDegrees(double a, double b, double t) { 
			static double rad(double x) => x / (180 / Math.PI);
			static double deg(double x) => x * 180 / Math.PI;
			static double pos(double x) => 0 > x ? x + 360 : x;

			double a1 = rad(a), a2 = rad(b);
			double x1 = Math.Cos(a1), y1 = Math.Sin(a1);
			double x2 = Math.Cos(a2), y2 = Math.Sin(a2);
			double x3 = LerpDouble(x1, x2, t), y3 = LerpDouble(y1, y2, t);
			double a3 = Math.Atan2(y3, x3);

			return pos(deg(a3));
		}
	}
}
