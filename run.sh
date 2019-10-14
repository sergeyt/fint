#!/usr/bin/env bash

set -e

./build.sh
IN=$1
SAMPLE=${IN:=sample1}
./csc.sh test/${SAMPLE}.cs
dotnet run test/${SAMPLE}.dll
