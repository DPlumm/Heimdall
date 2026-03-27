# Heimdall MVP Open Questions

Only unresolved decisions that materially affect delivery are listed here.
Resolved decisions must be recorded in `docs/decision-log.md` and reflected in `docs/delivery-plan.md`.

## Must resolve before coding starts
- None currently.

## Can resolve during early implementation
- **Label normalization rules**: case sensitivity, allowed characters, and duplicate handling.
- **Threshold configuration shape**: minimal schema for stat threshold definitions and default behavior when thresholds are missing.

## Can defer until after first usable MVP
- **API versioning policy beyond v1**.
- **Admin configuration UX approach** (UI vs config-only tooling).
- **Authentication/audit model for non-public deployments**.
- **Alerting and notification integration priorities**.
- **Multi-environment and tenancy strategy**.

## 2026-03-27 implementation note
- No additional blocking open questions were introduced while starting `HUG-001`.
- Current implementation direction uses canonical camelCase heartbeat fields and `X-API-Key` for ingestion auth header in the bootstrap client.
