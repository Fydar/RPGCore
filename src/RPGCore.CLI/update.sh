dotnet pack --verbosity minimal

dotnet tool uninstall --global RPGCore.CLI

dotnet tool install --global --add-source ./nupkg RPGCore.CLI
dotnet tool install --global dotnet-suggest
