DELETE FROM S_FUNCTIONS A WHERE A.FUNCTIONNAME IN 
('WikiQLDMBaiViet',
'WikiListPeding',
'WikiListLuuTam',
'WikiListApproved',
'WikiAddNew');

 INSERT INTO s_functions (ID,FUNCTIONNAME,DISPLAYNAME,FUNCTIONNAME_ENG,DISPLAYNAME_ENG,FUNCTIONTYPE,HREFGET,HREFPOST,POSITION,PARENTID,LEV,MENUID,DELETED) 
VALUES(173,'WikiQLDMBaiViet','Quản lý danh mục bài viết','WikiQLDMBaiViet','Quản lý danh mục bài viết',1,'/wiki-manage/wiki-doc/catalogue-list','/wiki-manage/wiki-doc/do-delete-catalogue',9,0,0,15,0);
INSERT INTO s_functions (ID,FUNCTIONNAME,DISPLAYNAME,FUNCTIONNAME_ENG,DISPLAYNAME_ENG,FUNCTIONTYPE,HREFGET,HREFPOST,POSITION,PARENTID,LEV,MENUID,DELETED) 
VALUES(184,'WikiListPeding','(3) Danh sách bài viết chờ xử lý','WikiListPeding','(3) Danh sách bài viết chờ xử lý',1,'/wiki-manage/wiki-doc/pending-list','/wiki-manage/wiki-doc/pending-list',3,0,0,15,0);
INSERT INTO s_functions (ID,FUNCTIONNAME,DISPLAYNAME,FUNCTIONNAME_ENG,DISPLAYNAME_ENG,FUNCTIONTYPE,HREFGET,HREFPOST,POSITION,PARENTID,LEV,MENUID,DELETED) 
VALUES(183,'WikiListLuuTam','(2) Quản lý bài viết lưu tạm','WikiListLuuTam','(2) Quản lý bài viết lưu tạm',1,'/wiki-manage/wiki-doc/list','/wiki-manage/wiki-doc/do-delete-doc',2,0,0,15,0);
INSERT INTO s_functions (ID,FUNCTIONNAME,DISPLAYNAME,FUNCTIONNAME_ENG,DISPLAYNAME_ENG,FUNCTIONTYPE,HREFGET,HREFPOST,POSITION,PARENTID,LEV,MENUID,DELETED) 
VALUES(185,'WikiListApproved','(4) Danh sách bài viết đã đăng','WikiListApproved','(4) Danh sách bài viết đã đăng',1,'/wiki-manage/wiki-doc/approved-list','/wiki-manage/wiki-doc/approved-list',4,0,0,15,0);
INSERT INTO s_functions (ID,FUNCTIONNAME,DISPLAYNAME,FUNCTIONNAME_ENG,DISPLAYNAME_ENG,FUNCTIONTYPE,HREFGET,HREFPOST,POSITION,PARENTID,LEV,MENUID,DELETED) 
VALUES(190,'WikiAddNew','(1)Thêm mới bài viết','WikiAddNew','(1)Thêm mới bài viết',1,'/wiki-manage/wiki-doc/add','/wiki-manage/wiki-doc/add',1,0,0,15,0);
COMMIT;