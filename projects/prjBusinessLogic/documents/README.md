# Surface: prjBusinessLogic

Owner boundary: application services, business rules, API clients, and orchestration used by the web application.

Keep page markup and database DDL out of this surface. Treat public method signatures, serialized payloads, and endpoint behavior as contracts that may require Owner approval.

Verification should include the narrowest available automated tests plus a build of the affected project or solution.
