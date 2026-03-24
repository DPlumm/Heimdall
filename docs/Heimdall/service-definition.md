# Heimdall Service Definition

## Service Name

Heimdall

## Purpose

Heimdall is the API layer of the platform.

Its purpose is to receive monitoring data from clients, apply platform rules, read and write platform data, and expose the information required by the S.W.O.T dashboard.

## Service Type

Backend API / application service layer

## Primary Responsibilities

- receive pushed monitoring updates from Huginn clients
- validate incoming monitoring payloads
- persist current and historical monitoring data through Muninn
- calculate service-level roll-up status from instance-level data
- expose read endpoints for dashboard consumption
- expose configuration data required by the dashboard
- provide a single application-facing interface to platform data

## Key Characteristics

- central platform API
- push-based monitoring model
- near real-time ingestion and read model
- production-only per deployment in the MVP
- simple and predictable rules for status freshness and roll-up

## Inputs

Heimdall accepts inputs from:

- Huginn monitoring clients
- platform configuration sources
- future administrative tooling, if introduced later

Typical monitoring input includes:

- monitored object identity
- instance identity
- timestamp
- status
- message
- labels
- stat values

## Outputs

Heimdall provides outputs to:

- S.W.O.T dashboard UI
- future admin tooling
- future automation or alerting integrations

Typical output includes:

- dashboard overview data
- object detail data
- status history
- stat history
- label and configuration data

## Dependencies

Heimdall depends on:

- Muninn for storage
- platform configuration
- network connectivity from clients and to the dashboard

## Core Rules

Heimdall is responsible for enforcing core platform rules such as:

- freshness windows
- stale/unknown behaviour
- service roll-up logic
- threshold evaluation
- label-based grouping metadata

## MVP Scope

Included in MVP:

- ingestion endpoints
- dashboard read endpoints
- storage orchestration
- roll-up calculation
- threshold handling
- configuration access for dashboard behaviour

Excluded from MVP:

- admin endpoints for end-user configuration
- alerting and notifications
- multi-environment support in one deployment
- tenancy boundaries
- complex workflow orchestration

## Future Direction

Potential future enhancements include:

- admin APIs
- user-editable dashboard configuration
- alerting integrations
- synthetic test orchestration
- more advanced querying and aggregation
- multi-environment support

## Summary

Heimdall is the central application layer of the platform. It connects the monitoring client, storage layer, and dashboard into a single working system.