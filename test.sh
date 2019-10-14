#!/usr/bin/env bash

function run() {
    ./build.sh
    SAMPLE=$1
    ./csc.sh ${SAMPLE}
    DLL=${SAMPLE%???}.dll
    EXPECTED=`dotnet ${DLL}`
    OUT=`dotnet run ${DLL}`
    if [[ $OUT != $EXPECTED ]]
    then
        echo "FAIL! EXPECTED '${EXPECTED}', BUT WAS '${OUT}'"
    else
        echo "PASS"
    fi
}

run $1
