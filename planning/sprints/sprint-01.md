# Sprint 01 Plan (Week 1)

## Sprint objective
Ship first usable Heimdall slice.

## Capacity assumptions
- Total capacity: 12 points.
- Planned commitment: 10 points.
- Reserve: 2 points.

## In-scope
- DEL-001 initial timeline page.
- Base event list rendering.
- Minimal filter (severity).

## Out-of-scope
- Advanced search syntax.
- Export actions.

## Deliverables by component (linked to outcomes)
- **Heimdall:** DEL-001 -> OUT-001
- **Huginn Muninn:** API stub only for future sprint -> OUT-002
- **SWOT:** none

## Cross-component dependencies
- Event schema must be stable enough for Huginn Muninn integration in Sprint 02.

## Risks + fallback scope cuts
- RSK-001: Underestimated UI integration effort.
- Fallback: remove secondary filter and ship list + severity only.

## Demo script
1. Load timeline.
2. Filter by severity.
3. Verify newest events appear first.

## Exit criteria
- Timeline renders test dataset consistently.
- Severity filter works.
- DEL-001 accepted.
