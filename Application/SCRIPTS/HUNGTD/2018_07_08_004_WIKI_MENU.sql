SELECT * FROM S_FUNCTIONS;
DELETE FROM S_MENU WHERE UPPER(NAME) = 'WIKI';
INSERT INTO S_MENU 
VALUES(15,'<span data-menu="item-main-menu"><i class="fas fa-cog fa-fw"></i> Wiki </span>',5,'Wiki','<span data-menu="item-main-menu"><i class="fas fa-cog fa-fw"></i> Wiki </span>','Wiki',0);
DELETE FROM S_FUNCTIONS A WHERE A.FUNCTIONNAME IN ('WikiQLDMBaiViet',
'WikiListLuuTam',
'WikiListPeding',
'WikiListApproved');
INSERT INTO S_FUNCTIONS 
VALUES(173,'WikiQLDMBaiViet','Quản lý danh mục bài viết',1,'/vi-vn/wiki-manage/wiki-cata/list','/vi-vn/wiki-manage/wiki-cata/list',9,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(183,'WikiListLuuTam','(1) Quản lý bài viết lưu tạm',1,'/vi-vn/wiki-manage/wiki-doc/list','/vi-vn/wiki-manage/wiki-doc/do-delete-doc',2,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(184,'WikiListPeding','(2) Danh sách bài viết chờ xử lý',1,'/vi-vn/wiki-manage/wiki-doc/pending-list','/vi-vn/wiki-manage/wiki-doc/pending-list',3,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(185,'WikiListApproved','(3) Danh sách bài viết đã đăng',1,'/vi-vn/wiki-manage/wiki-doc/approved-list','/vi-vn/wiki-manage/wiki-doc/approved-list',4,0,0,15,NULL,NULL,0);