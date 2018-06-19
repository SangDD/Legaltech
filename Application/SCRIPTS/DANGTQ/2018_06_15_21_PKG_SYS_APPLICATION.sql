-- Start of DDL Script for Package Body LEGALTECH.PKG_SYS_APPLICATION
-- Generated 15-Jun-2018 23:42:17 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_sys_application
  IS

PROCEDURE PROC_SYS_APPLICATION_GETALL
(
    P_CURSOR OUT PKG_COMMON.T_CURSOR
);

PROCEDURE proc_sys_app_fix_charge_getall
(
    P_CURSOR OUT PKG_COMMON.T_CURSOR
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_sys_application
IS

PROCEDURE PROC_SYS_APPLICATION_GETALL
(
    P_CURSOR OUT PKG_COMMON.T_CURSOR
)
IS

BEGIN
    OPEN P_CURSOR FOR
    SELECT * FROM SYS_APPLICATION WHERE DISPLAY = 1 ORDER BY LISTORD ;
END;

PROCEDURE proc_sys_app_fix_charge_getall
(
    P_CURSOR OUT PKG_COMMON.T_CURSOR
)
IS

BEGIN
    OPEN P_CURSOR FOR
    SELECT * FROM sys_app_fix_charge a ORDER BY appcode;
END;


END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_SYS_APPLICATION
