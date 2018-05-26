SELECT a.cdname, a.cdtype, a.cdval, a.content, a.lstodr, a.content_eng
  FROM allcode a WHERE cdname = 'TIMESHEET' and cdtype = 'STATUS'
  
INSERT INTO allcode 
VALUES('TIMESHEET','STATUS','0','Ch? duy?t',0,'New');
INSERT INTO allcode 
VALUES('TIMESHEET','STATUS','1','ау duy?t',1,'Approve');
INSERT INTO allcode 
VALUES('TIMESHEET','STATUS','2','ау t? ch?i',2,'Reject');
