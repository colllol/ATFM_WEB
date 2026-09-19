# Surface: deployment

Owner boundary: deployment and rollback scripts under `Deploy/`, publish manifests, and release preparation.

Deployment, database execution, publishing, and production configuration are outward-facing actions. Prepare evidence and rollback details in the ticket, then obtain the Owner gate immediately before execution.

Do not treat `services/` build artifacts as source ownership. Record artifact provenance and environment assumptions explicitly.
