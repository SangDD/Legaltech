-- Start of DDL Script for Table HNXCORE.LOG_ERROR
-- Generated 10-Jun-2018 14:01:30 from HNXCORE@DB_CODE_35

CREATE TABLE log_error
    (id                             NUMBER,
    log_timestamp                  DATE,
    error_message                  VARCHAR2(2000 CHAR),
    package_name                   VARCHAR2(50 CHAR))
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





-- End of DDL Script for Table HNXCORE.LOG_ERROR

