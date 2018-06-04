-- Start of DDL Script for Table LEGALTECH.APP_DOCUMENT
-- Generated 03/06/2018 3:21:43 PM from LEGALTECH@LOCAL
-- Start of DDL Script for Sequence LEGALTECH.SEQ_APP_DOCUMENT
-- Generated 03/06/2018 3:20:59 PM from LEGALTECH@LOCAL

CREATE SEQUENCE seq_app_document
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20
/


-- End of DDL Script for Sequence LEGALTECH.SEQ_APP_DOCUMENT


CREATE TABLE app_document
    (id                             NUMBER,
    language_code                  VARCHAR2(5 CHAR),
    app_header_id                  NUMBER,
    document_id                    VARCHAR2(10 CHAR),
    isuse                          NUMBER,
    note                           VARCHAR2(250 CHAR),
    status                         NUMBER DEFAULT 0,
    document_filing_date           DATE,
    filename                       VARCHAR2(250 CHAR),
    url_hardcopy                   VARCHAR2(250 CHAR),
    char01                         VARCHAR2(100 CHAR),
    char02                         VARCHAR2(100 CHAR),
    char03                         VARCHAR2(100 CHAR),
    char04                         VARCHAR2(100 CHAR),
    char05                         VARCHAR2(100 CHAR))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     131072
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- Comments for APP_DOCUMENT

COMMENT ON COLUMN app_document.document_id IS 'TAM THOI FIXED CUNG DI '
/
COMMENT ON COLUMN app_document.filename IS 'TEN FILE TAI LEN'
/
COMMENT ON COLUMN app_document.note IS 'NOI DUNG, SO TRANG, TIENG VIET ,TIENG ANH'
/
COMMENT ON COLUMN app_document.status IS '1:DA NOP BAN CUNG,0:CHUA NOP BAN CUNG'
/

-- End of DDL Script for Table LEGALTECH.APP_DOCUMENT


/

-- Start of DDL Script for Package LEGALTECH.PKG_APP_DOCUMENT
-- Generated 03/06/2018 3:31:15 PM from LEGALTECH@LOCAL

CREATE OR REPLACE 
PACKAGE pkg_app_document
  IS

PROCEDURE PROC_APP_DOCUMENT_INSERT
(
    P_LANGUAGE_CODE IN APP_DOCUMENT.LANGUAGE_CODE % TYPE,
    P_APP_HEADER_ID IN APP_DOCUMENT.APP_HEADER_ID % TYPE,
    P_DOCUMENT_ID IN APP_DOCUMENT.DOCUMENT_ID % TYPE,
    P_ISUSE IN APP_DOCUMENT.ISUSE % TYPE,
    P_NOTE IN APP_DOCUMENT.NOTE % TYPE,
    P_STATUS IN APP_DOCUMENT.STATUS % TYPE,
    P_DOCUMENT_FILING_DATE IN APP_DOCUMENT.DOCUMENT_FILING_DATE % TYPE,
    P_FILENAME IN APP_DOCUMENT.FILENAME % TYPE,
    P_URL_HARDCOPY IN APP_DOCUMENT.URL_HARDCOPY % TYPE,
    P_CHAR01 IN APP_DOCUMENT.CHAR01 % TYPE,
    P_CHAR02 IN APP_DOCUMENT.CHAR02 % TYPE,
    P_CHAR03 IN APP_DOCUMENT.CHAR03 % TYPE,
    P_CHAR04 IN APP_DOCUMENT.CHAR04 % TYPE,
    P_CHAR05 IN APP_DOCUMENT.CHAR05 % TYPE ,
    P_RETURN OUT NUMBER
) ;

