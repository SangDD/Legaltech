﻿CREATE OR REPLACE 
PACKAGE PKG_WIKI_DOCS
  IS
 PROCEDURE PROC_WIKI_DOCS_INSERT
(
    P_TITLE IN WIKI_DOCS.TITLE % TYPE,
    P_CONTENT IN WIKI_DOCS.CONTENT % TYPE,
    P_CREATED_BY IN WIKI_DOCS.CREATED_BY % TYPE,
    P_CREATED_DATE IN WIKI_DOCS.CREATED_DATE % TYPE,
    P_LANGUAGE_CODE IN WIKI_DOCS.LANGUAGE_CODE % TYPE,
    P_HASHTAG IN VARCHAR2,
    P_FILE_URL01 IN VARCHAR2,
    P_FILE_URL02 IN VARCHAR2,
    P_FILE_URL03 IN VARCHAR2,
    P_CATA_ID IN NUMBER,
    P_STATUS IN NUMBER,
    P_RETURN OUT NUMBER
);

PROCEDURE PROC_WIKI_DOCS_UPDATE
(
P_ID IN WIKI_DOCS.ID % TYPE,
P_TITLE IN WIKI_DOCS.TITLE % TYPE,
P_CONTENT IN WIKI_DOCS.CONTENT % TYPE,
P_MODIFIED_BY IN WIKI_DOCS.MODIFIED_BY % TYPE,
P_MODIFIED_DATE IN WIKI_DOCS.MODIFIED_DATE % TYPE,
P_LANGUAGE_CODE IN WIKI_DOCS.LANGUAGE_CODE % TYPE,
P_HASHTAG IN VARCHAR2,
P_FILE_URL01 IN VARCHAR2,
P_FILE_URL02 IN VARCHAR2,
P_FILE_URL03 IN VARCHAR2,
P_CATA_ID IN NUMBER,
P_STATUS IN NUMBER,
P_REFUSE_REASON IN VARCHAR2,
P_RETURN OUT NUMBER
);

PROCEDURE PROC_WIKI_DOCS_GETALL
(
P_CURSOR OUT PKG_COMMON.t_cursor
);


PROCEDURE PROC_WIKI_DOCS_GETBYID
(
  P_ID IN WIKI_DOCS.ID % TYPE,
  P_CURSOR OUT PKG_COMMON.t_cursor
);

PROCEDURE PROC_WIKI_DOCS_DELETE
(
  P_ID IN WIKI_DOCS.ID % TYPE,
  P_RETURN OUT NUMBER
);


PROCEDURE PROC_WIKI_DOC_SEARCH
(
    P_SEARCH_KEY IN VARCHAR2,
    P_FROM IN VARCHAR2,
    P_TO IN VARCHAR2,
    P_SORT_TYPE IN VARCHAR2,
    P_TOTAL_RECORD OUT NUMBER,
    P_CURSOR OUT PKG_COMMON.t_cursor
);

PROCEDURE PROC_WIKI_DOCS_UPDATE_HASHTAG
(
P_ID IN WIKI_DOCS.ID % TYPE,
P_HASHTAG IN VARCHAR2,
P_RETURN OUT NUMBER
);

PROCEDURE PROC_PORTAL_WIKI_DOCS_GETBYID
(
  P_ID IN WIKI_DOCS.ID % TYPE,
  P_CURSOR OUT PKG_COMMON.t_cursor
);
END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY PKG_WIKI_DOCS
IS


PROCEDURE PROC_WIKI_DOCS_INSERT
(
    P_TITLE IN WIKI_DOCS.TITLE % TYPE,
    P_CONTENT IN WIKI_DOCS.CONTENT % TYPE,
    P_CREATED_BY IN WIKI_DOCS.CREATED_BY % TYPE,
    P_CREATED_DATE IN WIKI_DOCS.CREATED_DATE % TYPE,
    P_LANGUAGE_CODE IN WIKI_DOCS.LANGUAGE_CODE % TYPE,
    P_HASHTAG IN VARCHAR2,
    P_FILE_URL01 IN VARCHAR2,
    P_FILE_URL02 IN VARCHAR2,
    P_FILE_URL03 IN VARCHAR2,
    P_CATA_ID IN NUMBER,
    P_STATUS IN NUMBER,
    P_RETURN OUT NUMBER
)
IS
V_ID NUMBER;
BEGIN

