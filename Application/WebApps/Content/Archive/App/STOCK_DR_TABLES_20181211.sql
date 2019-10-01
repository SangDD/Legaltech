-- Start of DDL Script for Table STOCK_DR.ALLCODE
-- Generated 11-Dec-2018 11:10:43 from STOCK_DR@PM01_DR

CREATE TABLE allcode
    (cdname                         VARCHAR2(20 BYTE) DEFAULT NULL NOT NULL,
    cdtype                         VARCHAR2(25 BYTE) NOT NULL,
    cdval                          VARCHAR2(50 BYTE) NOT NULL,
    content                        VARCHAR2(500 BYTE) NOT NULL,
    note                           NVARCHAR2(1000))
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





-- Comments for ALLCODE

COMMENT ON COLUMN allcode.cdname IS 'Mac dinh : truyen vao Ten Bang trong DB'
/
COMMENT ON COLUMN allcode.cdtype IS 'Ten truong tuong ung voi Bang can lay AllCode'
/
COMMENT ON COLUMN allcode.cdval IS 'Gia tri viet tat cua truong'
/
COMMENT ON COLUMN allcode.content IS 'Noi dung cua truong can mo ta de hien thi'
/

-- End of DDL Script for Table STOCK_DR.ALLCODE

-- Start of DDL Script for Table STOCK_DR.ALLCODE_CORE
-- Generated 11-Dec-2018 11:10:44 from STOCK_DR@PM01_DR

CREATE TABLE allcode_core
    (cdname                         VARCHAR2(20 BYTE) NOT NULL,
    cdtype                         VARCHAR2(25 BYTE) NOT NULL,
    cdval                          VARCHAR2(30 BYTE) NOT NULL,
    content                        VARCHAR2(500 BYTE) NOT NULL,
    lstodr                         NUMBER(2,0))
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





-- End of DDL Script for Table STOCK_DR.ALLCODE_CORE

-- Start of DDL Script for Table STOCK_DR.DEL_TT_ORDERS_TEMP
-- Generated 11-Dec-2018 11:10:44 from STOCK_DR@PM01_DR

CREATE TABLE del_tt_orders_temp
    (username                       VARCHAR2(30 BYTE) NOT NULL,
    order_id                       NUMBER NOT NULL,
    org_order_id                   NUMBER,
    floor_code                     VARCHAR2(20 BYTE) NOT NULL,
    order_confirm_no               VARCHAR2(20 BYTE),
    order_no                       VARCHAR2(20 BYTE) NOT NULL,
    co_order_no                    VARCHAR2(20 BYTE),
    org_order_no                   VARCHAR2(20 BYTE),
    order_date                     DATE,
    order_time                     VARCHAR2(25 BYTE) NOT NULL,
    member_id                      NUMBER(10,0) NOT NULL,
    co_member_id                   NUMBER(10,0),
    order_type                     NUMBER(10,0) DEFAULT 1 NOT NULL,
    priority                       NUMBER(1,0) NOT NULL,
    oorb                           NUMBER(1,0) NOT NULL,
    norp                           NUMBER(1,0) NOT NULL,
    norc                           NUMBER(2,0) NOT NULL,
    bore                           NUMBER(1,0),
    aori                           NUMBER(1,0),
    settlement_type                NUMBER(1,0),
    dorf                           NUMBER(1,0),
    order_qtty                     NUMBER,
    order_price                    NUMBER,
    status                         NUMBER(2,0),
    quote_price                    NUMBER DEFAULT 0,
    state                          NUMBER(1,0) DEFAULT 0,
    quote_time                     VARCHAR2(25 BYTE),
    quote_qtty                     NUMBER,
    exec_qtty                      NUMBER DEFAULT 0,
    correct_qtty                   NUMBER,
    cancel_qtty                    NUMBER,
    reject_qtty                    NUMBER,
    reject_reason                  NUMBER(1,0),
    account_no                     VARCHAR2(15 BYTE),
    co_account_no                  VARCHAR2(15 BYTE),
    broker_id                      NUMBER,
    co_broker_id                   NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    telephone                      VARCHAR2(50 BYTE) DEFAULT 0,
    org_order_base                 VARCHAR2(2 BYTE),
    settle_day                     NUMBER(2,0),
    aorc                           NUMBER(1,0),
    yieldmat                       NUMBER,
    order_qtty_display             NUMBER,
    order_price_stop               NUMBER,
    clordid                        VARCHAR2(100 BYTE) DEFAULT NULL,
    noro                           VARCHAR2(1 BYTE) DEFAULT '1',
    stock_id                       NUMBER(15,5) NOT NULL,
    sub_order_no                   VARCHAR2(20 BYTE),
    org_order_type                 NUMBER,
    trading_schedule_id            NUMBER,
    member_code_adv                VARCHAR2(100 BYTE),
    co_dorf                        NUMBER(1,0),
    current_trading_schedule_id    NUMBER,
    idmarket                       NUMBER)
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





-- End of DDL Script for Table STOCK_DR.DEL_TT_ORDERS_TEMP

-- Start of DDL Script for Table STOCK_DR.DR_OPERATOR
-- Generated 11-Dec-2018 11:10:45 from STOCK_DR@PM01_DR

CREATE TABLE dr_operator
    (current_date                   DATE NOT NULL,
    status_step                    VARCHAR2(3 CHAR) NOT NULL)
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





-- Comments for DR_OPERATOR

COMMENT ON COLUMN dr_operator.current_date IS 'NGAY GIAO DICH HIEN TAI'
/
COMMENT ON COLUMN dr_operator.status_step IS 'TRANG THAI HIEN TAI'
/

-- End of DDL Script for Table STOCK_DR.DR_OPERATOR

-- Start of DDL Script for Table STOCK_DR.DR_STEP_MATCHING
-- Generated 11-Dec-2018 11:10:45 from STOCK_DR@PM01_DR

CREATE TABLE dr_step_matching
    (object_name                    VARCHAR2(30 CHAR),
    object_id                      NUMBER,
    type                           NUMBER,
    step                           VARCHAR2(3 CHAR))
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





-- Comments for DR_STEP_MATCHING

COMMENT ON COLUMN dr_step_matching.object_id IS 'id bang / id chung khoan'
/
COMMENT ON COLUMN dr_step_matching.object_name IS 'ten bang / ten ck'
/
COMMENT ON COLUMN dr_step_matching.step IS 'buoc thuc hien, 0 la chua lam gi, 1 la huy ket qua khop cu, 2 chap nhan ket qua khop cu, 3 khop lai'
/
COMMENT ON COLUMN dr_step_matching.type IS '1 bang,2 CK'
/

-- End of DDL Script for Table STOCK_DR.DR_STEP_MATCHING

-- Start of DDL Script for Table STOCK_DR.ETF_INAV
-- Generated 11-Dec-2018 11:10:45 from STOCK_DR@PM01_DR

CREATE TABLE etf_inav
    (id                             NUMBER,
    etf_code                       VARCHAR2(30 BYTE),
    time                           VARCHAR2(30 BYTE),
    inav                           NUMBER,
    datecaculator                  DATE,
    timecaculator                  VARCHAR2(30 BYTE),
    seqrecvetf                     NUMBER DEFAULT 0)
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





-- End of DDL Script for Table STOCK_DR.ETF_INAV

-- Start of DDL Script for Table STOCK_DR.FUNCTION
-- Generated 11-Dec-2018 11:10:45 from STOCK_DR@PM01_DR

CREATE TABLE function
    (funcid                         NUMBER,
    funcname                       VARCHAR2(30 BYTE),
    funccontent                    VARCHAR2(255 BYTE),
    funcparent                     VARCHAR2(30 BYTE),
    funcimage                      VARCHAR2(1000 BYTE),
    functag                        VARCHAR2(50 BYTE),
    note                           NVARCHAR2(1000))
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





-- End of DDL Script for Table STOCK_DR.FUNCTION

-- Start of DDL Script for Table STOCK_DR.IDX_INDEX_INFO
-- Generated 11-Dec-2018 11:10:45 from STOCK_DR@PM01_DR

