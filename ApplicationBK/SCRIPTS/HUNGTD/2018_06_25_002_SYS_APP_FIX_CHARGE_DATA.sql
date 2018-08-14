DELETE FROM sys_app_fix_charge a WHERE a.APPCODE = 'TM06DKQT' AND a.FEE_ID = 5;
INSERT INTO sys_app_fix_charge 
VALUES(1,'TM06DKQT',5,2000000, NULL, NULL, NULL, 'Phí dịch vụ làm thủ tục đăng ký quốc tế nhãn hiệu');