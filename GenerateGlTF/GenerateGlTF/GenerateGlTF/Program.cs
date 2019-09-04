using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using System;
using System.IO;
using System.Numerics;

namespace GenerateGlTF
{
    using VPOSNRM = VertexBuilder<VertexPositionNormal, VertexEmpty, VertexEmpty>;

    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Les create some glTF with spheres to test.");
            // read points.geojson
            var json = File.ReadAllText("Points.geojson");
            var fc = JsonConvert.DeserializeObject<FeatureCollection>(json);
            Console.WriteLine("Features: " + fc.Features.Count);

            foreach(var f in fc.Features)
            {
                var z = f.Properties["z"];
                var timestamp = f.Properties["timestamp"];
                var amount = f.Properties["amount"];

                var point = (Point)f.Geometry;
                var coords = point.Coordinates;
                Console.WriteLine(timestamp + ": " + coords.Longitude + ", " + coords.Latitude + ", " + z + ", " + amount);

                // todo: draw sphere per feature
            }

            var material = MaterialBuilder.CreateDefault();
            var mesh = VPOSNRM.CreateCompatibleMesh("shape");
            mesh.AddSphere(material, 0.5f, Matrix4x4.Identity);
            var scene = new SceneBuilder();
            scene.AddMesh(mesh, Matrix4x4.Identity);
            scene.ToSchema2().SaveGLB("sphere.glb");
        }
    }
}