CREATE TABLE idx_index_info
    (index_id                       NUMBER,
    short_name                     VARCHAR2(30 BYTE),
    name                           VARCHAR2(100 BYTE),
    description                    VARCHAR2(200 BYTE),
    floor_code                     VARCHAR2(50 BYTE),
    trading_date                   DATE,
    time                           VARCHAR2(20 BYTE),
    total_stock                    NUMBER,
    advances                       NUMBER,
    nochange                       NUMBER,
    declines                       NUMBER,
    up_volume                      NUMBER,
    nochange_volume                NUMBER,
    down_volume                    NUMBER,
    total_qtty                     NUMBER,
    total_value                    NUMBER,
    prior_index_val                NUMBER,
    chg_index                      NUMBER DEFAULT 0,
    pct_index                      NUMBER DEFAULT 0,
    current_index                  NUMBER DEFAULT 0,
    base_index                     NUMBER,
    highest_index                  NUMBER,
    lowest_index                   NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    base_divide                    NUMBER DEFAULT 0 NOT NULL,
    session_no                     NUMBER,
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    type_index                     NUMBER(2,0) DEFAULT 1,
    is_calculated                  NUMBER(1,0) DEFAULT 1,
    type_calculated                NUMBER(1,0) DEFAULT 1,
    time_interval                  NUMBER(2,0) DEFAULT 3,
    used_free_float                NUMBER(1,0) DEFAULT 0,
    used_capped_ratio              NUMBER(1,0) DEFAULT 0,
    method_calulated               NUMBER(1,0) DEFAULT 1,
    industry_id                    NUMBER DEFAULT -1,
    open_index                     NUMBER DEFAULT 0,
    close_index                    NUMBER DEFAULT 0,
    param_cap_id                   NUMBER(10,0) DEFAULT 0,
    param_cap_key                  NUMBER(10,0) DEFAULT 0,
    method_cap                     NUMBER(1,0) DEFAULT 1,
    base_date                      DATE,
    base_divide_fisher             NUMBER DEFAULT 0,
    pt_total_qtty                  NUMBER,
    pt_total_value                 NUMBER,
    boardcode                      VARCHAR2(20 BYTE),
    is_calculate_tri               NUMBER(1,0) DEFAULT 0,
    tri_value                      NUMBER DEFAULT 0,
    return_one_day                 NUMBER DEFAULT 0,
    return_mtd                     NUMBER DEFAULT 0,
    return_qtd                     NUMBER DEFAULT 0,
    return_ytd                     NUMBER DEFAULT 0,
    is_calculate_dpi               NUMBER DEFAULT 0,
    dpi_value                      NUMBER DEFAULT 0,
    ff_marketcap                   NUMBER DEFAULT 0,
    prev_ff_marketcap              NUMBER DEFAULT 0,
    prev_tri_value                 NUMBER DEFAULT 0,
    open_tri                       NUMBER DEFAULT 0,
    close_tri                      NUMBER DEFAULT 0,
    highest_tri                    NUMBER DEFAULT 0,
    lowest_tri                     NUMBER DEFAULT 0,
    change_tri                     NUMBER DEFAULT 0,
    pct_tri                        NUMBER DEFAULT 0,
    prev_dpi                       NUMBER DEFAULT 0,
    basedate_tri                   DATE,
    basepoint_tri                  NUMBER,
    basedate_dpi                   DATE,
    basepoint_dpi                  NUMBER,
    return_one_day_pri             NUMBER DEFAULT 0,
    return_mtd_pri                 NUMBER DEFAULT 0,
    return_qtd_pri                 NUMBER DEFAULT 0,
    return_ytd_pri                 NUMBER DEFAULT 0,
    notraded                       NUMBER,
    up_ceiling                     NUMBER,
    down_floor                     NUMBER,
    is_scale_idx_normal            NUMBER,
    tri_code                       VARCHAR2(30 BYTE),
    dpi_code                       VARCHAR2(30 BYTE),
    annualized_1y_pri              NUMBER(10,5),
    annualized_3y_pri              NUMBER(10,5),
    annualized_5y_pri              NUMBER(10,5),
    annualized_1y_tri              NUMBER(10,5),
    annualized_3y_tri              NUMBER(10,5),
    annualized_5y_tri              NUMBER(10,5))
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





-- End of DDL Script for Table STOCK_DR.IDX_INDEX_INFO

-- Start of DDL Script for Table STOCK_DR.IDX_STOCKS_INFO
-- Generated 11-Dec-2018 11:10:45 from STOCK_DR@PM01_DR

CREATE TABLE idx_stocks_info
    (stock_id                       NUMBER(*,0) NOT NULL,
    trading_date                   DATE NOT NULL,
    time                           VARCHAR2(30 BYTE),
    code                           VARCHAR2(50 BYTE) NOT NULL,
    stock_type                     NUMBER(*,0) NOT NULL,
    total_listing_qtty             NUMBER DEFAULT 0,
    total_limit_qtty               NUMBER DEFAULT 0,
    adjust_qtty                    NUMBER DEFAULT 0,
    adjust_limit_qtty              NUMBER DEFAULT 0,
    free_float_rate                NUMBER(10,5) DEFAULT 0,
    basic_price                    NUMBER DEFAULT 0,
    ceiling_price                  NUMBER DEFAULT 0,
    floor_price                    NUMBER DEFAULT 0,
    open_price                     NUMBER DEFAULT 0,
    close_price                    NUMBER DEFAULT 0,
    average_price                  NUMBER,
    index_price                    NUMBER,
    total_offer_qtty               NUMBER(*,0) DEFAULT 0,
    total_bid_qtty                 NUMBER(*,0) DEFAULT 0,
    prior_price                    NUMBER DEFAULT 0,
    prior_close_price              NUMBER DEFAULT 0,
    name                           VARCHAR2(250 BYTE),
    parvalue                       NUMBER DEFAULT 0,
    floor_code                     VARCHAR2(30 BYTE),
    is_calcindex                   NUMBER(1,0),
    date_no                        NUMBER,
    nm_total_traded_qtty           NUMBER,
    nm_total_traded_value          NUMBER,
    pt_total_traded_qtty           NUMBER,
    pt_total_traded_value          NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    date_created                   DATE DEFAULT NULL,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    status                         NUMBER DEFAULT 0,
    flag                           NUMBER DEFAULT 0,
    origin_free_float_rate         NUMBER(*,2) DEFAULT 0,
    industry_code                  VARCHAR2(20 BYTE) DEFAULT '00000',
    prev_index_price               NUMBER DEFAULT 0,
    listing_date                   DATE,
    cancel_listing_date            DATE,
    index_price_change             NUMBER DEFAULT 0,
    prev_basic_price               NUMBER DEFAULT 0,
    total_req_qtty                 NUMBER DEFAULT 0,
    recive_index_price             NUMBER DEFAULT 0,
    recive_basic_price             NUMBER DEFAULT 0,
    index_price_backup             NUMBER DEFAULT 0,
    board_id                       NUMBER,
    flag_25                        NUMBER DEFAULT 0,
    stock_exchange                 VARCHAR2(10 BYTE),
    listing_status                 NUMBER(2,0),
    money_dividend                 NUMBER,
    hsx_idstocks                   NUMBER,
    reference_status               VARCHAR2(20 BYTE),
    free_float_rate_old            NUMBER(*,2),
    total_req_qtty_core            NUMBER,
    begin_idxprice                 NUMBER)
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     262144
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- End of DDL Script for Table STOCK_DR.IDX_STOCKS_INFO

-- Start of DDL Script for Table STOCK_DR.LOG_ERROR
-- Generated 11-Dec-2018 11:10:46 from STOCK_DR@PM01_DR

CREATE TABLE log_error
    (log_timestamp                  DATE NOT NULL,
    error_message                  VARCHAR2(2000 BYTE),
    package_name                   VARCHAR2(50 BYTE))
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





-- End of DDL Script for Table STOCK_DR.LOG_ERROR

-- Start of DDL Script for Table STOCK_DR.MT_USERS
-- Generated 11-Dec-2018 11:10:46 from STOCK_DR@PM01_DR

CREATE TABLE mt_users
    (id                             NUMBER NOT NULL,
    traderid                       NUMBER,
    username                       NVARCHAR2(20),
    fullname                       NVARCHAR2(100),
    type                           VARCHAR2(1 BYTE),
    password                       VARCHAR2(50 BYTE),
    ip_address                     VARCHAR2(100 BYTE),
    group_id                       NUMBER(5,0),
    status                         VARCHAR2(1 BYTE),
    lastlogin                      VARCHAR2(25 BYTE),
    onlinests                      VARCHAR2(100 BYTE) DEFAULT 'OF',
    notes                          NVARCHAR2(250),
    deleted                        NUMBER(1,0) DEFAULT 0 NOT NULL,
    created_by                     VARCHAR2(20 BYTE),
    created_date                   DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE,
    is_reset_pass                  NUMBER(1,0) DEFAULT 1,
    last_update_pass               DATE,
    active_date                    DATE,
    expire_date                    DATE,
    is_re_set_right                NUMBER DEFAULT 0,
    machine_login                  VARCHAR2(200 BYTE))
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





-- Constraints for MT_USERS

ALTER TABLE mt_users
ADD CONSTRAINT id_mt_users UNIQUE (id)
USING INDEX
  PCTFREE     10
  INITRANS    2
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
/


-- Comments for MT_USERS

COMMENT ON COLUMN mt_users.active_date IS 'Ngay bat dau bi ky luat. Ap dung cho user la dai dien giao dich'
/
COMMENT ON COLUMN mt_users.expire_date IS 'Ngay het han ky luat. Ap dung cho user la dai dien giao dich'
/
COMMENT ON COLUMN mt_users.fullname IS 'TEN DIEN GIAI'
/
COMMENT ON COLUMN mt_users.id IS 'ID. SO TU TANG'
/
COMMENT ON COLUMN mt_users.is_re_set_right IS 'so lan thay doi quyen cho user, de xem co lg out khi phan lai quyen hay ko'
/
COMMENT ON COLUMN mt_users.is_reset_pass IS 'co de biet la moi reset pass'
/
COMMENT ON COLUMN mt_users.last_update_pass IS 'ngay cap nhat mat khau cuoi'
/
COMMENT ON COLUMN mt_users.lastlogin IS 'LAN TRUY CAP GAN NHAT'
/
COMMENT ON COLUMN mt_users.machine_login IS 'ten may dang nhap lan cuoi'
/
COMMENT ON COLUMN mt_users.onlinests IS 'TRANG THAI ONLINE/OFFLINE'
/
COMMENT ON COLUMN mt_users.status IS 'TRANG THAI: HIEU LUC(A); KHONG HIEU LUC(C)'
/
COMMENT ON COLUMN mt_users.traderid IS 'ID LIEN KET VOI BANG TRADERS'
/
COMMENT ON COLUMN mt_users.type IS 'LOAI USER:  1- DDGD_TX;2-Chuyen Vien ; 3-DDGD_TT;'
/
COMMENT ON COLUMN mt_users.username IS 'MA TRUY CAP'
/

