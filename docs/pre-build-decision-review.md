# Heimdall MVP Pre-Build Decision Review

## Executive summary

The current documentation gives a clear MVP intent and stable boundaries (public read-only dashboard, push ingestion, production-only deployments, service + instance visibility, simple thresholds/labels). What is still missing is **implementation-level decision closure** needed to start build execution confidently.

The highest-priority unresolved items are: repository/project structure, concrete technology stack choices, storage technology/schema approach, ingestion authentication, Huginn payload contract/versioning, roll-up + threshold precedence edge cases, and the first API surface. There are also naming/document consistency issues (for example `Heimdal` vs `Heimdall`, `decisionLog.md` vs documented `decision-log.md`, and `Requirments` folder spelling) that should be fixed early to avoid drift.

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
- Status model includes: `Healthy`, `Degraded`, `Unhealthy`, `Stale`, `Unknown`, `Maintenance`.
- Freshness windows are defined:
  - 0–60s live
  - 61s–5m stale
  - >5m unknown
- Default roll-up direction is defined at a high level:
  - any Unhealthy -> Unhealthy
  - else any Degraded -> Degraded
  - else any Stale -> Stale
  - else all valid Healthy -> Healthy
  - else no valid data -> Unknown
- Platform should support configurable stats + threshold influence.
- Monitoring payload fields are broadly defined (object/instance identity, timestamp, status, labels, stats, message, version/build).

### Non-functional direction already settled
- Keep MVP simple/operable over premature optimisation.
- Support organisational and personal/self-hosted use.
- Target capacity is tens to low hundreds of monitored objects for MVP.

---

## Ambiguities, contradictions, and vague areas

### Naming/documentation consistency gaps
- Inconsistent naming appears across docs (`Heimdal` and `Heimdall` both used).
- README references `docs/decision-log.md` and `docs/requirements.md`, but repository uses `docs/decisionLog.md` and `docs/Requirments/MVP.md`.
- `Muninn` is described as reserved for future in one decision, but is also documented as an MVP storage layer elsewhere.

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

1. **Repository and solution structure (blocker)**
   - Decide monorepo layout for `S.W.O.T`, `Heimdall`, `Huginn`, shared contracts, and infra.
   - Decide where shared domain contracts live to prevent model drift.

2. **Backend and frontend technology choices (blocker)**
   - Confirm concrete backend runtime/framework.
   - Confirm frontend framework and rendering model.
   - Confirm API style (REST-first strongly implied; should be formalized).

3. **Storage decision for Muninn (blocker)**
   - Pick DB engine and migration strategy.
   - Finalize current-state + history schema boundaries.
   - Define indexes/query paths needed for overview/detail refresh patterns.

4. **Configuration storage approach (blocker)**
   - Finalize what lives in DB vs deployment config files/env vars.
   - Define branding/config change propagation behavior (reload/restart/cache TTL).

5. **Monitoring ingestion authentication (blocker)**
   - Choose auth mechanism (deployment-level API key vs per-client key).
   - Define key distribution, rotation, revocation, and auditability expectations for MVP.

6. **Monitoring payload contract + versioning (blocker)**
   - Publish authoritative schema and endpoint contract.
   - Decide required vs optional fields and validation/error codes.
   - Define contract versioning and backward compatibility approach.

7. **Service/instance/check modelling (blocker)**
   - Canonicalize entity definitions and IDs.
   - Decide whether checks are first-class persisted entities in MVP or only summarized into instance payloads.

8. **Roll-up + threshold rule details (blocker)**
   - Finalize precedence matrix among explicit status, thresholds, maintenance, stale, unknown.
   - Define deterministic rules for partial/missing data and mixed instance populations.

9. **Initial API surface (blocker)**
   - Lock first endpoint list for ingestion and dashboard reads.
   - Define pagination/sorting/filtering conventions and response shapes.

10. **Deployment + local dev model (high priority)**
    - Define MVP deployment reference architecture.
    - Define local developer startup path including sample data + Huginn simulation.

11. **Testing strategy and CI quality gates (high priority)**
    - Agree minimal required automated tests per component.
    - Define merge gates for contract, unit, and integration coverage.

12. **Heimdall observability for itself (high priority)**
    - Decide structured logging format, log fields, health/readiness endpoints, and minimal platform metrics.

13. **Huginn per-host/node configuration model (high priority)**
    - Define static file/env/CLI config shape for endpoint/auth/object+instance mapping/check definitions.
    - Define update/reload behavior and retry/backoff defaults.

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

---

## Suggested “ready to build MVP” checklist

Use this as a pre-build gate. MVP build should start only when all items are checked.

- [ ] Canonical naming glossary approved (`Heimdall`, `S.W.O.T`, `Muninn`, `Huginn`; entity terms finalized).
- [ ] Documentation path inconsistencies resolved or formally mapped.
- [ ] Monorepo/solution structure agreed and scaffolded.
- [ ] Backend and frontend technology stacks approved.
- [ ] Muninn database engine selected with migration strategy.
- [ ] Canonical monitoring payload schema (versioned) published.
- [ ] Initial API endpoint list and response contracts approved.
- [ ] Ingestion authentication mechanism finalized (including rotation plan).
- [ ] Roll-up and threshold precedence matrix documented with examples.
- [ ] Service/instance/check persistence model finalized.
- [ ] Configuration/branding source-of-truth model finalized.
- [ ] Retention window for “recent history” finalized.
- [ ] Deployment reference topology documented (org + self-hosted friendly).
- [ ] Local development bootstrap path documented (including sample data/harness).
- [ ] Testing strategy documented with required CI checks.
- [ ] Heimdall observability baseline defined (logs/metrics/health endpoints).
- [ ] Huginn host/node configuration contract documented.

---

## Practical note

This review intentionally avoids redesigning the architecture. It focuses on **decision closure required to execute the documented MVP** with low ambiguity and minimal rework.
