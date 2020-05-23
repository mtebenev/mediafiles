# mediafiles
The tool for searching duplicate video files.

[![Build Status](https://dev.azure.com/mtebenev/mediafiles/_apis/build/status/mediafiles-develop?branchName=develop)](https://dev.azure.com/mtebenev/mediafiles/_build/latest?definitionId=19&branchName=develop)

## Installing
Use Chocolatey package manager to install the mediafiles.
1. Add dev feed to the package manager:
```bash
choco source add -n mediafiles -s https://pkgs.dev.azure.com/mtebenev/mediafiles/_packaging/mediafiles-dev/nuget/v2
```
Then:
```bash
choco install mediafiles -pre
```

## Usage

### Scanning files into the catalog
```bash
mf scan c:\my_media
```

### Searching for video duplicates
```bash
mf search-vdups
```

### Search for duplicated video among local files (compared to cataloged)

### Searching for video duplicates
```bash
mf search-video
```

## Building
* Use IDE to build the solution
* To build using the command line install Invoke-Build first:
```bash
Install-Module InvokeBuild
Import-Module InvokeBuild
```
Then:
```bash
Invoke-Build
```