# Heimdall MVP Ticket Set (Initial)

This ticket set is scoped to the **Heimdall** workstream (API and application layer) and is aligned to the current MVP docs and decision log. It prioritizes thin vertical slices that produce end-to-end usable value early.

## Ticket HMD-01: Scaffold Heimdall API solution skeleton with contract-first baseline
- **Short description**: Create the Heimdall API project skeleton with route versioning (`/api/v1`), contract loading, SQLite connectivity wiring, structured logging, health/readiness endpoints, and correlation ID support.
- **Why it matters**: Establishes the minimum operational foundation for all MVP slices and prevents parallel work from diverging.
- **Dependencies**:
  - None.
- **Acceptance criteria**:
  - Repository contains a runnable Heimdall API skeleton with `/health/live`, `/health/ready`, and `/metrics` endpoints.
  - Request/response correlation via `X-Request-ID` is implemented.
  - Structured JSON logging is enabled with metadata-only defaults.
  - SQLite connection and migration bootstrap are wired (no business tables required yet).
  - CI can run baseline lint/unit/contract checks for Heimdall.
- **Suggested priority**: P0
- **Phase**: MVP

## Ticket HMD-02: Implement heartbeat ingestion endpoint with auth and schema validation
- **Short description**: Implement `POST /api/v1/heartbeats` to accept Heartbeat submissions from Huginn, enforce API key auth, validate contract fields, and return accepted/rejected outcomes.
- **Why it matters**: This is the start of the thinnest end-to-end path; without ingestion there is no live platform behavior.
- **Dependencies**:
  - HMD-01
- **Acceptance criteria**:
  - Endpoint exists at `POST /api/v1/heartbeats`.
  - Per-node API key auth is enforced via header.
  - Required contract fields are validated, including canonical MVP fields (`serviceName`, `instanceName`, `hostName`, `version`, `heartbeatTimestamp`, `status`).
  - `environment` is optional and ignored by MVP ingest logic.
  - Invalid payloads/auth failures return standard error envelope with correct status codes.
  - Accepted payload returns `202 Accepted` with receipt metadata.
- **Suggested priority**: P0
- **Phase**: MVP

## Ticket HMD-03: Persist heartbeat write path (current state + history + dedupe)
- **Short description**: Store accepted Heartbeats in Muninn-backed SQLite tables for both latest state and history, with idempotent dedupe on heartbeat identifier.
- **Why it matters**: Converts ingestion into durable platform state and enables dashboard reads.
- **Dependencies**:
  - HMD-02
- **Acceptance criteria**:
  - Schema includes current-state and history tables for Service/Instance/Heartbeat outcomes.
  - Inserts enforce dedupe via unique heartbeat identity.
  - Latest per-instance state is updated atomically with history append.
  - Retention policy hooks for 90-day history are present (manual/background execution acceptable for MVP).
  - Integration tests confirm duplicate submissions do not create duplicate history rows.
- **Suggested priority**: P0
- **Phase**: MVP

## Ticket HMD-04: Freshness evaluation engine (Live/Stale/Unknown)
- **Short description**: Implement freshness calculation from `heartbeatTimestamp` using the defined windows and expose freshness metadata in stored/derived state.
- **Why it matters**: Freshness is core to MVP correctness and user trust in status meaning.
- **Dependencies**:
  - HMD-03
- **Acceptance criteria**:
  - Freshness derived from heartbeat age using rules: 0-60s Live, 61s-5m Stale, >5m Unknown.
  - Evaluation consistently applied in both overview and detail read paths.
  - Edge-case tests cover boundary timestamps.
  - Unknown/stale transition behavior preserves last known base status data.
- **Suggested priority**: P0
- **Phase**: MVP

## Ticket HMD-05: Service roll-up computation from Instance state
- **Short description**: Implement deterministic roll-up from Instance state to Service state, including base status, freshness, and maintenance effect behavior.
- **Why it matters**: Service-level status is the main dashboard signal and must be consistent across endpoints.
- **Dependencies**:
  - HMD-04
- **Acceptance criteria**:
  - Roll-up logic follows documented MVP precedence for base status and freshness.
  - Maintenance effect handling is implemented per decision-log behavior.
  - Mixed-instance and partial-data scenarios are covered by tests.
  - Roll-up output is stored or computed consistently for all read endpoints.
- **Suggested priority**: P0
- **Phase**: MVP

