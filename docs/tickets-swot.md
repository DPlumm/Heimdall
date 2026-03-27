# S.W.O.T MVP Ticket Set

Initial implementation ticket set for the `S.W.O.T` (`Software Well-being Observability Tool`) workstream, aligned to current Heimdall MVP docs and thin vertical-slice delivery.

## Ticket SW-001: Bootstrap S.W.O.T app shell and read-only routing

- **Short description**: Create the S.W.O.T project skeleton with baseline layout, read-only route structure (`/` overview, `/services/:serviceName` detail), shared status/freshness UI primitives, and configuration wiring for API base URL and refresh interval.
- **Why it matters**: Establishes the smallest production-capable UI foundation so subsequent slices can ship value quickly without reworking structure.
- **Dependencies**:
  - Repo/package conventions for frontend app location.
  - Confirmed Heimdall read endpoint paths for overview and detail.
- **Acceptance criteria**:
  - App runs locally and serves a read-only shell page.
  - Overview and detail routes exist with placeholder content.
  - Shared components exist for status badge, freshness badge, and last-updated text.
  - Config supports API base URL and refresh interval (default 60 seconds).
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket SW-002: Overview page vertical slice (live data list)

- **Short description**: Implement overview page that calls Heimdall overview API and renders Service-level rows/cards with serviceName, rolled-up status, freshness, last update time, and latest message.
- **Why it matters**: This is the first usable dashboard outcome and the core MVP value.
- **Dependencies**:
  - SW-001 app shell.
  - Heimdall overview endpoint returning Service summary data.
- **Acceptance criteria**:
  - Overview fetches and displays Service summaries from API.
  - Each Service tile/row shows: `serviceName`, roll-up status, freshness state, last update, latest message (if present).
  - Clicking a Service navigates to detail route.
  - Unknown/missing optional fields are rendered safely.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket SW-003: Loading, error, and empty-state handling for overview/detail

- **Short description**: Add explicit UX states for API pending, failure, and zero-data conditions, reusable across overview and detail views.
- **Why it matters**: Public operational dashboards must fail clearly and predictably; this is essential MVP usability and supportability.
- **Dependencies**:
  - SW-001 shell.
  - SW-002 overview wiring (for first use).
- **Acceptance criteria**:
  - Loading state is visible during API requests.
  - Error state shows user-friendly message and retry action.
  - Empty state is shown when no Services exist.
  - State components are reused on detail page once detail API is added.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket SW-004: Auto-refresh with configurable interval and manual refresh

- **Short description**: Implement periodic refresh for overview/detail using configurable interval (default 60 seconds), plus manual refresh action.
- **Why it matters**: Near real-time visibility is a core MVP requirement.
- **Dependencies**:
  - SW-002 overview API consumption.
  - SW-003 resilient request-state handling.
  - Refresh interval value provided via config.
- **Acceptance criteria**:
  - Overview auto-refreshes at configured interval (default 60 seconds).
  - Detail view (once implemented) uses same refresh mechanism.
  - Manual refresh action triggers immediate refetch.
  - Refresh failures do not break existing rendered data.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket SW-005: Service detail view with Instance status and recent history

- **Short description**: Implement detail page backed by Heimdall detail API showing Service metadata, roll-up status, freshness, Instance list, per-Instance status/freshness, and recent status/stat history.
- **Why it matters**: MVP requires instance-level visibility and detail troubleshooting context.
- **Dependencies**:
  - SW-001 routing and shared UI primitives.
  - Heimdall detail endpoint with Service + Instance + history payload.
  - SW-003 state handling.
- **Acceptance criteria**:
  - Detail route fetches Service data by `serviceName`.
  - Displays current Service roll-up status and freshness clearly.
  - Displays Instance list with per-Instance status and freshness.
  - Displays recent status history and recent stat history sections.
  - Supports auto-refresh behavior from SW-004.
- **Suggested priority**: P0
- **Delivery bucket**: MVP

---

## Ticket SW-006: Label-based filtering and grouping on overview

- **Short description**: Add UI controls to filter and group Services using labels from Heimdall read endpoints.
- **Why it matters**: Label filter/grouping is in MVP scope and improves usability once base overview is working.
- **Dependencies**:
  - SW-002 overview rendering.
  - Heimdall label data availability in overview response or supporting endpoint.
  - Open question closure for label normalization behavior.
- **Acceptance criteria**:
  - User can filter overview list by one or more labels.
  - User can group overview by label value (single selected label key at minimum).
  - Active filters/groups are visibly indicated and resettable.
  - Behavior is deterministic for missing/duplicate label values based on agreed normalization rules.
- **Suggested priority**: P1
- **Delivery bucket**: MVP

---

## Ticket SW-007: Branding/config integration (title, colors, icons)

- **Short description**: Integrate dashboard branding/config values from Heimdall-managed configuration into the S.W.O.T shell and theme.
- **Why it matters**: Configurable branding is an explicit MVP requirement and required for deployment identity.
- **Dependencies**:
  - SW-001 app shell.
  - Heimdall read endpoint(s) for branding/config.
  - Branding source-of-truth behavior from decision log.
