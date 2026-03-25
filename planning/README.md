# Outcome-first MVP Planning System

## Context and explicit assumptions
Because initiative specifics were not supplied, this system is initialized with assumptions optimized for **one engineer** and fast feature delivery:
- Product/initiative: **Heimdall MVP** (single-user ops cockpit).
- MVP horizon: **4 sprints, 1 week each**.
- Team composition/capacity: **1 full-stack engineer (you)** with effective capacity **12 points/sprint**.
- Components/workstreams (named by product surface): **Heimdall**, **Huginn Muninn**, **SWOT**.
- Constraints: limited implementation bandwidth, no dedicated DevOps support, one staging environment.
- Non-goals: infra hardening programs, platform migrations, enterprise admin controls.

## How this system works
1. **Outcomes (`OUT-###`)** are the source of truth.
2. **Deliverables (`DEL-###`)** define concrete product slices.
3. **Tickets (`TKT-###`)** implement deliverables.
4. **DoD** confirms shippable quality.

Rule: if a ticket does not map to a deliverable and outcome, it is not planned.

## Weekly cadence (solo-friendly)
- **Monday (30m):** pick sprint goal + commit top tickets.
- **Mid-week (20m):** replan if blocked or over-capacity.
- **Friday (30m):** demo to yourself/stakeholders + close sprint.

## How to use with Codex (prompt snippets)
### Outcome-to-ticket generation
```text
Given OUT-002 and DEL-004, generate 3-5 implementation tickets using /planning/templates/ticket-template.md with Gherkin acceptance criteria and rollout/rollback notes.
```

### Sprint planning
```text
Plan next sprint using /planning/sprints/sprint-03.md and component backlogs. Keep total estimate <= 12 points and maximize outcome impact.
```

### Mid-sprint replanning
```text
I lost 4 points of capacity this week. Replan sprint scope with smallest feature cuts that still preserve sprint objective.
```

### Retro synthesis
```text
Using completed tickets and /planning/metrics/mvp-success-metrics.md, summarize what shipped, what slipped, and top 3 improvements for next sprint.
```
