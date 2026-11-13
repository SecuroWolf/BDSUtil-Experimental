using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows.Forms;
using BDSUtil.UI;

namespace BDSUtil
{
    static class Program
    {
        private static readonly Dictionary<string, Assembly> _loadedEmbedded =
            new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _resolveLock = new object();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AssemblyLoadContext.Default.Resolving += ResolveEmbeddedContextDependency;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbeddedDependencies;
            RunApplicationPipeline();
        }

        private static void RunApplicationPipeline()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Material3AppTheme.EnsureInitialized();
            AppSettings.LoadSettings();
            Application.Run(new MainWindow());
        }

        private static Assembly ResolveEmbeddedContextDependency(AssemblyLoadContext context, AssemblyName name)
        {
            return LoadEmbeddedAssembly(name?.Name, context);
        }

        private static Assembly ResolveEmbeddedDependencies(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;
            return LoadEmbeddedAssembly(assemblyName, AssemblyLoadContext.Default);
        }

        private static Assembly LoadEmbeddedAssembly(string assemblyName, AssemblyLoadContext targetContext)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                return null;
            }

            string resourcePath;
            if (assemblyName.Equals("Material3.Core", StringComparison.OrdinalIgnoreCase))
            {
                resourcePath = "BDSUtil.Resources.Dependencies.Material3.Core.dll";
            }
            else if (assemblyName.Equals("Material3.WinForms", StringComparison.OrdinalIgnoreCase))
            {
                resourcePath = "BDSUtil.Resources.Dependencies.Material3.WinForms.dll";
            }
            else
            {
                return null;
            }

            lock (_resolveLock)
            {
                if (_loadedEmbedded.TryGetValue(assemblyName, out Assembly cached))
                {
                    return cached;
                }

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    if (stream == null)
                    {
                        return null;
                    }

                    Assembly loaded = (targetContext ?? AssemblyLoadContext.Default).LoadFromStream(stream);
                    _loadedEmbedded[assemblyName] = loaded;
                    return loaded;
                }
            }
        }
    }
}