-- End of DDL Script for Table STOCK_DR.MT_USERS

-- Start of DDL Script for Table STOCK_DR.TEMP_CURRENT_SYMBOL_PRICE
-- Generated 11-Dec-2018 11:10:46 from STOCK_DR@PM01_DR

CREATE TABLE temp_current_symbol_price
    (stock_id                       NUMBER(15,5) NOT NULL,
    order_price                    NUMBER,
    order_qtty                     NUMBER)
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





-- End of DDL Script for Table STOCK_DR.TEMP_CURRENT_SYMBOL_PRICE

-- Start of DDL Script for Table STOCK_DR.TEMP_SYMBOL_INFO
-- Generated 11-Dec-2018 11:10:47 from STOCK_DR@PM01_DR

CREATE TABLE temp_symbol_info
    (stock_id                       NUMBER NOT NULL,
    symbol                         VARCHAR2(30 BYTE),
    max_price_buy                  NUMBER,
    max_qtty_buy                   NUMBER,
    max_price_sell                 NUMBER,
    max_qtty_sell                  NUMBER,
    match_price                    NUMBER,
    match_qtty                     NUMBER,
    match_value                    NUMBER,
    pt_match_price                 NUMBER,
    pt_match_qtty                  NUMBER,
    excute_rdlot_price             NUMBER,
    excute_oddlot_price            NUMBER,
    klkchan_gannhat                NUMBER,
    klkle_gannhat                  NUMBER,
    gtkchan_gannhat                NUMBER,
    gtkle_gannhat                  NUMBER,
    current_price                  NUMBER,
    current_qtty                   NUMBER)
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





-- Constraints for TEMP_SYMBOL_INFO

ALTER TABLE temp_symbol_info
ADD CHECK ("SYMBOL" IS NOT NULL)
/


-- End of DDL Script for Table STOCK_DR.TEMP_SYMBOL_INFO

-- Start of DDL Script for Table STOCK_DR.TS_BOARDS
-- Generated 11-Dec-2018 11:10:47 from STOCK_DR@PM01_DR

CREATE TABLE ts_boards
    (id                             NUMBER NOT NULL,
    name                           NVARCHAR2(100),
    short_name                     NVARCHAR2(100),
    code                           NVARCHAR2(30),
    idmarket                       NUMBER,
    idcbrule                       NUMBER,
    status                         VARCHAR2(1 BYTE) DEFAULT 'A' NOT NULL,
    idschedule                     NUMBER,
    idtradingrule                  NUMBER,
    floor_allow                    VARCHAR2(1 BYTE) DEFAULT 'N',
    remote_allow                   VARCHAR2(1 BYTE) DEFAULT 'N',
    online_allow                   VARCHAR2(1 BYTE) DEFAULT 'N',
    prolong_allow                  VARCHAR2(1 BYTE) DEFAULT 'N',
    random_end                     VARCHAR2(1 BYTE) DEFAULT 'N',
    deleted                        NUMBER(1,0) DEFAULT 0 NOT NULL,
    create_by                      VARCHAR2(20 BYTE),
    create_date                    DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE,
    prolong_match                  VARCHAR2(1 BYTE),
    prolong_time                   NUMBER,
    num_non_trading                NUMBER DEFAULT 25 NOT NULL,
    name_index                     VARCHAR2(100 BYTE),
    def_tradingrule_25             NUMBER DEFAULT 0 NOT NULL,
    def_tradingrule_nym            NUMBER DEFAULT 0 NOT NULL,
    def_tradingrule_sap_huy_ny     NUMBER DEFAULT 0 NOT NULL,
    def_schedule_rule_25           NUMBER DEFAULT 0 NOT NULL,
    def_schedule_rule_nym          NUMBER DEFAULT 0 NOT NULL,
    def_schedule_rule_sap_huy_ny   NUMBER DEFAULT 0 NOT NULL,
    date_no                        NUMBER DEFAULT 0 NOT NULL)
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





-- Comments for TS_BOARDS

COMMENT ON COLUMN ts_boards.code IS 'MA Bang'
/
COMMENT ON COLUMN ts_boards.date_no IS 'Phien giao dich thu'
/
COMMENT ON COLUMN ts_boards.def_schedule_rule_25 IS 'KET CAU PHIEN DEF 25 PHIEN (ID LIEN KET VOI BANG TRADING_RULES)'
/
COMMENT ON COLUMN ts_boards.def_schedule_rule_nym IS 'KET CAU PHIEN DEF NIEM YET MOI (ID LIEN KET VOI BANG TRADING_RULES)'
/
COMMENT ON COLUMN ts_boards.def_schedule_rule_sap_huy_ny IS 'KET CAU PHIEN DEF SAP HUY NIEM YET (ID LIEN KET VOI BANG TRADING_RULES)'
/
COMMENT ON COLUMN ts_boards.def_tradingrule_25 IS 'RULE DEF 25 PHIEN (ID LIEN KET VOI BANG TRADING_RULES)'
/
COMMENT ON COLUMN ts_boards.def_tradingrule_nym IS 'RULE DEF NIEM YET MOI (ID LIEN KET VOI BANG TRADING_RULES)'
/
COMMENT ON COLUMN ts_boards.def_tradingrule_sap_huy_ny IS 'RULE DEF SAP HUY NIEM YET (ID LIEN KET VOI BANG TRADING_RULES)'
/
COMMENT ON COLUMN ts_boards.floor_allow IS '(Bo)CHO PHEP GIAO DICH TAI SAN KHONG? CO(Y); KHONG(N)'
/
COMMENT ON COLUMN ts_boards.idcbrule IS '(Bo)MAP VOI ID CUA BANG CIRCUIT BREAKER; =0: NEU NHU KO AP DUNG CB'
/
COMMENT ON COLUMN ts_boards.idmarket IS 'IDMARKET LIEN KET VOI BANG MARKET'
/
COMMENT ON COLUMN ts_boards.idschedule IS 'ID CUA BANG SCHEDULES XAC DINH'
/
COMMENT ON COLUMN ts_boards.idtradingrule IS 'ID LIEN KET VOI BANG TRADING_RULES'
/
COMMENT ON COLUMN ts_boards.name IS 'TEN TO bang'
/
COMMENT ON COLUMN ts_boards.name_index IS 'Ten chi so'
/
COMMENT ON COLUMN ts_boards.num_non_trading IS 'tham so dac biet 25 phien'
/
COMMENT ON COLUMN ts_boards.online_allow IS '(Bo)CHO PHEP GIAO DICH TRUC TUYEN KHONG? CO(Y); KHONG (N)'
/
COMMENT ON COLUMN ts_boards.prolong_allow IS '(Bo)CHO PHEP AP DUNG KEO DAI GIAO DICH CHO CAC CK KHONG CO GIAO DICH KHONG? CO(Y); KHONG(N)'
/
COMMENT ON COLUMN ts_boards.prolong_match IS '(Bo)'
/
COMMENT ON COLUMN ts_boards.prolong_time IS '(Bo)'
/
COMMENT ON COLUMN ts_boards.random_end IS '(Bo)CHO PHEP AP DUNG KET THUC NGAU NHIEN PHIEN KHOP LENH DINH KY DOI VOI RIENG TUNG MA CHUNG KHOAN CO BIEU HIEN LAM GIA DE NGAN CHAN LAM GIA? CO(Y); KHONG(N)'
/
COMMENT ON COLUMN ts_boards.remote_allow IS '(Bo)CHO PHEP GIAO DICH TU XA KHONG? CO(Y); KHONG(N)'
/
COMMENT ON COLUMN ts_boards.short_name IS 'TEN VIET TAT'
/
COMMENT ON COLUMN ts_boards.status IS 'TRANG THAI: NOSTARTED (0) ,NORMAL(1) ,PAUSE(2),RE(3),CB(4)'
/

-- End of DDL Script for Table STOCK_DR.TS_BOARDS

-- Start of DDL Script for Table STOCK_DR.TS_FOREIGN_ROOM
-- Generated 11-Dec-2018 11:10:47 from STOCK_DR@PM01_DR

CREATE TABLE ts_foreign_room
    (symbol                         VARCHAR2(30 BYTE) NOT NULL,
    foreignrate                    NUMBER,
    deleted                        NUMBER(1,0) NOT NULL,
    created_by                     VARCHAR2(20 BYTE),
    created_date                   DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE,
    current_room                   NUMBER NOT NULL,
    buy_match                      NUMBER NOT NULL,
    sell_match                     NUMBER NOT NULL,
    isincode                       NVARCHAR2(40),
    total_room                     NUMBER,
    frgroom_date                   DATE)
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





-- End of DDL Script for Table STOCK_DR.TS_FOREIGN_ROOM

-- Start of DDL Script for Table STOCK_DR.TS_MARKETS
-- Generated 11-Dec-2018 11:10:48 from STOCK_DR@PM01_DR

CREATE TABLE ts_markets
    (id                             NUMBER NOT NULL,
    code                           NVARCHAR2(30) NOT NULL,
    name                           NVARCHAR2(250) NOT NULL,
    idexchange                     NUMBER NOT NULL,
    idcbrule                       NUMBER DEFAULT 0,
    status                         VARCHAR2(1 BYTE) DEFAULT 'A' NOT NULL,
    idcalendar                     NUMBER NOT NULL,
    note                           NVARCHAR2(250),
    deleted                        NUMBER(1,0) DEFAULT 0 NOT NULL,
    created_by                     VARCHAR2(20 BYTE),
    created_date                   DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE,
    num_trading                    NUMBER DEFAULT 0)
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





