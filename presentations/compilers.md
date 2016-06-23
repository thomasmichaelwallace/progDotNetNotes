# Compilers

- Start with a DSL.
- 

## When?
- Syntax doesn't fit within another language
- Proprietary encoding
- Custom execution environments (constrain CPU; ability)

## Steps
- Define Abstract Syntax Tree
- Parse
- Execute

## Parsing (in F#)
- By hand (Regex, Active Patters)
- FPrasec (Combinators; good, Monadic; nice but bad performance)
- FsLex, FsYacc (Not very forgiving)

## Tips
- Start with an AST and then see if it works; make the parsing a secondary step.

Fun fact- Excel keywords are translated.