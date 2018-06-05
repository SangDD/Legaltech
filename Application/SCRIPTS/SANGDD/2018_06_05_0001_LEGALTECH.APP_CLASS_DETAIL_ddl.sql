-- Start of DDL Script for Table LEGALTECH.APP_CLASS_DETAIL
-- Generated 05/06/2018 10:43:26 PM from LEGALTECH@LOCAL

CREATE TABLE app_class_detail
    (id                             NUMBER,
    textinput                      VARCHAR2(200 CHAR),
    code                           VARCHAR2(30 CHAR))
  SEGMENT CREATION IMMEDIATE
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
 


-- Comments for APP_CLASS_DETAIL

COMMENT ON COLUMN app_class_detail.code IS 'Mã Nhóm class'
/
COMMENT ON COLUMN app_class_detail.id IS 'ID t? tang'
/
COMMENT ON COLUMN app_class_detail.textinput IS 'N?i dung ngu?i dùng nh?p'
/

-- End of DDL Script for Table LEGALTECH.APP_CLASS_DETAIL

