namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class NormalEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static NormalEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/NormalEffect.ps");
        }

        public NormalEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
