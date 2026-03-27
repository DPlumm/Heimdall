# Huginn MVP Ticket Set

Initial implementation ticket set for the `Huginn` workstream, aligned to current Heimdall MVP requirements, architecture, service definitions, delivery plan, and open questions.

This set prioritizes the smallest useful monitoring client first, then adds immediate hardening and near-term expansion in thin vertical slices.

## Ticket HUG-001: Bootstrap Huginn client skeleton and one-cycle runner

- **Short description**: Create a minimal Huginn project skeleton with config loading, a single execution cycle, and submission to Heimdall.
- **Why it matters**: Establishes the smallest end-to-end path (`Host -> Huginn -> Heimdall ingest`) and unblocks all follow-on slices.
- **Dependencies**:
  - Confirmed Heimdall ingest endpoint path and auth header convention.
- **Acceptance criteria**:
  - Huginn can start with a local config file and run one monitoring cycle.
  - A single heartbeat payload is built and submitted to Heimdall.
  - Required canonical fields are present: `serviceName`, `instanceName`, `hostName`, `version`, `heartbeatTimestamp`, `status`.
  - Startup and submission logs are emitted with no secrets.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-002: Host/node configuration model (MVP minimal)

- **Short description**: Define and implement a simple per-host configuration model covering endpoint, auth key, Service/Instance identity, loop interval, and enabled checks.
- **Why it matters**: Huginn must be practical to deploy on monitored nodes without an admin UI.
- **Dependencies**:
  - HUG-001.
- **Acceptance criteria**:
  - Config schema supports at least: `serviceName`, `instanceName`, `hostName`, `version`, endpoint URL, API key/secret reference, interval, check list.
  - Config validation fails fast on missing required values.
  - `environment` is optional; if provided in MVP deployments it is `Production`.
  - Includes one documented sample host configuration file.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-003: Check abstraction and execution contract

- **Short description**: Implement a lightweight check abstraction so Huginn can run multiple checks and normalize results into a single internal model.
- **Why it matters**: Enables extensible monitoring targets while keeping MVP implementation simple.
- **Dependencies**:
  - HUG-002.
- **Acceptance criteria**:
  - A check interface/contract exists with deterministic result shape (status, message, optional stats, duration).
  - Huginn can execute multiple configured checks in one cycle.
  - Check failures are isolated; one failed check does not crash the whole cycle.
  - Result model maps cleanly into heartbeat payload construction.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-004: Implement first website/web app HTTP check

- **Short description**: Deliver one concrete HTTP-based check for website/web app targets (status code + response-time baseline).
- **Why it matters**: Provides immediate practical value with the smallest broadly useful check type.
- **Dependencies**:
  - HUG-003.
- **Acceptance criteria**:
  - HTTP/HTTPS target check supports URL, method (GET minimum), timeout, and expected status range.
  - Check outputs Healthy/Degraded/Unhealthy plus message and response-time stat.
  - Timeout/connection failures map to deterministic unhealthy outcomes.
  - Includes unit tests for success, timeout, and non-expected status cases.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-005: Instance health compilation from check results

- **Short description**: Implement instance-level status compilation that reduces check outcomes into one instance result for submission.
- **Why it matters**: Heimdall MVP roll-up depends on coherent instance-level reporting from each Huginn node.
- **Dependencies**:
  - HUG-004.
- **Acceptance criteria**:
  - Compilation rules are deterministic (`Unhealthy` if any check is Unhealthy; else `Degraded` if any check is Degraded; else `Healthy`).
  - Compiled message summarizes primary failure/degradation reason.
  - Compiled output includes heartbeat timestamp and optional aggregated stats.
  - Tests cover mixed-result combinations and no-check edge case behavior.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-006: Canonical heartbeat payload builder

- **Short description**: Build payload construction logic that maps compiled instance health and metadata into the canonical Huginn -> Heimdall submission contract.
- **Why it matters**: Contract consistency prevents integration drift and simplifies Heimdall validation.
- **Dependencies**:
  - HUG-005.
- **Acceptance criteria**:
  - Payload builder emits canonical field names and valid status values.
  - Optional fields (`message`, `labels`, `stats`, `environment`) are included only when configured/available.
  - Payload timestamps are emitted in UTC ISO-8601 format.
  - Golden-file tests validate payload shape against agreed MVP contract examples.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-007: Auth + endpoint configuration and secure submission client

- **Short description**: Implement HTTP submission client with endpoint/auth configuration and secure request defaults.
- **Why it matters**: Ingestion integrity is required even with a public read dashboard.
- **Dependencies**:
  - HUG-006.
- **Acceptance criteria**:
  - Endpoint URL and auth credential source are configurable per host deployment.
  - Submission includes required auth header/token for Heimdall ingest.
  - TLS-enabled endpoints are supported by default.
  - Auth failures are logged as diagnostics without secret leakage.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-008: Retry behavior with bounded backoff and jitter

- **Short description**: Add transient-failure retry handling with bounded exponential backoff and jitter to avoid retry storms.
- **Why it matters**: Required MVP resilience behavior for temporary network/backend interruptions.
- **Dependencies**:
  - HUG-007.
- **Acceptance criteria**:
  - Retries only occur for transient classes (timeouts, 5xx, network errors).
  - Non-retriable failures (e.g., 4xx auth/validation) fail fast.
  - Retry policy has max attempts and backoff ceiling.
  - Tests validate retry/no-retry behavior by status/error category.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-009: Low-overhead periodic execution loop

- **Short description**: Convert one-cycle execution into a lightweight periodic loop with graceful shutdown.
- **Why it matters**: Near real-time monitoring requires continuous operation with safe production footprint.
- **Dependencies**:
  - HUG-008.
