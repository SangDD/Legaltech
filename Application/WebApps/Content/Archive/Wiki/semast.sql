-- 2 ban ghi
UPDATE semast a SET a.TRADEQTTY = 2201592, a.TRANSFERQTTY = 0 WHERE afacctid in (SELECT afacctid FROM afmast WHERE afacctno IN ('BIDB801234', 'BIDB801234000024') )
AND secid =(SELECT secid FROM SECURITIES WHERE symbol = 'DAG' );
commit;
