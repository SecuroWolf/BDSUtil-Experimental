using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BDSUtil
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbeddedDependencies;
            RunApplicationPipeline();
        }

        private static void RunApplicationPipeline()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppSettings.LoadSettingsAsync().GetAwaiter().GetResult();
            Application.Run(new MainWindow());
        }


        private static Assembly ResolveEmbeddedDependencies(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;

            if (assemblyName.Equals("Open.Nat", StringComparison.OrdinalIgnoreCase))
            {
                // NOTE: Replace "BDSUtil" with your exact root namespace if it differs
                string resourcePath = "BDSUtil.Resources.Dependencies.Open.Nat.dll";

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    if (stream == null) return null;

                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            }

            if (assemblyName.Equals("ReaLTaiizor", StringComparison.OrdinalIgnoreCase))
            {
                // NOTE: Replace "BDSUtil" with your exact root namespace if it differs
                string resourcePath = "BDSUtil.Resources.Dependencies.ReaLTaiizor.dll";

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    if (stream == null) return null;

                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            }

            return null;
        }
    }
}
