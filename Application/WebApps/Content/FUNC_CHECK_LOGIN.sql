﻿-- FUNCTION: FUNC_CHECK_LOGIN()

-- DROP FUNCTION FUNC_CHECK_LOGIN();

CREATE OR REPLACE FUNCTION FUNC_CHECK_LOGIN(P_USERNAME VARCHAR,P_PASSWORD VARCHAR )
  RETURNS NUMERIC AS
$BODY$
    DECLARE
      P_RETURN NUMERIC ;                                                   
    BEGIN
          SELECT  COUNT(1) INTO P_RETURN  FROM S_USERS WHERE USERNAME = P_USERNAME AND PASSWORD = P_PASSWORD AND STATUS='A' AND DELETED = 0;
           IF(P_RETURN>0) THEN 
	     SELECT COUNT(*) INTO P_RETURN FROM S_USERS WHERE USERNAME = P_USERNAME AND PASSWORD = P_PASSWORD AND STATUS='S' AND DELETED = 0 ;
	 
              IF(P_RETURN = 0) THEN
            P_RETURN:= -1002;
        ELSE
            P_RETURN:= -1003;
        END IF;
    ELSE
        P_RETURN:= 1000;
    END IF;
    
    END;
    $BODY$
  LANGUAGE PLPGSQL VOLATILE COST 100;
 
