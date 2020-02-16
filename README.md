# NugetLicenseCsvParser

Provides a solution for extracting and parsing nuget license information in an efficient manner.

## How to generate csv
In Package Manager Console input the following command

`Get-Package | Select-Object Id,Version,LicenseUrl | Export-Csv -Path <path to outputted csv> -NoTypeInformation`
