using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using GenerateGlTF.Core;

namespace GenerateGlTF.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var json = File.ReadAllText("Points.geojson");
            var fc = JsonConvert.DeserializeObject<FeatureCollection>(json);
            Console.WriteLine("Features: " + fc.Features.Count);
            var glbBytes = fc.ToGlb("amount", 4.941729, 52.314286, Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 0), 10, 40);
            File.WriteAllBytes("test.glb", glbBytes);
            stopwatch.Stop();
            Console.WriteLine("Model creation time: " + stopwatch.ElapsedMilliseconds);
        }

    }
}
