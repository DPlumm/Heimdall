# Heimdall Delivery Plan (MVP-first Thin Vertical Slices)

This plan translates existing requirements and architecture into a fast implementation path. It is intentionally practical and lightweight.

## Source alignment
This plan is based on:
- `docs/requirements.md`
- `docs/architecture.md`
- `docs/relationships.md`
- `docs/decision-log.md`
- service definitions for `S.W.O.T`, `Heimdall`, `Muninn`, `Huginn`

## Resolved MVP implementation decisions
- Canonical ingest naming uses `Service` / `Instance` / `Heartbeat` terms.
- Required ingestion fields for MVP: `serviceName`, `instanceName`, `hostName`, `version`, `heartbeatTimestamp`, `status`.
- `environment` is optional and ignored by MVP ingest behavior.
- Roll-up scope for MVP is `instance -> service` only (no parent-service hierarchy in MVP).
- Freshness is calculated from client-provided `heartbeatTimestamp`.
- Muninn MVP storage engine is SQLite.
- MVP history retention target is 90 days.
- S.W.O.T auto-refresh default is 60 seconds and configurable.

## Delivery objective
Ship the smallest end-to-end usable platform increment where:
1. `Huginn` can push heartbeat/status for `Service` + `Instance`.
2. `Heimdall` persists latest state in `Muninn` and applies freshness rules.
3. `S.W.O.T` shows a public, read-only overview with near real-time refresh.

## Must do for MVP

### Slice A - Minimal ingestion to latest-state read path
- Implement minimal ingestion endpoint in `Heimdall` for heartbeat/status submissions.
- Validate required fields: `serviceName`, `instanceName`, `hostName`, `version`, `heartbeatTimestamp`, `status`.
- Accept optional fields without blocking MVP ingest: `environment`, `message`, `labels`, `stats`.
- Persist latest per-instance state in `Muninn` (SQLite).
- Apply freshness rules (live/stale/unknown windows) using client-provided `heartbeatTimestamp`.
- Expose minimal overview read endpoint for `S.W.O.T`.

### Slice B - Service roll-up + overview dashboard
- Implement service-level roll-up from instance states (`instance -> service` only).
- Render `S.W.O.T` overview page with:
  - service name
  - rolled-up status
  - freshness
  - last update time
  - brief message (if present)
- Add auto-refresh with default 60-second interval (configurable).

### Slice C - Detail view + short recent history
- Add service detail endpoint returning:
  - service metadata
  - instance list + per-instance status/freshness
  - recent status/stat history (retained up to 90 days)
- Add `S.W.O.T` detail page showing current state and recent history.

### Slice D - Label filtering and threshold basics
- Add label-based filtering/grouping in read path.
- Add basic threshold evaluation for configured stats.
- Ensure dashboard displays threshold-influenced status clearly.

### Slice E - Branding/config read support
- Read dashboard branding/config values from central configuration storage.
- Apply title/branding assets/colors in `S.W.O.T` without adding admin UI.

## Should do soon after MVP
- Expand ingestion contract hardening (versioning strategy, stricter schema validation, compatibility tests).
- Improve historical querying (time windows, pagination, targeted retrieval).
- Add operational quality improvements:
  - health endpoints
  - structured logging
  - basic metrics for ingestion/read latency
- Add seed/dev tooling for local demo data.
- Add minimal deployment packaging guidance for org + self-hosted setups.

## Later / deferred work
- Dashboard access controls and audit logging.
- Admin UI for configuration management.
- Alerting and notifications.
- Multi-environment support in one deployment.
- Tenancy/isolation boundaries.
- Browser-based synthetic checks from `Huginn`.
- Parent-service hierarchy roll-up.

## Recommended first implementation sequence
1. **Contract first**: lock minimal heartbeat schema and canonical required fields.
2. **Write path**: implement ingestion and latest-state SQLite persistence.
3. **Read path (overview)**: implement service roll-up + overview endpoint.
4. **UI first value**: implement basic public `S.W.O.T` overview with configurable auto-refresh (default 60 seconds).
5. **Detail path**: add detail endpoint + detail UI + retained history rendering.
6. **MVP finishing**: labels, thresholds, branding config read support.
7. **Stabilization**: focused tests, docs alignment, and deployment/runbook notes.

## 2026-03-27 bootstrap update
- Added first-codebase bootstrap direction: each component (`Heimdall`, `Huginn`, `Muninn`, `S.W.O.T`) should be independently buildable through its own `.sln`, while the repository root `.sln` builds the full package together.
- Began with `HUG-001` implementation path: Huginn one-cycle config-load and heartbeat submit bootstrap.
- Heartbeat schema authority aligned to canonical camelCase MVP required fields (`serviceName`, `instanceName`, `hostName`, `version`, `heartbeatTimestamp`, `status`).

## Practical guardrails for rapid follow-on work
- Keep slice size to 1-3 days where possible.
- Merge only end-to-end slices that are demonstrably usable.
- Defer non-blocking abstractions until repeated need appears.
- Move resolved decision items from `docs/open-questions.md` into this delivery plan and `docs/decision-log.md`.
- Keep terminology canonical (`Service`, `Instance`, `Heartbeat`).
