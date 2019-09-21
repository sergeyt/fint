#!/usr/bin/env bash

set -e

./build.sh

cp -f test/hello/hello.dll bin/Debug/

cd bin/Debug

dotnet fint.dll hello.dll
