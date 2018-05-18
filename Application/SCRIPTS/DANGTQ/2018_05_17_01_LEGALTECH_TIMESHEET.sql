-- Start of DDL Script for Table LEGALTECH.TIMESHEET
-- Generated 18-May-2018 0:32:01 from LEGALTECH@LOCALHOST

-- Drop the old instance of TIMESHEET
DROP TABLE timesheet
/

CREATE TABLE timesheet
    (id                             NUMBER,
    app_header_id                  NUMBER,
    lawer_id                       NUMBER,
    time_date                      DATE,
    hours                          NUMBER,
    notes                          VARCHAR2(2000 CHAR),
    status                         NUMBER(1,0),
    reject_reason                  VARCHAR2(2000 CHAR),
    deleted                        NUMBER(1,0) DEFAULT 0,
    created_by                     VARCHAR2(50 CHAR),
    created_date                   DATE,
    modify_by                      VARCHAR2(50 CHAR),
    modify_date                    DATE,
    name                           VARCHAR2(200 CHAR))
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





-- Comments for TIMESHEET

COMMENT ON COLUMN timesheet.app_header_id IS 'ID DON'
/
COMMENT ON COLUMN timesheet.hours IS 'SO GIO'
/
COMMENT ON COLUMN timesheet.id IS 'ID TU TANG'
/
COMMENT ON COLUMN timesheet.lawer_id IS 'ID LUAT SU'
/
COMMENT ON COLUMN timesheet.notes IS 'GHI CHU'
/
COMMENT ON COLUMN timesheet.reject_reason IS 'MO TA LOI'
/
COMMENT ON COLUMN timesheet.status IS 'TRANG THAI'
/
COMMENT ON COLUMN timesheet.time_date IS 'NGAY GHI'
/

-- End of DDL Script for Table LEGALTECH.TIMESHEET

CREATE SEQUENCE seq_timesheet
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20
/


