﻿using SharpGLTF.Materials;
using System.Numerics;

namespace GenerateGlTF.Core
{
    public class MaterialCreator
    {
        public static MaterialBuilder CreateMaterial(float r, float g, float b)
        {
            var material = new MaterialBuilder().
                WithDoubleSide(true).
                WithMetallicRoughnessShader().
                WithChannelParam(KnownChannels.BaseColor, ColorToVector4(r, g, b));
            return material;
        }

        private static Vector4 ColorToVector4(float r, float g, float b)
        {
            return new Vector4(r / 255, g / 255, b / 255, 1);
        }
    }
}
