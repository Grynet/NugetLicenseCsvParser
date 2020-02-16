# NugetLicenseCsvParser

This parser relies on the integration that the Package Manager Console has with the visual studio objects to extract nuget licensing information. It currently takes a .csv that is generated with a command in the package manager console and then parses that file, groups the nugets based on the license they use, and then presents it in a readable format. 

## How to generate csv
In Package Manager Console input the following command

`Get-Package | Select-Object Id,Version,LicenseUrl | Export-Csv -Path <path to outputted csv> -NoTypeInformation`

## Further reading
This project utilizes the concurrency that the TPL dataflow library provides, you can read more about how that works here:
https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library
