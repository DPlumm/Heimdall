# Heimdall Decision Log

This document records key product and technical decisions made during the early definition of Heimdall.

---

## 2026-03-24: Product name

### Decision
The product will be called `Heimdall`.

### Reasoning
A project name was chosen so repository structure and documentation can be created around a fixed identity.

---

## 2026-03-24: Product purpose

### Decision
Heimdall will be a public, read-only production status board backed by a lightweight monitoring client.

### Reasoning
The initial requirement is to provide broad visibility of service health and selected stats without introducing write actions or operational control through the dashboard.

---

## 2026-03-24: Dashboard access model

### Decision
The MVP dashboard will have no access controls.

### Reasoning
The board is intended to be openly viewable. The guiding principle is that operational status data should be visible to everyone.

### Notes
This applies to dashboard viewing only and may be revisited later if requirements change.

---

## 2026-03-24: Dashboard auditing

### Decision
The MVP will not include dashboard audit logging.

### Reasoning
The dashboard is read-only and public, so auditing viewer activity is not currently considered necessary.

---

## 2026-03-24: Environment scope

### Decision
Each Heimdall deployment will support production only.

### Reasoning
The product should remain simple in the MVP. If UAT visibility is needed, that can be handled by a separate deployment rather than adding environment complexity into the first version.

---

## 2026-03-24: Monitoring model

### Decision
Heimdall will use a push-based monitoring model.

### Reasoning
A monitoring client attached to services aligns naturally with a push model and supports consistent reporting of health, heartbeat, and stats to a central service.

---

## 2026-03-24: Data freshness model

### Decision
Heimdall will treat monitoring data using the following freshness rules:

- 0 to 60 seconds old: live
- older than 60 seconds and up to 5 minutes: stale but still valid
- older than 5 minutes: unknown

### Reasoning
This gives a near real-time experience without making the board too sensitive to brief interruptions or delayed updates.

---

## 2026-03-24: Real-time requirement

### Decision
The MVP will target near real-time status rather than true real-time streaming.

### Reasoning
Near real-time is sufficient for the intended operational use case and is simpler to implement and operate.

---

## 2026-03-24: Deployment tenancy model

### Decision
The MVP will be a single shared production board with a common configuration model and no tenancy boundaries.

### Reasoning
There is no current need for separate team-level isolation, delegated ownership boundaries, or tenant-specific configuration. These can be introduced later through metadata if needed.

### Notes
Simple metadata such as labels, owner, group, or category can still be used to organise monitored objects.

---

## 2026-03-24: Visibility model

### Decision
Heimdall will support both service-level and instance-level visibility.

### Reasoning
Users need to understand both the overall health of a monitored object and the state of the individual instances that make it up.

### Notes
A logical monitored object will roll up the status of one or more underlying instances.

---

## 2026-03-24: Initial configuration model

### Decision
The MVP configuration model will support thresholds and labels only.

### Reasoning
This provides useful flexibility without introducing complex dashboard customisation too early.

### Notes
More advanced configuration may be added later if there is a clear need.

---

## 2026-03-24: Initial monitoring targets

### Decision
The MVP should support monitoring targets such as:

- web apps
- Windows services
- websites
- servers
- similar runtime or deployable components

### Reasoning
These match the initial intended operational use cases for Heimdall.

---

## 2026-03-24: Dashboard feature scope

### Decision
The dashboard will be read-only in the MVP.

### Reasoning
The product is intended to provide visibility, not control. Operational actions and write capabilities would add complexity and risk that are outside the initial scope.

---

## 2026-03-24: UAT handling

### Decision
UAT should not be included as an environment within the initial Heimdall deployment model.

### Reasoning
If UAT monitoring is required, it can be provided via a separate deployment rather than by broadening the MVP scope.

---

## 2026-03-24: Dashboard product name

### Decision
The dashboard product name will be `S.W.O.T`, which stands for `Software Well-being Observability Tool`.

### Reasoning
This provides a distinct end-user-facing identity for the dashboard, while `Heimdall` remains the project and backend name.

### Notes
`Heimdall` should continue to be used as the repository and backend project name unless a later decision changes this.

---

## 2026-03-24: Dashboard branding configuration

### Decision
Dashboard branding will be configurable through deployment application configuration.

