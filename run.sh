#!/usr/bin/env bash

set -e

./build.sh

dotnet run test/sample1.dll
