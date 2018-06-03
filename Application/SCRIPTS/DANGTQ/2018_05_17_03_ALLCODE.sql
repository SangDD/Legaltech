SELECT a.cdname, a.cdtype, a.cdval, a.content, a.lstodr, a.content_eng
  FROM allcode a WHERE cdname = 'TIMESHEET' and cdtype = 'STATUS'
  
INSERT INTO allcode 
VALUES('TIMESHEET','STATUS','0','Ch? duy?t',0,'New');
INSERT INTO allcode 
VALUES('TIMESHEET','STATUS','1','ау duy?t',1,'Approve');
INSERT INTO allcode 
VALUES('TIMESHEET','STATUS','2','ау t? ch?i',2,'Reject');


-- Begin 20180602
SELECT * FROM allcode a WHERE cdname = 'USER' and cdtype = 'LAWER_TYPE';
    
INSERT INTO allcode 
VALUES('USER','LAWER_TYPE','1','Trademark',1,'Trademark');
INSERT INTO allcode 
VALUES('USER','LAWER_TYPE','2','Patent',2,'Patent');

SELECT * FROM allcode a WHERE cdname = 'USER' and cdtype = 'REASON_SELECT';
    
INSERT INTO allcode 
VALUES('USER','REASON_SELECT','1','Law group/Network',1,'Law group/Network');
INSERT INTO allcode 
VALUES('USER','REASON_SELECT','2','Continuing Relationship',2,'Continuing Relationship');
INSERT INTO allcode 
VALUES('USER','REASON_SELECT','3','Referred by client/friends',3,'Referred by client/friends');
INSERT INTO allcode 
VALUES('USER','REASON_SELECT','4','Marketing (Ad., Brochure, Articles, Speaker, Seminar, etc.)',4,'Marketing (Ad., Brochure, Articles, Speaker, Seminar, etc.)');

SELECT * FROM allcode a WHERE cdname = 'USER' and cdtype = 'REQUEST_CREDIT';
    
INSERT INTO allcode 
VALUES('USER','REQUEST_CREDIT','1','New Client',1,'New Client');
INSERT INTO allcode 
VALUES('USER','REQUEST_CREDIT','2','Pharmaceutical Company',2,'Pharmaceutical Company');
INSERT INTO allcode 
VALUES('USER','REQUEST_CREDIT','3','ICT Company',3,'ICT Company');
INSERT INTO allcode 
VALUES('USER','REQUEST_CREDIT','4','Others',4,'Others');








