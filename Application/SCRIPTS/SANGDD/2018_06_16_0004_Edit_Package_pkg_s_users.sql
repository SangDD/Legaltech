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
        WHERE f.functionType = pkg_common.FN_IMMEDIATE_SELF OR f.id IN (
            SELECT DISTINCT functionId
            FROM s_group_function
            WHERE groupId IN (
                SELECT groupId FROM s_group_user WHERE userId=p_userId
            )
        ) ORDER BY  menuid , position  ;
    ELSE
        OPEN p_cursor FOR
        SELECT * FROM s_functions f ORDER BY   menuid , position  ;
    END IF;

    EXCEPTION
    WHEN OTHERS THEN
    RAISE;
END;