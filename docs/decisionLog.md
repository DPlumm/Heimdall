# Heimdall Decision Log

This document records key product and technical decisions made during the early definition of Heimdall.

---

## 2026-03-24: Product name

### Decision
The product will be called `Heimdall`.

### Reasoning
A project name was chosen so repository structure and documentation can be created around a fixed identity.

---

## 2026-03-24: Product purpose

### Decision
Heimdall will be a public, read-only production status board backed by a lightweight monitoring client.

### Reasoning
The initial requirement is to provide broad visibility of service health and selected stats without introducing write actions or operational control through the dashboard.

---

## 2026-03-24: Dashboard access model

### Decision
The MVP dashboard will have no access controls.

### Reasoning
The board is intended to be openly viewable. The guiding principle is that operational status data should be visible to everyone.

### Notes
This applies to dashboard viewing only and may be revisited later if requirements change.

---

## 2026-03-24: Dashboard auditing

### Decision
The MVP will not include dashboard audit logging.

### Reasoning
The dashboard is read-only and public, so auditing viewer activity is not currently considered necessary.

---

## 2026-03-24: Environment scope

### Decision
Each Heimdall deployment will support production only.

### Reasoning
The product should remain simple in the MVP. If UAT visibility is needed, that can be handled by a separate deployment rather than adding environment complexity into the first version.

---

## 2026-03-24: Monitoring model

### Decision
Heimdall will use a push-based monitoring model.

### Reasoning
A monitoring client attached to services aligns naturally with a push model and supports consistent reporting of health, heartbeat, and stats to a central service.

---

## 2026-03-24: Data freshness model

### Decision
Heimdall will treat monitoring data using the following freshness rules:

- 0 to 60 seconds old: live
- older than 60 seconds and up to 5 minutes: stale but still valid
- older than 5 minutes: unknown

### Reasoning
This gives a near real-time experience without making the board too sensitive to brief interruptions or delayed updates.

---

## 2026-03-24: Real-time requirement

### Decision
The MVP will target near real-time status rather than true real-time streaming.

### Reasoning
Near real-time is sufficient for the intended operational use case and is simpler to implement and operate.

---

## 2026-03-24: Deployment tenancy model

### Decision
The MVP will be a single shared production board with a common configuration model and no tenancy boundaries.

### Reasoning
There is no current need for separate team-level isolation, delegated ownership boundaries, or tenant-specific configuration. These can be introduced later through metadata if needed.

### Notes
Simple metadata such as labels, owner, group, or category can still be used to organise monitored objects.

---

## 2026-03-24: Visibility model

### Decision
Heimdall will support both service-level and instance-level visibility.

### Reasoning
Users need to understand both the overall health of a monitored object and the state of the individual instances that make it up.

### Notes
A logical monitored object will roll up the status of one or more underlying instances.

---

## 2026-03-24: Initial configuration model

### Decision
The MVP configuration model will support thresholds and labels only.

### Reasoning
This provides useful flexibility without introducing complex dashboard customisation too early.

### Notes
More advanced configuration may be added later if there is a clear need.

---

## 2026-03-24: Initial monitoring targets

### Decision
The MVP should support monitoring targets such as:

- web apps
- Windows services
- websites
- servers
- similar runtime or deployable components

### Reasoning
These match the initial intended operational use cases for Heimdall.

---

## 2026-03-24: Dashboard feature scope

### Decision
The dashboard will be read-only in the MVP.

### Reasoning
The product is intended to provide visibility, not control. Operational actions and write capabilities would add complexity and risk that are outside the initial scope.

---

## 2026-03-24: UAT handling

### Decision
UAT should not be included as an environment within the initial Heimdall deployment model.

### Reasoning
If UAT monitoring is required, it can be provided via a separate deployment rather than by broadening the MVP scope.

---

## 2026-03-24: Dashboard product name

### Decision
The dashboard product name will be `S.W.O.T`, which stands for `Software Well-being Observability Tool`.

### Reasoning
This provides a distinct end-user-facing identity for the dashboard, while `Heimdall` remains the project and backend name.

### Notes
`Heimdall` should continue to be used as the repository and backend project name unless a later decision changes this.

---

## 2026-03-24: Dashboard branding configuration

### Decision
Dashboard branding will be configurable through `Web.config`.

### Reasoning
Brand styling should be adjustable without code changes so the dashboard can be branded appropriately for its deployment context.

### Notes
This configuration is intended to include items such as:
- colours
- product branding values
- file paths for favicons
- file paths for branding icons
- similar UI identity settings

---

## 2026-03-24: Dashboard configuration storage model

### Decision
Dashboard configuration will be stored centrally and can be edited directly in the underlying configuration store, with the intention of later exposing this through an admin view.

### Reasoning
The MVP needs configurable dashboard behaviour, but does not require a full end-user administration interface yet.

### Notes
The long-term intention is for end users to manage dashboard configuration through an admin view.
This admin view is explicitly out of scope for the MVP.

---

## 2026-03-24: Long-term monitoring client capability for browser-based testing

### Decision
In the long term, the monitoring client will support running tests in headless browser windows and reporting status based on the results.

### Reasoning
Some monitored systems, especially user-facing web applications, may need status to reflect real user journey checks rather than only service-local health and metrics.

### Notes
Playwright-style browser automation is a likely model for this capability.
This is a future capability and is not part of the MVP.

---

## 2026-03-24: Monitoring client name

### Decision
The monitoring client will be called `Huginn`.

### Reasoning
`Huginn` fits the Norse mythology naming theme already used by Heimdall and is a strong thematic match for a lightweight client that gathers monitoring information and reports it back to the central service.

### Notes
This name should be used for the monitoring client in documentation, code, configuration, and deployment artefacts.

---

## 2026-03-24: Historical data storage component name

### Decision
The name `Muninn` will be reserved for a  future data stroage component related to retained history, analysis, or longer-term operational memory.

### Reasoning
`Muninn` complements `Huginn` within the same Norse mythology theme and is a good fit for a future component focused on memory, history, or analysis rather than active monitoring.

### Notes
This does not introduce any new MVP scope. It only reserves the name for possible future use.

---

## Open items

The following areas are not yet fully decided and should be revisited in later design work:

- backend and frontend technology choices
- storage model and retention periods
- client authentication approach for monitoring submissions
- default reporting cadence
- roll-up rules for instance-to-service status calculation
- approach for monitoring non-.NET targets where no client can be attached directly

---
