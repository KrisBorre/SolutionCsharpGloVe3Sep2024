using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace ConsoleCodeAnalysis25Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static async Task Main()
        {
            // Define the package to install
            var packageId = "Newtonsoft.Json";
            var version = "13.0.1";

            // Get the global NuGet packages folder
            var settings = Settings.LoadDefaultSettings(null);
            var packagePathResolver = new NuGet.Packaging.PackagePathResolver(SettingsUtility.GetGlobalPackagesFolder(settings));

            // Resolve the package
            var sourceRepositoryProvider = new SourceRepositoryProvider(Settings.LoadDefaultSettings(null), Repository.Provider.GetCoreV3());
            var repository = sourceRepositoryProvider.CreateRepository(new PackageSource("https://api.nuget.org/v3/index.json"));
            var packageMetadataResource = await repository.GetResourceAsync<FindPackageByIdResource>();

            // Download the package
            var packageIdentity = new PackageIdentity(packageId, NuGetVersion.Parse(version));
            var downloadPath = Path.Combine(SettingsUtility.GetGlobalPackagesFolder(settings), packageId, version);
            var nupkgFile = Path.Combine(downloadPath, $"{packageId}.{version}.nupkg");

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
                using var downloadStream = File.Create(nupkgFile);

                await packageMetadataResource.CopyNupkgToStreamAsync(
                    packageId,
                    NuGetVersion.Parse(version),
                    downloadStream,
                    cacheContext: null,
                    logger: null,
                    cancellationToken: CancellationToken.None 
                );
            }

            // Reference the package DLL
            var dllPath = Path.Combine(downloadPath, "lib/netstandard2.0/Newtonsoft.Json.dll"); // Adjust based on package contents
            if (!File.Exists(dllPath))
            {
                Console.WriteLine("Failed to locate the DLL in the NuGet package.");
                return;
            }

            // Use the DLL in a Roslyn Compilation
            var syntaxTree = CSharpSyntaxTree.ParseText("public class Test { }");
            var compilation = CSharpCompilation.Create("TestAssembly")
                .AddReferences(MetadataReference.CreateFromFile(dllPath))
                .AddSyntaxTrees(syntaxTree);

            Console.WriteLine("Compilation created with NuGet package reference.");

            /*
            Compilation created with NuGet package reference.
            */


            Console.ReadLine();
        }
    }
}
