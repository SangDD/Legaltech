-- Start of DDL Script for Package Body LEGALTECH.PKG_S_USERS
-- Generated 9-Jul-2018 23:59:11 from LEGALTECH@LOCALHOST

CREATE OR REPLACE 
PACKAGE pkg_s_users
  IS TYPE tcursor IS REF Cursor;

PROCEDURE proc_User_CheckLogin
(
    p_username IN VARCHAR2,
    p_password  IN VARCHAR2,
    p_return    OUT NUMBER
);

PROCEDURE proc_User_GetById
(
    p_userId NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_User_GetByUsername
(
    p_username VARCHAR2,
    p_cursor OUT tcursor
);

PROCEDURE proc_User_GetAll
(
    p_cursor OUT tcursor
);

PROCEDURE proc_User_GetAllUserId
(
    p_groupId NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_User_Find
(
    p_userName IN VARCHAR2,
    p_fullName IN VARCHAR2,
    p_type IN VARCHAR2,
    p_status IN VARCHAR2,
    p_orderBy IN VARCHAR2,
    p_startAt IN NUMBER,
    p_endAt IN NUMBER,
    p_totalRecord OUT NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_User_AddNew
(
    p_username IN s_users.username % type,
    p_password IN s_users.password % type,
    p_fullName IN s_users.fullName % type,
    p_DateOfBirth IN s_users.DateOfBirth % type,
    p_Sex IN s_users.Sex % type,
    p_Email IN s_users.Email % type,
    p_Phone IN s_users.Phone % type,
    p_fax IN VARCHAR2,
    p_Status In s_users.Status % type,
    p_type IN s_users.type % type,

    p_country IN s_users.country % TYPE,
    p_company_name IN s_users.company_name % TYPE,
    p_main_business IN s_users.main_business % TYPE,
    p_title IN s_users.title % TYPE,
    p_copyto IN s_users.copyto % TYPE,
    p_face_link IN s_users.face_link % TYPE,
    p_linkedin_link IN s_users.linkedin_link % TYPE,
    p_wechat_link IN s_users.wechat_link % TYPE,
    p_other_link IN s_users.other_link % TYPE,
    p_reason_select IN s_users.reason_select % TYPE,
    p_request_credit IN s_users.request_credit % TYPE,
    p_customer_code IN s_users.customer_code % TYPE,
    p_contact_person IN s_users.contact_person % TYPE,
    p_other_type IN s_users.other_type % TYPE,
    p_hourly_rate IN s_users.hourly_rate % TYPE,

    p_GroupId IN VARCHAR2,
    p_createdby IN s_users.createdby % type,
    p_return OUT NUMBER
);

PROCEDURE proc_User_Edit
(
    p_userId   IN s_users.id % type,
    p_fullName IN s_users.fullName % type,
    p_DateOfBirth IN s_users.DateOfBirth % type,
    p_Sex IN s_users.Sex % type,
    p_Email IN s_users.Email % type,
    p_Phone IN s_users.Phone % type,
    p_fax IN VARCHAR2,
    p_Status In s_users.Status % type,
    p_type IN s_users.type % type,

    p_country IN s_users.country % TYPE,
    p_company_name IN s_users.company_name % TYPE,
    p_main_business IN s_users.main_business % TYPE,
    p_title IN s_users.title % TYPE,
    p_copyto IN s_users.copyto % TYPE,
    p_face_link IN s_users.face_link % TYPE,
    p_linkedin_link IN s_users.linkedin_link % TYPE,
    p_wechat_link IN s_users.wechat_link % TYPE,
    p_other_link IN s_users.other_link % TYPE,
    p_reason_select IN s_users.reason_select % TYPE,
    p_request_credit IN s_users.request_credit % TYPE,
    p_customer_code IN s_users.customer_code % TYPE,
    p_contact_person IN s_users.contact_person % TYPE,
    p_other_type IN s_users.other_type % TYPE,
    p_hourly_rate IN s_users.hourly_rate % TYPE,

    p_GroupId IN VARCHAR2,
    p_modifiedBy IN s_users.modifiedby % type,
    p_return OUT NUMBER
);

PROCEDURE proc_User_Delete
(
    p_userId IN NUMBER,
    p_modifiedby VARCHAR2,
    p_return OUT NUMBER
);

PROCEDURE proc_User_ChangePassword
(
    p_userId   IN s_users.id % type,
    p_oldPwd IN s_users.fullName % type,
    p_newPwd IN s_users.DateOfBirth % type,
    p_modifiedBy IN s_users.modifiedby % type,

    p_return OUT NUMBER
);

PROCEDURE proc_User_GetAllUserRoles
(
    p_userId IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_User_GetAllSelfGroup
(
    p_userId NUMBER,
    p_cursor OUT tcursor
);

END;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_s_users
IS

PROCEDURE proc_User_CheckLogin
(
    p_username  IN VARCHAR2,
    p_password  IN VARCHAR2,
    p_return    OUT NUMBER
)
IS
BEGIN
    p_return:= 0;
    SELECT COUNT(*) INTO p_return FROM s_users
    WHERE username = p_username AND password = p_password AND status = pkg_common.USER_STATUS_ACTIVE AND deleted = 0;

    IF(p_return <> 1) THEN
        SELECT COUNT(*) INTO p_return FROM s_users
        WHERE username = p_username AND password = p_password AND status=pkg_common.USER_STATUS_STOPPED AND deleted = 0;

        IF(p_return = 0) THEN
            p_return:= -1002;
        ELSE
            p_return:= -1003;
        END IF;
    ELSE
        p_return:= 1000;
    END IF;


    EXCEPTION
    WHEN OTHERS THEN
        p_return:= -1001; -- failed
    RAISE;
END;

PROCEDURE proc_User_GetById
(
    p_userId NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT * FROM view_users a WHERE a.id = p_userId;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_User_GetByUsername
(
    p_username VARCHAR2,
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT * FROM view_users a WHERE a.username = p_username;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_User_GetAll
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT * FROM view_users a ORDER BY username;

    EXCEPTION
    WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_User_GetAllUserId
(
    p_groupId NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT a.id
    FROM s_users a
    WHERE a.deleted = 0 AND a.id IN (SELECT userId FROM s_group_user WHERE groupId = p_groupId);

    EXCEPTION
    WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_User_Find
(
    p_userName IN VARCHAR2,
    p_fullName IN VARCHAR2,
    p_type IN VARCHAR2,
    p_status IN VARCHAR2,
    p_orderBy IN VARCHAR2,
    p_startAt IN NUMBER,
    p_endAt IN NUMBER,
    p_totalRecord OUT NUMBER,
    p_cursor OUT tcursor
)
IS
    V_SQL VARCHAR2(16000) DEFAULT '';
    V_SQL_COUNT_TOTAL VARCHAR2(16000) DEFAULT '';
    V_CONDITION VARCHAR2(8000) DEFAULT '';
    V_ORDERBY VARCHAR(500) DEFAULT '';
BEGIN
    V_CONDITION:= V_CONDITION || ' AND a.deleted=0 ';

    IF(LENGTH(p_fullName) > 0) THEN
        V_CONDITION:= V_CONDITION || ' AND UPPER(a.FullName) LIKE UPPER(''%' || p_fullName || '%'')';
    END IF;

    IF(LENGTH(p_userName) > 0) THEN
        V_CONDITION:= V_CONDITION || ' AND UPPER(a.Username) LIKE UPPER(''%' || p_userName || '%'')';
    END IF;

    IF(LENGTH(p_type) > 0) THEN
        V_CONDITION:= V_CONDITION || ' AND UPPER(a.type) IN (' || p_type || ')';
    END IF;

    IF(LENGTH(p_status) > 0) THEN
        V_CONDITION:= V_CONDITION || ' AND a.status IN (' || p_status || ')';
    END IF;

    IF (LENGTH(p_orderby) > 0)  THEN
        V_ORDERBY := ' ORDER BY ' || p_orderby;
    ELSE
        V_ORDERBY := ' ORDER BY LastTimeUpdated DESC ';
    END IF;

    V_SQL := 'SELECT * from view_users a WHERE 1=1 ' || V_CONDITION || V_ORDERBY;

    V_SQL_COUNT_TOTAL:= 'SELECT COUNT(*) FROM s_users a WHERE 1 = 1 ' || V_CONDITION;

    EXECUTE IMMEDIATE V_SQL_COUNT_TOTAL INTO p_totalRecord ;
    IF p_totalRecord IS NULL THEN
        p_totalRecord := 0;
    END IF;

    IF p_endAt <> 0 THEN
        V_SQL := 'SELECT * FROM ( SELECT ROWNUM AS STT, a.* FROM  ( ' || V_SQL   ||' ) a ) where STT >= '|| p_startAt ||' and STT <= ' || p_endAt;
    ELSE
        V_SQL := 'SELECT ROWNUM STT, a.* FROM  ( ' || V_SQL  || ') a ' ;
    END IF;

    OPEN p_cursor FOR V_SQL;

EXCEPTION
WHEN OTHERS THEN
    raise_application_error(-20005, V_SQL_COUNT_TOTAL);
    --RAISE;
END;

PROCEDURE proc_User_Delete
(
    p_userId IN NUMBER,
    p_modifiedby VARCHAR2,
    p_return OUT NUMBER
)
IS
    V_EXISTED NUMBER DEFAULT 0;
    V_USER_ACTIVE NUMBER DEFAULT 0;
BEGIN
    SELECT COUNT(*) INTO V_EXISTED FROM s_users a WHERE a.id = p_userId AND a.deleted = 0;

    IF(V_EXISTED = 1) THEN
        SELECT a.status INTO V_USER_ACTIVE FROM s_users a WHERE a.id = p_userId AND a.deleted = 0;

        IF(V_USER_ACTIVE <> pkg_common.USER_STATUS_ACTIVE) THEN
            UPDATE s_users a SET a.deleted = 1, a.modifiedby = p_modifiedby, a.modifieddate = SYSDATE
            WHERE a.id = p_userId;

            DELETE FROM s_group_user WHERE userId=p_userId;

            COMMIT;
            p_return:= p_userId;
        ELSE
            p_return:= -1208; -- user dang hoat dong
        END IF;
    ELSE
        p_return:= -1209; -- tai khoan khong ton tai
    END IF;

EXCEPTION
WHEN OTHERS THEN
     p_return:= -1207;
    RAISE;
END;

PROCEDURE proc_User_ChangePassword
(
    p_userId   IN s_users.id % type,
    p_oldPwd IN s_users.fullName % type,
    p_newPwd IN s_users.DateOfBirth % type,
    p_modifiedBy IN s_users.modifiedby % type,

    p_return OUT NUMBER
)
IS
    V_EXISTED NUMBER DEFAULT 0;
BEGIN
    SELECT COUNT(*) INTO V_EXISTED FROM s_users a WHERE a.id = p_userId AND a.deleted = 0;

    IF(V_EXISTED = 1) THEN
        SELECT COUNT(*) INTO V_EXISTED FROM s_users a WHERE a.id = p_userId AND a.deleted = 0 AND a.password = p_oldPwd;

        IF(V_EXISTED = 1) THEN
            UPDATE s_users SET
            password = p_newPwd,
            modifiedby = p_modifiedBy,
            modifieddate = SYSDATE
            WHERE id = p_userId;

            COMMIT;
            p_return := 1216;
        ELSE
            p_return := -1215; -- wrong password
        END IF;
    ELSE
        p_return := -1214; -- data invalid
    END IF;

    EXCEPTION
    WHEN OTHERS THEN
        p_return:= -1213;
    RAISE;
END;


PROCEDURE proc_User_GetAllUserRoles
(
    p_userId IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    IF (p_userId <> -99) THEN
        OPEN p_cursor FOR
        SELECT * FROM s_functions f
        WHERE f.deleted = 0 AND f.functionType = pkg_common.FN_IMMEDIATE_SELF OR f.id IN (
            SELECT DISTINCT functionId
            FROM s_group_function
            WHERE groupId IN (
                SELECT groupId FROM s_group_user WHERE userId=p_userId
            )
        ) ORDER BY  menuid , position  ;
    ELSE
        OPEN p_cursor FOR
        SELECT * FROM s_functions f WHERE deleted = 0 ORDER BY   menuid , position  ;
    END IF;

    EXCEPTION
    WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_User_GetAllSelfGroup
(
    p_userId NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT DISTINCT GroupId FROM s_group_user WHERE UserId = p_userId;

    EXCEPTION
    WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_User_AddNew
(
    p_username IN s_users.username % type,
    p_password IN s_users.password % type,
    p_fullName IN s_users.fullName % type,
    p_DateOfBirth IN s_users.DateOfBirth % type,
    p_Sex IN s_users.Sex % type,
    p_Email IN s_users.Email % type,
    p_Phone IN s_users.Phone % type,
    p_fax IN VARCHAR2,
    p_Status In s_users.Status % type,
    p_type IN s_users.type % type,

    p_country IN s_users.country % TYPE,
    p_company_name IN s_users.company_name % TYPE,
    p_main_business IN s_users.main_business % TYPE,
    p_title IN s_users.title % TYPE,
    p_copyto IN s_users.copyto % TYPE,
    p_face_link IN s_users.face_link % TYPE,
    p_linkedin_link IN s_users.linkedin_link % TYPE,
    p_wechat_link IN s_users.wechat_link % TYPE,
    p_other_link IN s_users.other_link % TYPE,
    p_reason_select IN s_users.reason_select % TYPE,
    p_request_credit IN s_users.request_credit % TYPE,
    p_customer_code IN s_users.customer_code % TYPE,
    p_contact_person IN s_users.contact_person % TYPE,
    p_other_type IN s_users.other_type % TYPE,
    p_hourly_rate IN s_users.hourly_rate % TYPE,

    p_GroupId IN VARCHAR2,
    p_createdby IN s_users.createdby % type,
    p_return OUT NUMBER
)
IS
    V_EXISTED NUMBER DEFAULT 0;
    V_USERID NUMBER DEFAULT 0;
BEGIN
    SELECT COUNT(*) INTO V_EXISTED FROM s_users a
    WHERE UPPER(a.username) = UPPER(p_username) AND a.deleted = 0;
    IF(V_EXISTED = 0) THEN
        SELECT seq_s_users.NEXTVAL INTO V_USERID FROM dual;

        INSERT INTO s_users a
        (
            a.id, a.Username, a.Password, a.FullName, a.DateOfBirth, a.Sex, a.Email, a.Phone,
            a.TYPE, a.Status, a.createdBy, a.createddate, a.deleted,
            fax, country, company_name, main_business, title, copyto, face_link, linkedin_link, wechat_link, other_link,
            reason_select, request_credit, other_type, hourly_rate,customer_code,contact_person

        )
        VALUES
        (
            V_USERID, p_username, p_password, p_fullName, p_DateOfBirth, p_Sex, p_Email, p_Phone,
            p_type, p_Status, p_createdby ,SYSDATE, 0,
            p_fax, p_country, p_company_name, p_main_business, p_title, p_copyto, p_face_link, p_linkedin_link, p_wechat_link, p_other_link,
            p_reason_select, p_request_credit, p_other_type, p_hourly_rate, p_customer_code,p_contact_person
        );

        IF(LENGTH(p_GroupId) > 0) THEN
            INSERT INTO s_group_user(groupId, userId)
            VALUES(p_GroupId, V_USERID);
        END IF;

        COMMIT;
        p_return:= 1200;
    ELSE
        p_return:= -1202;
    END IF;

    EXCEPTION
    WHEN OTHERS THEN
        p_return:= -1201;
    RAISE;
END;

PROCEDURE proc_User_Edit
(
    p_userId   IN s_users.id % type,
    p_fullName IN s_users.fullName % type,
    p_DateOfBirth IN s_users.DateOfBirth % type,
    p_Sex IN s_users.Sex % type,
    p_Email IN s_users.Email % type,
    p_Phone IN s_users.Phone % type,
    p_fax IN VARCHAR2,
    p_Status In s_users.Status % type,
    p_type IN s_users.type % type,

    p_country IN s_users.country % TYPE,
    p_company_name IN s_users.company_name % TYPE,
    p_main_business IN s_users.main_business % TYPE,
    p_title IN s_users.title % TYPE,
    p_copyto IN s_users.copyto % TYPE,
    p_face_link IN s_users.face_link % TYPE,
    p_linkedin_link IN s_users.linkedin_link % TYPE,
    p_wechat_link IN s_users.wechat_link % TYPE,
    p_other_link IN s_users.other_link % TYPE,
    p_reason_select IN s_users.reason_select % TYPE,
    p_request_credit IN s_users.request_credit % TYPE,
    p_customer_code IN s_users.customer_code % TYPE,
    p_contact_person IN s_users.contact_person % TYPE,
    p_other_type IN s_users.other_type % TYPE,
    p_hourly_rate IN s_users.hourly_rate % TYPE,

    p_GroupId IN VARCHAR2,
    p_modifiedBy IN s_users.modifiedby % type,
    p_return OUT NUMBER
)
IS
    V_EXISTED NUMBER DEFAULT 0;
BEGIN
    SELECT COUNT(*) INTO V_EXISTED FROM s_users a
    WHERE a.id = p_userId AND a.deleted = 0;

    IF(V_EXISTED = 1) THEN

        UPDATE s_users SET
            FullName = p_fullName, DateOfBirth = p_DateOfBirth, Sex = p_Sex,
            Email = p_Email, Phone = p_Phone,
            TYPE = p_type, Status = p_Status,
            country = p_country,
            face_link = p_face_link,
            linkedin_link = p_linkedin_link,
            wechat_link = p_wechat_link,
            other_link = p_other_link,

            MODIFIEDDATE = SYSDATE,
            ModifiedBy = p_modifiedBy
        WHERE id = p_userId;

        IF p_type = pkg_common.USER_TYPE_LAWER THEN
            UPDATE s_users SET
                other_type = p_other_type,
                hourly_rate = p_hourly_rate
            WHERE id = p_userId;
        ELSIF p_type = pkg_common.USER_TYPE_CUSTOMER THEN
            UPDATE s_users SET
                company_name = p_company_name,
                main_business = p_main_business,
                title = p_title, copyto = p_copyto,
                reason_select = p_reason_select,
                request_credit = p_request_credit,
                contact_person = p_contact_person,
                customer_code =  p_customer_code
            WHERE id = p_userId;
        END IF;


        DELETE FROM s_group_user WHERE userId = p_userId;
        IF(LENGTH(p_GroupId) > 0) THEN
            INSERT INTO s_group_user(groupId, userId)
            VALUES(p_GroupId, p_userId);
        END IF;

        COMMIT;
        p_return:= p_userId;
    ELSE
        p_return:= -1205;
    END IF;

    EXCEPTION
    WHEN OTHERS THEN
        p_return:= -1204;
    RAISE;
END;


END;
/


-- End of DDL Script for Package Body LEGALTECH.PKG_S_USERS

