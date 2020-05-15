param(
  $PackageVersion
)

# Synopsis: Build the app.
task Build-App {
  exec {
    cd ClientApp.Cli
    dotnet @(
        "publish",
        "-v",
        "minimal",
        "-c",
        "Release",
        "-r",
        "win-x64",
        "--self-contained",
        "true",
        "-p:PublishSingleFile=false",
        "-p:PublishTrimmed=true"
      )
  }
}

# Synopsis: Build the Chocolatey package (CI).
task Build-Package {
  assert(![string]::IsNullOrWhiteSpace($PackageVersion)) "Package version should be specified"
  New-Item -ItemType Directory -Force -Path artifacts | Out-Null
  cd chocolatey
  exec {
      choco @(
        "pack",
        "mediafiles.nuspec",
        "--out",
        "..\artifacts",
        "--version",
        "$PackageVersion"
      )
  }
}

# Synopsis: Build the Chocolatey package (for local tests).
task Build-Package-Local {
  New-Item -ItemType Directory -Force -Path artifacts | Out-Null
  cd chocolatey
  exec {
      choco @(
        "pack",
        "mediafiles.nuspec",
        "--out",
        "..\artifacts",
        "--version",
        "0.1-local"
      )
  }
}

# Synopsis: Install the Chocolatey package locally (for local tests).
task Install-Package-Local Build-Package-Local, {
  cd artifacts
  exec {
      choco @(
        "install",
        "mediafiles",
        "-dv",
        "--force",
        "-pre",
        "-s",
        "`"'.;https://chocolatey.org/api/v2/'`""
      )
  }
}
