PROCEDURE Proc_GetBy_App_Header
(
    p_app_header_id IN app_detail_plb01_sdd.app_header_id % TYPE,
    p_language_code IN app_detail_plb01_sdd.language_code % TYPE,
    p_cursor_doc OUT tcursor
)
IS
BEGIN
    --LAY DANH SACH CAC TAI LIEU DINH KEM
    OPEN p_cursor_doc FOR
    SELECT * FROM APP_DOCUMENT A  WHERE app_header_id = p_app_header_id AND  language_code = p_language_code ;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;