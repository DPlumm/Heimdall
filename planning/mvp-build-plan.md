# MVP Build Plan

## Build strategy
Ship visible user value every sprint with thin vertical slices.

## Outcome sequencing
1. **OUT-001:** User can see events in one timeline.
2. **OUT-002:** User can triage and acknowledge critical alerts quickly.
3. **OUT-003:** User can view/export weekly SLA summary.
4. **OUT-004:** User can configure basic preferences to reduce manual steps.

## Deliverable map
| Deliverable | Outcome | Component | Target Sprint |
|---|---|---|---|
| DEL-001 | OUT-001 | Heimdall | Sprint 01 |
| DEL-002 | OUT-001 | Heimdall | Sprint 02 |
| DEL-003 | OUT-002 | Huginn | Sprint 02 |
| DEL-004 | OUT-002 | Muninn | Sprint 03 |
| DEL-005 | OUT-003 | SWOT | Sprint 03 |
| DEL-006 | OUT-003 | SWOT | Sprint 04 |
| DEL-007 | OUT-004 | Heimdall | Sprint 04 |
| DEL-008 | OUT-004 | Huginn | Sprint 04 |
| DEL-009 | OUT-004 | Muninn | Sprint 04 |

## Capacity guardrails
- Capacity: **12 points/sprint**.
- Commit max: **10 points**.
- Reserve: **2 points** for bug fixes/unplanned work.

## Scope-cut ladder (if over capacity)
1. Cut polish/UI nice-to-haves.
2. Cut secondary filters and exports.
3. Keep only core happy-path flows for each outcome.