## Ticket HMD-06: Dashboard overview read endpoint for S.W.O.T
- **Short description**: Implement `GET /api/v1/services` returning the overview dataset (service status, freshness, last update, message, labels, selected stats).
- **Why it matters**: Delivers the first end-user value by enabling a public read-only S.W.O.T overview.
- **Dependencies**:
  - HMD-05
- **Acceptance criteria**:
  - Endpoint returns paged service list with roll-up state and freshness metadata.
  - Includes fields needed for MVP overview display and auto-refresh use.
  - Supports deterministic sorting and stable pagination defaults.
  - Response includes standard correlation/error behavior.
- **Suggested priority**: P0
- **Phase**: MVP

## Ticket HMD-07: Service detail and recent history endpoints
- **Short description**: Implement `GET /api/v1/services/{serviceId}` and `GET /api/v1/services/{serviceId}/history` for instance-level detail and short recent timeline.
- **Why it matters**: Completes MVP read usefulness beyond simple overview and enables root-cause triage.
- **Dependencies**:
  - HMD-06
- **Acceptance criteria**:
  - Detail endpoint returns service metadata and instance list with per-instance status/freshness.
  - History endpoint returns recent time-ordered status/stat summary records.
  - 404 behavior for unknown service IDs follows standard error envelope.
  - Query performance remains acceptable for tens to low hundreds of Services.
- **Suggested priority**: P1
- **Phase**: MVP

## Ticket HMD-08: Payload/domain normalization and canonical modelling guardrails
- **Short description**: Add mapping/validation guardrails to keep canonical domain terms (`Service`, `Instance`, `Heartbeat`) and canonical payload field usage consistent internally.
- **Why it matters**: Prevents terminology drift and avoids long-term schema/API inconsistencies.
- **Dependencies**:
  - HMD-02
- **Acceptance criteria**:
  - Ingestion path normalizes to canonical domain model.
  - New internal/read models avoid `object` naming.
  - Contract tests verify canonical field behavior and backward-safe error handling.
  - Documentation comments and examples in Heimdall code follow canonical terminology.
- **Suggested priority**: P1
- **Phase**: MVP

## Ticket HMD-09: Threshold evaluation for configured stats
- **Short description**: Implement threshold evaluation in Heimdall so stat thresholds can influence base status according to documented precedence.
- **Why it matters**: Threshold support is required MVP behavior and drives useful signal quality.
- **Dependencies**:
  - HMD-05
  - Threshold configuration shape clarification from open questions (can be resolved during this ticket).
- **Acceptance criteria**:
  - Threshold rules can evaluate high/low warning/critical settings for configured stats.
  - Conflict resolution between explicit status and threshold-derived status uses "worse wins" behavior.
  - Missing/non-numeric stat values have deterministic handling.
  - Tests cover healthy/degraded/unhealthy threshold transitions.
- **Suggested priority**: P1
- **Phase**: MVP

## Ticket HMD-10: Label filtering/grouping support on read endpoints
- **Short description**: Add label-aware query/filter support for dashboard read endpoints and apply label normalization rules.
- **Why it matters**: Label-based filtering/grouping is an explicit MVP requirement for usability at scale.
- **Dependencies**:
  - HMD-06
  - Label normalization rules from open questions (can be resolved during this ticket).
- **Acceptance criteria**:
  - `GET /api/v1/services` supports label filter parameters.
  - Response includes label metadata required by S.W.O.T grouping/filter UI.
  - Duplicate/case-variant label handling follows agreed normalization rules.
  - Tests verify query behavior with multiple labels.
- **Suggested priority**: P1
- **Phase**: MVP

## Ticket HMD-11: Configuration and branding exposure endpoints for S.W.O.T
- **Short description**: Provide read endpoints for dashboard configuration and branding values stored centrally (no admin write UI).
- **Why it matters**: Required for deployment-specific dashboard identity and centrally managed presentation behavior.
- **Dependencies**:
  - HMD-01
  - Config source-of-truth split in decision log.
- **Acceptance criteria**:
  - Heimdall exposes read API for branding/config values needed by S.W.O.T.
  - Response includes versioning/cache metadata suitable for 5-minute TTL behavior.
  - Sensitive config/secrets are excluded from responses.
  - Tests confirm config changes are reflected after cache expiry/reload behavior.
- **Suggested priority**: P1
- **Phase**: MVP

## Ticket HMD-12: Ingestion write-protection hardening and auth observability
- **Short description**: Harden ingestion security basics with deny-list revocation path, auth audit logs, and key fingerprint tracking.
- **Why it matters**: Public-read/private-write safety depends on robust lightweight ingestion protection.
- **Dependencies**:
  - HMD-02
