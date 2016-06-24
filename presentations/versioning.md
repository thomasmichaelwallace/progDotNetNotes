# API Versioning

@serialseb

## Why version?
- It's all about change control.
- Allow client and server (and all interfaces) to evolve independently.
- Communicate changes.

## But it is evil.
- Uri versioning introduce 404s; malformed domains; get in the way of the meaning of the URLs.
- Domain versioning has no semantic value.
- Version media types; should only be used if the difference etween formats is mecahnical in nature _to the client_ (i.e. image types.)- however this is hidden (and content negotiation is not garunteed).
- This also creates statically.

## We want evaolability of contracts.

### Backward compatibility
- Just look at the web!
- Look at HTML- continue to support, but depreciate <blink />
- But this is super expensive!

### Forward compatibility
- Look at css. (ignore what's not understood). 
- _Degrade_ the experience; create fallback rules to make extensibility points.
- Don't validate across full schemas; _maybe_ just validate what you care about.

### Deprecieating Well
- Keep both alongside.
- Use metrics to decide when to switch off.
- Decide how many people can be lost / gained.
- Do this on a feature basis to lower risk (only lose those that use that feature.)
- Although requires clients to send telemetry.
- Consider client driven contracts; this is a good idea.