-- Comments for TS_MARKETS

COMMENT ON COLUMN ts_markets.code IS 'CODE CUA THI TRUONG. VI DU: HNX.LISTED; HNX.UPCOM; HNX.BOND'
/
COMMENT ON COLUMN ts_markets.id IS 'MA THI TRUONG. SO TU TANG THEO SEQ'
/
COMMENT ON COLUMN ts_markets.idcalendar IS 'ID BANG CALENDARS  LICH LIEN QUAN'
/
COMMENT ON COLUMN ts_markets.idcbrule IS 'MAP VOI ID CUA BANG CIRCUIT BREAKER=0: NEU NHU KO AP DUNG CB'
/
COMMENT ON COLUMN ts_markets.idexchange IS 'MA SGDCK LIEN KET VOI ID CUA BANG EXCHANGES'
/
COMMENT ON COLUMN ts_markets.name IS 'TEN THI TRUONG'
/
COMMENT ON COLUMN ts_markets.num_trading IS 'so phien giao dich cua thj truong'
/
COMMENT ON COLUMN ts_markets.status IS 'TRANG THAI: DANG HOAT DONG(A), NGUNG HOAT DONG(C), TAM THOI DUNG HOAT DONG(P)'
/

-- End of DDL Script for Table STOCK_DR.TS_MARKETS

-- Start of DDL Script for Table STOCK_DR.TS_MEMBERS
-- Generated 11-Dec-2018 11:10:48 from STOCK_DR@PM01_DR

CREATE TABLE ts_members
    (id                             NUMBER ,
    name                           NVARCHAR2(100),
    short_name                     NVARCHAR2(100),
    code                           NVARCHAR2(30),
    code_trade                     NVARCHAR2(30),
    type                           NUMBER,
    dorf_flag                      NUMBER,
    borf_flag                      NUMBER,
    address                        VARCHAR2(250 BYTE),
    phone                          VARCHAR2(20 BYTE),
    fax                            VARCHAR2(20 BYTE),
    telex                          VARCHAR2(20 BYTE),
    capital                        NUMBER,
    capital_rule                   NUMBER,
    status                         VARCHAR2(1 BYTE),
    deleted                        NUMBER(1,0) DEFAULT 0 NOT NULL,
    create_by                      VARCHAR2(20 BYTE),
    create_date                    DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE)
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





-- Constraints for TS_MEMBERS

ALTER TABLE ts_members
ADD CONSTRAINT pk_member_id PRIMARY KEY (id)
USING INDEX
  PCTFREE     10
  INITRANS    2
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
/


-- Comments for TS_MEMBERS

COMMENT ON COLUMN ts_members.address IS 'DIA CHI'
/
COMMENT ON COLUMN ts_members.borf_flag IS 'LOAI THANH VIEN: CONG TY CHUNG KHOAN(1), NGAN HANG LUU KY TRONG NUOC(3), NGAN HANG LUU KY NUOC NGOAI(5), SGD NHNN(6)'
/
COMMENT ON COLUMN ts_members.capital IS 'VON DIEU LE'
/
COMMENT ON COLUMN ts_members.capital_rule IS 'VON PHAP DINH'
/
COMMENT ON COLUMN ts_members.code IS 'MA LUU KY'
/
COMMENT ON COLUMN ts_members.code_trade IS 'MA GIAO DICH'
/
COMMENT ON COLUMN ts_members.dorf_flag IS 'NUOC NGOAI(2) HAY TRONG NUOC(1)'
/
COMMENT ON COLUMN ts_members.id IS 'ID TO CHUC PHAT HANH, KHOA CHINH CUA BANG'
/
COMMENT ON COLUMN ts_members.name IS 'TEN TCPH'
/
COMMENT ON COLUMN ts_members.phone IS 'SDT'
/
COMMENT ON COLUMN ts_members.short_name IS 'TEN VIET TAT'
/
COMMENT ON COLUMN ts_members.status IS 'TRANG THAI: DANG HOAT DONG(A), NGUNG HOAT DONG(C), TAM THOI DUNG HOAT DONG(P)'
/
COMMENT ON COLUMN ts_members.type IS 'LOAI THANH VIEN'
/

-- End of DDL Script for Table STOCK_DR.TS_MEMBERS

-- Start of DDL Script for Table STOCK_DR.TS_OPERATOR_MARKETS
-- Generated 11-Dec-2018 11:10:48 from STOCK_DR@PM01_DR

CREATE TABLE ts_operator_markets
    (market_id                      NUMBER NOT NULL,
    current_date                   DATE,
    status_step                    VARCHAR2(10 BYTE),
    is_backup                      NUMBER(1,0),
    is_send_close_price            NUMBER DEFAULT 0,
    is_send_basic_price            NUMBER DEFAULT 0)
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





-- End of DDL Script for Table STOCK_DR.TS_OPERATOR_MARKETS

-- Start of DDL Script for Table STOCK_DR.TS_ORDER_TYPE
-- Generated 11-Dec-2018 11:10:48 from STOCK_DR@PM01_DR

CREATE TABLE ts_order_type
    (id                             NUMBER NOT NULL,
    code                           NVARCHAR2(30),
    name                           NVARCHAR2(50) NOT NULL,
    inputcode                      NVARCHAR2(3),
    base                           VARCHAR2(1 BYTE),
    priority                       NUMBER(2,0) NOT NULL,
    is_convert                     VARCHAR2(1 BYTE),
    convert_cond                   VARCHAR2(2 BYTE),
    price_req                      VARCHAR2(2 BYTE),
    pricecalc                      VARCHAR2(2 BYTE),
    stopcond                       VARCHAR2(2 BYTE),
    defcanccond                    VARCHAR2(3 BYTE),
    deftimecond                    VARCHAR2(3 BYTE),
    deleted                        NUMBER(1,0) DEFAULT 0 NOT NULL,
    created_by                     NVARCHAR2(20),
    created_date                   DATE,
    modified_by                    NVARCHAR2(20),
    modified_date                  DATE,
    odcorrallow                    VARCHAR2(1 BYTE) NOT NULL,
    odcancallow                    VARCHAR2(1 BYTE) NOT NULL,
    convert_type                   VARCHAR2(3 BYTE) DEFAULT 'LO',
    displayqttymin                 NUMBER DEFAULT 0)
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





-- Constraints for TS_ORDER_TYPE

ALTER TABLE ts_order_type
ADD CONSTRAINT pk_ordertype_id PRIMARY KEY (id)
USING INDEX
  PCTFREE     10
  INITRANS    2
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
/


-- Comments for TS_ORDER_TYPE

COMMENT ON COLUMN ts_order_type.base IS 'LOAI LENH CO SO MA SE DUA VAO. L: LENH GIOI HAN; M: LENH THI TRUONG; S: LENH STOP; I: LENH TANG BANG'
/
COMMENT ON COLUMN ts_order_type.code IS 'MA LOAI LENH'
/
COMMENT ON COLUMN ts_order_type.convert_cond IS 'DK CHUYEN DOI. HIEU LUC KHI IS_CONVERT = Y. ES: KHI KET THUC PHIEN; SP: KHI DAT DIEU KIEN DUNG( AP DUNG VOI LENH STOP); QO: KHI LENH DUOC DUA VAO SO LENH; NM: KHI LENH DAT VAO KO KHOP(AP DUNG VOI FAS)'
/
COMMENT ON COLUMN ts_order_type.defcanccond IS 'DIEU KIEN HUY MAC DINH. KHI LENH DAT VAO NEU KHONG LUA CHON DK HUY SE LAY GIA TRI MAC DINH DA DINH NGHIA FAS/FAK/FOK'
/
COMMENT ON COLUMN ts_order_type.deftimecond IS 'DIEU KIEN THOI GIAN MAC DINH. KHI LENH DAT VAO NEU KHONG LUA CHON DIEU KIEN THOI GIAN SE LAY GIA TRI MAC DINH DA DINH NGHIA. GFD/GTS: AP DUNG; GTC/GTD: CHUA AP DUNG, NHUNG DE MO CHO TUONG LAI'
/
COMMENT ON COLUMN ts_order_type.displayqttymin IS 'KHOI LUONG DINH TOI THIEU'
/
COMMENT ON COLUMN ts_order_type.inputcode IS 'MA HIEN THI LOAI LENH KHI NHAP LENH. VD L: LIMIT,ATO,ATC; SL:STOP TO LIMIT; MP: MARKET PURE'
/
COMMENT ON COLUMN ts_order_type.is_convert IS 'LENH CO KHA NANG CHUYEN THANH LOAI LENH KHAC KHONG(AP DUNG KHI LOAI LENH HIEN TAI KHI DAT MOT DIEU KIEN DINH NGHIA TRUOC SE CHUYEN SANG THANH MOT LOAI LENH XAC DINH) Y: CO; N: KHONG'
/
COMMENT ON COLUMN ts_order_type.name IS 'TEN LOAI LENH'
/
COMMENT ON COLUMN ts_order_type.odcancallow IS 'CO DUOC PHEP HUY LENH KO?'
/
COMMENT ON COLUMN ts_order_type.odcorrallow IS 'CO DUOC PHEP SUA LENH KO?'
/
COMMENT ON COLUMN ts_order_type.price_req IS 'GIA CUA LENH DUOC XAC DINH. I: KHI NHAP LENH; Q: KHI DUA VAO SO LENH CHO KHOP; M: KHI KHOP LENH; C: KHI CHUYEN DOI LENH THANH LOAI LENH KHAC'
/
COMMENT ON COLUMN ts_order_type.pricecalc IS 'PHUONG THUC XAC DINH GIA VOI TH PRICE_REQ<>1. L: GIA KHOP GAN NHAT; LP: GIA KHOP GAN NHAT +/- 1 THANG GIA; S: GIA CUNG BEN TOT NHAT; O: GIA KHAC BEN TOT NHAT'
/
COMMENT ON COLUMN ts_order_type.priority IS 'THU TU UU TIEN KHOP LENH. UU TIEN SE XEP TU THAP LEN CAO: 1...->n'
/
COMMENT ON COLUMN ts_order_type.stopcond IS 'VOI CAC LENH CO BASE = S (STOP). DK LENH DUNG DUOC DUA VAO SO LENH CHO KHOP: L: GIA KHOP GAN NHAT (VD: DOI VOI LENH MUA THI GIA DUNG <= GIA KHOP GAN NHAT); B: GIA MUA TOT NHAT; O: GIA BAN TOT NHAT;'
/

