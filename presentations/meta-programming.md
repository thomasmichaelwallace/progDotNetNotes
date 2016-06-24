# Meta Programming

@david_whitney

## What is it?
- Reflection
- Inspection of programming model at run time.
- Foundation of libraries and frameworks that augment your code, rather than adding functionality.

## What can you use it for?
- Test frameworks.
- Anything that is just a pattern (think MVC; RESTful protocols).
- Do the right thing by default- implement strong conventions.
- Auto-wiring containers.
- Protecting code quality; move soft-conventions into tests.

## Tips
- Cache, because cache can be quite slow.
- "Everything should _just work_".
- Reflection is not as slow as it used to be- it's slow because it's not compiler optimised. However most use cases can be mitigated using caching (which is now built in; to a point.).
- Better to give people the trigger for magic, rather than just doing it for them.