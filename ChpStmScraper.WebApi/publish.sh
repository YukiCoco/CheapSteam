version="latest"

dotnet publish --runtime linux-x64 -p:PublishSingleFile=true --self-contained true -o ./bin/Release/WebApi/${version}/CheapSteam-${version}-linux-x64 -c Release
dotnet publish --runtime osx-x64 -p:PublishSingleFile=true --self-contained true -o ./bin/Release/WebApi/${version}/CheapSteam-${version}-macOS-x64 -c Release
dotnet publish --runtime win-x64 -p:PublishSingleFile=true --self-contained true -o ./bin/Release/WebApi/${version}/CheapSteam-${version}-win-x64 -c Release

cd ./bin/Release/WebApi/${version}/
zip -r CheapSteam-${version}-linux-x64.zip CheapSteam-${version}-linux-x64
zip -r CheapSteam-${version}-macOS-x64.zip CheapSteam-${version}-macOS-x64
zip -r CheapSteam-${version}-win-x64.zip CheapSteam-${version}-win-x64