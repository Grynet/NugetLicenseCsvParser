using NugetLicenseCsvParser.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace NugetLicenseCsvParser
{
    public class LicenseCsvParser
    {
        private ConcurrentDictionary<string, HashSet<NuGet>> _nugetsByLicenseUrl;

        private readonly BufferBlock<string> _csvBuffer;
        private readonly TransformBlock<string, NuGet> _parseBlock;
        private readonly ActionBlock<NuGet> _aggregationBlock;       

        public LicenseCsvParser()
        {
            _csvBuffer = new BufferBlock<string>();
            _parseBlock = new TransformBlock<string, NuGet>(MapToNuget);
            _aggregationBlock = new ActionBlock<NuGet>(AggregateNugets);

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            _csvBuffer.LinkTo(_parseBlock, linkOptions);
            _parseBlock.LinkTo(_aggregationBlock, linkOptions);
        }

        public ConcurrentDictionary<string, HashSet<NuGet>> ParseLicenseCsv(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"{nameof(filePath)} cannot be null or empty");

            _nugetsByLicenseUrl = new ConcurrentDictionary<string, HashSet<NuGet>>();

            using (System.IO.StreamReader file = new System.IO.StreamReader(filePath))
            {
                string line = file.ReadLine();
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Replace("\"", "");
                    _csvBuffer.Post(line);
                }
            }

            _csvBuffer.Complete();
            _aggregationBlock.Completion.Wait();
            return _nugetsByLicenseUrl;
        }

        private NuGet MapToNuget(string csvLine)
        {
            var csv = csvLine.Split(',');
            var nugetName = csv[0];
            var nugetVersion = csv[1];
            var nugetLicenseUrl = csv[2];

            var nuget = new NuGet
            {
                Name = nugetName,
                Version = nugetVersion,
                LicenseUrl = string.IsNullOrWhiteSpace(nugetLicenseUrl) ? null : nugetLicenseUrl
            };

            return nuget;
        }

        private void AggregateNugets(NuGet nuget)
        {
            _nugetsByLicenseUrl.AddOrUpdate(nuget.LicenseUrl ?? "", new HashSet<NuGet> { nuget }, (licenseUrl, oldNugets) =>
            {
                oldNugets.Add(nuget);
                return oldNugets;
            });
        }
    }
}