- **Acceptance criteria**:
  - Loop interval is config-driven and defaults to an MVP-safe value.
  - Each cycle executes checks, compiles instance health, builds payload, and submits heartbeat.
  - Graceful shutdown stops new cycles and drains in-flight submission.
  - Basic runtime metrics/logs show cycle duration and check counts.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket HUG-010: Logging and diagnostics baseline

- **Short description**: Add structured logging and diagnostic events for startup, config validation, check execution, payload submission, retries, and failures.
- **Why it matters**: Operators need quick troubleshooting signals for distributed host agents.
- **Dependencies**:
  - HUG-009.
- **Acceptance criteria**:
  - Logs are structured and include correlation metadata where available.
  - Sensitive values (API keys/secrets) are always redacted.
  - Failure logs clearly distinguish check failures vs submission failures.
  - Log verbosity is configurable (e.g., info/debug).
- **Suggested priority**: P1
- **Delivery bucket**: MVP

---

## Ticket HUG-011: Sample host packages and quickstart docs

- **Short description**: Provide sample host configuration and minimal runbook for deploying Huginn on a monitored node.
- **Why it matters**: Fast MVP adoption depends on easy first deployment.
- **Dependencies**:
  - HUG-002, HUG-004, HUG-009.
- **Acceptance criteria**:
  - Repository includes at least one production-like sample config for a website/web app check.
  - Quickstart documents required settings, startup command, and expected first heartbeat result.
  - Troubleshooting section covers common misconfiguration/auth/connectivity failures.
- **Suggested priority**: P1
- **Delivery bucket**: MVP

---

## Ticket HUG-012: Windows service/process check for .NET-first hosts

- **Short description**: Add a Windows service/process health check suitable for initial .NET-oriented deployment environments.
- **Why it matters**: Aligns with MVP monitoring targets and .NET-first integration direction.
- **Dependencies**:
  - HUG-003.
- **Acceptance criteria**:
  - Check can verify configured service/process presence and running state.
  - Deterministic mapping to Healthy/Degraded/Unhealthy.
  - Platform-guard behavior is documented (e.g., unsupported OS handling).
  - Includes tests for running, stopped, and not-found scenarios.
- **Suggested priority**: P1
- **Delivery bucket**: Post-MVP soon

---

## Ticket HUG-013: Submission durability option (local spool for outages)

- **Short description**: Add optional lightweight local queue/spool for heartbeats when endpoint connectivity is unavailable beyond retry limits.
- **Why it matters**: Improves resilience for unstable links without changing core MVP contract.
- **Dependencies**:
  - HUG-008.
- **Acceptance criteria**:
  - Failed submissions can be queued locally up to configured size/time limits.
  - Replay logic drains queue when connectivity returns.
  - Queue overflow behavior is explicit and observable.
  - Feature can be disabled for strict low-footprint mode.
- **Suggested priority**: P2
- **Delivery bucket**: Post-MVP soon

---

## Ticket HUG-014: Contract compatibility test pack against Heimdall ingest

- **Short description**: Add integration tests using canonical sample payloads to verify ongoing compatibility with Heimdall ingestion behavior.
- **Why it matters**: Prevents regressions as Huginn and Heimdall evolve in parallel.
- **Dependencies**:
  - HUG-006, Heimdall ingest test environment.
- **Acceptance criteria**:
  - Test suite covers valid payload acceptance and known invalid payload rejection.
  - Includes canonical fields and optional metadata cases.
  - CI-ready execution path exists for local and pipeline runs.
- **Suggested priority**: P2
- **Delivery bucket**: Post-MVP soon

---

## Ticket HUG-015: Synthetic/browser check extension boundary (no implementation yet)

- **Short description**: Define explicit extension seam that separates MVP local checks from later synthetic/browser testing capability.
- **Why it matters**: Keeps MVP small while preventing architecture lock-in before future Playwright-style checks.
- **Dependencies**:
  - HUG-003.
- **Acceptance criteria**:
  - Documentation clearly identifies MVP check set vs deferred synthetic/browser checks.
  - Check abstraction supports adding non-MVP check providers later without breaking current config.
  - No browser runtime dependencies are added in this ticket.
- **Suggested priority**: P3
- **Delivery bucket**: Later

---

## Recommended implementation order

1. **HUG-001** — minimal client skeleton and one-cycle submit.
2. **HUG-002** — host configuration model + validation + sample config.
3. **HUG-003** — check abstraction contract.
4. **HUG-004** — first HTTP website/web app check.
5. **HUG-005** — instance health compilation.
6. **HUG-006** — canonical payload builder.
7. **HUG-007** — auth + endpoint submission client.
8. **HUG-008** — retry policy with bounded backoff.
9. **HUG-009** — periodic low-overhead execution loop.
10. **HUG-010** — structured logging/diagnostics baseline.
11. **HUG-011** — quickstart and sample host package.
12. **HUG-012** / **HUG-013** / **HUG-014** — immediate post-MVP hardening and second check type.
13. **HUG-015** — future synthetic/browser separation guardrail.

## Biggest unresolved blockers for Huginn workstream

1. **Canonical field-name alignment across docs/artifacts**: some existing artifacts still show snake_case payload forms while MVP planning specifies canonical fields (`serviceName`, `instanceName`, `heartbeatTimestamp`); this must be reconciled before contract tests are locked.
2. **Final Heimdall ingest auth/header convention**: Huginn needs exact auth mechanism and header naming to complete submission client behavior without rework.
3. **Exact endpoint and error envelope contract for ingest**: retry classification and diagnostics quality depend on stable status/error response semantics.
4. **Label normalization and threshold schema decisions**: mainly Heimdall/Muninn concerns, but they affect what Huginn should emit/validate for labels/stats in payloads.
