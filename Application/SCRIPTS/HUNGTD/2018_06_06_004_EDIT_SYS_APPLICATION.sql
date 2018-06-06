ALTER TABLE SYS_APPLICATION 
 ADD (
  URL VARCHAR2 (200 CHAR)
 )
/

UPDATE SYS_APPLICATION a SET a.url =   '/trade-mark/sua-doi-don-dang-ky/' || UPPER(a.APPCODE);

DELETE FROM SYS_APPLICATION WHERE id IN (3);
INSERT INTO SYS_APPLICATION 
VALUES(3,'VI_VN','TỜ KHAI \n ĐĂNG KÝ NHÃN HIỆU  ','06DKQT','C.06 Request for international trademark registration (Vietnamese)','Kính gửi:  Cục Sở hữu trí tuệ  386 Nguyễn Trãi, Hà Nội  ','Chủ đơn dưới đây yêu cầu Cục Sở hữu trí tuệ xem xét đơn và cấp Giấy chứng nhận đăng ký nhãn hiệu*',13,NULL,NULL,NULL,1,NULL,'/trade-mark-01/sua-doi-don-dang-ky/DKQT');
INSERT INTO SYS_APPLICATION 
VALUES(3,'EN_US','REQUEST \n FOR REGISTRATION OF TRADEMARK   ','06DKQT','C.06 Request for international trademark registration','To: National Office of Intellectual Property       386, Nguyen Trai Str., Hanoi   ','The undersigned requests the National Office of Intellectual Property to examine and grant a Certificate of Registration of the Trademark*',14,NULL,NULL,NULL,1,NULL,'/trade-mark-01/sua-doi-don-dang-ky/DKQT');




COMMIT;