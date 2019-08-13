dotnet pack --verbosity minimal

dotnet tool uninstall -g RPGCore.CLI

dotnet tool install --global --add-source ./nupkg RPGCore.CLI
