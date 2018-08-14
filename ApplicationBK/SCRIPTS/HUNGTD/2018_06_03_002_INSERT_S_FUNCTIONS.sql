DELETE  FROM S_FUNCTIONS a WHERE a.FUNCTIONNAME_ENG = 'Class list';
INSERT INTO S_FUNCTIONS 
VALUES(163,'Quản lý hàng hóa/dịch vụ','Quản lý hàng hóa/dịch vụ',1,'/quan-ly-thong-tin/hang-hoa-dich-vu/danh-sach-hang-hoa','/quan-ly-thong-tin/hang-hoa-dich-vu/find-class',9,0,1,11,'Class list','Class list',0);
 COMMIT;