# AGENTS.md

Repository-wide instructions for Codex contributors.

## Scope and intent
- Keep delivery MVP-first: prefer thin, end-to-end vertical slices over large horizontal rewrites.
- Use existing docs as source-of-truth before changing code or behavior:
  - `README.md`
  - `docs/requirements.md`
  - `docs/architecture.md`
  - `docs/relationships.md`
  - `docs/*/service-definition.md`
  - `docs/decision-log.md`

## Naming and terminology (must follow)
- `Heimdall` = project and platform name.
- `Heimdall` = API/application layer.
- `S.W.O.T` = user-facing web dashboard (`Software Well-being Observability Tool`).
- `Muninn` = storage layer.
- `Huginn` = monitoring client.
- Canonical domain terms: `Service`, `Instance`, `Heartbeat`.
- Use canonical payload names in planning/docs/examples: `serviceName`, `instanceName`, `heartbeatTimestamp`.
- Do not introduce or reuse `object` in new API/domain docs or code.

## Planning-first workflow
- Before substantial implementation work, create or update:
  - `PLANS.md` (shared planning template/approach)
  - `docs/delivery-plan.md` (current delivery slices)
  - `docs/open-questions.md` (active unresolved decisions only)
- Confirm the next slice is small, shippable, and testable end-to-end.
- Work in this order unless there is a documented reason not to:
  1. define/confirm contract fields and behaviors,
  2. implement storage write/read path,
  3. implement API endpoints over that path,
  4. implement/adjust S.W.O.T read UI against those endpoints.

## Build and test expectations
- If code is changed, run the narrowest relevant build/tests for touched components.
- If no automated tests exist yet for touched areas, add minimal tests where practical or document the gap explicitly.
- For docs-only changes, run lightweight checks (e.g., markdown/link/format checks if available) and ensure consistency with source docs.

## Documentation alignment and review expectations
- Keep docs and code aligned in the same change whenever behavior, contracts, or terminology changes.
- Record decisions that close open questions in `docs/decision-log.md`.
- Avoid speculative design docs; document implemented or immediately planned behavior.
- Keep plans lightweight: implementation-oriented, short horizon, and iteration-friendly.
