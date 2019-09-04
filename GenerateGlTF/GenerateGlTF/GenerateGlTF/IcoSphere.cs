using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using System;
using System.Numerics;

namespace GenerateGlTF
{
    using VERTEX = VertexBuilder<VertexPositionNormal, VertexColor1Texture1, VertexEmpty>;

    interface IParametricShape<TMaterial>
    {
        void AddTo(IMeshBuilder<TMaterial> meshBuilder, Matrix4x4 xform);
    }


    class IcoSphere<TMaterial> : IParametricShape<TMaterial>
    {
        #region lifecycle

        public IcoSphere(TMaterial material, float radius = 0.5f)
        {
            _Material = material;
            _Radius = radius;
        }

        #endregion

        #region data

        private float _Radius = 0.5f;
        private TMaterial _Material;
        private int _Subdivision = 3;

        #endregion

        #region API        

        public void AddTo(IMeshBuilder<TMaterial> meshBuilder, Matrix4x4 xform)
        {
            // http://blog.andreaskahler.com/2009/06/creating-icosphere-mesh-in-code.html

            var t = 1 + (float)(Math.Sqrt(5.0) / 2);

            var v0 = new Vector3(-1, t, 0) * _Radius;
            var v1 = new Vector3(1, t, 0) * _Radius;
            var v2 = new Vector3(-1, -t, 0) * _Radius;
            var v3 = new Vector3(1, -t, 0) * _Radius;

            var v4 = new Vector3(0, -1, t) * _Radius;
            var v5 = new Vector3(0, 1, t) * _Radius;
            var v6 = new Vector3(0, -1, -t) * _Radius;
            var v7 = new Vector3(0, 1, -t) * _Radius;

            var v8 = new Vector3(t, 0, -1) * _Radius;
            var v9 = new Vector3(t, 0, 1) * _Radius;
            var v10 = new Vector3(-t, 0, -1) * _Radius;
            var v11 = new Vector3(-t, 0, 1) * _Radius;

            var prim = meshBuilder.UsePrimitive(_Material);

            // 5 faces around point 0
            _AddSphereFace(prim, xform, v0, v11, v5, _Subdivision);
            _AddSphereFace(prim, xform, v0, v5, v1, _Subdivision);
            _AddSphereFace(prim, xform, v0, v1, v7, _Subdivision);
            _AddSphereFace(prim, xform, v0, v7, v10, _Subdivision);
            _AddSphereFace(prim, xform, v0, v10, v11, _Subdivision);

            // 5 adjacent faces
            _AddSphereFace(prim, xform, v1, v5, v9, _Subdivision);
            _AddSphereFace(prim, xform, v5, v11, v4, _Subdivision);
            _AddSphereFace(prim, xform, v11, v10, v2, _Subdivision);
            _AddSphereFace(prim, xform, v10, v7, v6, _Subdivision);
            _AddSphereFace(prim, xform, v7, v1, v8, _Subdivision);

            // 5 faces around point 3
            _AddSphereFace(prim, xform, v3, v9, v4, _Subdivision);
            _AddSphereFace(prim, xform, v3, v4, v2, _Subdivision);
            _AddSphereFace(prim, xform, v3, v2, v6, _Subdivision);
            _AddSphereFace(prim, xform, v3, v6, v8, _Subdivision);
            _AddSphereFace(prim, xform, v3, v8, v9, _Subdivision);

            // 5 adjacent faces
            _AddSphereFace(prim, xform, v4, v9, v5, _Subdivision);
            _AddSphereFace(prim, xform, v2, v4, v11, _Subdivision);
            _AddSphereFace(prim, xform, v6, v2, v10, _Subdivision);
            _AddSphereFace(prim, xform, v8, v6, v7, _Subdivision);
            _AddSphereFace(prim, xform, v9, v8, v1, _Subdivision);
        }

        private static void _AddSphereFace(IPrimitiveBuilder primitiveBuilder, Matrix4x4 xform, Vector3 a, Vector3 b, Vector3 c, int iterations = 0)
        {
            if (iterations <= 0)
            {
                var tt = (a + b + c) / 3.0f;

                var aa = _CreateVertex(a, xform);
                var bb = _CreateVertex(b, xform);
                var cc = _CreateVertex(c, xform);
                primitiveBuilder.AddTriangle(aa, bb, cc);
                return;
            }

            --iterations;

            var ab = Vector3.Normalize(a + b) * a.Length();
            var bc = Vector3.Normalize(b + c) * b.Length();
            var ca = Vector3.Normalize(c + a) * c.Length();

            // central triangle
            _AddSphereFace(primitiveBuilder, xform, ab, bc, ca, iterations);

            // vertex triangles
            _AddSphereFace(primitiveBuilder, xform, a, ab, ca, iterations);
            _AddSphereFace(primitiveBuilder, xform, b, bc, ab, iterations);
            _AddSphereFace(primitiveBuilder, xform, c, ca, bc, iterations);
        }

        private static VERTEX _CreateVertex(Vector3 position, Matrix4x4 xform)
        {
            var v = new VERTEX();

            v.Geometry.Position = Vector3.Transform(position, xform);
            v.Geometry.Normal = Vector3.Normalize(Vector3.TransformNormal(position, xform));
            v.Material.Color = Vector4.One;
            v.Material.TexCoord = Vector2.Zero;

            return v;
        }

        #endregion
    }
}
