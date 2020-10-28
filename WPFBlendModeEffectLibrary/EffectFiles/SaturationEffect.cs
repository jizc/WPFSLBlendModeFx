namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class SaturationEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static SaturationEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/SaturationEffect.ps");
        }

        public SaturationEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
