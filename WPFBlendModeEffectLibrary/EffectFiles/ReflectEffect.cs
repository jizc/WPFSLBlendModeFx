namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class ReflectEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static ReflectEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/ReflectEffect.ps");
        }

        public ReflectEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
