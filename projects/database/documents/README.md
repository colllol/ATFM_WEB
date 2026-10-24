# Surface: database

Owner boundary: Oracle or SQL schema, packages, procedures, indexes, data migrations, and rollback scripts under `Database/`.

Every schema or data change requires a rollback story and explicit verification evidence. Deployment to a real database is an Owner-gated outward-facing action.

Keep application code changes in their owning surface and link the ticket or decision that defines any contract change.
