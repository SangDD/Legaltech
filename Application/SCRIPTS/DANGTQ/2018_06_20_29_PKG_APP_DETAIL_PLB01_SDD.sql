-- Start of DDL Script for Package Body LEGALTECH.PKG_APP_DETAIL_PLB01_SDD
-- Generated 21-Jun-2018 00:05:04 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_app_detail_plb01_sdd
  IS TYPE tcursor IS ref CURSOR;

PROCEDURE Proc_GetById
(
    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_cursor OUT tcursor,
    p_cursorHeader OUT tcursor,
    p_cursor_doc OUT tcursor,
    p_cursor_fee OUT tcursor
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
    p_number_pic IN NUMBER,
    p_number_page IN NUMBER,
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
    p_number_pic IN NUMBER,
    p_number_page IN NUMBER,
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
    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_cursor OUT tcursor,
    p_cursorHeader OUT tcursor,
    p_cursor_doc OUT tcursor,
    p_cursor_fee OUT tcursor
)
IS
BEGIN

    OPEN p_cursor FOR
    SELECT d.*,ah.* FROM APP_DETAIL_PLB01_SDD d
    JOIN APPLICATION_HEADER ah ON ah.id = d.app_header_id AND ah.LANGUAGUE_CODE = d.language_code AND ah.deleted = 0 AND ah.id = p_app_header_id;

    OPEN p_cursorHeader FOR
    SELECT * FROM VIEW_APP_HEADER WHERE id = p_app_header_id;

    --LAY DANH SACH CAC TAI LIEU DINH KEM
    OPEN p_cursor_doc FOR
    SELECT * FROM APP_DOCUMENT A  WHERE app_header_id = p_app_header_id AND  language_code = p_language_code ;

    OPEN p_cursor_fee FOR
    SELECT * FROM APP_FEE_FIX  WHERE app_header_id= p_app_header_id ;

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
    p_number_pic IN NUMBER,
    p_number_page IN NUMBER,
    p_return OUT NUMBER
)
IS
    v_id NUMBER;
BEGIN


    SELECT SEQ_PLB01_SDD.NEXTVAL INTO v_id FROM dual;
    INSERT INTO app_detail_plb01_sdd
    (
        id, app_header_id, appcode, request_change_type, app_no_change, request_to_type,
        request_to_content, language_code,number_pic,number_page
    )
    VALUES
    (
        v_id, p_app_header_id, p_appcode, p_request_change_type, p_app_no_change, p_request_to_type,
        p_request_to_content, p_language_code,p_number_pic,p_number_page
    );

    p_return := 1;

--COMMIT;
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
    p_number_pic IN NUMBER,
    p_number_page IN NUMBER,
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
        request_to_content = p_request_to_content,
        number_pic = p_number_pic,
        number_page = p_number_page

    WHERE app_header_id = p_app_header_id AND language_code = p_language_code;

    p_return := p_id;

--COMMIT;
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

    p_return:= P_APP_HEADER_ID;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := PKG_COMMON.ERROR;
    RAISE;
END  ;



END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_APP_DETAIL_PLB01_SDD

