#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR"

# Full path for environments with external NuGet feed access.
dotnet restore Heimdall.sln --configfile nuget.config
dotnet build Heimdall.sln --no-restore
dotnet test tests/Huginn.Client.Tests/Huginn.Client.Tests.csproj --no-build
