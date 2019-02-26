SELECT a.created_by,a.shipment_code, A.ref_shipment_code, request_object,is_lock, a.status,b.product_type from shipment_detail a
LEFT JOIN product b ON b.product_id = a.product_id
 WHERE a.shipment_code IN ('SH070119437' ,'SH070119437');
 --a.ref_shipment_code = 'SH040119364';
 
SELECT  a.* from shipment_approve a WHERE a.shipment_code IN ( 'SH070119437','SH070119437') ORDER BY id DESC ;

select * from todo a WHERE a.code IN ( 'SH070119437','SH070119437') AND deleted = 0 ORDER BY id DESC ;

SELECT * FROM shipment_detail a WHERE a.shipment_code IN ( 'SH070119437','SH070119437');