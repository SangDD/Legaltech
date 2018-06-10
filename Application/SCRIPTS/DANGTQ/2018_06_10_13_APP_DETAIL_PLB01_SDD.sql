-- Start of DDL Script for Sequence LEGALTECH.SEQ_PLB01_SDD
-- Generated 10-Jun-2018 23:24:22 from LEGALTECH@LOCALHOST

CREATE SEQUENCE seq_plb01_sdd
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20
/


-- End of DDL Script for Sequence LEGALTECH.SEQ_PLB01_SDD
-- Start of DDL Script for Table LEGALTECH.APP_DETAIL_PLB01_SDD

-- Generated 10-Jun-2018 23:24:55 from LEGALTECH@LOCALHOST
CREATE TABLE app_detail_plb01_sdd
    (id                             NUMBER,
    app_header_id                  NUMBER,
    appcode                        VARCHAR2(50 CHAR),
    request_change_type            NUMBER,
    app_no_change                  VARCHAR2(200 CHAR),
    request_to_type                NUMBER,
    request_to_content             VARCHAR2(500 CHAR),
    language_code                  VARCHAR2(5 CHAR))
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- Comments for APP_DETAIL_PLB01_SDD

COMMENT ON COLUMN app_detail_plb01_sdd.app_no_change IS 'SO DON SUA DOI'
/
COMMENT ON COLUMN app_detail_plb01_sdd.request_change_type IS 'YEU CAU SUA DOI'
/
COMMENT ON COLUMN app_detail_plb01_sdd.request_to_content IS 'NOI DUNG SUA DOI'
/
COMMENT ON COLUMN app_detail_plb01_sdd.request_to_type IS 'LOAI NOI DUNG SUA DOI'
/

-- End of DDL Script for Table LEGALTECH.APP_DETAIL_PLB01_SDD


-- Start of DDL Script for Package Body LEGALTECH.PKG_APP_DETAIL_PLB01_SDD
-- Generated 10-Jun-2018 23:25:16 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_app_detail_plb01_sdd
  IS TYPE tcursor IS ref CURSOR;

PROCEDURE Proc_GetById
(
    p_id IN app_detail_plb01_sdd.id % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_cursor OUT tcursor
);

PROCEDURE Proc_Plb01_Sdd_Insert
(
    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_appcode IN app_detail_plb01_sdd.appcode % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_request_change_type IN app_detail_plb01_sdd.request_change_type % TYPE,
    p_app_no_change IN app_detail_plb01_sdd.app_no_change % TYPE,
    p_request_to_type IN app_detail_plb01_sdd.request_to_type % TYPE,
    p_request_to_content IN app_detail_plb01_sdd.request_to_content % TYPE,
    p_return OUT NUMBER
);


PROCEDURE Proc_Plb01_Sdd_Update
(
    p_id IN app_detail_plb01_sdd.id % TYPE,
    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_appcode IN app_detail_plb01_sdd.appcode % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_request_change_type IN app_detail_plb01_sdd.request_change_type % TYPE,
    p_app_no_change IN app_detail_plb01_sdd.app_no_change % TYPE,
    p_request_to_type IN app_detail_plb01_sdd.request_to_type % TYPE,
    p_request_to_content IN app_detail_plb01_sdd.request_to_content % TYPE,
    p_return OUT NUMBER
);

PROCEDURE Proc_Plb01_Sdd_Delete
(

    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_appcode IN app_detail_plb01_sdd.appcode % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_return OUT number
);
END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_app_detail_plb01_sdd
IS

PROCEDURE Proc_GetById
(
    p_id IN app_detail_plb01_sdd.id % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_cursor OUT tcursor
)
IS
BEGIN

    OPEN p_cursor FOR
    SELECT d.*,ah.* FROM APP_DETAIL_PLB01_SDD d
    JOIN APPLICATION_HEADER ah ON ah.id = d.app_header_id AND ah.LANGUAGUE_CODE = d.language_code AND ah.deleted = 0;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE Proc_Plb01_Sdd_Insert
(
    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_appcode IN app_detail_plb01_sdd.appcode % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_request_change_type IN app_detail_plb01_sdd.request_change_type % TYPE,
    p_app_no_change IN app_detail_plb01_sdd.app_no_change % TYPE,
    p_request_to_type IN app_detail_plb01_sdd.request_to_type % TYPE,
    p_request_to_content IN app_detail_plb01_sdd.request_to_content % TYPE,
    p_return OUT NUMBER
)
IS
    v_id NUMBER;
BEGIN


    SELECT SEQ_PLB01_SDD.NEXTVAL INTO v_id FROM dual;
    INSERT INTO app_detail_plb01_sdd
    (
        id, app_header_id, appcode, request_change_type, app_no_change, request_to_type, request_to_content, language_code

    )
    VALUES
    (
        v_id, p_app_header_id, p_appcode, p_request_change_type, p_app_no_change, p_request_to_type, p_request_to_content, p_language_code

    );

    p_return := PKG_COMMON.SUCESS ;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := PKG_COMMON.ERROR;
    RAISE;
END;

PROCEDURE Proc_Plb01_Sdd_Update
(
    p_id IN app_detail_plb01_sdd.id % TYPE,
    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_appcode IN app_detail_plb01_sdd.appcode % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_request_change_type IN app_detail_plb01_sdd.request_change_type % TYPE,
    p_app_no_change IN app_detail_plb01_sdd.app_no_change % TYPE,
    p_request_to_type IN app_detail_plb01_sdd.request_to_type % TYPE,
    p_request_to_content IN app_detail_plb01_sdd.request_to_content % TYPE,
    p_return OUT NUMBER
)
IS
BEGIN
    UPDATE app_detail_plb01_sdd SET
        --app_header_id = p_app_header_id,
        --appcode = p_appcode,
        language_code = p_language_code,
        request_change_type = p_request_change_type,
        app_no_change = p_app_no_change,
        request_to_type = p_request_to_type,
        request_to_content = p_request_to_content

    WHERE id = p_id;

    p_return := PKG_COMMON.SUCESS ;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := PKG_COMMON.ERROR;
    RAISE;
END;

PROCEDURE Proc_Plb01_Sdd_Delete
(

    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_appcode IN app_detail_plb01_sdd.appcode % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_return OUT number
)
IS
BEGIN

    DELETE app_detail_plb01_sdd
    WHERE APP_HEADER_ID = P_APP_HEADER_ID AND APPCODE = P_APPCODE AND LANGUAGE_CODE = P_LANGUAGE_CODE;

    p_return:= PKG_COMMON.SUCESS;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := PKG_COMMON.ERROR;
    RAISE;
END  ;



END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_APP_DETAIL_PLB01_SDD

