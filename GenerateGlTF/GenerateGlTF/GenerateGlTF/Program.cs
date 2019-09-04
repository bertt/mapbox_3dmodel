﻿using GeoCoordinatePortable;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;

namespace GenerateGlTF
{
    using VPOSNRM = VertexBuilder<VertexPositionNormal, VertexEmpty, VertexEmpty>;

    class Program
    {

        static void Main(string[] args)
        {
            var c1 = Color.FromArgb(0, 0, 255);
            var c2 = Color.FromArgb(255, 0, 0);
            var colors = ColorRange.GetGradients(c1, c2, 10).ToList();
            var max_amount = 50;
            // var step = max_amount / colors.Count;

            var center_longitude = 4.941729;
            var center_latitude = 52.314286;

            var center_coordinate = new GeoCoordinate(center_latitude, center_longitude);;

            Console.WriteLine("Les create some glTF with spheres to test.");
            // read points.geojson
            var json = File.ReadAllText("Points.geojson");
            var fc = JsonConvert.DeserializeObject<FeatureCollection>(json);
            Console.WriteLine("Features: " + fc.Features.Count);

            var materialCache = new MaterialsCache();

            var default_hex_color = "#bb3333";
            var material = materialCache.GetMaterialBuilderByColor(default_hex_color);

            var mesh = VPOSNRM.CreateCompatibleMesh("shape");
            //var mesh = new MeshBuilder<VertexPosition, VertexTexture1>("terrain");


            foreach (var f in fc.Features)
            {
                // Console.Write(".");
                var z = Convert.ToDouble(f.Properties["z"]);
                var timestamp = f.Properties["timestamp"];
                var amount = Convert.ToInt32(f.Properties["amount"]);

                var point = (GeoJSON.Net.Geometry.Point)f.Geometry;
                var coords = point.Coordinates;
                // Console.WriteLine(timestamp + ": " + coords.Longitude + ", " + coords.Latitude + ", " + z + ", " + amount);

                var distance_latitude = new GeoCoordinate(coords.Latitude, center_longitude).GetDistanceTo(center_coordinate);
                distance_latitude = coords.Latitude < center_coordinate.Latitude ? distance_latitude * -1 : distance_latitude;
                var distance_longitude = new GeoCoordinate(center_latitude, coords.Longitude).GetDistanceTo(center_coordinate);
                distance_longitude = coords.Longitude < center_coordinate.Longitude ? distance_longitude * -1 : distance_longitude;

                var color_index = (int)Math.Round((double)(amount * colors.Count / max_amount ));
                var rgb = colors[color_index];
                material = MaterialCreator.CreateMaterial(rgb.R, rgb.G, rgb.B);
                Console.WriteLine(color_index + " , " + amount);

                // Console.WriteLine(distance_longitude + ", " + distance_latitude);

                var translate = Matrix4x4.CreateTranslation((float)distance_longitude, (float)z * -2, (float)distance_latitude);
                mesh.AddCube(material, translate);
                //mesh.AddSphere(material, 0.5f, translate);
                //mesh.UsePrimitive(material).AddQuadrangle(v1, v2, v3, v4);


            }
            var scene = new SceneBuilder();
            scene.AddMesh(mesh, Matrix4x4.Identity);
            scene.ToSchema2().SaveGLB("sphere.glb");


        }
    }
}
