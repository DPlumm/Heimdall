# Sprint 03 Plan (Week 3)

## Sprint objective
Improve Huginn Muninn speed and deliver first SLA summary view.

## Capacity assumptions
- Total capacity: 12 points.
- Planned commitment: 10 points.
- Reserve: 2 points.

## In-scope
- DEL-004 alert inbox filtering + ack history.
- DEL-005 weekly SLA summary view.

## Out-of-scope
- Custom report builder.
- Multi-format exports.

## Deliverables by component (linked to outcomes)
- **Huginn Muninn:** DEL-004 -> OUT-002
- **SWOT:** DEL-005 -> OUT-003
- **Heimdall:** bug fixes only

## Cross-component dependencies
- SLA summary derives from acknowledged alert timestamps.

## Risks + fallback scope cuts
- RSK-003: Data consistency between alert and SLA summaries.
- Fallback: ship SLA view without historical comparison.

## Demo script
1. Filter alerts to critical only.
2. Show acknowledgement history.
3. Open weekly SLA summary page.

## Exit criteria
- OUT-002 target trend is met.
- SLA summary shows current week values.