PROCEDURE PROC_APP_DOCUMENT_UPDATE
(
    P_ID IN NUMBER ,
    P_LANGUAGE_CODE IN APP_DOCUMENT.LANGUAGE_CODE % TYPE,
    P_APP_HEADER_ID IN APP_DOCUMENT.APP_HEADER_ID % TYPE,
    P_DOCUMENT_ID IN APP_DOCUMENT.DOCUMENT_ID % TYPE,
    P_ISUSE IN APP_DOCUMENT.ISUSE % TYPE,
    P_NOTE IN APP_DOCUMENT.NOTE % TYPE,
    P_STATUS IN APP_DOCUMENT.STATUS % TYPE,
    P_DOCUMENT_FILING_DATE IN APP_DOCUMENT.DOCUMENT_FILING_DATE % TYPE,
    P_FILENAME IN APP_DOCUMENT.FILENAME % TYPE,
    P_URL_HARDCOPY IN APP_DOCUMENT.URL_HARDCOPY % TYPE,
    P_CHAR01 IN APP_DOCUMENT.CHAR01 % TYPE,
    P_CHAR02 IN APP_DOCUMENT.CHAR02 % TYPE,
    P_CHAR03 IN APP_DOCUMENT.CHAR03 % TYPE,
    P_CHAR04 IN APP_DOCUMENT.CHAR04 % TYPE,
    P_CHAR05 IN APP_DOCUMENT.CHAR05 % TYPE ,
    P_RETURN OUT NUMBER
);