SELECT SEQ_WIKI_DOCS.NEXTVAL INTO V_ID FROM DUAL;
P_RETURN := V_ID;
    INSERT INTO WIKI_DOCS
    (
         ID, TITLE, CONTENT,  CREATED_BY, CREATED_DATE,
         LANGUAGE_CODE, HASHTAG, FILE_URL01,
         FILE_URL02, FILE_URL03, STATUS, MODIFIED_BY
    )
    VALUES
    (
        V_ID, P_TITLE, P_CONTENT,  P_CREATED_BY,
         TRUNC(P_CREATED_DATE), P_LANGUAGE_CODE, P_HASHTAG,
         P_FILE_URL01, P_FILE_URL02, P_FILE_URL03, P_STATUS, TRUNC(P_CREATED_DATE)
     );

INSERT INTO WIKI_DOCS_CATALOGUES A (ID, A.DOC_ID, A.CATALOGUE_ID )
VALUES(SEQ_WIKI_CATAS.NEXTVAL, V_ID, P_CATA_ID);

COMMIT;
EXCEPTION
WHEN OTHERS THEN
P_RETURN :=-1;
RAISE;
END;




--2.UPDATE

PROCEDURE PROC_WIKI_DOCS_UPDATE
(
P_ID IN WIKI_DOCS.ID % TYPE,
P_TITLE IN WIKI_DOCS.TITLE % TYPE,
P_CONTENT IN WIKI_DOCS.CONTENT % TYPE,
P_MODIFIED_BY IN WIKI_DOCS.MODIFIED_BY % TYPE,
P_MODIFIED_DATE IN WIKI_DOCS.MODIFIED_DATE % TYPE,
P_LANGUAGE_CODE IN WIKI_DOCS.LANGUAGE_CODE % TYPE,
P_HASHTAG IN VARCHAR2,
P_FILE_URL01 IN VARCHAR2,
P_FILE_URL02 IN VARCHAR2,
P_FILE_URL03 IN VARCHAR2,
P_CATA_ID IN NUMBER,
P_STATUS IN NUMBER,
P_REFUSE_REASON  IN VARCHAR2,
P_RETURN OUT NUMBER
)
IS
BEGIN
P_RETURN := P_ID;
UPDATE WIKI_DOCS SET
TITLE = P_TITLE,
CONTENT = P_CONTENT,
MODIFIED_BY = P_MODIFIED_BY,
MODIFIED_DATE = TRUNC(P_MODIFIED_DATE),
LANGUAGE_CODE = P_LANGUAGE_CODE,
HASHTAG = P_HASHTAG,
FILE_URL01 = P_FILE_URL01,
FILE_URL02 = P_FILE_URL02,
FILE_URL03 = P_FILE_URL03,
STATUS = P_STATUS,
REFUSE_REASON = P_REFUSE_REASON
WHERE ID = P_ID;

DELETE FROM WIKI_DOCS_CATALOGUES WHERE DOC_ID = P_ID;
INSERT INTO WIKI_DOCS_CATALOGUES A (ID, A.DOC_ID, A.CATALOGUE_ID )
VALUES(SEQ_WIKI_CATAS.NEXTVAL, P_ID, P_CATA_ID);

COMMIT;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK;
RAISE;
END;
--2END

--3.GELALL

PROCEDURE PROC_WIKI_DOCS_GETALL
(
P_CURSOR OUT PKG_COMMON.t_cursor
)
IS
 BEGIN
 OPEN P_CURSOR FOR SELECT * FROM WIKI_DOCS;
END;

--4.GELBYID

PROCEDURE PROC_WIKI_DOCS_GETBYID
(
  P_ID IN WIKI_DOCS.ID % TYPE,
  P_CURSOR OUT PKG_COMMON.t_cursor
)
IS
 BEGIN
