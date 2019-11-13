#!/bin/sh

{
    unity_project_path="$(git rev-parse --show-toplevel)/src/RPGCoreUnity"

    if [ ! -d "$unity_project_path" ]; then
        echo "Couldn't find target destination to distribute build output."
        exit 0
    fi
    
    cp -fv $1 "$unity_project_path/$2"
    exit 0
} || {
    echo "Unable to copy files to target destination to distribute build output."
}