PROCEDURE PROC_APP_DOCUMENT_DELETE(
    P_ID IN NUMBER ,
    P_LANGUAGE_CODE IN APP_DOCUMENT.LANGUAGE_CODE % TYPE,
    P_APP_HEADER_ID IN APP_DOCUMENT.APP_HEADER_ID % TYPE,
    P_DOCUMENT_ID IN APP_DOCUMENT.DOCUMENT_ID % TYPE  ,
    P_RETURN OUT NUMBER
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_app_document
IS

PROCEDURE PROC_APP_DOCUMENT_INSERT
(
    P_LANGUAGE_CODE IN APP_DOCUMENT.LANGUAGE_CODE % TYPE,
    P_APP_HEADER_ID IN APP_DOCUMENT.APP_HEADER_ID % TYPE,
    P_DOCUMENT_ID IN APP_DOCUMENT.DOCUMENT_ID % TYPE,
    P_ISUSE IN APP_DOCUMENT.ISUSE % TYPE,
    P_NOTE IN APP_DOCUMENT.NOTE % TYPE,
    P_STATUS IN APP_DOCUMENT.STATUS % TYPE,
    P_DOCUMENT_FILING_DATE IN APP_DOCUMENT.DOCUMENT_FILING_DATE % TYPE,
    P_FILENAME IN APP_DOCUMENT.FILENAME % TYPE,
    P_URL_HARDCOPY IN APP_DOCUMENT.URL_HARDCOPY % TYPE,
    P_CHAR01 IN APP_DOCUMENT.CHAR01 % TYPE,
    P_CHAR02 IN APP_DOCUMENT.CHAR02 % TYPE,
    P_CHAR03 IN APP_DOCUMENT.CHAR03 % TYPE,
    P_CHAR04 IN APP_DOCUMENT.CHAR04 % TYPE,
    P_CHAR05 IN APP_DOCUMENT.CHAR05 % TYPE ,
    P_RETURN OUT NUMBER
)
IS
V_ID NUMBER ;
BEGIN
    P_RETURN:= PKG_COMMON.SUCESS ;
    SELECT SEQ_APP_DOCUMENT.NEXTVAL INTO V_ID FROM DUAL;

    INSERT INTO APP_DOCUMENT
    (
        ID, LANGUAGE_CODE, APP_HEADER_ID, DOCUMENT_ID, ISUSE, NOTE, STATUS, DOCUMENT_FILING_DATE, FILENAME, URL_HARDCOPY,
        CHAR01, CHAR02, CHAR03, CHAR04, CHAR05    )
    VALUES
    (
         V_ID, P_LANGUAGE_CODE, P_APP_HEADER_ID, P_DOCUMENT_ID, P_ISUSE, P_NOTE, P_STATUS, TRUNC(P_DOCUMENT_FILING_DATE), P_FILENAME,
         P_URL_HARDCOPY, P_CHAR01, P_CHAR02, P_CHAR03, P_CHAR04, P_CHAR05);


EXCEPTION WHEN OTHERS THEN
RAISE;
 P_RETURN:= PKG_COMMON.ERROR ;
END;



PROCEDURE PROC_APP_DOCUMENT_UPDATE
(
    P_ID IN NUMBER ,
    P_LANGUAGE_CODE IN APP_DOCUMENT.LANGUAGE_CODE % TYPE,
    P_APP_HEADER_ID IN APP_DOCUMENT.APP_HEADER_ID % TYPE,
    P_DOCUMENT_ID IN APP_DOCUMENT.DOCUMENT_ID % TYPE,
    P_ISUSE IN APP_DOCUMENT.ISUSE % TYPE,
    P_NOTE IN APP_DOCUMENT.NOTE % TYPE,
    P_STATUS IN APP_DOCUMENT.STATUS % TYPE,
    P_DOCUMENT_FILING_DATE IN APP_DOCUMENT.DOCUMENT_FILING_DATE % TYPE,
    P_FILENAME IN APP_DOCUMENT.FILENAME % TYPE,
    P_URL_HARDCOPY IN APP_DOCUMENT.URL_HARDCOPY % TYPE,
    P_CHAR01 IN APP_DOCUMENT.CHAR01 % TYPE,
    P_CHAR02 IN APP_DOCUMENT.CHAR02 % TYPE,
    P_CHAR03 IN APP_DOCUMENT.CHAR03 % TYPE,
    P_CHAR04 IN APP_DOCUMENT.CHAR04 % TYPE,
    P_CHAR05 IN APP_DOCUMENT.CHAR05 % TYPE ,
    P_RETURN OUT NUMBER
)
IS
V_ID NUMBER ;
BEGIN
    P_RETURN:= PKG_COMMON.SUCESS ;
    UPDATE APP_DOCUMENT SET
    DOCUMENT_ID = P_DOCUMENT_ID,
    ISUSE = P_ISUSE,
    NOTE = P_NOTE,
    STATUS = P_STATUS,
    DOCUMENT_FILING_DATE = TRUNC(P_DOCUMENT_FILING_DATE), FILENAME = P_FILENAME,
    URL_HARDCOPY = P_URL_HARDCOPY,
    CHAR01 = P_CHAR01,
    CHAR02 = P_CHAR02,
    CHAR03 = P_CHAR03,
    CHAR04 = P_CHAR04,
    CHAR05 = P_CHAR05 WHERE ID = P_ID AND APP_HEADER_ID =P_APP_HEADER_ID;

EXCEPTION WHEN OTHERS THEN
RAISE;
 P_RETURN:= PKG_COMMON.ERROR ;
END;


PROCEDURE PROC_APP_DOCUMENT_DELETE(
    P_ID IN NUMBER ,
    P_LANGUAGE_CODE IN APP_DOCUMENT.LANGUAGE_CODE % TYPE,
    P_APP_HEADER_ID IN APP_DOCUMENT.APP_HEADER_ID % TYPE,
    P_DOCUMENT_ID IN APP_DOCUMENT.DOCUMENT_ID % TYPE  ,
    P_RETURN OUT NUMBER
)
IS
BEGIN
 P_RETURN:= PKG_COMMON.SUCESS ;
     IF(P_LANGUAGE_CODE = PKG_COMMON.VI_VN) THEN
        DELETE  APP_DOCUMENT WHERE  ID = P_ID AND  APP_HEADER_ID = P_APP_HEADER_ID AND  DOCUMENT_ID = P_DOCUMENT_ID ;
     ELSE
        DELETE  APP_DOCUMENT WHERE  ID = P_ID AND  APP_HEADER_ID = P_APP_HEADER_ID
            AND  DOCUMENT_ID = P_DOCUMENT_ID AND LANGUAGE_CODE =P_LANGUAGE_CODE ;
     END IF ;

EXCEPTION WHEN OTHERS THEN
RAISE;
 P_RETURN:= PKG_COMMON.ERROR ;
END;

   -- ENTER FURTHER CODE BELOW AS SPECIFIED IN THE PACKAGE SPEC.
END;
/


-- End of DDL Script for Package LEGALTECH.PKG_APP_DOCUMENT