-- End of DDL Script for Table STOCK_DR.TS_ORDER_TYPE

-- Start of DDL Script for Table STOCK_DR.TS_SECURITIES_TYPE
-- Generated 11-Dec-2018 11:10:48 from STOCK_DR@PM01_DR

CREATE TABLE ts_securities_type
    (id                             NUMBER NOT NULL,
    name                           NVARCHAR2(100),
    short_name                     NVARCHAR2(100),
    type                           VARCHAR2(2 BYTE),
    idboard                        NUMBER,
    idtradingrule                  NUMBER,
    status                         VARCHAR2(1 BYTE),
    notes                          NVARCHAR2(250),
    deleted                        NUMBER(1,0),
    created_by                     NVARCHAR2(20),
    created_date                   DATE,
    modified_by                    NVARCHAR2(20),
    modified_date                  DATE,
    is_check_room                  NUMBER)
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





-- End of DDL Script for Table STOCK_DR.TS_SECURITIES_TYPE

-- Start of DDL Script for Table STOCK_DR.TS_SYMBOL
-- Generated 11-Dec-2018 11:10:49 from STOCK_DR@PM01_DR

CREATE TABLE ts_symbol
    (id                             NUMBER NOT NULL,
    symbol                         VARCHAR2(30 BYTE) ,
    isincode                       NVARCHAR2(12),
    name                           NVARCHAR2(250) NOT NULL,
    idsectype                      NUMBER NOT NULL,
    idissuer                       NUMBER NOT NULL,
    idindustry                     NUMBER NOT NULL,
    idboard                        NUMBER,
    dvrtype                        NUMBER(2,0),
    idtradingrule                  NUMBER(*,0),
    parvalue                       NUMBER,
    issueqtty                      NUMBER,
    freefloat                      NUMBER,
    fundqtty                       NUMBER,
    listingqtty                    NUMBER,
    listingdate                    DATE,
    delistingdate                  DATE,
    issuedate                      DATE,
    maturitydate                   DATE,
    interestrate                   NUMBER,
    prevpaydate                    DATE,
    nextpaydate                    DATE,
    terms                          NUMBER,
    statedcoupon                   NUMBER,
    frequency                      NUMBER,
    expmonth                       VARCHAR2(6 BYTE),
    contractsize                   NUMBER,
    settmultiplier                 NUMBER,
    exerciseprice                  NUMBER,
    righttype                      VARCHAR2(100 BYTE),
    nearspread                     VARCHAR2(40 BYTE),
    farspread                      VARCHAR2(40 BYTE),
    adjusttype                     NUMBER,
    lasttradingday                 DATE,
    settmethod                     NUMBER,
    random_end                     VARCHAR2(1 BYTE),
    prolong_allow                  VARCHAR2(2 BYTE),
    notes                          VARCHAR2(250 BYTE),
    deleted                        NUMBER(1,0) NOT NULL,
    created_by                     VARCHAR2(20 BYTE),
    created_date                   DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE,
    celling_price                  NUMBER,
    floor_price                    NUMBER,
    basic_price                    NUMBER,
    status                         VARCHAR2(20 BYTE) NOT NULL,
    idschedule                     NUMBER,
    match_price                    NUMBER,
    totalreq                       NUMBER,
    open_price                     NUMBER,
    close_price                    NUMBER,
    listing_status                 NUMBER(2,0),
    reference_status               VARCHAR2(20 BYTE),
    idcalendar                     NUMBER,
    flag_25                        NUMBER,
    prev_basic_price               NUMBER,
    about_delistingdate            DATE,
    date_no                        NUMBER,
    prior_close_price              NUMBER,
    prior_open_price               NUMBER)
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     262144
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- Constraints for TS_SYMBOL

ALTER TABLE ts_symbol
ADD CHECK ("SYMBOL" IS NOT NULL)
/


-- End of DDL Script for Table STOCK_DR.TS_SYMBOL

-- Start of DDL Script for Table STOCK_DR.TS_SYMBOL_CALENDAR
-- Generated 11-Dec-2018 11:10:49 from STOCK_DR@PM01_DR

CREATE TABLE ts_symbol_calendar
    (id                             NUMBER NOT NULL,
    symbol_id                      NUMBER NOT NULL,
    current_status                 NUMBER(2,0) DEFAULT 0,
    current_trading_schedule_id    NUMBER DEFAULT 0,
    deleted                        NUMBER(1,0) DEFAULT 0 NOT NULL,
    current_trading_state          NUMBER DEFAULT 0,
    pre_status_pause               NUMBER(2,0),
    last_action_time               DATE,
    first_trading_schedule_id      NUMBER,
    first_current_status           NUMBER DEFAULT 90)
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





-- End of DDL Script for Table STOCK_DR.TS_SYMBOL_CALENDAR

-- Start of DDL Script for Table STOCK_DR.TS_SYMBOL_LUAN
-- Generated 11-Dec-2018 11:10:49 from STOCK_DR@PM01_DR

CREATE TABLE ts_symbol_luan
    (id                             NUMBER NOT NULL,
    symbol                         VARCHAR2(30 BYTE) ,
    isincode                       NVARCHAR2(12),
    name                           NVARCHAR2(250) NOT NULL,
    idsectype                      NUMBER NOT NULL,
    idissuer                       NUMBER NOT NULL,
    idindustry                     NUMBER NOT NULL,
    idboard                        NUMBER,
    dvrtype                        NUMBER(2,0),
    idtradingrule                  NUMBER(*,0),
    parvalue                       NUMBER,
    issueqtty                      NUMBER,
    freefloat                      NUMBER,
    fundqtty                       NUMBER,
    listingqtty                    NUMBER,
    listingdate                    DATE,
    delistingdate                  DATE,
    issuedate                      DATE,
    maturitydate                   DATE,
    interestrate                   NUMBER,
    prevpaydate                    DATE,
    nextpaydate                    DATE,
    terms                          NUMBER,
    statedcoupon                   NUMBER,
    frequency                      NUMBER,
    expmonth                       VARCHAR2(6 BYTE),
    contractsize                   NUMBER,
    settmultiplier                 NUMBER,
    exerciseprice                  NUMBER,
    righttype                      VARCHAR2(100 BYTE),
    nearspread                     VARCHAR2(40 BYTE),
    farspread                      VARCHAR2(40 BYTE),
    adjusttype                     NUMBER,
    lasttradingday                 DATE,
    settmethod                     NUMBER,
    random_end                     VARCHAR2(1 BYTE),
    prolong_allow                  VARCHAR2(2 BYTE),
    notes                          VARCHAR2(250 BYTE),
    deleted                        NUMBER(1,0) NOT NULL,
    created_by                     VARCHAR2(20 BYTE),
    created_date                   DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE,
    celling_price                  NUMBER,
    floor_price                    NUMBER,
    basic_price                    NUMBER,
    status                         VARCHAR2(20 BYTE) NOT NULL,
    idschedule                     NUMBER,
    match_price                    NUMBER,
    totalreq                       NUMBER,
    open_price                     NUMBER,
    close_price                    NUMBER,
    listing_status                 NUMBER(2,0),
    reference_status               VARCHAR2(20 BYTE),
    idcalendar                     NUMBER,
    flag_25                        NUMBER,
    prev_basic_price               NUMBER,
    about_delistingdate            DATE,
    date_no                        NUMBER,
    prior_close_price              NUMBER,
    prior_open_price               NUMBER)
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





-- Constraints for TS_SYMBOL_LUAN

ALTER TABLE ts_symbol_luan
ADD CHECK ("SYMBOL" IS NOT NULL)
/


-- End of DDL Script for Table STOCK_DR.TS_SYMBOL_LUAN

-- Start of DDL Script for Table STOCK_DR.TS_TRADING_CALENDARS
-- Generated 11-Dec-2018 11:10:49 from STOCK_DR@PM01_DR

CREATE TABLE ts_trading_calendars
    (id                             NUMBER,
    boardid                        NUMBER(20,0),
    working_day                    DATE,
    name                           NVARCHAR2(100),
    is_beginday                    NUMBER(1,0),
    is_endday                      NUMBER(1,0),
    is_openmarket                  NUMBER(1,0),
    is_closemarket                 NUMBER(1,0),
    is_netting                     NUMBER(1,0),
    is_reciveinfo                  NUMBER(1,0),
    is_hasopenclose                NUMBER(1,0),
    current_status                 NUMBER(2,0),
    session_num                    NUMBER(*,0),
    is_workingday                  NUMBER(1,0),
    next_date                      DATE,
    previous_date                  DATE,
    processrunning                 NUMBER(1,0),
    notes                          NVARCHAR2(255),
    deleted                        NUMBER(1,0) DEFAULT 0 NOT NULL,
    created_by                     VARCHAR2(20 BYTE),
    created_date                   DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE,
    trading_schedule_id            NUMBER NOT NULL,
    pre_status_pause               NUMBER(2,0),
    last_action_time               DATE)
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