### Reasoning
Brand styling should be adjustable without code changes so the dashboard can be branded appropriately for its deployment context.

### Notes
This configuration is intended to include items such as:
- colours
- product branding values
- file paths for favicons
- file paths for branding icons
- similar UI identity settings

---

## 2026-03-24: Dashboard configuration storage model

### Decision
Dashboard configuration will be stored centrally and can be edited directly in the underlying configuration store, with the intention of later exposing this through an admin view.

### Reasoning
The MVP needs configurable dashboard behaviour, but does not require a full end-user administration interface yet.

### Notes
The long-term intention is for end users to manage dashboard configuration through an admin view.
This admin view is explicitly out of scope for the MVP.

---

## 2026-03-24: Long-term monitoring client capability for browser-based testing

### Decision
In the long term, the monitoring client will support running tests in headless browser windows and reporting status based on the results.

### Reasoning
Some monitored systems, especially user-facing web applications, may need status to reflect real user journey checks rather than only service-local health and metrics.

### Notes
Playwright-style browser automation is a likely model for this capability.
This is a future capability and is not part of the MVP.

---

## 2026-03-24: Monitoring client name

### Decision
The monitoring client will be called `Huginn`.

### Reasoning
`Huginn` fits the Norse mythology naming theme already used by Heimdall and is a strong thematic match for a lightweight client that gathers monitoring information and reports it back to the central service.

### Notes
This name should be used for the monitoring client in documentation, code, configuration, and deployment artefacts.

---

## 2026-03-24: Storage component name and MVP role

### Decision
The storage component will be named `Muninn` and is part of the MVP platform scope.

### Reasoning
`Muninn` complements `Huginn` within the same Norse mythology theme and is a good fit for the platform persistence layer responsible for current state and recent history.

### Notes
`Muninn` is the persistence layer used by `Heimdall` in the MVP.

---

## 2026-03-24: Canonical terminology model

### Decision
Heimdall MVP terminology is standardized as `Service`, `Instance`, and `Heartbeat`, and the term `object` is disallowed immediately.

### Reasoning
A single canonical vocabulary prevents semantic drift in payloads, API responses, storage schema, and dashboard behavior.

### Notes
Product/component names remain `Heimdall`, `S.W.O.T`, `Huginn`, and `Muninn`.

---

## 2026-03-24: Repository and package structure

### Decision
The MVP will use a monorepo with shared packages for common contracts/types, and both `Huginn` and `Muninn` will have dedicated top-level code directories.

### Reasoning
This supports fast parallel delivery while reducing cross-component contract drift.

---

## 2026-03-24: API and heartbeat contract authority

### Decision
Heimdall will use REST + JSON over HTTPS with OpenAPI and JSON Schema as authoritative contract artifacts. Versioning will use `/api/v1/...` and a payload `schema_version` field. The MVP ingestion endpoint is `POST /api/v1/heartbeats`.

### Reasoning
Schema-first contracts create reliable integration boundaries between Huginn, Heimdall, and S.W.O.T.

---

## 2026-03-24: Muninn storage engine and migration strategy

### Decision
The MVP persistence engine is SQLite with SQL-first migrations. Data retention is 90 days. Persistence includes dedicated current-state tables and history tables, with deduplication via client-provided `heartbeat_id` (UUID).

### Reasoning
This keeps initial operations simple while preserving deterministic schema control and ingestion idempotency.

---

## 2026-03-24: Ingestion authentication model

### Decision
Ingestion auth will use per-Huginn-node static API keys passed in headers, with immediate deny-list revocation and auth audit logging.

### Reasoning
Per-node credentials provide better accountability and revocation granularity than a single deployment key while remaining MVP-simple.

### Notes
Key rotation is manual in MVP and must be formally reviewed before 1.0 release planning. Target review date: 2026-09-01.

---

## 2026-03-24: Status roll-up and threshold precedence

### Decision
Instance-level evaluation gives Maintenance highest precedence and evaluates freshness (`Stale`/`Unknown`) before threshold logic. If explicit status conflicts with threshold-derived status, the worse state wins. Service roll-up order is `Unhealthy > Degraded > Stale > Unknown > Healthy`, and all-maintenance populations roll up to `Maintenance`.

### Reasoning
A deterministic precedence model prevents inconsistent status outcomes between backend and dashboard implementations.

---

