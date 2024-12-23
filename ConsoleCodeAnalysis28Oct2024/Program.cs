using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace ConsoleCodeAnalysis28Oct2024
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
            var dllPath = Path.Combine(downloadPath, "lib/netstandard2.0/Newtonsoft.Json.dll");

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
                using var downloadStream = File.Create(Path.Combine(downloadPath, $"{packageId}.{version}.nupkg"));
                await packageMetadataResource.CopyNupkgToStreamAsync(
                    packageId,
                    NuGetVersion.Parse(version),
                    downloadStream,
                    cacheContext: null,
                    logger: null,
                    cancellationToken: CancellationToken.None
                );
            }

            if (!File.Exists(dllPath))
            {
                Console.WriteLine("Failed to locate the DLL in the NuGet package.");
                return;
            }

            // Use the DLL in a Roslyn Compilation
            var syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;
                using Newtonsoft.Json;

                public class Test
                {
                    public static void Main()
                    {
                        var data = new { Name = ""John Doe"", Age = 30 };
                        string json = JsonConvert.SerializeObject(data);
                        Console.WriteLine(""Serialized JSON: "" + json);
                    }
                }");

            var compilation = CSharpCompilation.Create("TestAssembly")
                .AddReferences(MetadataReference.CreateFromFile(dllPath))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)) // Core assembly
                .AddSyntaxTrees(syntaxTree);

            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                Console.WriteLine("Compilation failed:");
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    Console.WriteLine(diagnostic.ToString());
                }
                return;
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = System.Reflection.Assembly.Load(ms.ToArray());
            var entryPoint = assembly.EntryPoint;
            entryPoint?.Invoke(null, null);

            /*
            Compilation failed:
            (10,51): error CS0012: The type 'Object' is defined in an assembly that is not referenced. You must add a reference to assembly 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'.
            (11,25): error CS0103: The name 'Console' does not exist in the current context
            */

            Console.ReadLine();
        }
    }
}
