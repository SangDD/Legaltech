SELECT * FROM s_functions;

INSERT INTO s_functions 
VALUES(71,'Qu?n lý timesheet','Qu?n lý timesheet',1,'/quan-ly-timesheet/danh-sach-timesheet','/quan-ly-timesheet/danh-sach-timesheet',1,0,0,11,'Roles Manager','Roles Manager',0);

DELETE SYS_APPLICATION WHERE appcode = 'TM_PLB01SDD';

INSERT INTO SYS_APPLICATION 
VALUES(4,'VI_VN','T? KHAI \n S?A Ð?I ÐON ÐANG KÝ \n Ð?I TU?NG S? H?U CÔNG NGHI?P','TM_PLB01SDD','3b - B01. Request for amendment of application (Vietnamese)','Kính g?i: C?c S? h?u trí tu? \n 386 Nguy?n Trãi, Hà N?i','Ch? don du?i dây yêu c?u C?c S? h?u trí tu? s?a d?i don dang ký d?i tu?ng s? h?u công nghi?p*',15,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM_PLB01SDD');
INSERT INTO SYS_APPLICATION 
VALUES(4,'EN_US','REQUEST \n FOR RECORDAL OF AMENDMENT \n OF APPLICATION FOR REGISTRATION OF \n INDUSTRIAL PROPERTY','TM_PLB01SDD','3b - B01. Request for amendment of application','To: National Office of Intellectual Property       386, Nguyen Trai Str., Hanoi   ','The undersigned requests the National Office of Intellectual Property to examine and grant a Certificate of Registration of the Trademark *',16,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM_PLB01SDD');












