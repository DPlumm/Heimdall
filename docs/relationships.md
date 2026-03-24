# Platform Relationships

## Purpose

This document describes how the core Heimdall platform services relate to one another.

It explains the role of each named component and how data flows through the platform in the MVP.

## Platform Overview

The platform is made up of four main components:

1. S.W.O.T
2. Heimdall
3. Muninn
4. Huginn

Each component has a distinct responsibility, but they work together as one monitoring and status platform.

## Component Roles

## S.W.O.T

S.W.O.T (`Software Well-being Observability Tool`) is the user-facing web dashboard.

It is responsible for presenting monitoring information in a simple, read-only format for end users.

S.W.O.T is the presentation layer.

## Heimdall

Heimdall is the backend API and application layer.

It is responsible for receiving monitoring data, applying platform rules, and serving data to the dashboard.

Heimdall is the central coordination layer.

## Muninn

Muninn is the storage layer.

It is responsible for persisting monitoring state, monitoring history, dashboard configuration, and branding-related configuration.

Muninn is the persistence layer.

## Huginn

Huginn is the monitoring client.

It is responsible for gathering health, heartbeat, and stat information from monitored systems and pushing that information to Heimdall.

Huginn is the reporting edge of the platform.

## Relationship Summary

The relationship between the components can be described simply as:

- Huginn reports monitoring data into Heimdall
- Heimdall validates and processes that data
- Heimdall stores and reads platform data through Muninn
- S.W.O.T reads platform data from Heimdall
- End users view the resulting dashboard in S.W.O.T

## High-Level Flow
```mermaid
flowchart LR
    Huginn[Huginn<br/>Monitoring Client] --> Heimdall[Heimdall<br/>API Layer]
    Heimdall --> Muninn[Muninn<br/>Storage Layer]
    SWOT[S.W.O.T<br/>Web Dashboard UI] <--> Heimdall
```
## Sequence Diagram 
```mermaid
sequenceDiagram

    actor User as End User
    participant SWOT as S.W.O.T
    participant Heimdall as Heimdall
    participant Muninn as Muninn
    participant Huginn as Huginn
    participant Target as Monitored System

    Note over Target,Huginn: Monitoring collection
    Huginn->>Target: Collect health, heartbeat, stats, metadata
    Target-->>Huginn: Current status data

    Note over Huginn,Heimdall: Push-based ingestion
    Huginn->>Heimdall: POST monitoring submission
    Note right of Huginn: object name, instance name, timestamp,<br/>status, message, labels, stats
    Heimdall->>Heimdall: Validate payload
    Heimdall->>Heimdall: Apply freshness, thresholds, roll-up inputs

    Note over Heimdall,Muninn: Persistence
    Heimdall->>Muninn: Write current state
    Heimdall->>Muninn: Write status/stat history
    Heimdall->>Muninn: Read/write config and branding
    Muninn-->>Heimdall: Persisted data confirmed

    Note over User,SWOT: Dashboard read flow
    User->>SWOT: Open dashboard
    SWOT->>Heimdall: GET overview / detail / labels / config
    Heimdall->>Muninn: Read current state, history, config, branding
    Muninn-->>Heimdall: Return requested data
    Heimdall->>Heimdall: Calculate service roll-up state
    Heimdall-->>SWOT: Return dashboard data
    SWOT-->>User: Render read-only dashboard

    Note over SWOT,Heimdall: Auto-refresh
    loop Near real-time refresh
        SWOT->>Heimdall: Refresh dashboard data
        Heimdall->>Muninn: Read latest state
        Muninn-->>Heimdall: Latest persisted data
        Heimdall-->>SWOT: Updated overview/detail
    end
```