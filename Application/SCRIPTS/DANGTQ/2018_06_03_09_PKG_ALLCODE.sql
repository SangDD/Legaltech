-- Start of DDL Script for Package Body LEGALTECH.PKG_ALLCODE
-- Generated 3-Jun-2018 10:20:11 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_allcode
  IS TYPE tcursor IS ref CURSOR;

PROCEDURE proc_AllCode_GetAll
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Country_GetAll
(
    p_cursor OUT tcursor
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_allcode
IS

PROCEDURE proc_AllCode_GetAll
(
    p_cursor OUT tcursor
)
IS
BEGIN

    OPEN p_cursor FOR SELECT * FROM allcode ORDER BY cdname,cdtype,lstodr;
END;

PROCEDURE proc_Country_GetAll
(
    p_cursor OUT tcursor
)
IS
BEGIN

    OPEN p_cursor FOR SELECT * FROM country ORDER BY name;
END;

END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_ALLCODE

