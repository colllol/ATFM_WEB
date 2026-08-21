# Gangline workspace rules

This repository uses Gangline as its file-native operating model.

## Roles

- The Owner approves product choices, contract changes, permission changes, and irreversible or outward-facing actions.
- The PM/coordinator clarifies intent, searches related work, scopes tickets, selects a surface, and records outcomes.
- A surface agent edits only the assigned surface and follows its documents before changing code.
- An ops agent prepares and verifies releases; every public release still needs a fresh Owner go-ahead.

## Board of record

- `backlog/STATUS.md` indexes planned work and `bugs/STATUS.md` indexes defects.
- Ticket files in `backlog/` and `bugs/` are the source of truth for existence and status. `STATUS.md` is a derived index.
- `decisions/` stores accepted decisions and decision candidates. Do not hide product or contract decisions in ticket prose.
- Ticket IDs are derived from ticket filenames, never from `STATUS.md`.
- Surface mapping is explicit: `prjApplication`, `prjBusinessLogic`, `database`, `tools-services-tests`, `shared-libraries` (`prjInfo`, `prjComponents`, `HPCServerDataAccess`, `HPCShareDLL`, `CustomControl`), and `deployment` (`Deploy`). The `services/` tree is artifact-only until tracked source returns.

## Required loop

1. Read the root `AGENTS.md` and the assigned surface documents.
2. Search both boards for related work before opening a ticket.
3. Apply the four-signal rubric in `management/pm-playbook.md`.
4. Persist one coherent ticket and synchronize its board row.
5. Verify with counted evidence, record the harness delta, and move the lane.

## Repository-specific rule

Every file creation, edit, rename, or deletion must also append an entry to `NHAT_KY_THAY_DOI.txt` and be committed locally. Never push automatically. Preserve unrelated user changes in the worktree.
