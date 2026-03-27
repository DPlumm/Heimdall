# Heimdall Implementation Planning (Lightweight)

Use this document as the reusable planning format for Codex work. Keep plans practical, short, and implementation-focused.

## How to use this file
1. Copy the template section below for each substantial work item.
2. Keep each plan focused on a deployable vertical slice.
3. Link to supporting docs rather than repeating architecture/requirements.
4. Update status as work progresses; remove stale assumptions quickly.

---

## Plan Template

### Plan Title
<!-- Example: MVP Slice 1 - Ingestion to Overview Read Path -->

### Goal
- What working outcome this plan should deliver (implementation outcome, not broad strategy).

### Scope
- In scope (explicit).
- Out of scope (explicit).

### Constraints
- Technical/process constraints that limit implementation choices.
- Include known deployment, compatibility, or operational limits.

### Assumptions
- Assumptions being made to move fast.
- Mark assumptions that need validation later.

### Open Questions
- Questions that block implementation or affect design choices.
- Link to `docs/open-questions.md` items when relevant.

### Vertical Slices / Milestones
- Slice 1: smallest end-to-end usable increment.
- Slice 2: next thin increment.
- Slice 3+: follow-on increments.

### Implementation Order
1. Concrete first step.
2. Next step.
3. Next step.

### Risks
- Delivery risk(s) and practical mitigation.
- Scope creep risk(s) and guardrails.

### Definition of Done
- Clear checks for completion (code, docs, tests, deployability).
- Include "docs aligned" and "no unresolved blockers for next slice".

---

## Starter Plan: Heimdall MVP Delivery

### Goal
- Deliver the smallest usable Heimdall MVP that ingests heartbeat/status data and renders a public read-only S.W.O.T overview and detail view with freshness-aware status.

### Scope
- In scope: first end-to-end ingestion -> storage -> API read -> dashboard read flow.
- Out of scope: admin UI, auth controls, alerting, multi-environment tenancy.

### Constraints
- Must follow existing requirements, architecture, relationships, and decision log.
- Must stay production-only per deployment for MVP.
- Must keep planning and execution lightweight.

### Assumptions
- Initial persistence and API contracts can evolve in early slices if changes remain backwards-compatible for current clients.
- Single deployment context is sufficient for first delivery.

### Open Questions
- See `docs/open-questions.md`.

### Vertical Slices / Milestones
- Slice 1: ingestion contract + persistence of latest instance heartbeat/status.
- Slice 2: service roll-up + overview read API + basic S.W.O.T list view.
- Slice 3: detail API + recent history + label filter/group read path.
- Slice 4: threshold handling + branding config read support.

### Implementation Order
1. Lock minimal ingestion contract and canonical status/freshness behavior.
2. Implement write path and latest-state read path.
3. Add overview dashboard endpoint + minimal S.W.O.T overview screen.
4. Add detail endpoint and recent history rendering.
5. Add threshold/label refinements and branding configuration.

### Risks
- Over-design before first usable slice ships.
- Contract churn between Huginn and Heimdall.
- Delayed UI value if backend slices are too broad.

### Definition of Done
- Thin vertical slice deployed and manually verifiable end-to-end.
- Relevant tests/checks pass for touched components.
- Docs updated (`PLANS.md`, `docs/delivery-plan.md`, `docs/open-questions.md`, and any contract docs touched).

---

## Active Plan: HUG-001 Bootstrap + Monorepo Build Skeleton (2026-03-27)

### Goal
- Deliver the first runnable code baseline with separate .NET solutions for `Heimdall`, `Huginn`, `Muninn`, and `S.W.O.T`, plus a root aggregate solution, while implementing `HUG-001` one-cycle Huginn submit behavior.

### Scope
- In scope:
  - Repository folder layout for all four components.
  - Individual component solution files and root solution file.
  - Minimal Huginn client config-load -> one-cycle -> heartbeat submit path.
  - Contract alignment update for heartbeat schema field naming.
- Out of scope:
  - Full Heimdall ingestion implementation.
  - Muninn schema/migration runtime behavior.
  - S.W.O.T feature-complete UI.

### Constraints
- Keep implementation MVP-thin and aligned to canonical names (`Service`, `Instance`, `Heartbeat`).
- Use `X-API-Key` header convention for current ingestion calls.
- Preserve production-safe logging by avoiding secret output.

### Assumptions
- `.NET` toolchain is available in contributor/dev environments even if unavailable in the current CI sandbox shell.
- Minimal bootstrap projects are acceptable placeholders for non-Huginn components at this slice.

### Open Questions
- No new blockers for this slice after confirming schema naming direction and auth header convention.

### Vertical Slices / Milestones
- Slice 1: Monorepo folder structure + per-component `.sln` scaffolds.
- Slice 2: Huginn one-cycle runner and payload submission.
- Slice 3: Minimal test coverage for Huginn config/payload/logging safeguards.

### Implementation Order
1. Create source/test folder structure and solution files.
2. Implement Huginn bootstrap runtime and models.
3. Add Huginn tests.
4. Align heartbeat contract docs with canonical camelCase fields.
5. Add minimal placeholder projects for Heimdall, Muninn, and S.W.O.T.

### Risks
- Environment may not have `dotnet` CLI for validation in this sandbox.
- Over-expanding placeholder components beyond bootstrap intent.

### Definition of Done
- All four components have independent solution entry points.
- Root solution references all first-pass projects.
- Huginn can perform one config-driven cycle and submit a heartbeat with canonical required fields.
- Documentation reflects current schema naming and build layout direction.
