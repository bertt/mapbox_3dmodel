using SharpGLTF.Geometry;
using System.Numerics;

namespace GenerateGlTF.Core
{
    public static class MeshBuilderExtensionMethods
    {
        public static void AddCube<TMaterial>(this IMeshBuilder<TMaterial> meshBuilder, TMaterial material, Matrix4x4 xform)
        {
            var cube = new Cube<TMaterial>(material);

            cube.AddTo(meshBuilder, xform);
        }
    }
}
