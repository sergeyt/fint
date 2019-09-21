#!/usr/bin/env bash

set -e

rm -rf bin
rm -rf obj

dotnet build
