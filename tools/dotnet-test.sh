#!/usr/bin/env bash
set -euo pipefail

export NUGET_PACKAGES="${NUGET_PACKAGES:-/tmp/nuget-packages}"
export NUGET_HTTP_CACHE_PATH="${NUGET_HTTP_CACHE_PATH:-/tmp/nuget-http-cache}"

mkdir -p "$NUGET_PACKAGES" "$NUGET_HTTP_CACHE_PATH"

dotnet test "${@:-app.Tests/app.Tests.csproj}"
