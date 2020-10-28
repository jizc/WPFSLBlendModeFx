﻿namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class MultiplyEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static MultiplyEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/MultiplyEffect.ps");
        }

        public MultiplyEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
