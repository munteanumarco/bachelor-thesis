#!/bin/bash

ENV_FILE=".env.local"

if [ ! -f "$ENV_FILE" ]; then
    echo "Error: $ENV_FILE file not found."
    exit 1
fi

set -a # automatically export all variables
source "$ENV_FILE"
set +a
echo "Environment variables loaded from $ENV_FILE."
