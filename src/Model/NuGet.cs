using System;

namespace NugetLicenseCsvParser.Model
{
    public class NuGet : IEquatable<NuGet>
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string LicenseUrl { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as NuGet);
        }

        public bool Equals(NuGet other)
        {
            return other != null &&
                   Name == other.Name &&
                   Version == other.Version &&
                   LicenseUrl == other.LicenseUrl;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Version, LicenseUrl);
        }
    }
}
