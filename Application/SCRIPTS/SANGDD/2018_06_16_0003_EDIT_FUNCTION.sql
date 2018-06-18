delete  s_functions where id =  71 ;
/
INSERT INTO s_functions 
VALUES(71,'Quản lý timesheet','Quản lý timesheet',1,'/quan-ly-timesheet/danh-sach-timesheet','/quan-ly-timesheet/danh-sach-timesheet',1,0,0,11,'Timesheet Manager','Timesheet Manager',0);


/
commit ;

/

delete  sys_application ;
/

INSERT INTO sys_application 
VALUES(1,'EN_US','REQUEST \n FOR RECORDAL OF AMENDMENT \n OF APPLICATION FOR REGISTRATION OF \n INDUSTRIAL PROPERTY','TM01SDD','Request for amendment of application (English)','To: National Office of Intellectual Property \n 386, Nguyen Trai Str., Hanoi ',NULL,2,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM01SDD');
INSERT INTO sys_application 
VALUES(1,'VI_VN','TỜ KHAI \n SỬA ĐỔI ĐƠN ĐĂNG KÝ \n ĐỐI TƯỢNG SỞ HỮU CÔNG NGHIỆP','TM01SDD','Request for amendment of application (Vietnamese)','Kính gửi:  Cục Sở hữu trí tuệ \n 386 Nguyễn Trãi, Hà Nội  ','Chủ đơn dưới đây yêu cầu Cục Sở hữu trí tuệ sửa đổi đơn đăng ký đối tượng sở hữu công nghiệp *        ',1,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM01SDD');
INSERT INTO sys_application 
VALUES(2,'EN_US','REQUEST \n FOR RENEWAL/AMENDMENT/ASSIGNMENT/EXTENSION \n OF TERRITORY/LIMITATION OF \n GOODS/SERVICES/TERMINATION/CANCELLATION \n OF INTERNATIONAL TRADEMARK \n REGISTRATION','TM08SDQT','Request for amendment of international trademark application',NULL,NULL,8,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM08SDQT');
INSERT INTO sys_application 
VALUES(2,'VI_VN','TỜ KHAI \n YÊU CẦU GIA HẠN/SỬA ĐỔI/CHUYỂN \n NHƯỢNG/MỞ RỘNG LÃNH THỔ/HẠN CHẾ DANH \n MỤC/CHẤM DỨT/ HUỶ BỎ ĐĂNG KÝ QUỐC TẾ \n NHÃN HIỆU  ','TM08SDQT','Request for amendment of international trademark application (Vietnamese)',NULL,NULL,7,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM08SDQT');
INSERT INTO sys_application 
VALUES(99,'EN_US','TỜ KHAI \n YÊU CẦU ĐĂNG KÝ QUỐC TẾ NHÃN HIỆU \n CÓ NGUỒN GỐC VIỆT NAM  ','TM06DKQT','Request for international trademark registration',NULL,NULL,4,NULL,NULL,NULL,1,NULL,'/trade-mark-01/request-for-trade-mark/TM06DKQT');
INSERT INTO sys_application 
VALUES(99,'VI_VN','REQUEST \n FOR INTERNATIONAL TRADEMARK \n REGISTRATION \n BASED ON VIETNAM TRADEMARK APPLICATION   ','TM06DKQT','Request for international trademark registration(Vietnamese)',NULL,NULL,3,NULL,NULL,NULL,1,NULL,'/trade-mark-01/request-for-trade-mark/TM06DKQT');
INSERT INTO sys_application 
VALUES(101,'EN_US','REQUEST \n FOR REGISTRATION OF CONVERSION OF TRADEMARK','TM07DKCD','Request for conversion of international trademark',NULL,NULL,6,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM07DKCD');
INSERT INTO sys_application 
VALUES(101,'VI_VN','TỜ KHAI \n ĐĂNG KÝ NHÃN HIỆU CHUYỂN ĐỔI','TM07DKCD','Request for conversion of international trademark(Vietnamese)',NULL,NULL,5,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM07DKCD');
INSERT INTO sys_application 
VALUES(102,'EN_US','REQUEST \n FOR TRADEMARK SEARCH','TM03YCTCNH','Request for trademark search',NULL,NULL,10,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM03YCTCNH');
INSERT INTO sys_application 
VALUES(102,'VI_VN','YÊU CẦU \n TRA CỨU NHÃN HIỆU','TM03YCTCNH','Request for trademark search (Vietnamese)',NULL,NULL,9,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM03YCTCNH');
INSERT INTO sys_application 
VALUES(103,'EN_US','REQUEST \n FOR REGISTRATION OF TRADEMARK   ','TM04NH','Request for trademark registration','To: National Office of Intellectual Property       386, Nguyen Trai Str., Hanoi   ','The undersigned requests the National Office of Intellectual Property to examine and grant a Certificate of Registration of the Trademark*',0,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM04NH');
INSERT INTO sys_application 
VALUES(103,'VI_VN','TỜ KHAI \n ĐĂNG KÝ NHÃN HIỆU  ','TM04NH','Request for trademark registration (Vietnamese)','Kính gửi:  Cục Sở hữu trí tuệ  386 Nguyễn Trãi, Hà Nội  ','Chủ đơn dưới đây yêu cầu Cục Sở hữu trí tuệ xem xét đơn và cấp Giấy chứng nhận đăng ký nhãn hiệu*',0,NULL,NULL,NULL,1,NULL,'/trade-mark/request-for-trade-mark/TM04NH');


/

COMMIT ;

/

ALTER TABLE SYS_APP_FIX_CHARGE 
 ADD (
  CHAR02 VARCHAR2 (50 CHAR),
  CHAR03 VARCHAR2 (50 CHAR),
  DESCRIPTION VARCHAR2 (200 CHAR)
 )
/
/
commit ;

/

delete  sys_app_fix_charge where  appcode ='TM04NH';
/

INSERT INTO sys_app_fix_charge 
VALUES(200,'TM04NH',200,150000,'1',NULL,NULL,'Lệ phí nộp đơn');
INSERT INTO sys_app_fix_charge 
VALUES(201,'TM04NH',201,200000,NULL,NULL,NULL,'Phí phân loại quốc tế về Nhãn hiệu');
INSERT INTO sys_app_fix_charge 
VALUES(202,'TM04NH',2011,22000,'6',NULL,NULL,'Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)');
INSERT INTO sys_app_fix_charge 
VALUES(203,'TM04NH',203,600000,NULL,NULL,NULL,'Lệ phí  yêu cầu hưởng quyền ưu tiên                                                    ');
INSERT INTO sys_app_fix_charge 
VALUES(204,'TM04NH',204,120000,NULL,NULL,NULL,'Lệ phí công bố đơn');
INSERT INTO sys_app_fix_charge 
VALUES(205,'TM04NH',205,360000,NULL,NULL,NULL,'Phí tra cứu phục vụ thẩm định nội dung cho mỗi nhóm hàng hoá, dịch vụ  ');
INSERT INTO sys_app_fix_charge 
VALUES(206,'TM04NH',2051,30000,'6',NULL,NULL,'Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)');
INSERT INTO sys_app_fix_charge 
VALUES(207,'TM04NH',207,550000,'1',NULL,NULL,'Phí thẩm định đơn');
INSERT INTO sys_app_fix_charge 
VALUES(208,'TM04NH',2071,120000,'6',NULL,NULL,'Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)');
/

commit ;
/

