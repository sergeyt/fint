#!/usr/bin/env bash

set -e

./build.sh

dotnet run test/hello/hello.dll
