#!/usr/bin/env bash

./build.sh

function run() {
    SAMPLE=$1
    ./csc.sh ${SAMPLE}
    DLL=${SAMPLE%???}.dll
    EXPECTED=`dotnet ${DLL}`
    OUT=`dotnet run run ${DLL}`
    if [[ $OUT != $EXPECTED ]]
    then
        echo "FAIL ${SAMPLE}"
        icdiff <(echo "${EXPECTED}") <(echo "${OUT}")
    else
        echo "PASS ${SAMPLE}"
    fi
}

SAMPLE=$1
if [[ $SAMPLE == "all" ]]
then
    for f in test/*.cs
    do
        run $f
    done
else
    run $SAMPLE
fi
