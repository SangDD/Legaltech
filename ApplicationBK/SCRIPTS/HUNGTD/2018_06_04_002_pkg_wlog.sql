DROP TABLE WLOG;
CREATE TABLE WLOG
    (ID                             NUMBER(20,0),
    MSG                            VARCHAR2(4000 CHAR),
    DATES                          DATE,
    TYPE                           VARCHAR2(100 CHAR))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  USERS
  STORAGE   (
    INITIAL     15728640
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/




COMMENT ON COLUMN WLOG.TYPE IS 'INFO , ERROR'
/

DROP SEQUENCE SEQ_LOG;
CREATE SEQUENCE SEQ_LOG
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 5
/


CREATE OR REPLACE 
PACKAGE PKG_LOG
 is TYPE tcursor IS REF Cursor ;
   TYPE t_array IS TABLE OF VARCHAR2(50) INDEX BY BINARY_INTEGER;

/*-------------------------CREATE BY SANGDD-----------------------------------
 * CREATE BY SANGDD :
 * CREATE DATE :28-03-2014
 * MUC DICH : LUU LOG UNG DUNG HE THONG
 *----------------------------------------------------------------------------*/
 --    raise_application_error(-20001,'An error was encountered - '||SQLCODE||' -ERROR- '||SQLERRM);
 --serror := 'N' || ': ' || SQLCODE || SQLERRM;
 -- serror := SUBSTR (serror, 1, 2000);
 --|| SUBSTR(SQLCODE ,1,500)||' -ERROR- '||SUBSTR(SQLERRM,1,500)

  PROCEDURE LOGMSG(P_MSG IN NVARCHAR2 );

  PROCEDURE LOG(P_TYPE IN VARCHAR2,P_MSG IN VARCHAR2 );


 

   PROCEDURE LOGMSGALL(P_MSG IN NVARCHAR2 );
/*
PROCEDURE proc_get_groupstock_bycus
(
p_id_cus in NUMBER,
p_cursor_dis OUT tcursor,
p_cursor out tcursor
);
*/
END;
/


CREATE OR REPLACE 
PACKAGE BODY PKG_LOG
IS

/* CREATE BY SANGDD :
 * CREATE DATE :28-03-2014
 * MUC DICH : LUU LOG UNG DUNG HE THONG
 */

  -- HAM LUU LOG HE THONG PHUC VU LUU VET
  PROCEDURE LOGMSG(P_MSG IN NVARCHAR2 )
  IS
   P_SYSDATE DATE ;
   V_SEQLOG NUMBER ;
  BEGIN
   SELECT SEQ_LOG.NEXTVAL INTO V_SEQLOG FROM DUAL ;
   P_SYSDATE :=SYSDATE ;
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,P_MSG,P_SYSDATE);
    COMMIT ;
      EXCEPTION
      WHEN others THEN
          RAISE ;
  END ;


  PROCEDURE LOG(P_TYPE IN VARCHAR2,P_MSG IN VARCHAR2 )
  IS
   P_SYSDATE DATE ;
   V_SEQLOG NUMBER ;
   V_MSG VARCHAR2(2000) DEFAULT '';
  BEGIN

   SELECT SEQ_LOG.NEXTVAL INTO V_SEQLOG FROM DUAL ;
    V_MSG:=SUBSTR(P_MSG,1,2000);
    P_SYSDATE :=SYSDATE ;
    INSERT INTO WLOG(ID,MSG,DATES,TYPE) VALUES(V_SEQLOG,P_MSG,P_SYSDATE,P_TYPE);
    COMMIT ;
      EXCEPTION
      WHEN others THEN
          RAISE ;
  END ;








 PROCEDURE LOGMSGALL(P_MSG IN NVARCHAR2 )
  IS
   P_SYSDATE DATE ;
   V_SEQLOG NUMBER ;
   V_MSG VARCHAR2(16000) DEFAULT '';
  BEGIN
   SELECT SEQ_LOG.NEXTVAL INTO V_SEQLOG FROM DUAL ;
   P_SYSDATE :=SYSDATE ;
     V_MSG:=SUBSTR(P_MSG,0,3999);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);
    V_MSG:=SUBSTR(P_MSG,4000,7900);
    INSERT INTO WLOG(ID,MSG,DATES) VALUES(V_SEQLOG,V_MSG,P_SYSDATE);
    COMMIT ;
      EXCEPTION
      WHEN others THEN
          RAISE ;
  END ;


   FUNCTION SPLIT (p_in_string VARCHAR2, p_delim VARCHAR2) RETURN t_array
   IS
        i       number :=0;
        pos     number :=0;
        lv_str  varchar2(8000) := p_in_string;
        strings t_array;
   BEGIN
      pos := instr(lv_str,p_delim,1,1);
      WHILE ( pos != 0) LOOP
         i := i + 1;
         strings(i) := substr(lv_str,1,pos-1);
         lv_str := substr(lv_str,pos+1,length(lv_str));
         pos := instr(lv_str,p_delim,1,1);
         IF pos = 0 THEN
            strings(i+1) := lv_str;
         END IF;
      END LOOP;
      RETURN strings;
   END SPLIT;




END;
/

