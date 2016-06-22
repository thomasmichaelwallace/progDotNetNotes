# Patterns of High Availability

https://github.com/iancooper/Availability-Tutorial

## Defining Microserivces
- Typically these days have monolithic central system (functionality), with _adapters_ to allow sections to be replaced.
- However this a) doesn't scale; b) has to be compiled as a single unit.
- Microserivces suggests we break monoliths into pieces (embrace Conways law); but still keep the adapters, etc.
- However it allows you to deploy separately- allows scaling software _teams_; but also agility.
- Encourages API Gateways (however beware the temptation to put anything but cross-cutting concerns in them; do not lock in business logic.)
- Introduces a interprocess communication problem.
- Movements can be summarised as SOA 3.0 / Gerrilla SOA++ (see Martin Fowler); or Extreme SOA or $\mu$icroserices (Fred George).

## Defining Availability
- A fault is a defect within the system; left unmitigated they will lead to failure.
- An error is a deviation of the specification. **Can** be caused by a fault.
- Thus mitigating a fault can prevent errors.
- Dormant faults are those that lie in waiting; e.g. only under unexpected load; resource leaks; concurrency.
- Failure is an error that is observed. An unmitigated error is the cause of a failure.
- Availability is the inverse of downtime a year; normally measure in count of nines (99.[n9]%)

## Tactics
### Fault Prevention
- [B/T]DD, pair reviews, acceptance tests, exploratory testing.
- Training (awareness of where to look)
- But many faults will slip through- _especially_ dormant faults.
- Dormant faults can be waked; e.g. trailing concurrency; monitoring for resource leaks.
- Or mentally talking through issues.
- Beware of memory fragmentation (.net can _now_ compact, but this is a GC.pause- better to recycle).
- However you _must_ accept that **faults will happen**
- Continuously deploy- target daily releases. This means you're releasing in small units, which means pin pointing bugs is easy because you can match it to a release. This is blocked by the monolith pattern as there's too much to do- but small microservice teams can typically achieve this.

### Fault Detection
- Logging! You can't manage what you cannot measure.
- Forensic- after the incident.
- APM (application performance Management); gives you a real time view of the system.

### Fault recovery
- System must stop chain failure (progressive collapse).
- You cannot stop faults- so survive them.
- _Resilience_ is the only solution.
- e.g. introducing new nodes, falling back to behaviour that doesn't need currently failing service.

## Key Issues

### Integration Points
- The fallacy of distributed computing:
    - Network is reliable
    - bandwidth is infinite
    - topology doesn't change
    - transport cost is zero
    - latency is zero.
- However any call across a network can fail! Therefore any integration point _will_ fail.
- But you **need** them, so we have to cope.

### Slow Responses
- These tie up resources; which can ultimately cascade.
- So fail fast! Give up and respond.
- Fast enough is 250ms.

### Shared Resources
- Anything that is shared will become a bottleneck. (e.g. a database).
- So is memory (a GC pause is a fail) and storage (capacity and IO).
- Avoid one of anything!
- This includes threads! Best thing you can do is keep STA and thus scale outside.

### Unbound Result Sets
- Breaks at scale.
- Watch for one-to-many with ORM; never accept these if possible. _Always_ agree what the maximum can be.
- Setting limits is helpful as it defines the business case for increasing.

### Manual Configuration
- This prevents repeatable infrastructure.
- Hard coded configuration is also not flexible enough to survive a dynamic envionment; espeically in recovery scenarios.
- Use service registration.


