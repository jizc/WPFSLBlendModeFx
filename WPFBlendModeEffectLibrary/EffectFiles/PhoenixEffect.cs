namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class PhoenixEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static PhoenixEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/PhoenixEffect.ps");
        }

        public PhoenixEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