OPEN P_CURSOR FOR SELECT A.*, C.ID AS CATA_ID, C.NAME AS CATA_NAME FROM WIKI_DOCS A INNER JOIN WIKI_DOCS_CATALOGUES B
        ON A.ID = B.DOC_ID INNER JOIN WIKI_CATALOGUES
        C ON B.CATALOGUE_ID = C.ID WHERE NVL(A.DELETED,0) = 0
       AND A.ID = P_ID;
END;

PROCEDURE PROC_WIKI_DOCS_DELETE
(
  P_ID IN WIKI_DOCS.ID % TYPE,
  P_RETURN OUT NUMBER
)
IS
 BEGIN
 P_RETURN:=0;
 UPDATE  WIKI_DOCS SET DELETED = 1
 WHERE ID = P_ID;

 EXCEPTION
WHEN OTHERS THEN
P_RETURN := -1;
ROLLBACK;
RAISE;
END;



PROCEDURE PROC_WIKI_DOC_SEARCH
(
    P_SEARCH_KEY IN VARCHAR2,
    P_FROM IN VARCHAR2,
    P_TO IN VARCHAR2,
    P_SORT_TYPE IN VARCHAR2,
    P_TOTAL_RECORD OUT NUMBER,
    P_CURSOR OUT PKG_COMMON.t_cursor
)
IS
    V_SQL VARCHAR2(32000) DEFAULT '';
    V_SQL_TOTAL VARCHAR2(32000) DEFAULT '';
    V_CONDITION VARCHAR2(4000) DEFAULT '';
    V_TOTAL_COUNT NUMBER  ;

    V_INDEX NUMBER DEFAULT 0;
    CURSOR V_CUR_CONDITION IS
        SELECT COLUMN_VALUE AS CONDITION FROM TABLE(FN_PARSER(P_SEARCH_KEY,'|'));

    V_STATUS VARCHAR2(3) DEFAULT 'ALL';
    V_CATA_ID VARCHAR2(100) DEFAULT 'ALL';
    V_NAME_HASHTAG VARCHAR2(100) DEFAULT 'ALL';

    V_FROM NUMBER ;
    V_TO NUMBER;
BEGIN

   PKG_LOG.LOGMSG('vao 1'); commit;
    -- LAY RA CAC DK TIM KIEM
    FOR ITEM IN V_CUR_CONDITION LOOP

        IF V_INDEX = 0 THEN
            V_STATUS := ITEM.CONDITION;
        END IF;
         IF V_INDEX = 1 THEN
            V_CATA_ID := ITEM.CONDITION;
        END IF;
         IF V_INDEX = 2 THEN
            V_NAME_HASHTAG := ITEM.CONDITION;
        END IF;
        V_INDEX := V_INDEX + 1;
    END LOOP;

   PKG_LOG.LOGMSG('vao 2'); commit;
    IF V_STATUS <> 'ALL' THEN
        V_CONDITION :=  V_CONDITION || ' AND STATUS IN  (' || V_STATUS || ') ';
    END IF;
    IF V_CATA_ID <> 'ALL' AND V_CATA_ID <> 'null'  THEN
        V_CONDITION :=  V_CONDITION || ' AND B.CATALOGUE_ID  LIKE ''%' || UPPER(V_CATA_ID) || '%'' ';
    END IF;
    IF V_NAME_HASHTAG <> 'ALL' THEN
        V_CONDITION :=  V_CONDITION || ' AND ( UPPER(A.TITLE) LIKE ''%' || UPPER(V_NAME_HASHTAG) || '%''
         OR  UPPER(A.TITLE) LIKE ''%' || UPPER(V_NAME_HASHTAG) || '%''
         )
         ';
    END IF;



    V_SQL := 'SELECT  A.ID, A.TITLE,  A.VIEW_NUMBER, A.CREATED_BY,
        A.CREATED_DATE, A.MODIFIED_BY, A.MODIFIED_DATE, A.DELETED,
        A.LANGUAGE_CODE, A.HASHTAG, A.FILE_URL01, A.FILE_URL02,
        A.FILE_URL03, A.STATUS, C.ID AS CATA_ID, C.NAME AS CATA_NAME FROM WIKI_DOCS A INNER JOIN WIKI_DOCS_CATALOGUES B
        ON A.ID = B.DOC_ID INNER JOIN WIKI_CATALOGUES
        C ON B.CATALOGUE_ID = C.ID
        WHERE NVL(A.DELETED,0) = 0  ';

    V_SQL :=  V_SQL || V_CONDITION;

    IF P_SORT_TYPE <> 'ALL' THEN
        V_SQL :=  V_SQL || ' ORDER BY ' || P_SORT_TYPE;
    ELSE
        V_SQL :=  V_SQL || ' ORDER BY A.MODIFIED_BY DESC';
    END IF;


   PKG_LOG.LOGMSG(V_SQL); COMMIT;
    V_SQL_TOTAL := 'SELECT COUNT(*) FROM (' || V_SQL || ')';
    EXECUTE IMMEDIATE V_SQL_TOTAL INTO V_TOTAL_COUNT;

    V_FROM := P_FROM ;
    V_TO := P_TO;

    IF P_FROM = '0' AND P_TO = '0' THEN
        V_FROM := '1' ;
        V_TO := V_TOTAL_COUNT;
    END IF;

    V_SQL := 'SELECT * FROM ( SELECT ROWNUM STT, V.* FROM( ' || V_SQL || ') V ) A WHERE STT BETWEEN ' || V_FROM || ' AND ' || V_TO;
    OPEN P_CURSOR FOR V_SQL;



    P_TOTAL_RECORD := V_TOTAL_COUNT;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;


