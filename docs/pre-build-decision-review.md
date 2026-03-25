# Heimdall MVP Pre-Build Decision Review

## Executive summary

The current documentation gives a clear MVP intent and stable boundaries (public read-only dashboard, push ingestion, production-only deployments, service + instance visibility, simple thresholds/labels). What is still missing is **implementation-level decision closure** needed to start build execution confidently.

The highest-priority unresolved items are: repository/project structure, concrete technology stack choices, storage technology/schema approach, ingestion authentication, Huginn payload contract/versioning, roll-up + threshold precedence edge cases, and the first API surface.

---

## Decisions already made

### Product/scope decisions already settled
- Platform consists of four components with fixed naming:
  - `S.W.O.T` (dashboard UI)
  - `Heimdall` (API layer)
  - `Muninn` (storage layer)
  - `Huginn` (monitoring client)
- MVP dashboard is public and read-only.
- No dashboard access control and no dashboard audit logging in MVP.
- Deployment model is production-only per deployment (UAT handled as separate deployment).
- Push-based monitoring model.
- Near real-time target (not true real-time streaming).
- Service-level and instance-level visibility are both required.
- Initial configuration model is intentionally simple (thresholds + labels, with branding support).
- Admin UI is out of scope for MVP (direct store/config edits allowed).
- Tenancy boundaries are out of scope for MVP.

### Core behavior decisions already settled
- Base status model includes: `Healthy`, `Degraded`, `Unhealthy`.
- Freshness model includes:
  - 0–60s live
  - 61s–5m stale
  - >5m unknown
- Maintenance is treated as a status effect that can be applied in addition to base status.
- Default roll-up direction is defined at a high level by dimension:
  - base status roll-up: any Unhealthy -> Unhealthy; else any Degraded -> Degraded; else Healthy
  - freshness roll-up: any Unknown -> Unknown; else any Stale -> Stale; else Live
- Platform should support configurable stats + threshold influence.
- Monitoring payload fields are broadly defined (object/instance identity, timestamp, status, labels, stats, message, version/build).

### Non-functional direction already settled
- Keep MVP simple/operable over premature optimisation.
- Support organisational and personal/self-hosted use.
- Target capacity is tens to low hundreds of monitored objects for MVP.

---

## Ambiguities, contradictions, and vague areas

### Naming/documentation consistency gaps
- Path/name mismatches were corrected (`docs/decision-log.md` and `docs/requirements.md` are now canonical).
- The prior `Muninn` contradiction (future-only vs MVP storage layer) has been aligned to MVP storage-layer usage.
- Remaining naming cleanup is mostly terminology consistency (`service` vs `object`).

### Architecture/implementation ambiguity
- No final decision on backend framework/runtime.
- No final decision on frontend stack.
- `Muninn` is “relational direction” but no concrete DB engine is selected.
- No explicit repository layout/solution structure for 4 components.

### Data model/logic ambiguity
- Service vs object terminology is mixed; canonical entity names are not finalized.
- Roll-up handling does not define edge-case precedence (e.g., stale + unhealthy mix, maintenance precedence, unknown handling when partial data exists).
- Threshold semantics are unspecified:
  - whether thresholds affect only instance status or can directly affect service status
  - precedence between explicit client status and threshold-derived status
  - handling of missing/non-numeric stat values.
- History retention window is not finalized (“recent history” is defined conceptually only).

### API/payload ambiguity
- Huginn payload contract exists only as examples + descriptive bullets, not as a versioned API contract (OpenAPI/JSON schema).
- `environment` appears in Huginn sample payloads, but MVP architecture states production-only per deployment; purpose/validation rules are unclear.
- Idempotency and duplicate/out-of-order submission behavior are unspecified.

### Security/ops ambiguity
- Ingestion auth is only “lightweight mechanism” (API key/secret examples) with no final scheme.
- No decision on secret rotation strategy for ingestion credentials.
- Heimdall self-observability requirements (logs/metrics/traces, health endpoints, correlation IDs) are not defined.
- Deployment model is conceptually simple but lacks concrete target (containerized compose, Kubernetes, single host service, etc.).
- Local development workflow is not defined (single-command spin-up, seed data, mock Huginn, etc.).
- Testing strategy exists only implicitly; no agreed test pyramid and CI gate criteria.

---

## Remaining decisions to be made before MVP build

### A. Must be decided before implementation starts

