# Metric Driven Development

@samelamin

Answer the question- _why_ are we building; and that's not sorted by scrum.

## Why
- TDD needed translations.
- BDD isn't accessible to program managers.
- Engineers are the best placed to measure what's happening in a platform; because we built the damn thing.
- Focus on what matters; conversations, and features.
- Blocks out politics- you have to prove, and everyone knows what they want.
- Let's you learn how to scale (patterns).
- High visibility of the platform; achievable to the minute.
- It's rewarding to make a measured contribution.

## What is it?
- Not a substitution to BDD/TDD; this is a focuser.
- Sits upon best practice (CI, etc.)
- Continuously prove a hypothesis; test and inform your assumptions.

## Types of metrics
- Business metrics- recording what happens; measured in increments.
- Application metrics- recording how happens; time between it all. 

## How is it
- Start by proving the concept.
- Decided on metrics; and start collecting them- start on business logic.
- Dont measure _everything_; YAGNI!
- Publish all data centrally; elastic search it.
- AB Tests.
- See libraries like Graphite.

## Tips
- Feature toggling (AB tests); release internal, then scale slowly. Allows you to prove out in a controlled manner and reduce risk.
- Implement alerts and warnings.
- Go for a single source of truth (they use elastic; graphana; kibana)
- Make data avaialbe to all; do not assume it's only for the software developers.
- Run fake load; prove that you can cope with capacity.
- Undertaken "vison" meetings to discover your target.
- Team level screen/graph configurations; as well as a company wide one. Allows a focus review; but then make _everything_ available to the inquisitive.

## References
- Etsy do this
- Graphite is a good service for pushing stats.
- Statsd is good for this.
- Just eat also do this (unsurprisngly, given the speaker.)
