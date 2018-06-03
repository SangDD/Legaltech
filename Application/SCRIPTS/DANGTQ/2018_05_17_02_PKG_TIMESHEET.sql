-- Start of DDL Script for Package Body LEGALTECH.PKG_TIMESHEET
-- Generated 3-Jun-2018 22:10:51 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_timesheet
  IS TYPE tcursor IS ref CURSOR;


PROCEDURE proc_timesheet_getall
(
    p_cursor out tcursor
);

PROCEDURE proc_timesheet_search
(
    P_SEARCH_KEY IN VARCHAR2,
    P_FROM IN VARCHAR2,
    P_TO IN VARCHAR2,
    P_SORT_TYPE IN VARCHAR2,
    P_TOTAL_RECORD OUT NUMBER,
    P_CURSOR OUT PKG_COMMON.T_CURSOR
);


PROCEDURE proc_timesheet_getbyid
(
    p_id IN timesheet.id % TYPE,
    p_cursor out tcursor
);

PROCEDURE proc_timesheet_getby_lawer
(
    p_lawer_id IN timesheet.lawer_id % TYPE,
    p_cursor out tcursor
);

PROCEDURE Proc_Timesheet_Insert
(
    p_name IN timesheet.name % TYPE,
    p_app_header_id IN timesheet.app_header_id % TYPE,
    p_lawer_id IN timesheet.lawer_id % TYPE,
    p_time_date IN timesheet.time_date % TYPE,
    p_hours IN timesheet.hours % TYPE,
    p_notes IN timesheet.notes % TYPE,
    p_status IN timesheet.status % TYPE,
    p_created_by IN timesheet.created_by % TYPE,
    p_created_date IN timesheet.created_date % TYPE,
    p_return OUT NUMBER
);

PROCEDURE proc_timesheet_update
(
    p_id IN timesheet.id % TYPE,
    p_name IN timesheet.name % TYPE,
    p_time_date IN timesheet.time_date % TYPE,
    p_hours IN timesheet.hours % TYPE,
    p_notes IN timesheet.notes % TYPE,
    p_modify_by IN timesheet.modify_by % TYPE,
    p_modify_date IN timesheet.modify_date % TYPE,
    p_return OUT NUMBER
);

PROCEDURE proc_timesheet_delete
(
    p_id IN timesheet.id % TYPE,
    p_modify_by IN timesheet.modify_by % TYPE,
    p_modify_date IN timesheet.modify_date % TYPE,
    p_return OUT NUMBER
);


PROCEDURE proc_timesheet_approve
(
    p_id IN timesheet.id % TYPE,
    p_status IN timesheet.status % TYPE,
    p_reject_reason IN timesheet.reject_reason % TYPE,
    p_modify_by IN timesheet.modify_by % TYPE,
    p_modify_date IN timesheet.modify_date % TYPE,
    p_return OUT NUMBER
);
END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_timesheet
IS

