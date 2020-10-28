namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class LightenEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static LightenEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/LightenEffect.ps");
        }

        public LightenEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
