-- Start of DDL Script for Table FOTEST.WLOG
-- Generated 08-Jun-2018 21:59:27 from FOTEST@DB31

CREATE TABLE wlog
    (id                             VARCHAR2(4000 BYTE),
    msg                            VARCHAR2(4000 CHAR),
    dates                          DATE,
    type                           VARCHAR2(100 CHAR))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     524288
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/

-- Start of DDL Script for Sequence LEGALTECH.SEQ_LOG
-- Generated 08/06/2018 10:01:23 PM from LEGALTECH@LOCAL

CREATE SEQUENCE seq_log
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20
/


-- End of DDL Script for Sequence LEGALTECH.SEQ_LOG


/


-- End of DDL Script for Table FOTEST.WLOG

-- Start of DDL Script for Package LEGALTECH.PKG_LOG
-- Generated 08/06/2018 10:01:13 PM from LEGALTECH@LOCAL

CREATE OR REPLACE 
PACKAGE pkg_log
 is TYPE tcursor IS REF Cursor ;
   TYPE t_array IS TABLE OF VARCHAR2(50) INDEX BY BINARY_INTEGER;

PROCEDURE set_SEQ;

PROCEDURE LOGMSG
(
    P_MSG IN NVARCHAR2
);

PROCEDURE LOGMSGALL
(
    P_MSG IN VARCHAR2
);

PROCEDURE LOG_ERROR
(
    P_MSG IN NVARCHAR2,
    P_PACKAGE_NAME IN NVARCHAR2
);


/*DATE :27.03.2017
*COMPILE TOAN BO PACKAGE, FUNCTION ,PROCEDURE  LOI LAI CHAY JOB 12H DEM
*/
PROCEDURE COMPILIE_ALL_PACKAGE_ERR ;

END;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_log
IS

PROCEDURE set_SEQ
IS
    sequence_id NUMBER;
    temp_seq NUMBER;
BEGIN

    SELECT NVL(MAX(autoid), 0) + 1 INTO sequence_id FROM securities_info;
    EXECUTE IMMEDIATE 'ALTER SEQUENCE SEQ_SECURITIES_INFO INCREMENT BY ' || sequence_id;
    SELECT seq_securities_info.NEXTVAL INTO temp_seq FROM dual;
    EXECUTE IMMEDIATE 'ALTER SEQUENCE SEQ_SECURITIES_INFO INCREMENT BY 1';

    COMMIT;
    EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
END;

-- HAM LUU LOG HE THONG PHUC VU LUU VET
PROCEDURE LOGMSG
(
    P_MSG IN NVARCHAR2
)
IS
    P_SYSDATE DATE ;
    V_SEQLOG NUMBER ;
BEGIN
    SELECT SEQ_LOG.NEXTVAL INTO V_SEQLOG FROM DUAL ;
    P_SYSDATE :=SYSDATE ;
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,P_MSG,P_SYSDATE);


EXCEPTION
WHEN OTHERS THEN
    RAISE ;
END ;

-- HAM LUU LOG HE THONG PHUC VU LUU VET
PROCEDURE LOG_ERROR
(
    P_MSG IN NVARCHAR2,
    P_PACKAGE_NAME IN NVARCHAR2
)
IS
    P_SYSDATE DATE ;
    V_SEQLOG NUMBER ;
BEGIN
    P_SYSDATE :=SYSDATE ;
    INSERT INTO log_error
    (
        log_timestamp, error_message, package_name
    )
    VALUES
    (
        P_SYSDATE,P_MSG,P_PACKAGE_NAME
    );

COMMIT ;
EXCEPTION
WHEN OTHERS THEN
    RAISE ;
END ;

PROCEDURE LOGMSGALL
(
    P_MSG IN VARCHAR2
)
IS
    P_SYSDATE DATE ;
   V_SEQLOG NUMBER ;
   V_MSG VARCHAR2(16000) DEFAULT '';
