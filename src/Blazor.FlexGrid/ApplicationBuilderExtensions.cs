using Microsoft.AspNetCore.Builder;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Blazor.FlexGrid
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFlexGrid(this IApplicationBuilder applicationBuilder, string webRootPath)
        {
            var flexGridAssembly = Assembly.GetExecutingAssembly();
            var resources = flexGridAssembly.GetManifestResourceNames();

            var destinationFolderPath = Path.Combine(webRootPath, "Blazor.FlexGrid");
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            var fontAwesomeFolderPath = Path.Combine(destinationFolderPath, "fontawesome");
            if (!Directory.Exists(fontAwesomeFolderPath))
            {
                Directory.CreateDirectory(fontAwesomeFolderPath);
            }

            var fontAwesomeCssFolderPath = Path.Combine(fontAwesomeFolderPath, "css");
            if (!Directory.Exists(fontAwesomeCssFolderPath))
            {
                Directory.CreateDirectory(fontAwesomeCssFolderPath);
            }

            var fontAwesomeFontsFolderPath = Path.Combine(fontAwesomeFolderPath, "webfonts");
            if (!Directory.Exists(fontAwesomeFontsFolderPath))
            {
                Directory.CreateDirectory(fontAwesomeFontsFolderPath);
            }

            foreach (var resource in resources.Where(r => !r.Contains("awesome")))
            {
                var colonPosition = resource.LastIndexOf(":");
                var resourceName = resource.Substring(colonPosition + 1, resource.Length - colonPosition - 1);
                AddResource(flexGridAssembly, destinationFolderPath, resource, resourceName);
            }

            foreach (var resource in resources.Where(r => r.Contains("awesome") && r.Contains("css")))
            {
                var backSlashPosition = resource.LastIndexOf("\\");
                var resourceName = resource.Substring(backSlashPosition + 1, resource.Length - backSlashPosition - 1);
                AddResource(flexGridAssembly, fontAwesomeCssFolderPath, resource, resourceName);
            }

            foreach (var resource in resources.Where(r => r.Contains("awesome") && r.Contains("webfonts")))
            {
                var backSlashPosition = resource.LastIndexOf("\\");
                var resourceName = resource.Substring(backSlashPosition + 1, resource.Length - backSlashPosition - 1);
                AddResource(flexGridAssembly, fontAwesomeFontsFolderPath, resource, resourceName);
            }

            return applicationBuilder;
        }

        private static void AddResource(Assembly flexGridAssembly, string destinationFolderPath, string resource, string resourceName)
        {
            using (var resourceStream = flexGridAssembly.GetManifestResourceStream(resource))
            {
                if (resourceStream == null)
                {
                    throw new Exception($"Couldn't find the resource '{resource}' in the assembly");
                }

                var bufferSize = 1024 * 1024;
                using (var fileStream = new FileStream(Path.Combine(destinationFolderPath, resourceName), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.SetLength(resourceStream.Length);
                    var bytesRead = -1;
                    var bytes = new byte[bufferSize];

                    while ((bytesRead = resourceStream.Read(bytes, 0, bufferSize)) > 0)
                    {
                        fileStream.Write(bytes, 0, bytesRead);
                    }
                }
            }
        }
    }
}