-- End of DDL Script for Table STOCK_DR.TS_TRADING_CALENDARS

-- Start of DDL Script for Table STOCK_DR.TS_TRADING_SCHEDULES
-- Generated 11-Dec-2018 11:10:49 from STOCK_DR@PM01_DR

CREATE TABLE ts_trading_schedules
    (id                             NUMBER NOT NULL,
    name                           NVARCHAR2(100),
    code                           NVARCHAR2(30),
    start_time                     VARCHAR2(20 BYTE),
    finish_time                    VARCHAR2(20 BYTE),
    idschdrule                     NUMBER,
    idstate                        NUMBER,
    aorm                           VARCHAR2(1 BYTE),
    session_no                     NUMBER(1,0),
    status                         VARCHAR2(1 BYTE) NOT NULL,
    executed_by                    NVARCHAR2(20),
    notes                          NVARCHAR2(255),
    deleted                        NUMBER(1,0) NOT NULL,
    created_by                     NVARCHAR2(20),
    created_date                   DATE,
    modified_by                    VARCHAR2(20 BYTE),
    modified_date                  DATE)
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





-- End of DDL Script for Table STOCK_DR.TS_TRADING_SCHEDULES

-- Start of DDL Script for Table STOCK_DR.TT_ORDERS
-- Generated 11-Dec-2018 11:10:49 from STOCK_DR@PM01_DR

CREATE TABLE tt_orders
    (order_id                       NUMBER NOT NULL,
    org_order_id                   NUMBER,
    floor_code                     VARCHAR2(20 BYTE) NOT NULL,
    order_confirm_no               VARCHAR2(20 BYTE),
    order_no                       VARCHAR2(20 BYTE) NOT NULL,
    co_order_no                    VARCHAR2(20 BYTE),
    org_order_no                   VARCHAR2(20 BYTE),
    order_date                     DATE,
    order_time                     VARCHAR2(25 BYTE) NOT NULL,
    member_id                      NUMBER(10,0) NOT NULL,
    co_member_id                   NUMBER(10,0),
    order_type                     NUMBER(10,0) DEFAULT 1 NOT NULL,
    priority                       NUMBER(1,0) NOT NULL,
    oorb                           NUMBER(1,0) NOT NULL,
    norp                           NUMBER(1,0) NOT NULL,
    norc                           NUMBER(2,0) NOT NULL,
    bore                           NUMBER(1,0),
    aori                           NUMBER(1,0),
    settlement_type                NUMBER(1,0),
    dorf                           NUMBER(1,0),
    order_qtty                     NUMBER,
    order_price                    NUMBER,
    status                         NUMBER(2,0),
    quote_price                    NUMBER DEFAULT 0,
    state                          NUMBER(1,0) DEFAULT 0,
    quote_time                     VARCHAR2(25 BYTE),
    quote_qtty                     NUMBER,
    exec_qtty                      NUMBER DEFAULT 0,
    correct_qtty                   NUMBER,
    cancel_qtty                    NUMBER,
    reject_qtty                    NUMBER,
    reject_reason                  NUMBER(2,0),
    account_no                     VARCHAR2(15 BYTE),
    co_account_no                  VARCHAR2(15 BYTE),
    broker_id                      NUMBER,
    co_broker_id                   NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    telephone                      VARCHAR2(50 BYTE) DEFAULT 0,
    org_order_base                 VARCHAR2(2 BYTE),
    settle_day                     NUMBER(2,0),
    aorc                           NUMBER(1,0),
    yieldmat                       NUMBER,
    order_qtty_display             NUMBER,
    order_price_stop               NUMBER,
    clordid                        VARCHAR2(100 BYTE) DEFAULT NULL,
    noro                           VARCHAR2(1 BYTE) DEFAULT '1',
    stock_id                       NUMBER(15,5) NOT NULL,
    sub_order_no                   VARCHAR2(20 BYTE),
    org_order_type                 NUMBER,
    trading_schedule_id            NUMBER,
    member_code_adv                VARCHAR2(100 BYTE),
    co_dorf                        NUMBER(1,0),
    current_trading_schedule_id    NUMBER,
    co_sub_order_no                VARCHAR2(30 BYTE),
    real_correct_qtty              NUMBER,
    idmarket                       NUMBER,
    group_name                     VARCHAR2(100 BYTE),
    numseq_save                    NUMBER(12,0))
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     33554432
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- End of DDL Script for Table STOCK_DR.TT_ORDERS

-- Start of DDL Script for Table STOCK_DR.TT_ORDERS_PLUS
-- Generated 11-Dec-2018 11:10:50 from STOCK_DR@PM01_DR

CREATE TABLE tt_orders_plus
    (order_id                       NUMBER NOT NULL,
    org_order_id                   NUMBER,
    floor_code                     VARCHAR2(20 BYTE) NOT NULL,
    order_confirm_no               VARCHAR2(20 BYTE),
    order_no                       VARCHAR2(20 BYTE) NOT NULL,
    co_order_no                    VARCHAR2(20 BYTE),
    org_order_no                   VARCHAR2(20 BYTE),
    order_date                     DATE,
    order_time                     VARCHAR2(25 BYTE) NOT NULL,
    member_id                      NUMBER(10,0) NOT NULL,
    co_member_id                   NUMBER(10,0),
    order_type                     NUMBER(10,0) DEFAULT 1 NOT NULL,
    priority                       NUMBER(1,0) NOT NULL,
    oorb                           NUMBER(1,0) NOT NULL,
    norp                           NUMBER(1,0) NOT NULL,
    norc                           NUMBER(2,0) NOT NULL,
    bore                           NUMBER(1,0),
    aori                           NUMBER(1,0),
    settlement_type                NUMBER(1,0),
    dorf                           NUMBER(1,0),
    order_qtty                     NUMBER,
    order_price                    NUMBER,
    status                         NUMBER(2,0),
    quote_price                    NUMBER DEFAULT 0,
    state                          NUMBER(1,0) DEFAULT 0,
    quote_time                     VARCHAR2(25 BYTE),
    quote_qtty                     NUMBER,
    exec_qtty                      NUMBER DEFAULT 0,
    correct_qtty                   NUMBER,
    cancel_qtty                    NUMBER,
    reject_qtty                    NUMBER,
    reject_reason                  NUMBER(2,0),
    account_no                     VARCHAR2(15 BYTE),
    co_account_no                  VARCHAR2(15 BYTE),
    broker_id                      NUMBER,
    co_broker_id                   NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    telephone                      VARCHAR2(50 BYTE) DEFAULT 0,
    org_order_base                 VARCHAR2(2 BYTE),
    settle_day                     NUMBER(2,0),
    aorc                           NUMBER(1,0),
    yieldmat                       NUMBER,
    order_qtty_display             NUMBER,
    order_price_stop               NUMBER,
    clordid                        VARCHAR2(100 BYTE) DEFAULT NULL,
    noro                           VARCHAR2(1 BYTE) DEFAULT '1',
    stock_id                       NUMBER(15,5) NOT NULL,
    sub_order_no                   VARCHAR2(20 BYTE),
    org_order_type                 NUMBER,
    trading_schedule_id            NUMBER,
    member_code_adv                VARCHAR2(100 BYTE),
    co_dorf                        NUMBER(1,0),
    current_trading_schedule_id    NUMBER,
    co_sub_order_no                VARCHAR2(30 BYTE),
    real_correct_qtty              NUMBER,
    idmarket                       NUMBER,
    group_name                     VARCHAR2(100 BYTE),
    numseq_save                    NUMBER(12,0))
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     12582912
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- End of DDL Script for Table STOCK_DR.TT_ORDERS_PLUS

-- Start of DDL Script for Table STOCK_DR.TT_ORDERS_PLUS_LUAN
-- Generated 11-Dec-2018 11:10:50 from STOCK_DR@PM01_DR

