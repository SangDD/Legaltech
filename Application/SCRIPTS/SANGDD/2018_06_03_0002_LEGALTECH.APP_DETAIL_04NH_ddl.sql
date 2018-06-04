-- Start of DDL Script for Table LEGALTECH.APP_DETAIL_04NH
-- Generated 03/06/2018 11:24:02 AM from LEGALTECH@LOCAL

CREATE TABLE app_detail_04nh
    (id                             NUMBER,
    appcode                        VARCHAR2(50 CHAR),
    app_header_id                  NUMBER,
    language_code                  VARCHAR2(5 CHAR),
    appno                          VARCHAR2(50 CHAR),
    duadate                        DATE,
    logourl                        VARCHAR2(250 CHAR),
    dactichhanghoa                 NUMBER(1,0),
    color                          VARCHAR2(200 CHAR),
    description                    VARCHAR2(200 CHAR),
    huongquyenuutien               NUMBER(1,0),
    sodon_ut                       VARCHAR2(50 CHAR),
    ngaynopdon_ut                  DATE,
    nuocnopdon_ut                  VARCHAR2(150 CHAR),
    nguongocdialy                  VARCHAR2(150 CHAR),
    chatluong                      VARCHAR2(150 CHAR),
    dactinhkhac                    VARCHAR2(150 CHAR),
    cdk_name_1                     VARCHAR2(50 CHAR),
    cdk_address_1                  VARCHAR2(200 CHAR),
    cdk_phone_1                    VARCHAR2(50 CHAR),
    cdk_fax_1                      VARCHAR2(50 CHAR),
    cdk_email_1                    VARCHAR2(50 CHAR),
    cdk_name_2                     VARCHAR2(50 CHAR),
    cdk_address_2                  VARCHAR2(200 CHAR),
    cdk_phone_2                    VARCHAR2(50 CHAR),
    cdk_fax_2                      VARCHAR2(50 CHAR),
    cdk_email_2                    VARCHAR2(50 CHAR),
    cdk_name_3                     VARCHAR2(50 CHAR),
    cdk_address_3                  VARCHAR2(200 CHAR),
    cdk_phone_3                    VARCHAR2(50 CHAR),
    cdk_fax_3                      VARCHAR2(50 CHAR),
    cdk_email_3                    VARCHAR2(50 CHAR),
    cdk_name_4                     VARCHAR2(50 CHAR),
    cdk_address_4                  VARCHAR2(200 CHAR),
    cdk_phone_4                    VARCHAR2(50 CHAR),
    cdk_fax_4                      VARCHAR2(50 CHAR),
    cdk_email_4                    VARCHAR2(50 CHAR),
    used_special                   NUMBER(1,0) DEFAULT 0)
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





-- Comments for APP_DETAIL_04NH

COMMENT ON COLUMN app_detail_04nh.appno IS 'SO DON TACH RA TU SO NAO '
/
COMMENT ON COLUMN app_detail_04nh.cdk_address_1 IS 'DIA CHI CHU DON 1'
/
COMMENT ON COLUMN app_detail_04nh.cdk_email_1 IS 'EMAIL CHU DON 1'
/
COMMENT ON COLUMN app_detail_04nh.cdk_fax_1 IS 'FAX CHU DON 1'
/
COMMENT ON COLUMN app_detail_04nh.cdk_name_1 IS 'CHU DON 1'
/
COMMENT ON COLUMN app_detail_04nh.cdk_phone_1 IS 'PHONE CHU DON 1'
/
COMMENT ON COLUMN app_detail_04nh.chatluong IS 'CHAT LUONG , NEU CO DL THI MAC DINH CHECK BOX BEN TREN'
/
COMMENT ON COLUMN app_detail_04nh.color IS 'MAU SAC'
/
COMMENT ON COLUMN app_detail_04nh.dactichhanghoa IS 'DAC TINH HANG HOA 0:ko dung ,1:co dung'
/
COMMENT ON COLUMN app_detail_04nh.dactinhkhac IS 'DAC TINH , NEU CO DL THI MAC DINH CHECK BOX BEN TREN'
/
COMMENT ON COLUMN app_detail_04nh.description IS 'MO TA '
/
COMMENT ON COLUMN app_detail_04nh.duadate IS 'NGAY NOP DON'
/
COMMENT ON COLUMN app_detail_04nh.huongquyenuutien IS 'YEU CAU HUONG QUYEN UU TIEN  0:khong dung,1:co dung'
/
COMMENT ON COLUMN app_detail_04nh.logourl IS 'MAU NHAN HIEU'
/
COMMENT ON COLUMN app_detail_04nh.ngaynopdon_ut IS 'NGAY NOP DON'
/
COMMENT ON COLUMN app_detail_04nh.nguongocdialy IS 'NGUON GOC DIA LY, NEU CO DL THI MAC DINH CHECK BOX BEN TREN'
/
COMMENT ON COLUMN app_detail_04nh.nuocnopdon_ut IS 'NUOC NOP DON '
/
COMMENT ON COLUMN app_detail_04nh.sodon_ut IS 'SO DON UU TIEN'
/
COMMENT ON COLUMN app_detail_04nh.used_special IS 'CO SU DUNG DON UU TIEN  0:khong su dung don uu tien ,1: co su dung don uu tien'
/

-- End of DDL Script for Table LEGALTECH.APP_DETAIL_04NH

