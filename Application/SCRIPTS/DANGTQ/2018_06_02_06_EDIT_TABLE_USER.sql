ALTER TABLE S_USERS 
 ADD (
  fax VARCHAR2 (50 CHAR),
  Country VARCHAR2 (10 CHAR),
  Company_Name VARCHAR2 (500 CHAR),
  Main_Business VARCHAR2 (500 CHAR),
  Title VARCHAR2 (500 CHAR),
  CopyTo VARCHAR2 (200 CHAR),
  Face_Link VARCHAR2 (200 CHAR),
  LinkedIn_Link VARCHAR2 (200 CHAR),
  Wechat_Link VARCHAR2 (200 CHAR),
  Other_Link VARCHAR2 (200 CHAR),
  REASON_Select NUMBER,
  REQUEST_CREDIT NUMBER,
  Other_type NUMBER,
  Hourly_rate NUMBER
 )
 MODIFY (
  TYPE NUMBER (1),
  ADDRESS VARCHAR2 (200 CHAR)

 )
/
COMMENT ON COLUMN S_USERS.Company_Name IS 'Ten cong ty'
/
COMMENT ON COLUMN S_USERS.Main_Business IS 'Nganh nghe chinh'
/
COMMENT ON COLUMN S_USERS.Title IS 'Chuc vu'
/
COMMENT ON COLUMN S_USERS.CopyTo IS 'nguoi quan ly'
/
COMMENT ON COLUMN S_USERS.Face_Link IS 'thong tin face'
/
COMMENT ON COLUMN S_USERS.LinkedIn_Link IS 'LinkedIn'
/
COMMENT ON COLUMN S_USERS.Wechat_Link IS 'Link wechat'
/
COMMENT ON COLUMN S_USERS.Other_Link IS 'other link'
/
COMMENT ON COLUMN S_USERS.REASON_Select IS 'lý do su dung'
/
COMMENT ON COLUMN S_USERS.REQUEST_CREDIT IS 'yeu cau chinh sach gia'
/