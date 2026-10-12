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
            // STEP 1: Immediately bind the resolver hook at the absolute physical entry boundary
            AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbeddedMaterialSkin;

            // STEP 2: Pass execution into a completely isolated method frame.
            // This prevents .NET from checking form dependencies before the hook is registered.
            RunApplicationPipeline();
        }

        /// <summary>
        /// Isolated execution frame to protect dependency loading sequences safely.
        /// </summary>
        private static void RunApplicationPipeline()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Force local config settings to load into memory synchronously
            AppSettings.LoadSettingsAsync().GetAwaiter().GetResult();

            // Run the main framework loop smoothly from memory
            Application.Run(new MainWindow());
        }

        /// <summary>
        /// Automatically intercepts the runtime binding routine and feeds it the embedded DLL byte array.
        /// </summary>
        private static Assembly ResolveEmbeddedMaterialSkin(object sender, ResolveEventArgs args)
        {
            string requestedAssemblyName = new AssemblyName(args.Name).Name;

            if (requestedAssemblyName.Equals("MaterialSkin", StringComparison.OrdinalIgnoreCase))
            {
                // Safely extract the raw embedded binary data out of your resource manager as a true byte array
                byte[] assemblyRawBytes = (byte[])Properties.Resources.ResourceManager.GetObject("MaterialSkin");

                if (assemblyRawBytes != null)
                {
                    // Load and pass the binary structure straight into active application execution memory maps
                    return Assembly.Load(assemblyRawBytes);
                }
            }

            return null;
        }
    }
}
