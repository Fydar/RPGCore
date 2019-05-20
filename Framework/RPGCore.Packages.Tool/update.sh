dotnet pack

dotnet tool uninstall -g RPGCore.Packages.Tool

dotnet tool install --global --add-source ./nupkg RPGCore.Packages.Tool
