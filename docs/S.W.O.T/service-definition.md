# S.W.O.T Service Definition

## Service Name

S.W.O.T  
Software Well-being Observability Tool

## Purpose

S.W.O.T is the user-facing web dashboard for the Heimdall platform.

Its purpose is to provide a clear, read-only view of the current health, freshness, and selected statistics of monitored systems.

## Service Type

Web application / dashboard UI

## Primary Responsibilities

- display the current status of monitored objects
- display service-level roll-up status
- display instance-level detail
- show freshness of incoming monitoring data
- show selected configurable statistics
- support filtering and grouping using labels
- present a simple operational view suitable for desktop and wallboard use
- apply configurable branding and visual identity

## Key Characteristics

- public and read-only in the MVP
- no dashboard access controls in the MVP
- no dashboard auditing in the MVP
- production-focused deployment model
- near real-time display of monitoring data

## Inputs

S.W.O.T consumes data from the Heimdall API, including:

- monitored object summaries
- instance-level status data
- recent status history
- recent stat history
- dashboard configuration
- branding configuration

## Outputs

S.W.O.T produces:

- dashboard overview pages
- monitored object detail views
- filtered and grouped status views for end users

## Dependencies

S.W.O.T depends on:

- Heimdall for API access
- Muninn for stored monitoring and configuration data, indirectly through Heimdall

## Configuration

S.W.O.T should support configuration for:

- branding name and text
- colours
- favicon paths
- branding icon paths
- dashboard presentation settings

In the MVP, these settings are expected to come from configuration and central storage rather than from an admin UI.

## MVP Scope

Included in MVP:

- overview dashboard
- object detail view
- service and instance visibility
- status and freshness display
- label-based filtering/grouping
- configurable branding

Excluded from MVP:

- admin UI
- write actions
- alerting workflows
- user-specific customisation
- access control

## Future Direction

Potential future enhancements include:

- admin configuration screens
- richer charts and visualisations
- environment switching
- alert and notification integration
- more advanced dashboard layouts

## Summary

S.W.O.T is the presentation layer of the platform. It exists to make operational status easy to see and easy to understand.