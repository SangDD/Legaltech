SELECT a.*,UPPER(REPLACE(TRIM(NAME), ' ' , '')) AS trim_name 
FROM data_customer_dangtq a
WHERE INSTR(a.customer_code, '_') > 0
ORDER BY customer_code
HAVING 


--
--UPDATE customer SET customer_code_TEM = customer_code;

-- update thang nao trung ten ko trung ma
-- dè code c?a ch? h?nh
MERGE INTO customer c
USING
(
    SELECT a.*
    FROM data_customer_dangtq a
    WHERE INSTR(a.customer_code, '_') = 0 
    AND trim_name NOT IN 
    (
        SELECT trim_name FROM 
        (
            SELECT a.* FROM data_customer_dangtq a
            WHERE INSTR(a.customer_code, '_') = 0
        ) GROUP BY trim_name
        HAVING COUNT(*) > 1
    )   
) source
ON
(
    UPPER(REPLACE(TRIM(c.customer_name), ' ' , ''))  = source.trim_name
    --and c.customer_code <> source.customer_code
)
WHEN MATCHED THEN
UPDATE SET c.customer_code = source.customer_code;

DELETE customer a WHERE a.customer_code IN
(
    SELECT customer_code FROM data_customer_dangtq a WHERE trim_name
    IN 
    (
        SELECT trim_name FROM 
        (
            SELECT a.* FROM data_customer_dangtq a
            WHERE INSTR(a.customer_code, '_') = 0
        ) GROUP BY trim_name
        HAVING COUNT(*) > 1
    )   
);

INSERT INTO customer 
(
    id, customer_code, customer_name, deleted
)
SELECT seq_customer.NEXTVAL, customer_code, NAME,0 
FROM data_customer_dangtq a
WHERE trim_name
IN 
(
    SELECT trim_name FROM 
    (
        SELECT a.* FROM data_customer_dangtq a
        WHERE INSTR(a.customer_code, '_') = 0
    ) GROUP BY trim_name
    HAVING COUNT(*) > 1
)  ;

-- tinh nhung thang co dau gach 
DELETE customer a WHERE a.customer_code IN
(
    SELECT DISTINCT customer_code_real FROM data_customer_dangtq a
    WHERE INSTR(a.customer_code, '_') > 0
);

INSERT INTO customer 
(
    id, customer_code, customer_name, deleted
)
SELECT seq_customer.NEXTVAL, customer_code, NAME,0 
FROM data_customer_dangtq a
WHERE INSTR(a.customer_code, '_') > 0;