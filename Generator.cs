using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using static SyntheticData.Weather;

/* Temperature model:
 * seasonal only: https://www.desmos.com/calculator/8wibm10wpq
 * seasonal+daily: https://www.desmos.com/calculator/l9zj9icy2f
*/

namespace SyntheticData
{
    public class Region
    {
        public Temperature WinterTemperature;
        public Temperature SummerTemperature;
        public Temperature DailyTemperatureVariation;
        public int DaysOfRainPerYear;
        public double mmOfRainPerYear;
        public double MaxWindSpeed;
    }
    public class Generator
    {
        private Random RNG { get; set; }
        public DateTime Date { get; set; }
        public Region Region { get; set; }
        public Temperature CurrentTemperature()
        {
            double r1 = Region.WinterTemperature.C;
            double r2 = Region.SummerTemperature.C;

            double v = Region.DailyTemperatureVariation.C;
            double v1 = r1 + v / 2;
            double v2 = r2 - v / 2;

            int day = Date.DayOfYear;
            int hour = Date.Hour;

            double temp_c = v1 + (v2 - v1) / 2
                + (v2 - v1) / 2 * Math.Sin((double)day / 365 * 2 * Math.PI)
                + v / 2 * -Math.Cos((double)hour / 24 * 2 * Math.PI);

            return new Temperature(temp_c);
        }
        public List<WData> GetData(int days)
        {
            var ls = new List<WData>();

            int data_pts = 0;
            List<Precipitation> rain_pts = new List<Precipitation>();
            List<Wind> wind_pts = new List<Wind>();

            double rain_chance = (double)Region.DaysOfRainPerYear/365;
            double rain_avg = Region.mmOfRainPerYear/365;

            void add_data_pt()
            {
                wind_pts.Add(new Wind(RNG.NextDouble() * Region.MaxWindSpeed, RNG.Next(0, 360)));
                rain_pts.Add(new Precipitation(RNG.NextDouble() < rain_chance ? RNG.NextDouble() * rain_avg * 2 : 0));
                data_pts++;
            }

            add_data_pt();

            for (int day = 0; day < days; day++)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    if (hour % 6 == 0) add_data_pt();
                    double t = (double)(hour % 6) / 5;

                    Wind wind = wind_pts[data_pts - 2].Lerp(wind_pts[data_pts - 1], t);
                    Precipitation rain = rain_pts[data_pts - 2].Lerp(rain_pts[data_pts - 1], t);

                    ls.Add(new WData
                    {
                        Date = Date,
                        Temperature = CurrentTemperature(),
                        Precipitation = rain,
                        Wind = wind,
                    });

                    Date = Date.AddHours(1);
                }
            }

            return ls;
        }

        public Generator(
            DateTime startDate,
            Region region)
        {
            RNG = new Random();
            Date = startDate;
            Region = region;
        }
    }
}