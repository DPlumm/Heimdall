# Huginn Service Definition

## Service Name

Huginn

## Purpose

Huginn is the monitoring client for the platform.

Its purpose is to attach to monitored systems, gather health and stat information, and push that information to Heimdall for storage and display.

## Service Type

Monitoring client / agent / embedded reporting component

## Primary Responsibilities

- send heartbeat data
- send current health status
- send messages describing current state
- send configurable statistics
- identify the monitored object and instance
- include useful metadata such as version and host name
- retry on temporary submission failures
- keep runtime overhead low

## Key Characteristics

- push-based client model
- lightweight and simple to integrate
- initially focused on .NET services and applications
- suitable for use across workplace, home lab, and personal hosting scenarios

## Initial Monitoring Targets

Huginn is expected to support monitoring targets such as:

- web applications
- Windows services
- websites
- servers
- other runtime or deployable components

## Inputs

Huginn consumes information from the monitored system, such as:

- application health state
- service-local checks
- stat values
- host and instance identity
- timestamps
- version/build information

## Outputs

Huginn produces monitoring submissions to Heimdall, including:

- object name
- object type
- instance name
- host name
- timestamp
- status
- message
- labels
- stat values

## Dependencies

Huginn depends on:

- the monitored system it is attached to
- connectivity to Heimdall
- configuration for endpoint and submission behaviour

## MVP Scope

Included in MVP:

- heartbeat submission
- status submission
- message submission
- stat submission
- retry handling
- .NET-first integration approach

Excluded from MVP:

- browser-based synthetic testing
- full remote probe orchestration
- advanced scripting models
- complex distributed test scheduling

## Future Direction

In the long term, Huginn may support browser-based synthetic testing using headless browser execution.

This would allow Huginn to:

- run user-journey style checks
- execute Playwright-style automated tests
- report application health based on real interaction outcomes

This is explicitly outside MVP scope.

## Design Principles

Huginn should be:

- easy to adopt
- safe to run in production
- low overhead
- resilient to temporary failures
- flexible enough to report both simple and richer monitoring signals

## Summary

Huginn is the reporting edge of the platform. It collects monitoring information close to the monitored system and pushes that information into Heimdall for storage and presentation.