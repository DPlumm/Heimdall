# Sprint 02 Plan (Week 2)

## Sprint objective
Complete Heimdall core flow and launch first Huginn triage experience.

## Capacity assumptions
- Total capacity: 12 points.
- Planned commitment: 10 points.
- Reserve: 2 points.

## In-scope
- DEL-002 timeline detail view.
- DEL-003 alert intake list and triage filters.

## Out-of-scope
- Acknowledgement history.
- Notification channel integrations.

## Deliverables by component (linked to outcomes)
- **Heimdall:** DEL-002 -> OUT-001
- **Huginn:** DEL-003 -> OUT-002
- **Muninn:** none
- **SWOT:** none

## Cross-component dependencies
- Huginn uses Heimdall event IDs for drill-through links.

## Risks + fallback scope cuts
- RSK-002: Shared UI state complexity.
- Fallback: ship triage without drill-through links.

## Demo script
1. Open alert list.
2. Filter to critical alerts.
3. Open linked timeline event details.

## Exit criteria
- OUT-001 success metric validated.
- First in-app triage path is working.
