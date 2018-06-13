DELETE SYS_APPLICATION WHERE appcode = 'TM_PLB01SDD';

INSERT INTO sys_application 
VALUES(4,'VI_VN','TỜ KHAI \n SỬA ĐỔI ĐƠN ĐĂNG KÝ \n ĐỐI TƯỢNG SỞ HỮU CÔNG NGHIỆP','TM_PLB01SDD','3b - B01. Request for amendment of application (Vietnamese)','Kính gửi: Cục Sở hữu trí tuệ \n 386 Nguyễn Trãi, Hà Nội','Chủ đơn dưới đây yêu cầu Cục Sở hữu trí tuệ sửa đổi đơn đăng ký đối tượng sở hữu công nghiệp *',15,NULL,NULL,NULL,1,NULL,'/trade-mark-3b/register/TM_PLB01SDD');
INSERT INTO sys_application 
VALUES(4,'EN_US','REQUEST \n FOR RECORDAL OF AMENDMENT \n OF APPLICATION FOR REGISTRATION OF \n INDUSTRIAL PROPERTY','TM_PLB01SDD','3b - B01. Request for amendment of application','To: National Office of Intellectual Property       386, Nguyen Trai Str., Hanoi   ','The undersigned requests the National Office of Intellectual Property to examine and grant a Certificate of Registration of the Trademark *',16,NULL,NULL,NULL,1,NULL,'/trade-mark-3b/register/TM_PLB01SDD');


delete s_functions where id = 71;
INSERT INTO s_functions 
VALUES(71,'Quản lý timesheet','Quản lý timesheet',1,'/quan-ly-timesheet/danh-sach-timesheet','/quan-ly-timesheet/danh-sach-timesheet',1,0,0,11,'Timesheet Manager','Timesheet Manager',0);







