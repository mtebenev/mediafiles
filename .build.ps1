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
        "Release"
      )
  }
}

# Synopsis: Build the Chocolatey package.
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
