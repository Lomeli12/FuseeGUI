#!/bin/bash
PLATFORM="null"

if [[ ! -z $1 ]] && ([ $1 == "linux" ] || [ $1 == "osx" ] || [ $1 == "win" ]); then
    PLATFORM=$1
else
    case "$OSTYPE" in
        "linux-gnu"*)
            PLATFORM="linux"
            ;;
        "darwin"*)
            PLATFORM="osx"
            ;;
        "msys"* | "win32"*)
            PLATFORM="win"
            ;;
        *)
            echo "How'd we get here?"
            exit
            ;;
    esac
fi

if [[ PLATFORM != "null" ]]; then
    echo "Target Platform = $PLATFORM"
    dotnet publish -c Release -r $PLATFORM-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
    cp nintendo-switch.png bin/Release/netcoreapp3.0/$PLATFORM-x64/publish/
fi

