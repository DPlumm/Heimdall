# Heimdall
The All-Father's Status Board
---
Heimdall is a public, read-only production status board for operational visibility.

Its purpose is to provide a simple dashboard showing the current health, freshness, and selected configurable statistics for monitored systems, backed by a lightweight monitoring client that pushes updates to a central service.

## MVP Scope

The initial MVP focuses on:

- a read-only dashboard
- production environment only
- push-based monitoring
- near real-time status updates
- service-level and instance-level visibility
- configurable thresholds and labels
- simple support for monitoring web apps, Windows services, websites, servers, and similar components

## Core Principles

- The dashboard is public and read-only.
- No dashboard access controls are planned for the MVP.
- No dashboard audit logging is planned for the MVP.
- Production is the only supported environment in each deployment.
- UAT can be deployed separately if needed.
- Data is considered live for 60 seconds.
- Data older than 60 seconds is stale but still valid until 5 minutes.
- Data older than 5 minutes is treated as unknown.

## Planned Components

### Heimdall Dashboard
A read-only web dashboard that shows:

- current status of monitored objects
- last update time
- configurable stats
- labels for grouping and filtering
- service-level rollups
- instance-level detail

### Heimdall Monitoring Client
A lightweight client that can be attached to monitored systems and used to push:

- heartbeat data
- status updates
- messages
- configurable stats
- instance metadata

### Heimdall Backend
A central service responsible for:

- receiving pushed updates
- storing current and recent monitoring data
- calculating rollup status
- exposing data for the dashboard

## Status Model

The initial status model is:

- Healthy
- Degraded
- Unhealthy
- Stale
- Unknown
- Maintenance

## Freshness Rules

- 0 to 60 seconds: live
- 61 seconds to 5 minutes: stale
- over 5 minutes: unknown

## Configuration Model

The initial configuration model is intentionally simple and supports:

- thresholds
- labels

This may expand later as the product matures.

## Initial Monitoring Targets

Heimdall is intended to support monitoring of things such as:

- web applications
- Windows services
- websites
- servers
- other deployable or runtime components

## Repository Purpose

This repository contains the source code and supporting documentation for Heimdall.

As the project develops, it is expected to contain:

- dashboard application
- backend API
- monitoring client
- domain models
- tests
- architecture and decision documentation

## Documentation

Project documentation should live under the `docs/` folder.

Early WIP documents:
- `docs/decision-log.md`
- `docs/requirements.md`
- `docs/architecture.md`

## Current Project Status

Heimdall is currently in the definition and design stage, with MVP scope and core technical decisions being established before implementation begins.

## Repository bootstrap layout (2026-03-27)

Initial component scaffolding has started with separate .NET solution files for each core component and a root aggregate solution:

- `Heimdall.Api.sln`
- `Huginn.sln`
- `Muninn.sln`
- `SWOT.sln`
- `Heimdall.sln` (aggregate)

Source layout:

- `src/Heimdall/Heimdall.Api`
- `src/Huginn/Huginn.Client`
- `src/Muninn/Muninn.Storage`
- `src/SWOT/SWOT.Web`
- `tests/Huginn.Client.Tests`
