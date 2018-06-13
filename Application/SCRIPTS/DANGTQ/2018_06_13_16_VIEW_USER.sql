-- Start of DDL Script for View LEGALTECH.VIEW_USERS
-- Generated 13-Jun-2018 22:29:41 from LEGALTECH@LOCALHOST

CREATE OR REPLACE VIEW view_users (
   id,
   username,
   password,
   fullname,
   dateofbirth,
   sex,
   email,
   phone,
   type,
   status,
   createdby,
   createddate,
   modifiedby,
   modifieddate,
   deleted,
   address,
   fax,
   country,
   company_name,
   main_business,
   title,
   copyto,
   face_link,
   linkedin_link,
   wechat_link,
   other_link,
   reason_select,
   request_credit,
   other_type,
   hourly_rate,
   customer_code,
   sexdisplayname,
   statusdisplayname,
   type_name,
   lasttimeupdated,
   lawer_type_name,
   reason_select_name,
   request_credit_name )
AS
SELECT a.ID,a.USERNAME,a.PASSWORD,a.FULLNAME,a.DATEOFBIRTH,a.SEX,a.EMAIL,a.PHONE,a.TYPE,a.STATUS,a.CREATEDBY,a.CREATEDDATE,a.MODIFIEDBY,a.MODIFIEDDATE,a.DELETED,
    a.ADDRESS,a.FAX,a.COUNTRY,a.COMPANY_NAME,a.MAIN_BUSINESS,a.TITLE,a.COPYTO,a.FACE_LINK,a.LINKEDIN_LINK,a.WECHAT_LINK,a.OTHER_LINK,a.REASON_SELECT,a.REQUEST_CREDIT,
    a.OTHER_TYPE,a.HOURLY_RATE,Customer_Code, t1.content AS SexDisplayName, t2.content AS StatusDisplayName, t3.content AS Type_Name,
    NVL(a.modifiedDate, a.createdDate) AS LastTimeUpdated,
    t4.content AS lawer_type_name,
    t5.content AS reason_select_name,
    t6.content AS request_credit_name

FROM s_users a
LEFT JOIN allcode t1 ON TO_CHAR(a.sex) = t1.cdval AND t1.cdname='SEX_TYPE'
LEFT JOIN allcode t2 ON TO_CHAR(a.status) = t2.cdval AND t2.cdname='USER_STATUS'
LEFT JOIN allcode t3 ON TO_CHAR(a.type) = t3.cdval AND t3.cdname = 'USER' AND t3.cdtype = 'USER_TYPE'

LEFT JOIN allcode t4 ON TO_CHAR(a.Other_type) = t4.cdval AND t4.cdname = 'USER' AND t4.cdtype = 'LAWER_TYPE'
LEFT JOIN allcode t5 ON TO_CHAR(a.REASON_SELECT) = t5.cdval AND t5.cdname = 'USER' AND t5.cdtype = 'REASON_SELECT'
LEFT JOIN allcode t6 ON TO_CHAR(a.REQUEST_CREDIT) = t6.cdval AND t6.cdname = 'USER' AND t6.cdtype = 'REQUEST_CREDIT'

WHERE a.deleted = 0
/


-- End of DDL Script for View LEGALTECH.VIEW_USERS

