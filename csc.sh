#!/usr/bin/env bash

set -e

LIB=/usr/local/share/dotnet/shared/Microsoft.NETCore.App/3.0.0
CSC=/usr/local/share/dotnet/sdk/3.0.100/Roslyn/bincore/csc.dll
SAMPLE=$1
NAME=${SAMPLE%???}
OUT=${NAME}.dll

dotnet ${CSC} -nologo -r:${LIB}/System.Private.CoreLib.dll -r:${LIB}/System.Console.dll -r:${LIB}/System.Runtime.dll -out:${OUT} ${SAMPLE}
cp -f test/runtimeconfig.json ${NAME}.runtimeconfig.json
