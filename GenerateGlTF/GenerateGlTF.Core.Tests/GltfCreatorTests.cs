using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Drawing;
using System.IO;

namespace GenerateGlTF.Core.Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var json = File.ReadAllText("Points.geojson");
            var fc = JsonConvert.DeserializeObject<FeatureCollection>(json);
            var glbBytes = fc.ToGlb("amount", 4.941729, 52.314286, Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 0), 10, 40);
            Assert.IsTrue(glbBytes.Length == 705040);
        }
    }
}