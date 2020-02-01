#!/usr/bin/env bash

# Define default arguments.
SCRIPT="build.cake"
TARGET="Publish"
CONFIGURATION="Release"
VERBOSITY="verbose"
DRYRUN=
CAKE_ARGUMENTS()

# Parse arguments.
for i in "$@"; do
    case $1 in
        #-s|--script) SCRIPT="$2"; shift ;;
        -t|--target) TARGET="$2"; shift ;;
        -c|--configuration) CONFIGURATION="$2"; shift ;;
        -v|--verbosity) VERBOSITY="$2"; shift ;;
        -d|--dryrun) DRYRUN="-dryrun" ;;
        --) shift; CAKE_ARGUMENTS+=("$@"); break ;;
        *) CAKE_ARGUMENTS+=("$1") ;;
    esac
    shift
done

# Restore Cake tool
dotnet tool restore

if [ $? -ne 0 ]; then
    echo "An error occured while installing Cake."
    exit 1
fi

# Start Cake
dotnet tool run dotnet-cake "$SCRIPT" --verbosity=$VERBOSITY --configuration=$CONFIGURATION --target=$TARGET $DRYRUN "${CAKE_ARGUMENTS[@]}"