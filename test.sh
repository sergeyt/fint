#!/usr/bin/env bash

./build.sh

function run() {
    SAMPLE=$1
    ./csc.sh ${SAMPLE}
    DLL=${SAMPLE%???}.dll
    EXPECTED=`dotnet ${DLL}`
    OUT=`dotnet run ${DLL}`
    if [[ $OUT != $EXPECTED ]]
    then
        echo "FAIL ${SAMPLE}! EXPECTED '${EXPECTED}', BUT WAS '${OUT}'"
    else
        echo "PASS ${SAMPLE}"
    fi
}

run $1

# for f in test/Switch*.cs
# do
#     run $f
# done