BEGIN
    SELECT SEQ_LOG.NEXTVAL INTO V_SEQLOG FROM DUAL ;
    P_SYSDATE :=SYSDATE ;

    V_MSG:=SUBSTR(P_MSG,1,4000);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);

    V_MSG:=SUBSTR(P_MSG,4001,4000);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);

    V_MSG:=SUBSTR(P_MSG,8001,4000);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);

    V_MSG:=SUBSTR(P_MSG,12001,4000);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);

    V_MSG:=SUBSTR(P_MSG,16001,4000);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);

    V_MSG:=SUBSTR(P_MSG,20001,4000);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);

--COMMIT ;
EXCEPTION
WHEN OTHERS THEN
    RAISE ;
END;



/*DATE :27.03.2017  PKG_LOG.COMPILIE_ALL_PACKAGE_ERR();
*COMPILE TOAN BO PACKAGE, FUNCTION ,PROCEDURE  LOI LAI CHAY JOB 12H DEM
*/
PROCEDURE COMPILIE_ALL_PACKAGE_ERR

IS
BEGIN

   dbms_utility.compile_schema('FOVGS',false);


   pkg_log.LOGMSG('COMPILIE_ALL_PACKAGE_ERR FOVGS');
   COMMIT ;

EXCEPTION
   WHEN OTHERS THEN
   RETURN;
END;


END;
/


-- End of DDL Script for Package LEGALTECH.PKG_LOG

/

delete  s_menu  ;
/

INSERT INTO s_menu 
VALUES(2,'<span data-menu="item-main-menu"><i class="fas fa-home"></i> Trang chủ </span>',1,'Trang chủ','<span data-menu="item-main-menu"><i class="fas fa-home"></i> Home </span>','Home',0);
INSERT INTO s_menu 
VALUES(3,'<span data-menu="item-main-menu"><i class="far fa-comment"></i> Email </span>',2,'Email','<span data-menu="item-main-menu"><i class="far fa-comment"></i> Email </span>','Email',0);
INSERT INTO s_menu 
VALUES(12,'<span data-menu="item-main-menu"  ><i class="fas fa-universal-access"></i> TradeMark </span>',4,'TradeMark','<span data-menu="item-main-menu"  ><i class="fas fa-universal-access"></i> TradeMark </span>','TradeMark',0);
INSERT INTO s_menu 
VALUES(13,'<span data-menu="item-main-menu"><i class="fas fa-universal-access"></i> Patent </span>',5,'Patent','<span data-menu="item-main-menu"><i class="fas fa-universal-access"></i> Patent </span>','Patent',0);
INSERT INTO s_menu 
VALUES(11,'<span data-menu="item-main-menu"><i class="fas fa-cog fa-fw"></i> Quản trị hệ thống </span>',3,'Quản trị hệ thống','<span data-menu="item-main-menu"><i class="fas fa-cog fa-fw"></i> System Admin </span>','System Admin',0);

/
commit ;
/

