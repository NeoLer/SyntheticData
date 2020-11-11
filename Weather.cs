using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using static SyntheticData.Interpolable;
using static SyntheticData.Angle;

/* Reference doc: https://openweathermap.org/current */
// Important: values in implemented types currently only metric system

namespace SyntheticData
{
	public class Weather {
		public struct Temperature : IInterpolable<Temperature>
		{
			public double C { get; set; }
			public Temperature(double c) => C = c;
			public Temperature Lerp(Temperature b, double t)
				=> new Temperature(LerpDouble(C, b.C, t));
			public override string ToString() => $"{C:G2}°C";
		}
		public struct Wind : IInterpolable<Wind>
		{
			public double Speed { get; set; }
			public double Deg { get; set; }
			public Wind(double speed, double deg)
			{
				Speed = speed;
				Deg = deg;
			}
			public Wind Lerp(Wind b, double t)
            {
				double speed = LerpDouble(Speed, b.Speed, t);

				return new Wind(speed, LerpDegrees(Deg, b.Deg, t));
            }
			public override string ToString() => $"({Speed:G2}m/s, {Deg:G3}°)";
		}
        public enum PrecipitationType
        {
			Rain,
			Snow,
			Hail
        }
		public struct Precipitation
		{
			
			public PrecipitationType Type { get; set; }
			public double Amt { get; set; }
			public Precipitation(double amt)
			{
				Amt = amt;
				Type = PrecipitationType.Rain;
			}
			public Precipitation(double amt, PrecipitationType type)
            {
				Amt = amt;
				Type = type;
            }
			public Precipitation Lerp(Precipitation b, double t)
				=> new Precipitation(LerpDouble(Amt, b.Amt, t));
			public override string ToString() => $"{Type} {Amt:G1}mm";
		}
		public struct WData
		{
			public DateTime Date { get; set; }
			public Temperature Temperature { get; set; }
			public Temperature Feelslike { get; set; }
			public Precipitation Precipitation { get; set; }
			public Wind Wind { get; set; }
			public override string ToString() => $"{Date}\t{Temperature}\t{Precipitation}\t{Wind}";
		}
	}
}