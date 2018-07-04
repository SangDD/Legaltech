--SELECT * FROM S_MENU;
DELETE FROM S_MENU a WHERE a.NAME = 'Wiki';
INSERT INTO S_MENU 
VALUES(15,'<span data-menu="item-main-menu"><i class="fas fa-cog fa-fw"></i> Wiki </span>',5,'Wiki','<span data-menu="item-main-menu"><i class="fas fa-cog fa-fw"></i> Wiki </span>','Wiki',0);
DELETE FROM S_FUNCTIONS a WHERE a.FUNCTIONNAME IN ('WikiQLDMBaiViet', 'WikiQuanLyBaiViet');

INSERT INTO S_FUNCTIONS 
VALUES(173,'WikiQLDMBaiViet','Quản lý danh mục bài viết',1,'/vi-vn/wiki-manage/wiki-cata/list','/vi-vn/wiki-manage/wiki-cata/list',1,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(172,'WikiQuanLyBaiViet','Quản lý bài viết',1,'/vi-vn/wiki-manage/wiki-doc/list','/vi-vn/wiki-manage/wiki-doc/list',0,0,0,15,NULL,NULL,0);
COMMIT;