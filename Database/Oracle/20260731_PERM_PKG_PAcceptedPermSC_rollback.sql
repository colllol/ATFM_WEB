/*
  Rollback PERM_PKG.PAcceptedPermSC

  1. Remove this declaration from PACKAGE SPEC PERM_PKG:

     PROCEDURE PAcceptedPermSC
     (
         P_ID          IN NUMBER,
         P_RETURN_CODE OUT NUMBER
     );

  2. Remove the complete PAcceptedPermSC implementation from PACKAGE BODY.

  3. Compile and verify:

     ALTER PACKAGE PERM_PKG COMPILE SPECIFICATION;
     ALTER PACKAGE PERM_PKG COMPILE BODY;

     SELECT type, line, position, text
     FROM user_errors
     WHERE name = 'PERM_PKG'
     ORDER BY sequence;

  This rollback only removes the package API. It does not delete schedule rows
  that were already rendered by successful Accepted actions.
*/
