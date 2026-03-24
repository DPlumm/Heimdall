# Muninn Service Definition

## Service Name

Muninn

## Purpose

Muninn is the storage layer of the platform.

Its purpose is to persist monitoring data, configuration data, and dashboard-related settings in a way that supports both near real-time operational views and recent historical views.

## Service Type

Storage layer / persistence layer

## Primary Responsibilities

- store monitored object definitions
- store monitored instance definitions
- store current status data
- store current stat data
- store recent status history
- store recent stat history
- store dashboard configuration
- store branding configuration
- support efficient reads for dashboard and API use

## Key Characteristics

- central source of persisted platform data
- supports both current-state and history-based queries
- designed for simplicity and clarity in the MVP
- intended to back both S.W.O.T and Heimdall

## Data Areas

Muninn is expected to store at least the following categories of data:

### Monitoring metadata
- monitored objects
- monitored instances
- labels
- stat definitions

### Current operational state
- latest status per instance
- latest message per instance
- latest stat values per instance
- last seen timestamps

### Historical data
- status snapshots
- stat snapshots

### Configuration data
- dashboard configuration
- branding values
- presentation settings

## Inputs

Muninn receives data via Heimdall, including:

- validated monitoring submissions
- configuration changes
- branding settings
- future administrative changes

## Outputs

Muninn provides persisted data back to Heimdall for:

- overview queries
- detail queries
- history queries
- configuration reads
- roll-up calculation inputs

## Dependencies

Muninn depends on:

- a chosen database technology
- data access implemented through Heimdall or shared platform libraries

## MVP Scope

Included in MVP:

- current-state persistence
- recent history persistence
- configuration persistence
- branding persistence
- simple query support for dashboard scenarios

Excluded from MVP:

- advanced long-term aggregation
- archival policies
- cross-environment tenancy support
- complex analytics features

## Technology Direction

The MVP is expected to use a relational storage model, as this fits:

- structured monitoring data
- configuration data
- predictable dashboard query patterns

The exact technology may change later, but the storage contract should remain stable.

## Future Direction

Potential future enhancements include:

- retention policies
- summarised historical aggregates
- partitioning or scaling strategies
- support for additional storage backends
- richer analytics support

## Summary

Muninn is the persistence foundation of the platform. It holds the operational truth that powers both the Heimdall API and the S.W.O.T dashboard.