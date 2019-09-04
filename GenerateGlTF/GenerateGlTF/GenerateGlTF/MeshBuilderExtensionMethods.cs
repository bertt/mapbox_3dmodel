using SharpGLTF.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GenerateGlTF
{
    public static class MeshBuilderExtensionMethods
    {
        public static void AddSphere<TMaterial>(this IMeshBuilder<TMaterial> meshBuilder, TMaterial material, Single radius, Matrix4x4 xform)
        {
            var sphere = new IcoSphere<TMaterial>(material, radius);

            sphere.AddTo(meshBuilder, xform);
        }

        public static void AddCube<TMaterial>(this IMeshBuilder<TMaterial> meshBuilder, TMaterial material, Matrix4x4 xform)
        {
            var cube = new Cube<TMaterial>(material);

            cube.AddTo(meshBuilder, xform);
        }


    }
}
