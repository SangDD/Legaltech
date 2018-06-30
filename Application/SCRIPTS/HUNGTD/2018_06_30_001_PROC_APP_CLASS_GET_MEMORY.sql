PROCEDURE PROC_APP_CLASS_GET_MEMORY
(
     P_CURSOR OUT TCURSOR
)
IS
 BEGIN
  OPEN P_CURSOR FOR SELECT code , name_en,name_vi, code || name_vi AS DisplayValue, UPPER(code || name_en || name_vi) AS KeySearch,
  NVL( SUBSTR(CODE, 0, 2), '') AS GroupCode
   FROM APP_CLASS_INFO ORDER BY NAME_VI;
 EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;