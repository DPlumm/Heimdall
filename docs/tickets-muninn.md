# Muninn MVP Ticket Set

## Purpose

This ticket set defines the initial implementation backlog for the `Muninn` workstream, aligned to current Heimdall MVP documentation and decisions.

The sequencing prioritises thin vertical slices that unlock end-to-end value quickly (`Huginn -> Heimdall -> Muninn -> Heimdall -> S.W.O.T`).

---

## Ticket 1: Confirm and codify SQLite + SQL migration baseline for Muninn

- **Short description**
  - Create the initial Muninn persistence bootstrap using SQLite and SQL-first migrations in-repo, with a deterministic migration runner contract for local/dev/prod usage.
- **Why it matters**
  - MVP write/read slices depend on predictable schema creation and evolution. This is the foundation for all other Muninn tickets.
- **Dependencies**
  - Existing architecture and decision-log alignment only (no implementation dependency).
- **Acceptance criteria**
  - SQLite is configured as the active Muninn storage engine for MVP runtime.
  - Migration folder/versioning convention is documented and implemented.
  - First migration can be applied to an empty DB and re-run safely (idempotent behavior via migration tracking table).
  - Developer instructions exist for applying/resetting migrations locally.
- **Suggested priority**
  - P0
- **Phase**
  - MVP

---

## Ticket 2: Define and implement core entity schema (`Service`, `Instance`, `Heartbeat` history)

- **Short description**
  - Implement initial relational schema for canonical entities: services, instances, and heartbeat history with deduplication on `heartbeat_id`.
- **Why it matters**
  - This is the smallest schema that supports both current-state folding and timeline/history reads.
- **Dependencies**
  - Ticket 1.
- **Acceptance criteria**
  - Tables exist for `services`, `instances`, and `heartbeat_history` (naming can vary by code conventions, but semantics must match).
  - `heartbeat_history.heartbeat_id` uniqueness is enforced.
  - Foreign-key relationships support `instance -> service` and `heartbeat -> instance`.
  - Required heartbeat fields from MVP contract are persistable (`serviceName`, `instanceName`, `hostName`, `version`, `heartbeatTimestamp`, `status`; plus optional `message`, `labels`, `stats`, `maintenance`, `environment` ignored by behavior but safely storable if included).
- **Suggested priority**
  - P0
- **Phase**
  - MVP

---

## Ticket 3: Implement current-state projection tables and upsert path

- **Short description**
  - Add dedicated current-state tables (or equivalent materialized projection model) for latest per-instance and per-service state, updated from accepted heartbeat writes.
- **Why it matters**
  - Overview/detail dashboard queries should read “latest state” cheaply without scanning full history each refresh cycle.
- **Dependencies**
  - Ticket 2.
- **Acceptance criteria**
  - Latest per-instance state is persisted and queryable in one read.
  - Latest per-service roll-up input state is persisted/queryable (direct table or deterministic view/query).
  - Duplicate `heartbeat_id` writes do not corrupt or regress current-state rows.
  - Update behavior preserves last-seen timestamp and latest message/stat snapshot semantics.
- **Suggested priority**
  - P0
- **Phase**
  - MVP

---

## Ticket 4: Add minimal label/stat persistence needed for MVP filter + threshold reads

- **Short description**
  - Implement minimal storage shape for labels and stats attached to heartbeats/current state, sufficient for MVP filtering/grouping and threshold evaluation.
- **Why it matters**
  - Labels and stats are part of MVP functionality; storage must support overview filtering and detail rendering without over-modeling.
- **Dependencies**
  - Tickets 2-3.
- **Acceptance criteria**
  - Labels from heartbeat payloads are persisted in a queryable form.
  - Latest stats per instance are persisted in current-state form.
  - History rows retain status/stat snapshots for timeline reads.
  - Storage shape is documented with explicit MVP constraints (no first-class check entity).
- **Suggested priority**
  - P1
- **Phase**
  - MVP

---

## Ticket 5: Create overview/detail query indexes and baseline query contracts

- **Short description**
  - Add targeted indexes and data-access query contracts to support fast MVP reads for service overview and service detail endpoints.
- **Why it matters**
  - Near real-time dashboard refresh requires predictable query performance even at “tens to low hundreds” of services.
- **Dependencies**
  - Tickets 2-4.
- **Acceptance criteria**
  - Indexes are added for common access paths (service lookup, instance lookup by service, recent history by service/time window, label filter path).
  - SQL/query layer for `GET /api/v1/services`, `GET /api/v1/services/{service_id}`, and `GET /api/v1/services/{service_id}/history` is documented and implemented.
  - Query plans are reviewed with representative sample data and no obvious full-table scan on hot paths.
- **Suggested priority**
  - P1
- **Phase**
  - MVP

---

## Ticket 6: Persist configuration storage for thresholds and label metadata

- **Short description**
  - Create Muninn configuration tables/read model for MVP configuration items: threshold definitions and label metadata/rules required by Heimdall reads.
- **Why it matters**
  - Threshold behavior and label grouping are MVP features and need central persisted configuration.
- **Dependencies**
  - Ticket 1.
  - Clarification input from open questions on threshold shape/label normalization.