delete s_functions   ;
/
INSERT INTO s_functions 
VALUES(82,'Quản lý nhóm quyền','Quản lý nhóm quyền',1,'/vi-vn/quan-tri-he-thong/quan-ly-nhom-quyen','/quan-tri-he-thong/quan-ly-nhom-quyen/find-group',1,0,0,11,'Roles Manager','Roles Manager',0);
INSERT INTO s_functions 
VALUES(83,'Quản lý người dùng','Quản lý người dùng',1,'/vi-vn/quan-tri-he-thong/quan-ly-nguoi-dung','/quan-tri-he-thong/quan-ly-nguoi-dung/find-user',2,0,0,11,'User Manager','User Manager',0);
INSERT INTO s_functions 
VALUES(70,'Danh sách đơn','Danh sách đơn',1,'/vi-vn/trade-mark/dang-ky-nhan-hieu','/trade-mark/dang-ky-nhan-hieu',0,0,0,15,'Danh sách đơn','Danh sách đơn',0);
INSERT INTO s_functions 
VALUES(85,'(1) Quản lý đơn lưu tạm','(1) Quản lý đơn lưu tạm',1,'/vi-vn/trade-mark-mana/quan-ly-don-luu-tam','/trade-mark-mana/quan-ly-don-luu-tam',1,0,0,12,'(1) Quản lý đơn lưu tạm (Eng)','(1) Quản lý đơn lưu tạm(Eng)',0);
INSERT INTO s_functions 
VALUES(86,'(2) Quản lý đơn đã gửi - chờ phân loại','(2) Quản lý đơn đã gửi - chờ phân loại',1,'/vi-vn/trade-mark-mana/quan-ly-don-da-gui','/trade-mark-mana/quan-ly-don-da-gui',2,0,0,12,'(2) Quản lý đơn đã gửi(Eng)','(2) Quản lý đơn đã gửi(Eng)',0);
INSERT INTO s_functions 
VALUES(87,'(3) Đơn chờ luật sư xử lý','(3) Đơn chờ luật sư xử lý',1,'/vi-vn/trade-mark-mana/quan-ly-don-cho-luat-su-xu-ly','/trade-mark-mana/quan-ly-don-cho-luat-su-xu-ly',3,0,0,12,'(3) Đơn chờ luật sư xử lý (Eng)','(3) Đơn chờ luật sư xử lý(Eng)',0);
INSERT INTO s_functions 
VALUES(88,'(4) Chờ khách hàng xác nhận','(4) Chờ khách hàng xác nhận',1,'/vi-vn/trade-mark-mana/quan-ly-don-cho-kh-xac-nhan','/trade-mark-mana/quan-ly-don-cho-kh-xac-nhan',4,0,0,12,'(4) Chờ khách hàng xác nhận (Eng)','(4) Chờ khách hàng xác nhận (Eng)',0);
INSERT INTO s_functions 
VALUES(89,'(5) Xác nhận khách hàng','(5) Xác nhận khách hàng',1,'/vi-vn/trademark/quan-ly-don-luu-tam','/trademark/quan-ly-don-luu-tam',5,0,0,12,'(5) Xác nhận khách hàng(Eng)','(5) Xác nhận khách hàng(Eng)',0);
INSERT INTO s_functions 
VALUES(90,'(6) Đơn chờ nộp tới cục','(6) Đơn chờ nộp tới cục',1,'/vi-vn/trademark/quan-ly-don-da-gui','/trademark/quan-ly-don-da-gui',6,0,0,12,'(6) Đơn chờ nộp tới cục(Eng)','(6) Đơn chờ nộp tới cục(Eng)',0);
INSERT INTO s_functions 
VALUES(91,'(7) Thông báo kết quả hình thức','(7) Thông báo kết quả hình thức',1,'/vi-vn/trademark/quan-ly-don-luu-tam','/trademark/quan-ly-don-luu-tam',7,0,0,12,'(7) Thông báo kết quả hình thức(Eng)','(7) Thông báo kết quả hình thức(Eng)',0);
INSERT INTO s_functions 
VALUES(92,'(8) Thông báo kết quả nội dung','(8) Thông báo kết quả nội dung',1,'/vi-vn/trademark/quan-ly-don-da-gui','/trademark/quan-ly-don-da-gui',8,0,0,12,'(8) Thông báo kết quả nội dung(Eng)','(8) Thông báo kết quả nội dung(Eng)',0);
INSERT INTO s_functions 
VALUES(23,'Tin mới nhất','Tin mới nhất',1,'javascript:;','javascript:;',1,0,0,2,'News Hot','News Hot',0);
INSERT INTO s_functions 
VALUES(24,'SMS','SMS',1,'javascript:;','javascript:;',1,0,0,3,'SMS','SMS',0);
INSERT INTO s_functions 
VALUES(84,'(0) Danh sách đơn','(0) Danh sách đơn',1,'/vi-vn/trade-mark/dang-ky-nhan-hieu','/vi-vn/trade-mark/dang-ky-nhan-hieu',0,0,0,12,'(0) Danh sách đơn','(0) Danh sách đơn',0);

/
commit ;
/
