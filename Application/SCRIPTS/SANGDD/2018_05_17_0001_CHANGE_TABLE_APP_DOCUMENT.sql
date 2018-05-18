-- Start of DDL Script for Table LEGALTECH.APP_DOCUMENT
-- Generated 17/05/2018 10:22:16 PM from LEGALTECH@LOCAL
DROP TABLE  app_document ;
/
COMMIT ;

/
CREATE TABLE app_document
    (id                             NUMBER,
    language_code                  VARCHAR2(5 CHAR),
    app_header_id                  NUMBER,
    document_id                    VARCHAR2(10 CHAR),
    isuse                          NUMBER,
    note                           VARCHAR2(250 CHAR) ,
    status                         NUMBER DEFAULT 0,
    document_filing_date           DATE,
    filename                       VARCHAR2(250 CHAR),
    url_hardcopy                   VARCHAR2(250 CHAR)    
    )
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