- **Acceptance criteria**:
  - Dashboard title/brand text are config-driven.
  - Config-driven colors are applied to theme tokens/variables.
  - Config-driven favicon/icon paths are applied safely.
  - If branding config is unavailable, sensible defaults are rendered.
- **Suggested priority**: P1
- **Delivery bucket**: MVP

---

## Ticket SW-008: Wallboard-friendly layout pass for overview

- **Short description**: Optimize overview layout for passive display (large typography, high-contrast status indicators, dense-but-readable Service grid/list, reduced nonessential chrome).
- **Why it matters**: S.W.O.T is intended for desktop and wallboard use; readability at distance is core utility.
- **Dependencies**:
  - SW-002 overview content.
  - SW-003 state handling (for clear wallboard error visibility).
- **Acceptance criteria**:
  - Overview remains readable at wallboard scale (e.g., 1080p display at distance).
  - Status/freshness indicators remain distinguishable by color + text.
  - Layout is responsive and preserves important Service data above the fold where practical.
  - No regression to normal desktop usability.
- **Suggested priority**: P1
- **Delivery bucket**: MVP

---

## Ticket SW-009: Overview/detail API client contract tests (UI-side)

- **Short description**: Add UI-level contract-focused tests for parsing/rendering Heimdall overview/detail payloads, including stale/unknown freshness and mixed status combinations.
- **Why it matters**: Reduces integration drift and protects thin slices while backend contract evolves.
- **Dependencies**:
  - SW-002 and SW-005 implemented.
  - Stable v1 payload samples/schemas.
- **Acceptance criteria**:
  - Tests cover successful overview/detail payload rendering.
  - Tests cover malformed/partial payload handling and fallback rendering.
  - Freshness and status mapping behaviors are verified for key edge cases.
- **Suggested priority**: P1
- **Delivery bucket**: Post-MVP soon

---

## Ticket SW-010: URL-persisted filter state and shareable views

- **Short description**: Persist selected label filters/grouping in URL query params to support link sharing and wallboard reload continuity.
- **Why it matters**: Improves practical operations use without changing core read-only scope.
- **Dependencies**:
  - SW-006 filtering/grouping.
- **Acceptance criteria**:
  - Selected filters/grouping are reflected in URL query params.
  - Page reload preserves the same filtered/grouped view.
  - Invalid query params degrade gracefully to defaults.
- **Suggested priority**: P2
- **Delivery bucket**: Post-MVP soon

---

## Ticket SW-011: Detail-view historical visualization enhancements

- **Short description**: Improve recent history presentation in detail view (e.g., compact trend visualization and clearer timeline formatting) using existing data.
- **Why it matters**: Enhances troubleshooting speed after MVP baseline is stable.
- **Dependencies**:
  - SW-005 detail history sections.
- **Acceptance criteria**:
  - Recent status/stat history is easier to scan than plain tabular output.
  - Accessibility/readability maintained for color and text contrast.
  - Feature is optional and does not block core detail rendering.
- **Suggested priority**: P3
- **Delivery bucket**: Later

---

## Ticket SW-012: Multi-layout presets (operator vs wallboard)

- **Short description**: Add selectable presentation presets for operator-interactive mode vs passive wallboard mode.
- **Why it matters**: Provides deployment flexibility once base wallboard behavior is proven.
- **Dependencies**:
  - SW-008 wallboard baseline.
  - SW-010 URL/share state (preferred).
- **Acceptance criteria**:
  - At least two layout presets are available and switchable.
  - Wallboard preset minimizes controls and maximizes status visibility.
  - Operator preset preserves faster navigation/filter interaction.
- **Suggested priority**: P3
- **Delivery bucket**: Later

---

## Recommended implementation order

1. **SW-001** — app shell and routing baseline.
2. **SW-002** — first usable overview with live Heimdall data.
3. **SW-003** — robust loading/error/empty states.
4. **SW-004** — auto-refresh + manual refresh.
5. **SW-005** — Service detail with Instance and history visibility.
6. **SW-006** — label filtering/grouping.
7. **SW-007** — branding/config integration.
8. **SW-008** — wallboard-friendly layout pass.
9. **SW-009** — UI-side contract tests.
10. **SW-010** — shareable URL filter/group state.
11. **SW-011** — detail history visualization improvements.
12. **SW-012** — layout presets.

## Biggest unresolved blockers for the S.W.O.T workstream

1. **Heimdall read endpoint finalization** for overview/detail/config shapes and error formats (blocks stable API consumption tickets).
2. **Label normalization decision** (case sensitivity, allowed characters, duplicate handling), which directly affects predictable filter/group behavior.
3. **Threshold configuration shape finalization** (for consistent status interpretation and future detail rendering).
4. **Frontend stack choice and baseline tooling** (noted as open in decision log) if not already settled in codebase; this can block SW-001 start speed.