1. **Repository and solution structure**
   - Decide monorepo layout for `S.W.O.T`, `Heimdall`, `Huginn`, shared contracts, and infra.
   - Decide where shared domain contracts live to prevent model drift.

2. **Backend and frontend technology choices**
   - Confirm concrete backend runtime/framework.
   - Confirm frontend framework and rendering model.
   - Confirm API style (REST-first strongly implied; should be formalized).

3. **Storage decision for Muninn**
   - Pick DB engine and migration strategy.
   - Finalize current-state + history schema boundaries.
   - Define indexes/query paths needed for overview/detail refresh patterns.

4. **Configuration storage approach**
   - Finalize what lives in DB vs deployment config files/env vars.
   - Define branding/config change propagation behavior (reload/restart/cache TTL).

5. **Monitoring ingestion authentication**
   - Choose auth mechanism (deployment-level API key vs per-client key).
   - Define key distribution, rotation, revocation, and auditability expectations for MVP.

6. **Monitoring payload contract + versioning**
   - Publish authoritative schema and endpoint contract.
   - Decide required vs optional fields and validation/error codes.
   - Define contract versioning and backward compatibility approach.

7. **Service/instance/check modelling**
   - Canonicalize entity definitions and IDs.
   - Decide whether checks are first-class persisted entities in MVP or only summarized into instance payloads.

8. **Roll-up + threshold rule details**
   - Finalize precedence matrix among explicit status, thresholds, maintenance, stale, unknown.
   - Define deterministic rules for partial/missing data and mixed instance populations.

9. **Initial API surface**
   - Lock first endpoint list for ingestion and dashboard reads.
   - Define pagination/sorting/filtering conventions and response shapes.

10. **Deployment + local dev model**
    - Define MVP deployment reference architecture.
    - Define local developer startup path including sample data + Huginn simulation.

11. **Testing strategy and CI quality gates**
    - Agree minimal required automated tests per component.
    - Define merge gates for contract, unit, and integration coverage.

12. **Heimdall observability for itself**
    - Decide structured logging format, log fields, health/readiness endpoints, and minimal platform metrics.

13. **Huginn per-host/node configuration model**
    - Define static file/env/CLI config shape for endpoint/auth/object+instance mapping/check definitions.
    - Define update/reload behavior and retry/backoff defaults.

### B. Can be deferred until after the first implementation spike

- Advanced retention/cleanup and longer-horizon archival design (keep MVP focused on recent history while validating real write/query patterns first).
- Optional expansion of dashboard branding options beyond the currently documented set.
- Future admin UI details (already explicitly out of MVP scope).
- Future synthetic browser testing through Huginn (already explicitly out of MVP scope).

---

## Risks or assumptions to resolve early

- **Contract drift risk:** without a schema-first payload/API contract, `Huginn`, `Heimdall`, and `S.W.O.T` can diverge quickly.
- **Semantic drift risk:** unresolved roll-up/threshold precedence will produce inconsistent status interpretation.
- **Security risk:** vague ingestion auth can allow accidental weak deployments.
- **Operability risk:** absent local-dev standard and deployment reference will slow team onboarding.
- **Data growth risk:** no explicit retention/cleanup policy may degrade dashboard query performance over time.
- **Naming drift risk:** inconsistent names/paths can cause tooling and documentation confusion before codebase scales.

---

## Recommended order for making remaining decisions

1. **Canonical glossary + naming cleanup** (single source terms for service/object/instance/check, Heimdall spelling).
2. **Repo/solution structure** (enables parallel implementation).
3. **Payload/API contract first** (schema + initial endpoints).
4. **Storage engine + schema + migration tooling** (to support contract-backed persistence).
5. **Auth model for ingestion** (before Huginn integration hardens).
6. **Roll-up/threshold precedence matrix** (before dashboard logic and backend calc logic are implemented).
7. **Config/branding storage split** (DB vs file/env + runtime refresh rules).
8. **Deployment reference + local-dev workflow**.
9. **Testing strategy + CI gates**.
10. **Heimdall self-observability baseline**.
11. **Huginn node configuration profile and defaults**.

#### 1. Canonical glossary and naming
- Canonical terms: `Service`, `Instance`, `Heartbeat`.
- `object` alias is disallowed immediately.
- Product/component names remain: `Heimdall`, `S.W.O.T`, `Huginn`, `Muninn`.

#### 2. Repository and solution structure
- Use a monorepo.
- Use shared packages for cross-component contracts/types.
- `Huginn` and `Muninn` each have their own first-class code directories in the repo.

