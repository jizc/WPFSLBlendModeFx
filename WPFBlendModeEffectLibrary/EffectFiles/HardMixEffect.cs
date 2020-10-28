namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class HardMixEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static HardMixEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/HardMixEffect.ps");
        }

        public HardMixEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
