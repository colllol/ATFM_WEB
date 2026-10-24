# Surface: shared-libraries

Owner boundary: shared models and controls in `prjInfo`, `prjComponents`, `HPCServerDataAccess`, `HPCShareDLL`, and `CustomControl`.

Changes here can affect many pages and business services. Treat public DTOs, helper signatures, serialization, and control behavior as cross-surface contracts; link the coordinating ticket or decision before changing them.

Verification should include an affected-project or solution build and focused consumer checks when a shared contract changes.
