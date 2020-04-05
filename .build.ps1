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
  New-Item -ItemType Directory -Force -Path artifacts | Out-Null
  cd chocolatey
  exec {
      choco @(
        "pack",
        "mediafiles.nuspec",
        "--out",
        "..\artifacts"
      )
  }
}
