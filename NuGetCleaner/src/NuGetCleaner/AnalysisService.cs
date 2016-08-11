using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGetCleaner.Models;

namespace NuGetCleaner
{
    public class AnalysisService
    {
        private readonly List<string> _fileList;

        public AnalysisService()
        {
            _fileList = new List<string>();
        }

        public void Start(string targetDirectory)
        {
            DirectorySearch(targetDirectory, "*packages.config");

            var packages = GetPackageList();

            var packagesDirectory = $"{targetDirectory}..\\..\\..\\packages\\";

            foreach (var package in packages)
            {
                // Dont need to do that as files are already extracted!!
                //File.Copy($"{packagesDirectory}{package.DirectoryName}", $"{packagesDirectory}{package.DirectoryName.Replace("nupkg", "zip")}");
            }
        }

        private void DirectorySearch(string root, string pattern)
        {
            foreach (var directory in Directory.GetDirectories(root))
            {
                DirectorySearch(directory, pattern);
            }

            _fileList.AddRange(Directory.GetFiles(root, pattern).ToList());
        }

        private IEnumerable<Package> GetPackageList()
        {
            var packages = new List<Package>();

            foreach (var packageFile in _fileList)
            {
                foreach (var assemblyPackage in GetPackageList(packageFile))
                {
                    var directoryName = Path.GetDirectoryName(packageFile);

                    if (directoryName != null)
                    {
                        if (packages.SingleOrDefault(f => f.ToString() == assemblyPackage.ToString()) == null)
                        {
                            packages.Add(new Package { Id = assemblyPackage.Id, Version = assemblyPackage.Version });
                        }
                    }
                }
            }

            return packages;
        }

        private static IEnumerable<Package> GetPackageList(string packageFile)
        {
            var file = XDocument.Load(packageFile);

            if (file.Root != null)
            {
                var packages = file.Root.Elements("package");

                foreach (var package in packages)
                {
                    yield return new Package
                    {
                        Id = package.Attribute("id").Value,
                        Version = package.Attribute("version").Value
                    };
                }
            }
        }
    }
}
