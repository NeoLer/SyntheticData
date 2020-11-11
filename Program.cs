using System;
using System.Collections.Generic;
using System.Globalization;
using static SyntheticData.Weather;

namespace SyntheticData
{
    class Program
    {
        static void ShowTypes()
        {
            Wind w = new Wind(23, 89);
            Precipitation r = new Precipitation(0.9);
            WData wd = new WData { Precipitation = r, Wind = w };

            Console.WriteLine("Rain: " + r);
            Console.WriteLine("Wind: " + w);
            Console.WriteLine("Weather summary: " + wd);
        }
        static void GenEx()
        {
            Region somePlace = new Region {
                WinterTemperature = new Temperature(-10),
                SummerTemperature = new Temperature(30),
                DailyTemperatureVariation = new Temperature(10),
                DaysOfRainPerYear = 160,
                mmOfRainPerYear = 600,
                MaxWindSpeed = 5,
            };

            Generator ex = new Generator(
                new DateTime(2020, 1, 1, 0, 0, 0, 0), somePlace);
            
            List<WData> ls = ex.GetData(7);

            Console.WriteLine($"Weather data({ls.Count}):\n" + string.Join("\n", ls));
        }
        static void Main(string[] args)
        {
            GenEx();
        }
    }
}
