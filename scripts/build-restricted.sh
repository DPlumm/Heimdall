#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR"

# Restricted path for environments without reliable external NuGet feed access.
# Intentionally excludes solution-level restore/build and excludes test projects.
dotnet build src/Heimdall/Heimdall.Api/Heimdall.Api.csproj
dotnet build src/Huginn/Huginn.Client/Huginn.Client.csproj
dotnet build src/Muninn/Muninn.Storage/Muninn.Storage.csproj
dotnet build src/SWOT/SWOT.Web/SWOT.Web.csproj