PROCEDURE PROC_WIKI_DOCS_UPDATE_HASHTAG
(
P_ID IN WIKI_DOCS.ID % TYPE,
P_HASHTAG IN VARCHAR2,
P_RETURN OUT NUMBER
)
IS
BEGIN
P_RETURN := P_ID;
UPDATE WIKI_DOCS SET
HASHTAG = P_HASHTAG WHERE ID = P_ID;


COMMIT;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK;
RAISE;
END;

PROCEDURE PROC_PORTAL_WIKI_DOCS_GETBYID
(
  P_ID IN WIKI_DOCS.ID % TYPE,
  P_CURSOR OUT PKG_COMMON.t_cursor
)
IS
 BEGIN
  UPDATE WIKI_DOCS A SET  A.VIEW_NUMBER  =   NVL(A.VIEW_NUMBER,0) + 1 WHERE ID = P_ID;
  COMMIT;
OPEN P_CURSOR FOR SELECT A.*, C.ID AS CATA_ID, C.NAME AS CATA_NAME FROM WIKI_DOCS A INNER JOIN WIKI_DOCS_CATALOGUES B
        ON A.ID = B.DOC_ID INNER JOIN WIKI_CATALOGUES
        C ON B.CATALOGUE_ID = C.ID WHERE NVL(A.DELETED,0) = 0
       AND A.ID = P_ID;


END;

END;
/

CREATE OR REPLACE 
PACKAGE PKG_WIKI_PORTAL
  IS
 PROCEDURE PROC_GET_CATALOGUE
(
  P_CURSOR OUT PKG_COMMON.t_cursor
);

PROCEDURE PROC_GET_DOC_BY_CATA_ID
(
  P_CATA_ID IN NUMBER,
  P_CURSOR OUT PKG_COMMON.t_cursor
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY PKG_WIKI_PORTAL
IS


PROCEDURE PROC_GET_CATALOGUE
(
  P_CURSOR OUT PKG_COMMON.t_cursor
)
IS
 BEGIN
    OPEN P_CURSOR FOR SELECT A.* FROM WIKI_CATALOGUES A WHERE A.DELETED = 0
     CONNECT BY PRIOR ID = PARENT_ID ;

     EXCEPTION WHEN OTHERS THEN
     RAISE;
END;


PROCEDURE PROC_GET_DOC_BY_CATA_ID
(
  P_CATA_ID IN NUMBER,
  P_CURSOR OUT PKG_COMMON.t_cursor
)
IS
 BEGIN
    OPEN P_CURSOR FOR SELECT A.* FROM WIKI_DOCS A INNER JOIN WIKI_DOCS_CATALOGUES B ON A.ID = B.DOC_ID WHERE A.DELETED = 0
      AND B.CATALOGUE_ID =  P_CATA_ID ;

     EXCEPTION WHEN OTHERS THEN
     RAISE;
END;

END;
/