## 2026-03-24: Configuration and branding source-of-truth split

### Decision
Branding and thresholds are DB-backed. Secrets remain in file/env configuration. Runtime config propagation uses a TTL cache with a default 5-minute TTL.

### Reasoning
This balances operational flexibility for product configuration with safer handling of sensitive credentials.

---

## 2026-03-24: Deployment and local development model

### Decision
The MVP deployment reference is Docker Compose on a single host/VM. Local development uses a single compose bootstrap flow with deterministic seed/demo data and Huginn simulation support.

### Reasoning
A single operational model reduces onboarding time and environment inconsistency.

---

## 2026-03-24: Testing strategy and CI merge gates

### Decision
PR merges require lint, unit, and contract tests; integration tests and minimal UI smoke E2E are required on PR. No hard global coverage percentage gate is applied in MVP. Breaking schema changes require contract version bumps.

### Reasoning
This establishes strong quality gates on critical behavior without introducing excessive early-process friction.

---

## 2026-03-24: Heimdall self-observability baseline

### Decision
Heimdall will emit JSON structured logs, provide liveness/readiness endpoints, honor `X-Request-ID`, expose `/metrics`, and default to metadata-only logging for payload safety.

### Reasoning
A minimal but production-usable observability baseline is required for operating a public status platform.

---

## 2026-03-24: Huginn node configuration profile and runtime defaults

### Decision
Huginn uses file configuration with env overrides, a default 60-second heartbeat interval, exponential backoff with jitter, restart-required config reload in MVP, and non-crashing retry behavior when destination is unavailable.

### Reasoning
These defaults provide predictable and resilient agent behavior while keeping implementation complexity low.

---

## Open items

The following areas are not yet fully decided and should be revisited in later design work:

- backend and frontend technology choices
- client authentication approach for monitoring submissions
- approach for monitoring non-.NET targets where no client can be attached directly

---

---

## 2026-03-26: MVP ingestion contract required fields and canonical naming

### Decision
The MVP ingestion contract uses canonical field names aligned to `Service`, `Instance`, and `Heartbeat` terminology.

Required fields are:
- `serviceName`
- `instanceName`
- `hostName`
- `version`
- `heartbeatTimestamp`
- `status`

### Reasoning
Consistent naming avoids domain drift and aligns contracts with the canonical terminology model.

---

## 2026-03-26: MVP environment field behavior

### Decision
`environment` is optional in ingestion payloads and is ignored by MVP ingest behavior.

### Reasoning
The deployment model is production-only in MVP, so ingest must not require or depend on environment routing.

---

## 2026-03-26: MVP roll-up scope

### Decision
MVP status roll-up scope is `instance -> service` only.

### Reasoning
This keeps roll-up logic small and shippable while preserving a clear path for later parent-service hierarchy support.

---

## 2026-03-26: Freshness timestamp source

### Decision
Freshness is calculated from client-provided `heartbeatTimestamp`.

### Reasoning
The monitoring client is the source of truth for check execution timing, and this preserves expected stale/unknown behavior.

---

## 2026-03-26: Muninn MVP storage engine

### Decision
Muninn uses SQLite for MVP storage.

### Reasoning
SQLite supports rapid MVP implementation with low operational overhead for both organisational and self-hosted deployments.

---

## 2026-03-26: MVP retention and refresh defaults

### Decision
- MVP status/stat history retention target is 90 days.
- S.W.O.T auto-refresh default is 60 seconds and is configurable.

### Reasoning
These defaults provide practical utility without adding early operational complexity.

---

## 2026-03-27: Heartbeat schema canonical field alignment

### Decision
The canonical heartbeat JSON Schema uses camelCase field names aligned to MVP canonical payload naming. Required fields are:
- `serviceName`
- `instanceName`
- `hostName`
- `version`
- `heartbeatTimestamp`
- `status`

### Reasoning
This removes snake_case/camelCase ambiguity between tickets, delivery docs, and schema artifacts and reduces early integration rework risk for `Huginn` and `Heimdall`.

---

## 2026-03-27: HUG-001 bootstrap auth header convention

### Decision
For current bootstrap implementation work, Huginn sends ingestion authentication using the `X-API-Key` request header.

### Reasoning
The workstream required a concrete header convention to safely begin HUG-001 implementation without blocking on further auth envelope expansion.