#### 2a. Backend and frontend technology choices
- Backend stack: .NET (Heimdall API/runtime).
- Frontend stack: native HTML5-first implementation for `S.W.O.T` MVP.
- Rendering approach: keep `S.W.O.T` 100% native as much as possible through development.

#### 3. Payload and API contract
- Protocol: REST + JSON over HTTPS.
- Contract authority: OpenAPI + JSON Schema.
- Versioning: `/api/v1/...` + payload `schema_version`.
- Terminology: use `heartbeats` explicitly.
- MVP ingestion endpoint: `POST /api/v1/heartbeats`.
- Published JSON Schema: `docs/contracts/heartbeat.v1.schema.json`.
- Payload semantics: `status` carries base health (`Healthy|Degraded|Unhealthy`); maintenance is represented by a separate optional `maintenance` boolean effect.

#### 3a. Initial API endpoint list and response contracts (MVP)

| Endpoint | Direction | Purpose | Request body | Success response |
| --- | --- | --- | --- | --- |
| `POST /api/v1/heartbeats` | Push (Huginn -> Heimdall) | Submit heartbeat updates for service/instance state. | `heartbeat.v1` JSON schema | `202 Accepted` + `{ "heartbeat_id": "...", "received_at": "...", "status": "accepted" }` |
| `GET /api/v1/services` | Pull (S.W.O.T <- Heimdall) | List service roll-up status for dashboard overview. | n/a | `200 OK` + paged list of services with roll-up status and freshness metadata. |
| `GET /api/v1/services/{service_id}` | Pull (S.W.O.T <- Heimdall) | Service detail with instance states and latest stats/message. | n/a | `200 OK` + service detail object + instance list. |
| `GET /api/v1/services/{service_id}/history` | Pull (S.W.O.T <- Heimdall) | Recent status/history timeline for one service. | n/a | `200 OK` + time-ordered heartbeat summary records. |
| `GET /api/v1/config/huginn` | Pull (Huginn <- Heimdall) | Pull Huginn node config profile (thresholds/labels/check mappings; no secrets). | n/a | `200 OK` + node-scoped config document + `config_version`. |
| `POST /api/v1/config/events` | Push (operator/automation -> Heimdall) | Push config-change events for cache invalidation/reload coordination. | config event payload (`event_id`, `event_type`, `config_version`) | `202 Accepted` + event receipt object. |

Response/error baseline for all endpoints:
- Correlation ID support via `X-Request-ID` request/response header.
- Standard error envelope: `{ "error": { "code": "...", "message": "...", "details": [] } }`.
- Common error statuses: `400`, `401`, `403`, `404`, `409`, `413`, `422`, `429`, `500`.

#### 3b. Service/instance/check persistence model (MVP)
- `Service` and `Instance` are first-class persisted entities.
- `Heartbeat` events are persisted in history (90-day retention) and folded into current-state tables.
- `Check` is **not** a first-class persisted entity in MVP.
- Check outcomes are represented through heartbeat `status`, `message`, and `stats` fields.
- Future post-MVP option: promote checks to first-class entities only if query/use-cases justify it.

#### 4. Storage engine, schema, migrations
- DB engine: SQLite.
- Migrations: SQL-first migration files in repo.
- Retention: 90 days.
- Model: dedicated current-state tables + heartbeat history.
- Dedupe: client-provided `heartbeat_id` (UUID) with uniqueness enforcement.

#### 5. Ingestion authentication
- Per-Huginn-node API key.
- Static API key header authentication.
- Rotation: manual for MVP.
- Revocation: immediate deny-list behavior.
- Audit: log key fingerprint/ID + source IP + auth outcome.
- Governance: review manual-rotation policy before 1.0 release planning (target review date: 2026-09-01).

#### 6. Roll-up and threshold precedence
- Base health status uses: `Healthy`, `Degraded`, `Unhealthy`.
- `Maintenance` is a **status effect/flag** that can be applied in addition to base health status.
- Freshness (`Live`/`Stale`/`Unknown`) is derived from heartbeat age and tracked independently from base health status.
- Conflict handling: worse-of explicit base status vs threshold-derived base status wins.

Instance evaluation matrix (MVP):

