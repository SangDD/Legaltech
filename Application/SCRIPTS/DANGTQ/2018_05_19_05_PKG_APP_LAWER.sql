-- Start of DDL Script for Package Body LEGALTECH.PKG_APP_LAWER
-- Generated 19-May-2018 0:58:43 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_app_lawer
    IS TYPE tcursor IS ref CURSOR;

PROCEDURE proc_get_appgrant4Lawer
(
    p_lawer_id  IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_grant_app2_lawer
(
    p_application_header_id IN NUMBER,
    p_lawer_id  IN NUMBER,
    p_notes VARCHAR2,
    p_language_code VARCHAR2,
    p_created_by VARCHAR2,
    p_created_date DATE,
    p_return OUT NUMBER
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_app_lawer
IS

PROCEDURE proc_get_appgrant4Lawer
(
    p_lawer_id  IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN

    OPEN p_cursor FOR
    SELECT ah.gencode, ah.appcode AS app_code, sa.appname AS app_name,al.Application_Header_Id,al.Lawer_Id,al.Language_Code
    FROM app_lawer al
    JOIN application_header ah ON ah.id = al.application_header_id AND al.language_code = ah.languague_code
    JOIN sys_application sa ON ah.appcode = sa.appcode  AND ah.LANGUAGUE_CODE = sa.LANGUAGECODE
    WHERE al.lawer_id = p_lawer_id;

END;

PROCEDURE proc_grant_app2_lawer
(
    p_application_header_id IN NUMBER,
    p_lawer_id  IN NUMBER,
    p_notes VARCHAR2,
    p_language_code VARCHAR2,
    p_created_by VARCHAR2,
    p_created_date DATE,
    p_return OUT NUMBER
)
IS
    v_id NUMBER;
BEGIN

    SELECT SEQ_APP_LAWER.NEXTVAL into v_id FROM dual;
    INSERT INTO app_lawer
    (
        id, application_header_id,lawer_id,notes,language_code,created_by,created_date
    )
    VALUES
    (
        v_id, p_application_header_id,p_lawer_id,p_notes,p_language_code,p_created_by,p_created_date
    );

    UPDATE APPLICATION_HEADER SET status = pkg_app_header.APP_HEADER_STATUS_DAGUI2LS
    WHERE id = p_application_header_id;

    p_return := v_id;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_APP_LAWER