- **Acceptance criteria**
  - Config tables exist for threshold definitions and label config.
  - Heimdall can read config values from Muninn without admin UI dependency.
  - Schema supports config version tracking for change propagation.
  - Any unresolved normalization decisions are explicitly captured as temporary defaults.
- **Suggested priority**
  - P1
- **Phase**
  - MVP

---

## Ticket 7: Persist branding configuration model for S.W.O.T identity values

- **Short description**
  - Implement centralized branding persistence for dashboard title, color values, favicon/icon paths, and related identity settings.
- **Why it matters**
  - Branding is in MVP scope and should be centrally managed/readable, even without admin UI.
- **Dependencies**
  - Ticket 1.
- **Acceptance criteria**
  - Branding table(s) and read contract are implemented.
  - Required fields for MVP branding are storable and retrievable in one config read path.
  - Secrets are not stored in branding tables.
- **Suggested priority**
  - P2
- **Phase**
  - MVP

---

## Ticket 8: Implement retention execution for 90-day history window

- **Short description**
  - Add retention job/command to enforce 90-day status/stat history retention in Muninn history tables.
- **Why it matters**
  - Retention is a documented MVP target; without enforcement, storage growth is uncontrolled.
- **Dependencies**
  - Tickets 2-4.
- **Acceptance criteria**
  - A documented retention execution path exists (startup job, scheduled command, or maintenance task).
  - Records older than 90 days are removed from history tables safely.
  - Current-state tables are unaffected by history pruning.
  - Local test demonstrates retention behavior on seeded old records.
- **Suggested priority**
  - P2
- **Phase**
  - MVP

---

## Ticket 9: Local development DB bootstrap for Muninn (compose + seed)

- **Short description**
  - Provide local development bootstrap for Muninn DB creation, migration application, and deterministic seed/demo records aligned with MVP flows.
- **Why it matters**
  - Fast iteration on end-to-end slices requires one-command local setup with predictable data.
- **Dependencies**
  - Tickets 1-3.
- **Acceptance criteria**
  - Local bootstrap path creates/opens SQLite DB, applies migrations, and loads deterministic sample data.
  - Seed data includes at least one service with multiple instances and history rows.
  - Developer documentation covers reset/reseed workflow.
- **Suggested priority**
  - P1
- **Phase**
  - MVP

---

## Ticket 10: Post-MVP storage hardening for history query ergonomics

- **Short description**
  - Add pagination/windowing and optimized query patterns for larger historical reads beyond initial MVP detail needs.
- **Why it matters**
  - Prevents early technical debt as historical usage grows immediately after first release.
- **Dependencies**
  - Tickets 5 and 8.
- **Acceptance criteria**
  - Time-window + pagination query support added for service history.
  - Index adjustments validated against realistic sample volumes.
  - API read latency remains acceptable under expanded history usage.
- **Suggested priority**
  - P2
- **Phase**
  - Post-MVP soon

---

## Ticket 11: Post-MVP migration safety and backup/restore runbook

- **Short description**
  - Implement migration rollback guidance, backup/restore scripts, and operator runbook for Muninn schema evolution.
- **Why it matters**
  - As deployments mature, safe schema change operations become critical for reliability.
- **Dependencies**
  - Ticket 1 and first production-like deployment feedback.
- **Acceptance criteria**
  - Documented backup-before-migrate flow.
  - Restore procedure tested against a sample migrated DB.
  - Migration failure playbook exists.
- **Suggested priority**
  - P3
- **Phase**
  - Post-MVP soon

---

## Ticket 12: Later optional model expansion for first-class check entities

- **Short description**
  - Evaluate and, if justified, introduce first-class `Check` persistence separate from heartbeat summaries.
- **Why it matters**
  - Could unlock richer diagnostics if future requirements need check-level queryability.
- **Dependencies**
  - Product confirmation that check-level views are needed.
- **Acceptance criteria**
  - Decision memo confirming need and MVP compatibility strategy.
  - Schema/API impact documented before implementation.
  - No regression to existing service/instance heartbeat paths.
- **Suggested priority**
  - P4
- **Phase**
  - Later

---

## Recommended implementation order

1. Ticket 1 - SQLite + migrations baseline.
2. Ticket 2 - Core schema (`Service`, `Instance`, `Heartbeat` history).
3. Ticket 3 - Current-state projection/upsert path.
4. Ticket 5 - Overview/detail query contracts + indexes (minimum set).
5. Ticket 9 - Local DB bootstrap + deterministic seed data.
6. Ticket 4 - Labels/stats storage refinements for filter + threshold behavior.
7. Ticket 6 - Configuration storage (thresholds + label metadata).
8. Ticket 7 - Branding configuration storage.
9. Ticket 8 - Retention enforcement.
10. Tickets 10-11 - immediate post-MVP hardening.
11. Ticket 12 - deferred/later capability.

## Biggest unresolved blockers for the Muninn workstream

1. **Threshold configuration schema details are still open** (shape/default behavior), which can block final configuration table design (Ticket 6).
2. **Label normalization rules are still open** (case sensitivity/duplicates/allowed characters), which affects label storage uniqueness and query strategy (Tickets 4 and 6).
3. **Exact retention execution mechanism is not yet specified** (runtime scheduler vs operator task), which affects operational ownership for Ticket 8.

These blockers do not prevent starting MVP storage implementation; they mostly affect configuration and polish tickets and should be resolved during early slices.
