using GeoCoordinatePortable;
using GeoJSON.Net.Feature;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Scenes;
using System;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace GenerateGlTF.Core
{
    using VPOSNRM = VertexBuilder<VertexPositionNormal, VertexEmpty, VertexEmpty>;

    public static class FeatureCollectionExtensions
    {

        public static byte[] ToGlb(this FeatureCollection fc, string classifyField, double longitude, double latitude, Color fromColor, Color toColor, int numberOfClasses, int Maximum)
        {
            var colors = ColorRange.GetGradients(fromColor, toColor, numberOfClasses).ToList();

            var center_coordinate = new GeoCoordinate(latitude, longitude); ;

            var mesh = VPOSNRM.CreateCompatibleMesh("shape");

            foreach (var f in fc.Features)
            {
                var z = Convert.ToDouble(f.Properties["z"]);
                var amount = Convert.ToInt32(f.Properties[classifyField]);

                var point = (GeoJSON.Net.Geometry.Point)f.Geometry;
                var coords = point.Coordinates;

                var distance_latitude = new GeoCoordinate(coords.Latitude, longitude).GetDistanceTo(center_coordinate);
                distance_latitude = coords.Latitude < center_coordinate.Latitude ? distance_latitude * -1 : distance_latitude;
                var distance_longitude = new GeoCoordinate(latitude, coords.Longitude).GetDistanceTo(center_coordinate);
                distance_longitude = coords.Longitude < center_coordinate.Longitude ? distance_longitude * -1 : distance_longitude;

                var color_index = (int)Math.Round((double)(amount * colors.Count / Maximum));
                var rgb = colors[color_index];
                var material = MaterialCreator.CreateMaterial(rgb.R, rgb.G, rgb.B);
                var translate = Matrix4x4.CreateTranslation((float)distance_longitude, (float)z * 2, (float)distance_latitude);
                mesh.AddCube(material, translate);

            }
            var scene = new SceneBuilder();
            scene.AddMesh(mesh, Matrix4x4.Identity);
            var glb = scene.ToSchema2().WriteGLB();
            var bytes = glb.ToArray();
            return bytes;
        }

    }
}
