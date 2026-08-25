# Gangline PM playbook

## Intake

Start with the user intent and the affected repository surface. Read the root `AGENTS.md`, then the relevant `projects/<surface>/documents/README.md`. Search `backlog/`, `bugs/`, and `decisions/` for related work before creating anything new.

## Four-signal rubric

Record the verdict in every new ticket:

| Signal | Auto-persist when | Owner approval required when |
| --- | --- | --- |
| Blast radius | One surface and bounded consumers | Cross-surface behavior or shared infrastructure |
| Change type | Correction or internal maintenance | New product capability |
| Product decision | No user-facing choice | An explicit product, workflow, or policy choice |
| Contract impact | Internal implementation detail | API, URL, schema, permission, CLI flag, release, or deployment contract |

Any approval-required signal pauses persistence until the Owner approves the specific choice. A ticket may still contain investigation notes before approval, but must not silently commit the decision.

## Delegation brief

When delegation is useful, include the ticket path, owned surface, scope fence, acceptance checks, and evidence required. A surface agent must not edit another surface or broaden the ticket. Record out-of-scope findings as a separate ticket.

## Verification bar

A ticket is done only when the result includes:

- commands or harnesses run, with pass/fail counts where available;
- observed behavior, including relevant HTTP/UI/database output;
- the harness delta (tests, checks, or tooling added or changed);
- remaining risk or an explicit reason a check could not run.

Keep a Done ticket body immutable. Create a follow-up ticket for new work.

## Owner gates

Ask the Owner before persisting a product choice, changing a consumer-facing contract, changing permissions or schemas, deploying, publishing, deleting material data, or taking another irreversible outward-facing action.