CREATE TABLE tt_orders_plus_luan
    (order_id                       NUMBER NOT NULL,
    org_order_id                   NUMBER,
    floor_code                     VARCHAR2(20 BYTE) NOT NULL,
    order_confirm_no               VARCHAR2(20 BYTE),
    order_no                       VARCHAR2(20 BYTE) NOT NULL,
    co_order_no                    VARCHAR2(20 BYTE),
    org_order_no                   VARCHAR2(20 BYTE),
    order_date                     DATE,
    order_time                     VARCHAR2(25 BYTE) NOT NULL,
    member_id                      NUMBER(10,0) NOT NULL,
    co_member_id                   NUMBER(10,0),
    order_type                     NUMBER(10,0) NOT NULL,
    priority                       NUMBER(1,0) NOT NULL,
    oorb                           NUMBER(1,0) NOT NULL,
    norp                           NUMBER(1,0) NOT NULL,
    norc                           NUMBER(2,0) NOT NULL,
    bore                           NUMBER(1,0),
    aori                           NUMBER(1,0),
    settlement_type                NUMBER(1,0),
    dorf                           NUMBER(1,0),
    order_qtty                     NUMBER,
    order_price                    NUMBER,
    status                         NUMBER(2,0),
    quote_price                    NUMBER,
    state                          NUMBER(1,0),
    quote_time                     VARCHAR2(25 BYTE),
    quote_qtty                     NUMBER,
    exec_qtty                      NUMBER,
    correct_qtty                   NUMBER,
    cancel_qtty                    NUMBER,
    reject_qtty                    NUMBER,
    reject_reason                  NUMBER(1,0),
    account_no                     VARCHAR2(15 BYTE),
    co_account_no                  VARCHAR2(15 BYTE),
    broker_id                      NUMBER,
    co_broker_id                   NUMBER,
    deleted                        NUMBER(1,0),
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    telephone                      VARCHAR2(50 BYTE),
    org_order_base                 VARCHAR2(2 BYTE),
    settle_day                     NUMBER(2,0),
    aorc                           NUMBER(1,0),
    yieldmat                       NUMBER,
    order_qtty_display             NUMBER,
    order_price_stop               NUMBER,
    clordid                        VARCHAR2(100 BYTE),
    noro                           VARCHAR2(1 BYTE),
    stock_id                       NUMBER(15,5) NOT NULL,
    sub_order_no                   VARCHAR2(20 BYTE),
    org_order_type                 NUMBER,
    trading_schedule_id            NUMBER,
    member_code_adv                VARCHAR2(100 BYTE),
    co_dorf                        NUMBER(1,0),
    current_trading_schedule_id    NUMBER,
    co_sub_order_no                VARCHAR2(30 BYTE),
    real_correct_qtty              NUMBER,
    idmarket                       NUMBER,
    group_name                     VARCHAR2(100 BYTE),
    numseq_save                    NUMBER(12,0))
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





-- End of DDL Script for Table STOCK_DR.TT_ORDERS_PLUS_LUAN

-- Start of DDL Script for Table STOCK_DR.TT_ORDERS_PLUS_TEM
-- Generated 11-Dec-2018 11:10:51 from STOCK_DR@PM01_DR

CREATE TABLE tt_orders_plus_tem
    (order_id                       NUMBER NOT NULL,
    org_order_id                   NUMBER,
    floor_code                     VARCHAR2(20 BYTE) NOT NULL,
    order_confirm_no               VARCHAR2(20 BYTE),
    order_no                       VARCHAR2(20 BYTE) NOT NULL,
    co_order_no                    VARCHAR2(20 BYTE),
    org_order_no                   VARCHAR2(20 BYTE),
    order_date                     DATE,
    order_time                     VARCHAR2(25 BYTE) NOT NULL,
    member_id                      NUMBER(10,0) NOT NULL,
    co_member_id                   NUMBER(10,0),
    order_type                     NUMBER(10,0) DEFAULT 1 NOT NULL,
    priority                       NUMBER(1,0) NOT NULL,
    oorb                           NUMBER(1,0) NOT NULL,
    norp                           NUMBER(1,0) NOT NULL,
    norc                           NUMBER(2,0) NOT NULL,
    bore                           NUMBER(1,0),
    aori                           NUMBER(1,0),
    settlement_type                NUMBER(1,0),
    dorf                           NUMBER(1,0),
    order_qtty                     NUMBER,
    order_price                    NUMBER,
    status                         NUMBER(2,0),
    quote_price                    NUMBER DEFAULT 0,
    state                          NUMBER(1,0) DEFAULT 0,
    quote_time                     VARCHAR2(25 BYTE),
    quote_qtty                     NUMBER,
    exec_qtty                      NUMBER DEFAULT 0,
    correct_qtty                   NUMBER,
    cancel_qtty                    NUMBER,
    reject_qtty                    NUMBER,
    reject_reason                  NUMBER(1,0),
    account_no                     VARCHAR2(15 BYTE),
    co_account_no                  VARCHAR2(15 BYTE),
    broker_id                      NUMBER,
    co_broker_id                   NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    telephone                      VARCHAR2(50 BYTE) DEFAULT 0,
    org_order_base                 VARCHAR2(2 BYTE),
    settle_day                     NUMBER(2,0),
    aorc                           NUMBER(1,0),
    yieldmat                       NUMBER,
    order_qtty_display             NUMBER,
    order_price_stop               NUMBER,
    clordid                        VARCHAR2(100 BYTE) DEFAULT NULL,
    noro                           VARCHAR2(1 BYTE) DEFAULT '1',
    stock_id                       NUMBER(15,5) NOT NULL,
    sub_order_no                   VARCHAR2(20 BYTE),
    org_order_type                 NUMBER,
    trading_schedule_id            NUMBER,
    member_code_adv                VARCHAR2(100 BYTE),
    co_dorf                        NUMBER(1,0),
    current_trading_schedule_id    NUMBER,
    co_sub_order_no                VARCHAR2(30 BYTE),
    real_correct_qtty              NUMBER,
    idmarket                       NUMBER,
    group_name                     VARCHAR2(100 BYTE),
    numseq_save                    NUMBER(12,0))
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     12582912
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- End of DDL Script for Table STOCK_DR.TT_ORDERS_PLUS_TEM

-- Start of DDL Script for Table STOCK_DR.TT_ORDERS_TEM
-- Generated 11-Dec-2018 11:10:51 from STOCK_DR@PM01_DR

CREATE TABLE tt_orders_tem
    (order_id                       NUMBER NOT NULL,
    org_order_id                   NUMBER,
    floor_code                     VARCHAR2(20 BYTE) NOT NULL,
    order_confirm_no               VARCHAR2(20 BYTE),
    order_no                       VARCHAR2(20 BYTE) NOT NULL,
    co_order_no                    VARCHAR2(20 BYTE),
    org_order_no                   VARCHAR2(20 BYTE),
    order_date                     DATE,
    order_time                     VARCHAR2(25 BYTE) NOT NULL,
    member_id                      NUMBER(10,0) NOT NULL,
    co_member_id                   NUMBER(10,0),
    order_type                     NUMBER(10,0) DEFAULT 1 NOT NULL,
    priority                       NUMBER(1,0) NOT NULL,
    oorb                           NUMBER(1,0) NOT NULL,
    norp                           NUMBER(1,0) NOT NULL,
    norc                           NUMBER(2,0) NOT NULL,
    bore                           NUMBER(1,0),
    aori                           NUMBER(1,0),
    settlement_type                NUMBER(1,0),
    dorf                           NUMBER(1,0),
    order_qtty                     NUMBER,
    order_price                    NUMBER,
    status                         NUMBER(2,0),
    quote_price                    NUMBER DEFAULT 0,
    state                          NUMBER(1,0) DEFAULT 0,
    quote_time                     VARCHAR2(25 BYTE),
    quote_qtty                     NUMBER,
    exec_qtty                      NUMBER DEFAULT 0,
    correct_qtty                   NUMBER,
    cancel_qtty                    NUMBER,
    reject_qtty                    NUMBER,
    reject_reason                  NUMBER(1,0),
    account_no                     VARCHAR2(15 BYTE),
    co_account_no                  VARCHAR2(15 BYTE),
    broker_id                      NUMBER,
    co_broker_id                   NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    telephone                      VARCHAR2(50 BYTE) DEFAULT 0,
    org_order_base                 VARCHAR2(2 BYTE),
    settle_day                     NUMBER(2,0),
    aorc                           NUMBER(1,0),
    yieldmat                       NUMBER,
    order_qtty_display             NUMBER,
    order_price_stop               NUMBER,
    clordid                        VARCHAR2(100 BYTE) DEFAULT NULL,
    noro                           VARCHAR2(1 BYTE) DEFAULT '1',
    stock_id                       NUMBER(15,5) NOT NULL,
    sub_order_no                   VARCHAR2(20 BYTE),
    org_order_type                 NUMBER,
    trading_schedule_id            NUMBER,
    member_code_adv                VARCHAR2(100 BYTE),
    co_dorf                        NUMBER(1,0),
    current_trading_schedule_id    NUMBER,
    co_sub_order_no                VARCHAR2(30 BYTE),
    real_correct_qtty              NUMBER,
    idmarket                       NUMBER,
    group_name                     VARCHAR2(100 BYTE),
    numseq_save                    NUMBER(12,0))
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     31457280
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- End of DDL Script for Table STOCK_DR.TT_ORDERS_TEM

-- Start of DDL Script for Table STOCK_DR.TT_ORDERS_TEMP
-- Generated 11-Dec-2018 11:10:51 from STOCK_DR@PM01_DR

