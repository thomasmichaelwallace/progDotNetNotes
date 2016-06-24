# Event Driven Microservices

## In the real world
- You're talking 400 - 100 microservices; they tend to get "out of control".
- Leads to an impressive amount of technologies; "right tool for the job."
- Define Microservices, as something with a single purpose; in a functional way.
- Great because: scalable, independent release/fail, distribution of complexity.
- Note this creates an infrastructure burden.
- Events are immutable statements of significant change that has occured.
- Sourcing is an apparend-only sequence of events; a commit log.
- F# drove Microservices. 

## Tips
- Prefer immutability- state is painful and needs management.
- Use map/reduce/transform.
- Look at problems recursively
- Treat functions as units of work.
- Don't abstract; don't target multiple communication protocols, for example; don't treat message queues the same, just write a microservice for all of them that _you actually need_.
- Isoltate side effects. (Think about the "Thank you E-mail!")
- Use a backup service; replay events and the switch over.
- Stage a copy of any aggregate/data store until stream has completed replaying -> don't repeat making and undoing mistakes live!
- Use consistent formatting; makes it easier for people to be dropped in.
- Kafka/EventStore is good for backup.
- Grouped instances of services per spin-up; keep releated close.

## What do they look like?
- Define inuts and outputs up head.
- Define how input transforms to output (**handle**)
- Define what to do with output (**interpret**)
- Read events; handle & interpret; run (**consume**)
- This is kept _consistent_ for readability.

## Control
- Microservices should not control their own lifecycle.
- IoC -> Use docker, etc.
- Think failure (active/active; distributed - active/passive; waiting backup).