PROCEDURE proc_timesheet_getall
(
    p_cursor out tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT t.*, ac.content AS status_name, ah.APPCODE App_Code,sa.appname App_Name,ah.gencode AS app_gencode, l.fullname LAWER_NAME
    FROM timesheet t
    JOIN application_header ah ON ah.id = t.app_header_id AND ah.deleted = 0
    JOIN sys_application sa ON ah.appcode = sa.appcode  AND ah.LANGUAGUE_CODE = sa.LANGUAGECODE
    LEFT JOIN s_users l ON l.id = t.LAWER_ID
    LEFT JOIN allcode ac ON ac.cdval = t.status AND ac.cdname = 'TIMESHEET' AND ac.cdtype ='STATUS'
    ORDER BY t.app_header_id, t.id DESC;
END;

PROCEDURE proc_timesheet_getbyid
(
    p_id IN timesheet.id % TYPE,
    p_cursor out tcursor
)
IS
BEGIN
    OPEN p_cursor FOR SELECT t.*, ac.content AS status_name, ah.APPCODE App_Code,sa.appname App_Name,ah.gencode AS app_gencode, l.fullname LAWER_NAME
    FROM timesheet t
    JOIN application_header ah ON ah.id = t.app_header_id AND ah.deleted = 0
    JOIN sys_application sa ON ah.appcode = sa.appcode  AND ah.LANGUAGUE_CODE = sa.LANGUAGECODE
    LEFT JOIN s_users l ON l.id = t.LAWER_ID
    LEFT JOIN allcode ac ON ac.cdval = t.status AND ac.cdname = 'TIMESHEET' AND ac.cdtype ='STATUS'
    WHERE t.id = p_id
    ORDER BY t.app_header_id, t.id DESC;
END;

PROCEDURE proc_timesheet_getby_lawer
(
    p_lawer_id IN timesheet.lawer_id % TYPE,
    p_cursor out tcursor
)
IS
BEGIN
    OPEN P_CURSOR FOR SELECT t.*, ac.content AS status_name, ah.APPCODE, l.fullname LAWER_NAME
    FROM timesheet t
    JOIN application_header ah ON ah.id = t.app_header_id AND ah.deleted = 0
    LEFT JOIN s_users l ON l.id = t.LAWER_ID
    LEFT JOIN allcode ac ON ac.cdval = t.status AND ac.cdname = 'TIMESHEET' AND ac.cdtype ='STATUS'
    WHERE t.lawer_id = p_lawer_id
    ORDER BY t.app_header_id, t.id DESC;
END;

PROCEDURE Proc_Timesheet_Insert
(
    p_name IN timesheet.name % TYPE,
    p_app_header_id IN timesheet.app_header_id % TYPE,
    p_lawer_id IN timesheet.lawer_id % TYPE,
    p_time_date IN timesheet.time_date % TYPE,
    p_hours IN timesheet.hours % TYPE,
    p_notes IN timesheet.notes % TYPE,
    p_status IN timesheet.status % TYPE,
    p_created_by IN timesheet.created_by % TYPE,
    p_created_date IN timesheet.created_date % TYPE,
    p_return OUT NUMBER
)
IS
    v_id NUMBER;
BEGIN


    SELECT SEQ_TIMESHEET.nextval INTO v_id FROM dual;
    INSERT INTO timesheet
    (
        id,name, app_header_id, lawer_id, time_date, hours, notes, status, created_by, created_date

    )
    VALUES
    (
        v_id,p_name, p_app_header_id, p_lawer_id, p_time_date, p_hours, p_notes, p_status, p_created_by, p_created_date
    );

    p_return := v_id;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := -1;
    RAISE;
END;

PROCEDURE proc_timesheet_update
(
    p_id IN timesheet.id % TYPE,
    p_name IN timesheet.name % TYPE,
    p_time_date IN timesheet.time_date % TYPE,
    p_hours IN timesheet.hours % TYPE,
    p_notes IN timesheet.notes % TYPE,
    p_modify_by IN timesheet.modify_by % TYPE,
    p_modify_date IN timesheet.modify_date % TYPE,
    p_return OUT NUMBER
)
IS
BEGIN
    UPDATE timesheet SET
        name = p_name,
        time_date = p_time_date,
        hours = p_hours,
        notes = p_notes,
        modify_by = p_modify_by,
        modify_date = TRUNC(p_modify_date)

    WHERE id = p_id;

    p_return := p_id;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := -1;
    RAISE;
END;

PROCEDURE proc_timesheet_delete
(
    p_id IN timesheet.id % TYPE,
    p_modify_by IN timesheet.modify_by % TYPE,
    p_modify_date IN timesheet.modify_date % TYPE,
    p_return OUT NUMBER
)
IS
BEGIN
    UPDATE timesheet SET
        deleted = 1,
        modify_by = p_modify_by,
        modify_date = TRUNC(p_modify_date)
    WHERE id = p_id;

    p_return := p_id;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := -1;
    RAISE;
END;

PROCEDURE proc_timesheet_approve
(
    p_id IN timesheet.id % TYPE,
    p_status IN timesheet.status % TYPE,
    p_reject_reason IN timesheet.reject_reason % TYPE,
    p_modify_by IN timesheet.modify_by % TYPE,
    p_modify_date IN timesheet.modify_date % TYPE,
    p_return OUT NUMBER
)
IS
BEGIN
    UPDATE timesheet SET
        status = p_status,
        reject_reason = decode(p_status, 1, '', p_reject_reason),
        modify_by = p_modify_by,
        modify_date = TRUNC(p_modify_date)
    WHERE id = p_id;

    p_return := p_id;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    p_return := -1;
    RAISE;
END;

PROCEDURE proc_timesheet_search
(
    P_SEARCH_KEY IN VARCHAR2,
    P_FROM IN VARCHAR2,
    P_TO IN VARCHAR2,
    P_SORT_TYPE IN VARCHAR2,
    P_TOTAL_RECORD OUT NUMBER,
    P_CURSOR OUT PKG_COMMON.T_CURSOR
)
IS
    V_SQL VARCHAR2(32000) DEFAULT '';
    V_CONDITION VARCHAR2(4000) DEFAULT '';
    V_TOTAL_COUNT VARCHAR2(32000) DEFAULT '';

    V_INDEX NUMBER DEFAULT 0;
    CURSOR V_CUR_CONDITION IS
        SELECT COLUMN_VALUE AS CONDITION FROM TABLE(FN_PARSER(P_SEARCH_KEY,'|'));

    V_APP_CODE VARCHAR2(30) DEFAULT 'ALL';
    V_STATUS VARCHAR2(3) DEFAULT 'ALL';
    V_LAWER VARCHAR2(100) DEFAULT 'ALL';

    V_FROM NUMBER ;
    V_TO NUMBER;
BEGIN
    -- LAY RA CAC DK TIM KIEM
    FOR ITEM IN V_CUR_CONDITION LOOP

        IF V_INDEX = 0 THEN
            V_APP_CODE := ITEM.CONDITION;
        ELSIF V_INDEX = 1 THEN
            V_STATUS := ITEM.CONDITION;
        ELSIF V_INDEX = 2 THEN
            V_LAWER := ITEM.CONDITION;
        END IF;

        V_INDEX := V_INDEX + 1;
    END LOOP;

    IF V_APP_CODE <> 'ALL' THEN
        V_CONDITION := V_CONDITION || ' AND T.APPCODE = ''' || V_APP_CODE || '''';
    END IF;

    IF V_STATUS <> 'ALL' THEN
        V_CONDITION := V_CONDITION || ' AND T.STATUS = ' || V_STATUS;
    END IF;

    IF V_LAWER <> 'ALL' THEN
        --V_CONDITION :=  V_CONDITION || ' AND UPPER(L.LAWER_ID) LIKE ''%' || UPPER(V_LAWER) || '%'' ';
        V_CONDITION :=  V_CONDITION || ' AND UPPER(L.ID) = ' || V_LAWER;
    END IF;


    V_SQL := 'SELECT t.*, ac.content AS status_name, ah.APPCODE App_Code,sa.appname App_Name,ah.gencode AS app_gencode, l.fullname LAWER_NAME
        FROM timesheet t
        JOIN application_header ah ON ah.id = t.app_header_id AND ah.deleted = 0
        JOIN sys_application sa ON ah.appcode = sa.appcode  AND ah.LANGUAGUE_CODE = sa.LANGUAGECODE
        LEFT JOIN s_users l ON l.id = t.LAWER_ID
        LEFT JOIN allcode ac ON ac.cdval = t.status AND ac.cdname = ''TIMESHEET'' AND ac.cdtype =''STATUS''
        WHERE 1 = 1 ';

    V_SQL :=  V_SQL || V_CONDITION;

    IF P_SORT_TYPE <> 'ALL' THEN
        V_SQL :=  V_SQL || P_SORT_TYPE;
    ELSE
        V_SQL :=  V_SQL || ' ORDER BY T.APP_HEADER_ID, T.ID DESC';
    END IF;

    DBMS_OUTPUT.PUT_LINE(V_SQL);


    V_TOTAL_COUNT := 'SELECT COUNT(*) FROM (' || V_SQL || ')';
    EXECUTE IMMEDIATE V_TOTAL_COUNT INTO V_TOTAL_COUNT;

    V_FROM := P_FROM ;
    V_TO := P_TO;

    IF P_FROM = '0' AND P_TO = '0' THEN
        V_FROM := '1' ;
        V_TO := V_TOTAL_COUNT;
    END IF;

    V_SQL := 'SELECT * FROM ( SELECT ROWNUM STT, V.* FROM( ' || V_SQL || ') V ) A WHERE STT BETWEEN ' || V_FROM || ' AND ' || V_TO;
    OPEN P_CURSOR FOR V_SQL;
    --PKG_LOG.LOGMSG(V_SQL);


    P_TOTAL_RECORD := V_TOTAL_COUNT;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_TIMESHEET

