dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
dotnet publish -c Release -r osx-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
copy nintendo-switch.png bin\Release\netcoreapp3.0\linux-x64\publish\
copy nintendo-switch.png bin\Release\netcoreapp3.0\osx-x64\publish\
copy nintendo-switch.png bin\Release\netcoreapp3.0\win-x64\publish\