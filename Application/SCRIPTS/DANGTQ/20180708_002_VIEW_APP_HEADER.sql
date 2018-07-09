-- Start of DDL Script for View LEGALTECH.VIEW_APP_HEADER
-- Generated 9-Jul-2018 0:03:31 from LEGALTECH@LOCALHOST

CREATE OR REPLACE VIEW view_app_header (
   id,
   appcode,
   master_name,
   master_address,
   master_phone,
   rep_master_type,
   rep_master_name,
   rep_master_address,
   rep_master_phone,
   rep_master_fax,
   rep_master_email,
   send_date,
   status,
   status_form,
   status_content,
   filing_date,
   accept_date,
   public_date,
   accept_content_date,
   grant_date,
   grant_public_date,
   deleted,
   created_by,
   created_date,
   modify_by,
   modify_date,
   languague_code,
   remark,
   master_fax,
   master_email,
   notes,
   gencode,
   address,
   dateno,
   months,
   years,
   status_name,
   status_formm_name,
   status_content_name,
   appname,
   client_reference,
   case_name,
   app_no,
   app_degree )
AS
SELECT  A.ID,A.APPCODE,A.MASTER_NAME,A.MASTER_ADDRESS,A.MASTER_PHONE,A.REP_MASTER_TYPE,A.REP_MASTER_NAME,A.REP_MASTER_ADDRESS,
    A.REP_MASTER_PHONE,A.REP_MASTER_FAX,A.REP_MASTER_EMAIL,A.SEND_DATE,A.STATUS,A.STATUS_FORM,A.STATUS_CONTENT,A.FILING_DATE,A.ACCEPT_DATE,
    A.PUBLIC_DATE,A.ACCEPT_CONTENT_DATE,A.GRANT_DATE,A.GRANT_PUBLIC_DATE,A.DELETED,A.CREATED_BY,A.CREATED_DATE,A.MODIFY_BY,A.MODIFY_DATE,
    A.LANGUAGUE_CODE,A.REMARK,A.MASTER_FAX,A.MASTER_EMAIL,A.NOTES,A.GENCODE,A.ADDRESS,A.DATENO,A.MONTHS,A.YEARS,
    AL1.CONTENT AS STATUS_NAME, AL2.CONTENT AS STATUS_FORMM_NAME, AL3.CONTENT AS STATUS_CONTENT_NAME,SA.APPNAME,client_reference, case_name, App_No,App_Degree
FROM APPLICATION_HEADER A
JOIN SYS_APPLICATION SA ON SA.APPCODE = A.APPCODE AND A.LANGUAGUE_CODE = SA.LANGUAGECODE
LEFT JOIN ALLCODE AL1 ON AL1.CDVAL = TO_CHAR(A.STATUS) AND AL1.CDNAME = 'APP' AND AL1.CDTYPE = 'STATUS'
LEFT JOIN ALLCODE AL2 ON AL2.CDVAL = A.STATUS_FORM AND AL2.CDNAME = 'APP' AND AL2.CDTYPE = 'STATUS_FORM'
LEFT JOIN ALLCODE AL3 ON AL3.CDVAL = A.STATUS_CONTENT AND AL3.CDNAME = 'APP' AND AL3.CDTYPE = 'STATUS_CONTENT'

WHERE A.DELETED = 0
/


-- End of DDL Script for View LEGALTECH.VIEW_APP_HEADER