| Step | Condition | Result |
| --- | --- | --- |
| 1 | Determine base health from explicit status and thresholds | `Unhealthy > Degraded > Healthy` (worse wins) |
| 2 | Apply maintenance effect if maintenance mode is set | `maintenance=true` (base status remains) |
| 3 | Compute freshness from timestamp age | `Live` (0-60s), `Stale` (61s-5m), `Unknown` (>5m) |

Service roll-up matrix (MVP):

| Dimension | Rule across instances | Result |
| --- | --- | --- |
| Base service status | Any instance base status is `Unhealthy` -> `Unhealthy`; else any `Degraded` -> `Degraded`; else `Healthy` | `Healthy` / `Degraded` / `Unhealthy` |
| Service maintenance effect | If all instances have maintenance effect, service maintenance effect = true; otherwise false | `maintenance=true|false` |
| Service freshness | If any instance freshness is `Unknown` -> `Unknown`; else any `Stale` -> `Stale`; else `Live` | `Live` / `Stale` / `Unknown` |

Worked examples:
1. **Two instances**: base `Healthy`, base `Unhealthy` (both live) -> service base status `Unhealthy`, freshness `Live`, maintenance `false`.
2. **Two instances**: base `Healthy`, base `Degraded` (both live) -> service base status `Degraded`, freshness `Live`, maintenance `false`.
3. **Two instances**: base `Healthy` (stale), base `Healthy` (live) -> service base status `Healthy`, freshness `Stale`, maintenance `false`.
4. **One instance**: explicit base `Healthy`, threshold-derived base `Unhealthy` -> instance base status `Unhealthy` (worse wins), freshness based on timestamp.
5. **One instance**: base `Healthy`, maintenance mode enabled, stale timestamp -> instance base `Healthy` with `maintenance=true` and freshness `Stale`.
6. **All instances**: base `Healthy` + `maintenance=true` for each -> service base status `Healthy`, service maintenance `true`, freshness from newest/oldest rules above.

#### 7. Config and branding storage split
- Branding in DB.
- Thresholds in DB.
- Secrets in file/env.
- Runtime propagation: TTL cache.
- Default TTL: 5 minutes.

#### 8. Deployment and local dev model
- Deployment reference: Docker Compose on single host/VM.
- Local dev bootstrap: single `docker compose up` path.
- Include deterministic seed/demo data.
- Include Huginn simulation tooling in repo.
- Keep dev/prod shape aligned (resource-scaled differences only).

#### 9. Testing strategy and CI gates
- PR gates: lint + unit + contract tests.
- Integration tests required on PR.
- Minimal UI smoke E2E required on PR.
- No hard global coverage % gate yet; enforce critical-path coverage.
- Breaking schema changes require version bump.

#### 10. Heimdall observability baseline
- JSON structured logs.
- Health endpoints: liveness + readiness.
- Correlation: `X-Request-ID`.
- Metrics endpoint: `/metrics`.
- Logging policy: metadata-only by default; deny payload content logging by default.

#### 11. Huginn node configuration profile/defaults
- Config source: file with env overrides.
- Default heartbeat interval: 60s.
- Retry: exponential backoff + jitter.
- Reload behavior: restart required for MVP.
- Destination outage behavior: keep retrying; do not crash process.

---

## Suggested “ready to build MVP” checklist

Use this as a pre-build gate. MVP build should start only when all items are checked.

- [x] Canonical naming glossary approved (`Heimdall`, `S.W.O.T`, `Muninn`, `Huginn`; entity terms finalized).
- [x] Documentation path inconsistencies resolved or formally mapped.
- [x] Monorepo/solution structure agreed and scaffolded.
- [x] Backend and frontend technology stacks approved.
- [x] Muninn database engine selected with migration strategy.
- [x] Canonical monitoring payload schema (versioned) published.
- [x] Initial API endpoint list and response contracts approved.
- [x] Ingestion authentication mechanism finalized (including rotation plan).
- [x] Roll-up and threshold precedence matrix documented with examples.
- [x] Service/instance/check persistence model finalized.
- [x] Configuration/branding source-of-truth model finalized.
- [x] Retention window for “recent history” finalized.
- [x] Deployment reference topology documented (org + self-hosted friendly).
- [x] Local development bootstrap path documented (including sample data/harness).
- [x] Testing strategy documented with required CI checks.
- [x] Heimdall observability baseline defined (logs/metrics/health endpoints).
- [x] Huginn host/node configuration contract documented.

---

## Practical note

This review intentionally avoids redesigning the architecture. It focuses on **decision closure required to execute the documented MVP** with low ambiguity and minimal rework.
