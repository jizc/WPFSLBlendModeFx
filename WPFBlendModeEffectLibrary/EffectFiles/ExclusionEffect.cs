namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class ExclusionEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static ExclusionEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/ExclusionEffect.ps");
        }

        public ExclusionEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
