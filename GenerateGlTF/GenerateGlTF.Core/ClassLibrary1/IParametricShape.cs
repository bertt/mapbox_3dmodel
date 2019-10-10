
using SharpGLTF.Geometry;
using System.Numerics;

namespace GenerateGlTF.Core
{
    interface IParametricShape<TMaterial>
    {
        void AddTo(IMeshBuilder<TMaterial> meshBuilder, Matrix4x4 xform);
    }
}
