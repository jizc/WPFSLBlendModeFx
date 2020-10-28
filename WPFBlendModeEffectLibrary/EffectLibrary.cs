namespace BlendModeEffectLibrary
{
    using System;
    using System.Text;

    internal static class Global
    {
        private static string s_assemblyShortName;

        private static string AssemblyShortName
        {
            get
            {
                if (s_assemblyShortName is null)
                {
                    var a = typeof(Global).Assembly;

                    // Pull out the short name.
                    s_assemblyShortName = a.ToString().Split(',')[0];
                }

                return s_assemblyShortName;
            }
        }

        public static Uri MakePackUri(string relativeFile)
        {
            var uriString = new StringBuilder();
#if !SILVERLIGHT
            uriString.Append("pack://application:,,,");
#endif
            uriString.Append("/" + AssemblyShortName + ";component/" + relativeFile);
            return new Uri(uriString.ToString(), UriKind.RelativeOrAbsolute);
        }
    }
}