CREATE TABLE tt_orders_temp
    (username                       VARCHAR2(30 BYTE) NOT NULL,
    order_id                       NUMBER NOT NULL,
    org_order_id                   NUMBER,
    floor_code                     VARCHAR2(20 BYTE) NOT NULL,
    order_confirm_no               VARCHAR2(20 BYTE),
    order_no                       VARCHAR2(20 BYTE) NOT NULL,
    co_order_no                    VARCHAR2(20 BYTE),
    org_order_no                   VARCHAR2(20 BYTE),
    order_date                     DATE,
    order_time                     VARCHAR2(25 BYTE) NOT NULL,
    member_id                      NUMBER(10,0) NOT NULL,
    co_member_id                   NUMBER(10,0),
    order_type                     NUMBER(10,0) DEFAULT 1 NOT NULL,
    priority                       NUMBER(1,0) NOT NULL,
    oorb                           NUMBER(1,0) NOT NULL,
    norp                           NUMBER(1,0) NOT NULL,
    norc                           NUMBER(2,0) NOT NULL,
    bore                           NUMBER(1,0),
    aori                           NUMBER(1,0),
    settlement_type                NUMBER(1,0),
    dorf                           NUMBER(1,0),
    order_qtty                     NUMBER,
    order_price                    NUMBER,
    status                         NUMBER(2,0),
    quote_price                    NUMBER DEFAULT 0,
    state                          NUMBER(1,0) DEFAULT 0,
    quote_time                     VARCHAR2(25 BYTE),
    quote_qtty                     NUMBER,
    exec_qtty                      NUMBER DEFAULT 0,
    correct_qtty                   NUMBER,
    cancel_qtty                    NUMBER,
    reject_qtty                    NUMBER,
    reject_reason                  NUMBER(1,0),
    account_no                     VARCHAR2(15 BYTE),
    co_account_no                  VARCHAR2(15 BYTE),
    broker_id                      NUMBER,
    co_broker_id                   NUMBER,
    deleted                        NUMBER(1,0) DEFAULT 0,
    date_created                   DATE,
    date_modified                  DATE,
    modified_by                    VARCHAR2(20 BYTE),
    created_by                     VARCHAR2(20 BYTE),
    telephone                      VARCHAR2(50 BYTE) DEFAULT 0,
    org_order_base                 VARCHAR2(2 BYTE),
    settle_day                     NUMBER(2,0),
    aorc                           NUMBER(1,0),
    yieldmat                       NUMBER,
    order_qtty_display             NUMBER,
    order_price_stop               NUMBER,
    clordid                        VARCHAR2(100 BYTE) DEFAULT NULL,
    noro                           VARCHAR2(1 BYTE) DEFAULT '1',
    stock_id                       NUMBER(15,5) NOT NULL,
    sub_order_no                   VARCHAR2(20 BYTE),
    org_order_type                 NUMBER,
    trading_schedule_id            NUMBER,
    member_code_adv                VARCHAR2(100 BYTE),
    co_dorf                        NUMBER(1,0),
    current_trading_schedule_id    NUMBER,
    idmarket                       NUMBER)
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





-- End of DDL Script for Table STOCK_DR.TT_ORDERS_TEMP

-- Start of DDL Script for Table STOCK_DR.TT_SYMBOL_INFO
-- Generated 11-Dec-2018 11:10:52 from STOCK_DR@PM01_DR

CREATE TABLE tt_symbol_info
    (id                             NUMBER NOT NULL,
    symbol_date                    DATE NOT NULL,
    symbol                         NVARCHAR2(40),
    open_price                     NUMBER DEFAULT 0,
    close_price                    NUMBER DEFAULT 0,
    avg_price                      NUMBER DEFAULT 0,
    status                         NVARCHAR2(20) NOT NULL,
    cdttg                          NUMBER DEFAULT 0,
    kldttg                         NUMBER DEFAULT 0,
    ckttg                          NUMBER DEFAULT 0,
    klkttg                         NUMBER DEFAULT 0,
    gtkttg                         NUMBER DEFAULT 0,
    cdtt                           NUMBER DEFAULT 0,
    kldtt                          NUMBER DEFAULT 0,
    gtdtt                          NUMBER DEFAULT 0,
    cktt                           NUMBER DEFAULT 0,
    klktt                          NUMBER DEFAULT 0,
    gtktt                          NUMBER DEFAULT 0,
    cdtd                           NUMBER DEFAULT 0,
    kldtd                          NUMBER DEFAULT 0,
    cktd                           NUMBER DEFAULT 0,
    klktd                          NUMBER DEFAULT 0,
    gtktd                          NUMBER DEFAULT 0,
    cdmg                           NUMBER DEFAULT 0,
    kldmg                          NUMBER DEFAULT 0,
    ckmg                           NUMBER DEFAULT 0,
    klkmg                          NUMBER DEFAULT 0,
    gtkmg                          NUMBER DEFAULT 0,
    cdchan                         NUMBER DEFAULT 0,
    kldchan                        NUMBER DEFAULT 0,
    ckchan                         NUMBER DEFAULT 0,
    klkchan                        NUMBER DEFAULT 0,
    gtkchan                        NUMBER DEFAULT 0,
    cdle                           NUMBER DEFAULT 0,
    kldle                          NUMBER DEFAULT 0,
    ckle                           NUMBER DEFAULT 0,
    klkle                          NUMBER DEFAULT 0,
    gtkle                          NUMBER DEFAULT 0,
    id_symbol                      NUMBER,
    action_time                    NVARCHAR2(100),
    max_price_execute              NUMBER DEFAULT 0,
    min_price_execute              NUMBER DEFAULT 0,
    max_price_buy                  NUMBER DEFAULT 0,
    max_price_sell                 NUMBER DEFAULT 0,
    max_qtty_buy                   NUMBER DEFAULT 0,
    max_qtty_sell                  NUMBER DEFAULT 0,
    date_no                        NUMBER DEFAULT 0,
    trading_unit                   NUMBER DEFAULT 0,
    adjust_qtty                    NUMBER DEFAULT 0,
    adjust_rate                    NUMBER DEFAULT 0,
    divident_rate                  NUMBER DEFAULT 0,
    prior_price                    NUMBER DEFAULT 0,
    prior_close_price              NUMBER DEFAULT 0,
    is_calcindex                   NUMBER DEFAULT 0,
    is_determinecl                 NUMBER DEFAULT 0,
    index_price                    NUMBER DEFAULT 0,
    prev_prior_price               NUMBER DEFAULT 0,
    prev_prior_close_price         NUMBER DEFAULT 0,
    highest_price                  NUMBER DEFAULT 0,
    lowest_price                   NUMBER DEFAULT 0,
    total_offer_qtty               NUMBER DEFAULT 0,
    total_bid_qtty                 NUMBER DEFAULT 0,
    match_qtty                     NUMBER DEFAULT 0,
    match_value                    NUMBER DEFAULT 0,
    pt_match_qtty                  NUMBER DEFAULT 0,
    pt_match_price                 NUMBER DEFAULT 0,
    current_price                  NUMBER DEFAULT 0,
    current_qtty                   NUMBER DEFAULT 0,
    excute_rdlot_price             NUMBER DEFAULT 0,
    excute_oddlot_price            NUMBER DEFAULT 0,
    klkchan_gannhat                NUMBER DEFAULT 0,
    klkle_gannhat                  NUMBER DEFAULT 0,
    gtkchan_gannhat                NUMBER DEFAULT 0,
    gtkle_gannhat                  NUMBER DEFAULT 0,
    max_price_excute_pt            NUMBER DEFAULT 0,
    min_price_excute_pt            NUMBER DEFAULT 0,
    buy_foreign_traded_qtty        NUMBER DEFAULT 0,
    sell_foreign_traded_qtty       NUMBER DEFAULT 0,
    buy_foreign_traded_value       NUMBER DEFAULT 0,
    sell_foreign_traded_value      NUMBER DEFAULT 0,
    buy_foreign_qtty_pt            NUMBER DEFAULT 0,
    buy_foreign_value_pt           NUMBER DEFAULT 0,
    sell_foreign_qtty_pt           NUMBER DEFAULT 0,
    sell_foreign_value_pt          NUMBER DEFAULT 0,
    nm_bid_count                   NUMBER DEFAULT 0,
    nm_offer_count                 NUMBER DEFAULT 0,
    pt_bid_count                   NUMBER DEFAULT 0,
    pt_offer_count                 NUMBER DEFAULT 0,
    total_pt_bid_qtty              NUMBER DEFAULT 0,
    total_pt_offer_qtty            NUMBER DEFAULT 0,
    od_max_price_excute_nm         NUMBER DEFAULT 0,
    od_min_price_excute_nm         NUMBER DEFAULT 0,
    od_max_price_excute_pt         NUMBER DEFAULT 0,
    od_min_price_excute_pt         NUMBER DEFAULT 0,
    od_total_traded_qtty_pt        NUMBER DEFAULT 0,
    od_total_traded_value_pt       NUMBER DEFAULT 0,
    match_price                    NUMBER(*,0) DEFAULT 0,
    nm_bid_count_od                NUMBER,
    nm_offer_count_od              NUMBER,
    total_bid_qtty_od              NUMBER,
    total_offer_qtty_od            NUMBER,
    seqrecvmw                      NUMBER DEFAULT 0,
    idboard                        NUMBER DEFAULT 0)
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     262144
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- End of DDL Script for Table STOCK_DR.TT_SYMBOL_INFO

-- Start of DDL Script for Table STOCK_DR.USER_INFO
-- Generated 11-Dec-2018 11:10:52 from STOCK_DR@PM01_DR

CREATE TABLE user_info
    (userid                         NUMBER,
    username                       VARCHAR2(30 BYTE),
    password                       VARCHAR2(50 BYTE),
    fullname                       NVARCHAR2(255),
    status                         NUMBER(1,0),
    activedate                     DATE,
    lastlogin                      DATE,
    note                           NVARCHAR2(255),
    islogin                        NUMBER(1,0))
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





-- End of DDL Script for Table STOCK_DR.USER_INFO

-- Start of DDL Script for Table STOCK_DR.USER_RIGHT
-- Generated 11-Dec-2018 11:10:52 from STOCK_DR@PM01_DR

CREATE TABLE user_right
    (userid                         NUMBER,
    funcid                         NUMBER,
    used                           NUMBER(1,0))
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





-- End of DDL Script for Table STOCK_DR.USER_RIGHT

-- Start of DDL Script for Table STOCK_DR.WLOG
-- Generated 11-Dec-2018 11:10:52 from STOCK_DR@PM01_DR

CREATE TABLE wlog
    (msg                            VARCHAR2(4000 BYTE),
    dates                          DATE,
    type                           VARCHAR2(100 BYTE))
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





-- End of DDL Script for Table STOCK_DR.WLOG