- **Acceptance criteria**:
  - API key deny-list path is implemented and takes effect immediately.
  - Auth outcome logs include key fingerprint/ID and source IP metadata.
  - Unauthorized/forbidden requests produce safe error responses without leaking secrets.
  - Regression tests cover valid, revoked, missing, and malformed credentials.
- **Suggested priority**: P1
- **Phase**: MVP

## Ticket HMD-13: Heimdall operational observability baseline completion
- **Short description**: Complete MVP operational observability with ingestion/read latency metrics, request/result dimensions, and dashboard-safe health readiness behavior.
- **Why it matters**: Required to operate Heimdall reliably and diagnose failures quickly.
- **Dependencies**:
  - HMD-01
  - HMD-06
- **Acceptance criteria**:
  - `/metrics` includes ingestion and read latency metrics.
  - Health endpoints reflect dependency/readiness state (including storage).
  - Structured logs include endpoint, status code, correlation ID, and timing.
  - No payload content logging by default.
- **Suggested priority**: P1
- **Phase**: MVP

## Ticket HMD-14: Contract hardening and compatibility test pack
- **Short description**: Strengthen API contract verification with schema compatibility tests, negative cases, and documented versioning guardrails for post-MVP iteration.
- **Why it matters**: Reduces integration breakage risk as Huginn and S.W.O.T evolve.
- **Dependencies**:
  - HMD-02 through HMD-07
- **Acceptance criteria**:
  - Contract test suite validates success + failure payloads against v1 schema.
  - Backward-compatibility checks exist for non-breaking contract updates.
  - Breaking-change checklist documented for future v2 planning.
- **Suggested priority**: P2
- **Phase**: Post-MVP soon

## Ticket HMD-15: History query ergonomics (time windows + pagination)
- **Short description**: Improve history query APIs with explicit time range filters, pagination controls, and response-size safeguards.
- **Why it matters**: Needed soon after MVP for practical troubleshooting at higher data volumes.
- **Dependencies**:
  - HMD-07
- **Acceptance criteria**:
  - History endpoints accept validated `from`/`to` window parameters.
  - Pagination and ordering are explicit and documented.
  - Response-size guardrails prevent expensive unbounded queries.
- **Suggested priority**: P2
- **Phase**: Post-MVP soon

## Ticket HMD-16: Config event ingestion endpoint for cache invalidation
- **Short description**: Implement `POST /api/v1/config/events` for operational automation to signal config version changes and trigger controlled reload/invalidation.
- **Why it matters**: Enables low-friction config operations before any future admin UI.
- **Dependencies**:
  - HMD-11
- **Acceptance criteria**:
  - Endpoint accepts config event payload with event and config version identifiers.
  - Valid events trigger cache invalidation/reload workflow safely.
  - Audit logging exists for config event acceptance/rejection.
- **Suggested priority**: P3
- **Phase**: Later

## Recommended implementation order
1. HMD-01 - API skeleton and operational baseline.
2. HMD-02 - Ingestion endpoint with auth + validation.
3. HMD-03 - Durable write path and dedupe.
4. HMD-04 - Freshness engine.
5. HMD-05 - Service roll-up logic.
6. HMD-06 - Overview read endpoint (first full vertical slice for S.W.O.T).
7. HMD-07 - Detail + history endpoints.
8. HMD-12 - Ingestion write-protection hardening.
9. HMD-09 - Threshold evaluation.
10. HMD-10 - Label filtering/grouping.
11. HMD-11 - Config/branding exposure.
12. HMD-13 - Observability completion.
13. HMD-14 - Contract hardening pack.
14. HMD-15 - History ergonomics.
15. HMD-16 - Config event ingestion.

## Biggest unresolved blockers for Heimdall workstream
1. **Threshold configuration shape details** (open question): without this, threshold evaluation implementation can proceed only with interim assumptions.
2. **Label normalization rules** (open question): filtering/grouping behavior needs explicit case/duplicate rules to avoid UI/API mismatch.
3. **Identifier conventions for read endpoints** (`serviceId` derivation and stability): the docs define domain entities clearly, but deterministic ID conventions must be locked before broad client integration.
4. **Potential contract naming mismatch risk**: canonical planning names use `serviceName`/`instanceName`/`heartbeatTimestamp`, while current schema artifacts may still contain earlier naming forms; this should be reconciled before full integration test hardening.
