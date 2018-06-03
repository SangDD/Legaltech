-- Start of DDL Script for Package Body LEGALTECH.PKG_LAWER_INFO
-- Generated 3-Jun-2018 22:08:51 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_lawer_info
 IS TYPE tcursor IS ref CURSOR;

PROCEDURE proc_lawer_getall
(
    p_cursor out tcursor
);

PROCEDURE proc_lawer_search
(
    p_search_key IN VARCHAR2,
    p_from IN VARCHAR2,
    p_to IN VARCHAR2,
    p_sort_type IN VARCHAR2,
    p_total_record OUT NUMBER,
    p_cursor out tcursor
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_lawer_info
IS

PROCEDURE proc_lawer_getall
(
    p_cursor out tcursor
)
IS
BEGIN
    OPEN P_CURSOR FOR SELECT * FROM view_users WHERE TYPE = pkg_common.USER_TYPE_LAWER ORDER BY fullname;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_lawer_search
(
    p_search_key IN VARCHAR2,
    p_from IN VARCHAR2,
    p_to IN VARCHAR2,
    p_sort_type IN VARCHAR2,
    p_total_record OUT NUMBER,
    p_cursor out tcursor
)
IS
    v_sql VARCHAR2(32000) DEFAULT '';
    v_condition VARCHAR2(4000) DEFAULT '';
    v_total_count VARCHAR2(32000) DEFAULT '';

    v_index NUMBER DEFAULT 0;
    CURSOR v_cur_condition IS
        SELECT COLUMN_VALUE AS CONDITION FROM TABLE(FN_PARSER(p_search_key,'|'));

    v_status VARCHAR2(3) DEFAULT 'ALL';
    v_lawer_name VARCHAR2(100) DEFAULT 'ALL';

    v_from NUMBER ;
    v_to NUMBER;
BEGIN
    -- lay ra cac dk tim kiem
    FOR ITEM IN v_cur_condition LOOP

        IF v_index = 0 THEN
            v_lawer_name := ITEM.CONDITION;
        END IF;

        v_index := v_index + 1;
    END LOOP;

    IF v_lawer_name <> 'ALL' THEN
        v_condition :=  v_condition || ' AND UPPER(A.fullname) LIKE ''%' || UPPER(v_lawer_name) || '%'' ';
    END IF;


    v_sql := 'SELECT * FROM view_users A WHERE 1 = 1 ';

    v_sql :=  v_sql || v_condition;

    IF p_sort_type <> 'ALL' THEN
        v_sql :=  v_sql || p_sort_type;
    ELSE
        v_sql :=  v_sql || ' ORDER BY A.fullname';
    END IF;

    dbms_output.put_line(v_sql);


    v_total_count := 'SELECT COUNT(*) FROM (' || v_sql || ')';
    EXECUTE IMMEDIATE v_total_count INTO v_total_count;

    v_from := p_from ;
    v_to := p_to;

    IF p_from = '0' AND p_to = '0' THEN
        v_from := '1' ;
        v_to := v_total_count;
    END IF;

    v_sql := 'SELECT * FROM ( SELECT ROWNUM STT, V.* FROM( ' || v_sql || ') V ) a WHERE STT BETWEEN ' || v_from || ' AND ' || v_to;
    OPEN P_CURSOR FOR v_sql;
    --pkg_log.logmsg(v_sql);


    p_total_record := v_total_count;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_LAWER_INFO

