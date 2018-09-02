CREATE DATABASE QLBH_1004_TEST
GO
-----------------------------------------------------
-----------------------------------------------------
USE QLBH_1004_TEST
GO
---------------------------------------------
-- KHACHANG
CREATE TABLE KHACHHANG(
 MAKH char(4) not null, 
 HOTEN varchar(40),
 DCHI varchar(50),
 SODT varchar(20),
 NGSINH smalldatetime,
 NGDK smalldatetime,
 DOANHSO money,
 constraint pk_kh primary key(MAKH)
)
---------------------------------------------
-- NHANVIEN
CREATE TABLE NHANVIEN(
 MANV char(4) not null, 
 HOTEN varchar(40),
 SODT varchar(20),
 NGVL smalldatetime 
 constraint pk_nv primary key(MANV)
)
---------------------------------------------
-- SANPHAM
CREATE TABLE SANPHAM(
 MASP char(4) not null,
 TENSP varchar(40),
 DVT varchar(20),
 NUOCSX varchar(40),
 GIA money,
 constraint pk_sp primary key(MASP) 
)
---------------------------------------------
-- HOADON
CREATE TABLE HOADON(
 SOHD int not null,
 NGHD smalldatetime,
 MAKH char(4),
 MANV char(4),
 TRIGIA money,
 constraint pk_hd primary key(SOHD)
)
---------------------------------------------
-- CTHD
CREATE TABLE CTHD(
 SOHD int,
 MASP char(4),
 SL int,
 constraint pk_cthd primary key(SOHD,MASP)
)
 
-- Khoa ngoai cho bang HOADON
ALTER TABLE HOADON ADD CONSTRAINT fk01_HD FOREIGN KEY(MAKH) REFERENCES KHACHHANG(MAKH)
ALTER TABLE HOADON DROP CONSTRAINT FK01_HD
ALTER TABLE HOADON ADD CONSTRAINT fk02_HD FOREIGN KEY(MANV) REFERENCES NHANVIEN(MANV)
ALTER TABLE HOADON DROP CONSTRAINT FK02_HD
-- Khoa ngoai cho bang CTHD
ALTER TABLE CTHD ADD CONSTRAINT fk01_CTHD FOREIGN KEY(SOHD) REFERENCES HOADON(SOHD)
ALTER TABLE CTHD DROP CONSTRAINT FK01_CTHD
ALTER TABLE CTHD ADD CONSTRAINT fk02_CTHD FOREIGN KEY(MASP) REFERENCES SANPHAM(MASP)
ALTER TABLE CTHD DROP CONSTRAINT FK02_CTHD
-----------------------------------------------------
-----------------------------------------------------
set dateformat dmy
-------------------------------
-- KHACHHANG
insert into khachhang values('KH01','Nguyen Van A','731 Tran Hung Dao, Q5, TpHCM','8823451','22/10/1960','22/07/2006',13060000)
insert into khachhang values('KH02','Tran Ngoc Han','23/5 Nguyen Trai, Q5, TpHCM','908256478','03/04/1974','30/07/2006',280000)
insert into khachhang values('KH03','Tran Ngoc Linh','45 Nguyen Canh Chan, Q1, TpHCM','938776266','12/06/1980','08/05/2006',3860000)
insert into khachhang values('KH04','Tran Minh Long','50/34 Le Dai Hanh, Q10, TpHCM','917325476','09/03/1965','10/02/2006',250000)
insert into khachhang values('KH05','Le Nhat Minh','34 Truong Dinh, Q3, TpHCM','8246108','10/03/1950','28/10/2006',21000)
insert into khachhang values('KH06','Le Hoai Thuong','227 Nguyen Van Cu, Q5, TpHCM','8631738','31/12/1981','24/11/2006',915000)
insert into khachhang values('KH07','Nguyen Van Tam','32/3 Tran Binh Trong, Q5, TpHCM','916783565','06/04/1971','12/01/2006',12500)
insert into khachhang values('KH08','Phan Thi Thanh','45/2 An Duong Vuong, Q5, TpHCM','938435756','10/01/1971','13/12/2006',365000)
insert into khachhang values('KH09','Le Ha Vinh','873 Le Hong Phong, Q5, TpHCM','8654763','03/09/1979','14/01/2007',70000)
insert into khachhang values('KH10','Ha Duy Lap','34/34B Nguyen Trai, Q1, TpHCM','8768904','02/05/1983','16/01/2007',67500)
 
-------------------------------
-- NHANVIEN
insert into nhanvien values('NV01','Nguyen Nhu Nhut','927345678','13/04/2006')
insert into nhanvien values('NV02','Le Thi Phi Yen','987567390','21/04/2006')
insert into nhanvien values('NV03','Nguyen Van B','997047382','27/04/2006')
insert into nhanvien values('NV04','Ngo Thanh Tuan','913758498','24/06/2006')
insert into nhanvien values('NV05','Nguyen Thi Truc Thanh','918590387','20/07/2006')
 
-------------------------------
-- SANPHAM
insert into sanpham values('BC01','But chi','cay','Singapore',3000)
insert into sanpham values('BC02','But chi','cay','Singapore',5000)
insert into sanpham values('BC03','But chi','cay','Viet Nam',3500)
insert into sanpham values('BC04','But chi','hop','Viet Nam',30000)
insert into sanpham values('BB01','But bi','cay','Viet Nam',5000)
insert into sanpham values('BB02','But bi','cay','Trung Quoc',7000)
insert into sanpham values('BB03','But bi','hop','Thai Lan',100000)
insert into sanpham values('TV01','Tap 100 giay mong','quyen','Trung Quoc',2500)
insert into sanpham values('TV02','Tap 200 giay mong','quyen','Trung Quoc',4500)
insert into sanpham values('TV03','Tap 100 giay tot','quyen','Viet Nam',3000)
insert into sanpham values('TV04','Tap 200 giay tot','quyen','Viet Nam',5500)
insert into sanpham values('TV05','Tap 100 trang','chuc','Viet Nam',23000)
insert into sanpham values('TV06','Tap 200 trang','chuc','Viet Nam',53000)
insert into sanpham values('TV07','Tap 100 trang','chuc','Trung Quoc',34000)
insert into sanpham values('ST01','So tay 500 trang','quyen','Trung Quoc',40000)
insert into sanpham values('ST02','So tay loai 1','quyen','Viet Nam',55000)
insert into sanpham values('ST03','So tay loai 2','quyen','Viet Nam',51000)
insert into sanpham values('ST04','So tay','quyen','Thai Lan',55000)
insert into sanpham values('ST05','So tay mong','quyen','Thai Lan',20000)
insert into sanpham values('ST06','Phan viet bang','hop','Viet Nam',5000)
insert into sanpham values('ST07','Phan khong bui','hop','Viet Nam',7000)
insert into sanpham values('ST08','Bong bang','cai','Viet Nam',1000)
insert into sanpham values('ST09','But long','cay','Viet Nam',5000)
insert into sanpham values('ST10','But long','cay','Trung Quoc',7000)
 
-------------------------------
-- HOADON
insert into hoadon values(1001,'23/07/2006','KH01','NV01',320000)
insert into hoadon values(1002,'12/08/2006','KH01','NV02',840000)
insert into hoadon values(1003,'23/08/2006','KH02','NV01',100000)
insert into hoadon values(1004,'01/09/2006','KH02','NV01',180000)
insert into hoadon values(1005,'20/10/2006','KH01','NV02',3800000)
insert into hoadon values(1006,'16/10/2006','KH01','NV03',2430000)
insert into hoadon values(1007,'28/10/2006','KH03','NV03',510000)
insert into hoadon values(1008,'28/10/2006','KH01','NV03',440000)
insert into hoadon values(1009,'28/10/2006','KH03','NV04',200000)
insert into hoadon values(1010,'01/11/2006','KH01','NV01',5200000)
insert into hoadon values(1011,'04/11/2006','KH04','NV03',250000)
insert into hoadon values(1012,'30/11/2006','KH05','NV03',21000)
insert into hoadon values(1013,'12/12/2006','KH06','NV01',5000)
insert into hoadon values(1014,'31/12/2006','KH03','NV02',3150000)
insert into hoadon values(1015,'01/01/2007','KH06','NV01',910000)
insert into hoadon values(1016,'01/01/2007','KH07','NV02',12500)
insert into hoadon values(1017,'02/01/2007','KH08','NV03',35000)
insert into hoadon values(1018,'13/01/2007','KH08','NV03',330000)
insert into hoadon values(1019,'13/01/2007','KH01','NV03',30000)
insert into hoadon values(1020,'14/01/2007','KH09','NV04',70000)
insert into hoadon values(1021,'16/01/2007','KH10','NV03',67500)
insert into hoadon values(1022,'16/01/2007',Null,'NV03',7000)
insert into hoadon values(1023,'17/01/2007',Null,'NV01',330000)
 
-------------------------------
-- CTHD
insert into cthd values(1001,'TV02',10)
insert into cthd values(1001,'ST01',5)
insert into cthd values(1001,'BC01',5)
insert into cthd values(1001,'BC02',10)
insert into cthd values(1001,'ST08',10)
insert into cthd values(1002,'BC04',20)
insert into cthd values(1002,'BB01',20)
insert into cthd values(1002,'BB02',20)
insert into cthd values(1003,'BB03',10)
insert into cthd values(1004,'TV01',20)
insert into cthd values(1004,'TV02',10)
insert into cthd values(1004,'TV03',10)
insert into cthd values(1004,'TV04',10)
insert into cthd values(1005,'TV05',50)
insert into cthd values(1005,'TV06',50)
insert into cthd values(1006,'TV07',20)
insert into cthd values(1006,'ST01',30)
insert into cthd values(1006,'ST02',10)
insert into cthd values(1007,'ST03',10)
insert into cthd values(1008,'ST04',8)
insert into cthd values(1009,'ST05',10)
insert into cthd values(1010,'TV07',50)
insert into cthd values(1010,'ST07',50)
insert into cthd values(1010,'ST08',100)
insert into cthd values(1010,'ST04',50)
insert into cthd values(1010,'TV03',100)
insert into cthd values(1011,'ST06',50)
insert into cthd values(1012,'ST07',3)
insert into cthd values(1013,'ST08',5)
insert into cthd values(1014,'BC02',80)
insert into cthd values(1014,'BB02',100)
insert into cthd values(1014,'BC04',60)
insert into cthd values(1014,'BB01',50)
insert into cthd values(1015,'BB02',30)
insert into cthd values(1015,'BB03',7)
insert into cthd values(1016,'TV01',5)
insert into cthd values(1017,'TV02',1)
insert into cthd values(1017,'TV03',1)
insert into cthd values(1017,'TV04',5)
insert into cthd values(1018,'ST04',6)
insert into cthd values(1019,'ST05',1)
insert into cthd values(1019,'ST06',2)
insert into cthd values(1020,'ST07',10)
insert into cthd values(1021,'ST08',5)
insert into cthd values(1021,'TV01',7)
insert into cthd values(1021,'TV02',10)
insert into cthd values(1022,'ST07',1)
insert into cthd values(1023,'ST04',6)
----------------------------------------------------------------
----------------------------------------------------------------
SELECT * FROM KHACHHANG
SELECT * FROM NHANVIEN
SELECT * FROM SANPHAM
SELECT * FROM HOADON
SELECT * FROM CTHD



-- 1 In ra danh sách các sản phẩm chỉ lấy (MASP,TENSP) do “Trung Quoc” sản xuất.

select masp, tensp from sanpham where nuocsx in ('trung quoc')










select masp, tensp from sanpham  where nuocsx = 'trung quoc'


-- 2 In ra danh sách các sản phẩm chỉ lấy (MASP, TENSP) có đơn vị tính là “cay”, ”quyen”
select masp, tensp from SANPHAM where DVT in ('cay', 'quyen')






select masp, tensp from sanpham  where dvt in ('cay' , 'quyen')

--Câu này bạn có thể so sánh bằng trực tiếp tương tự như câu trên bằng cách tình những dòng thỏa DVT = ‘CAY’ OR DVT = ‘QUYEN’





select masp, tensp from sanpham  where dvt = 'cay' or dvt = 'quyen'

--3 3. In ra danh sách các sản phẩm (MASP,TENSP) có mã sản phẩm bắt đầu là “B” và kết thúc là “01”.


select masp, tensp from sanpham  where masp like 'B%01'




select masp, tensp from sanpham  where masp like 'B%01'
--4. In ra danh sách các sản phẩm (MASP,TENSP) do “Trung Quốc” sản xuất có giá từ 30.000 đến 40.000.
select masp, tensp from sanpham  where  NUOCSX = 'trung quoc' and (GIA between 30000 and 40000)




select masp, tensp from sanpham  where nuocsx = 'trung quoc' and (gia between  30000 and 40000)

--5. In ra danh sách các sản phẩm (MASP,TENSP) do “Trung Quoc” hoặc “Viet Nam” sản xuất có giá từ 30.000 đến 40.000.

select masp, tensp from sanpham  where (nuocsx = 'trung quoc' or NUOCSX = 'viet nam')  and (gia between  30000 and 40000)






select masp, tensp from sanpham  where nuocsx in ('trung quoc','viet nam') and (gia between  30000 and 40000)

--6. In ra các số hóa đơn, trị giá hóa đơn bán ra trong ngày 1/1/2007 và ngày 2/1/2007.
select sohd, trigia from HOADON where NGHD = ('01-jan-2007') or NGHD = ('02-jan-2007')







select sohd , trigia from hoadon where nghd = '01-jan-2007' or nghd = '02-jan-2007'


--7. In ra các số hóa đơn, trị giá hóa đơn trong tháng 1/2007, sắp xếp theo ngày (tăng dần) và trị giá của hóa đơn (giảm dần).

select sohd , trigia from hoadon where MONTH (nghd) = 1 and year (nghd) = 2007 order by nghd , trigia desc




select sohd , trigia from hoadon where month (nghd)= 1 and year (nghd) =2007
order by nghd , trigia desc

--8. In ra danh sách các khách hàng (MAKH, HOTEN) đã mua hàng trong ngày 1/1/2007.

select hd.makh, kh.hoten from khachhang kh, hoadon hd where hd.makh=kh.makh and nghd ='01-jan-2007'


select hd.makh , kh.hoten from hoadon hd , khachhang kh where hd.makh=kh.makh and nghd = '01-jan-2007'

--9. In ra số hóa đơn, trị giá các hóa đơn do nhân viên có tên “Nguyen Van B” lập trong ngày 28/10/2006.

select hd.sohd, hd.trigia from NHANVIEN nv , HOADON hd
 where nv.manv = hd.manv and nv.HOTEN = 'Nguyen Van B' and nghd = '28-oct-2006'




select hd.sohd , hd.trigia from hoadon hd, nhanvien nv where hd.manv=nv.manv and hoten = 'Nguyen Van B'
and nghd ='28-oct-2006'

--10. In ra danh sách các sản phẩm (MASP,TENSP) được khách hàng có tên “Nguyen Van A” mua trong tháng 10/2006.

select sp.masp, tensp from sanpham sp, hoadon hd , CTHD ct, KHACHHANG kh

where ct.SOHD=hd.SOHD and sp.MASP=ct.MASP and kh.MAKH=hd.MAKH and
HOTEN ='nguyen van A' and MONTH (nghd)=10 and year (nghd) =2006








select ct.masp, sp.tensp from khachhang kh, hoadon hd, sanpham sp, cthd ct 
where kh.makh=hd.makh and sp.masp=ct.masp and ct.sohd = hd.sohd 
and hoten ='nguyen van a' and month(nghd)= 10 and year(nghd) =2006
--11. Tìm các số hóa đơn đã mua sản phẩm có mã số “BB01” hoặc “BB02”.
select distinct sohd from CTHD where MASP in ('BB01' , 'BB02')






select distinct sohd from cthd where masp in ('BB01' , 'BB02')


SELECT distinct hd.SOHD
FROM HOADON hd, CTHD ct
WHERE hd.SOHD = ct.SOHD AND ct.MASP IN ('BB01','BB02')
--12. Tìm các số hóa đơn đã mua sản phẩm có mã số “BB01” hoặc “BB02”, mỗi sản phẩm mua với số lượng từ 10 đến 20.

select distinct sohd from cthd where masp in ('BB01' , 'BB02') and (sl between 10 and 20)





select distinct sohd from cthd where masp in ('BB01' , 'BB02') and (sl between 10 and 20)


--13. Tìm các số hóa đơn mua cùng lúc 2 sản phẩm có mã số “BB01” và “BB02”, mỗi sản phẩm mua với số lượng từ 10 đến 20.
select distinct sohd from cthd where 







select distinct sohd from cthd where masp= 'BB01'  and (sl between 10 and 20)

intersect 
select distinct sohd from cthd  where   masp='BB02' and (sl between 10 and 20)

1
2
3
4
5
6
7
8
9
SELECT distinct hd.SOHD
FROM HOADON hd, CTHD ct
WHERE hd.SOHD = ct.SOHD AND ct.MASP='BB01' AND ct.SL BETWEEN 10 AND 20
 
INTERSECT
 
SELECT distinct hd.SOHD
FROM HOADON hd, CTHD ct
WHERE hd.SOHD = ct.SOHD AND ct.MASP = 'BB02' AND ct.SL BETWEEN 10 AND 20
--14. In ra danh sách các sản phẩm (MASP,TENSP) do “Trung Quoc” sản xuất hoặc các sản phẩm được bán ra trong ngày 1/1/2007.

select masp, tensp from SANPHAM where NUOCSX = 'trung quoc' or









select distinct sp.masp, tensp from sanpham sp, hoadon hd, cthd ct  where sp.masp=ct.masp and hd.sohd=ct.sohd and (nghd = '01-jan-2007'
or nuocsx = 'trung quoc')

SELECT distinct sp.MASP, sp.TENSP
FROM SANPHAM sp, HOADON hd, CTHD ct
WHERE (sp.NUOCSX='Trung Quoc' or hd.NGHD='1/1/2007') AND hd.SOHD = ct.SOHD and sp.MASP=ct.MASP


--15. In ra danh sách các sản phẩm (MASP,TENSP) không bán được.








select sp.masp, tensp from sanpham sp
where sp.masp not in (select distinct masp from cthd)                                    



SELECT sp.MASP, sp.TENSP
FROM SANPHAM sp,  CTHD ct
WHERE  sp.masp=ct.masp and  sp.masp not in (select distinct masp from cthd)

--16. In ra danh sách các sản phẩm (MASP,TENSP) không bán được trong năm 2006.

select  sp.masp, tensp from sanpham sp
where sp.masp not in (select distinct masp from cthd ct, hoadon hd where ct.SOHD=hd.SOHD and year (nghd)=2006)


SELECT sp.MASP,sp.TENSP
FROM SANPHAM sp
WHERE sp.MASP not in ( SELECT ct.MASP FROM HOADON hd, CTHD ct WHERE hd.SOHD = ct.SOHD AND YEAR(hd.NGHD)=2006)

--17. In ra danh sách các sản phẩm (MASP,TENSP) do “Trung Quoc” sản xuất không bán được trong năm 2006.






select  sp.masp, tensp from sanpham sp
where nuocsx='trung quoc' and sp.masp not in (select distinct masp from cthd ct, hoadon hd where ct.SOHD=hd.SOHD and year (nghd)=2006)




--18. Tìm số hóa đơn đã mua tất cả các sản phẩm do Singapore sản xuất.

select sohd , count (a.masp)  from cthd ct, (select masp from sanpham where nuocsx ='singapore') a
where a.masp=ct.masp group by ct.sohd having count(a.masp) >= (select count(masp) from sanpham where nuocsx ='singapore')







select ct.sohd, count(a.masp) from cthd ct, (select masp from SANPHAM where nuocsx ='singapore') a
where a.masp = ct.masp group by ct.sohd having count(a.masp)>= (select count(masp) from sanpham where nuocsx ='singapore')

--19. Tìm số hóa đơn trong năm 2006 đã mua ít nhất tất cả các sản phẩm do Singapore sản xuất.
select sohd from

(select ct.sohd as sohd , count(a.masp) as sl_masp, sum(sl) as sl from  cthd ct, (select masp from sanpham where nuocsx ='singapore') a, hoadon hd 
where a.masp=ct.masp and ct.SOHD = hd.SOHD and year(nghd)=2006 group by ct.sohd having count(a.MASP) >= (select count(masp) from sanpham where nuocsx ='singapore')) b
where b.sl =(select min(b.sl) from 
(select ct.sohd as sohd , count(a.masp) as sl_masp, sum(sl) as sl from  cthd ct, 
(select masp from sanpham where nuocsx ='singapore') a, hoadon hd 
where a.masp=ct.masp and ct.SOHD = hd.SOHD and year(nghd)=2006 group by ct.sohd having count(a.MASP) >= (select count(masp) from sanpham where nuocsx ='singapore')
)b  
 )







select b.sohd  from 
(
select ct.sohd, count(a.masp) as masp, sum(ct.sl) as sl  
from cthd ct, hoadon hd, (select masp from SANPHAM where nuocsx ='singapore') a
where ct.sohd = hd.sohd and  year(NGHD)=2006 and a.masp = ct.masp 
group by ct.sohd having count(a.masp)>= (select count(masp) from sanpham where nuocsx ='singapore')
)  b where b.sl = (select min(b.sl) 
from (select ct.sohd, count(a.masp) as masp, sum(ct.sl) as sl  
from cthd ct, hoadon hd, (select masp from SANPHAM where nuocsx ='singapore') a
where ct.sohd = hd.sohd and  year(NGHD)=2006 and a.masp = ct.masp group by ct.sohd 
having count(a.masp)>= (select count(masp) from sanpham where nuocsx ='singapore')) b )






--20. Có bao nhiêu hóa đơn không phải của khách hàng đăng ký thành viên mua?

select count ( sohd)  from hoadon where makh is NULL 


select count(sohd) from hoadon where makh is null
3

--21. Có bao nhiêu sản phẩm khác nhau được bán ra trong năm 2006.

select count (distinct masp) from cthd ct, hoadon hd where year(hd.NGHD)=2006 and ct.sohd=hd.sohd





select count(distinct masp) from cthd ct, hoadon hd where ct.sohd = hd.sohd and year(nghd)=2006


--22. Cho biết trị giá hóa đơn cao nhất, thấp nhất là bao nhiêu ?
select max(trigia) as max , min(trigia) as min from HOADON






select max(trigia) max, min (trigia) min from hoadon

--23. Trị giá trung bình của tất cả các hóa đơn được bán ra trong năm 2006 là bao nhiêu?
select avg(trigia) from hoadon where year(nghd) =2006






select avg(trigia) from hoadon where year(nghd)=2006
--24. Tính doanh thu bán hàng trong năm 2006.

select sum(trigia) from hoadon where year(nghd)=2006



select sum(trigia) from hoadon where year(nghd)=2006

--25. Tìm số hóa đơn có trị giá cao nhất trong năm 2006.

select sohd from hoadon where trigia =(select max(trigia) from hoadon where year(nghd)=2006)




select sohd from hoadon where trigia =(
select max(trigia) max from hoadon where year(nghd)=2006)




--26. Tìm họ tên khách hàng đã mua hóa đơn có trị giá cao nhất trong năm 2006.
select hoten from HOADON hd, KHACHHANG kh where hd.makh=kh.makh and trigia =(
select max(trigia) max from hoadon where year(nghd)=2006)





select hoten from khachhang kh,  hoadon hd where hd.makh= kh.makh and trigia =(
select max(trigia) max from hoadon where year(nghd)=2006)
--27. In ra danh sách 3 khách hàng (MAKH, HOTEN) có doanh số cao nhất.

select top 3 makh, hoten from KHACHHANG order by DOANHSO desc



SELECT top 3 MAKH, HOTEN
FROM KHACHHANG 
order by DOANHSO desc
--28. In ra danh sách các sản phẩm (MASP, TENSP) có giá bán bằng 1 trong 3 mức giá cao nhất.

select masp, tensp from SANPHAM where gia in (select distinct top 3 gia from SANPHAM order by gia desc)



4
5
6
7
8
4
5
6
7
8
SELECT MASP, TENSP
FROM SANPHAM
WHERE GIA IN
(
SELECT DISTINCT TOP 3 sp.GIA
FROM SANPHAM sp
ORDER BY sp.GIA DESC
)


--29. In ra danh sách các sản phẩm (MASP, TENSP) do “Thai Lan” sản xuất có giá bằng 1 trong 3 mức giá cao nhất 
--(của tất cả các sản phẩm).
select masp, tensp from SANPHAM
where NUOCSX ='thai lan' and gia in
(SELECT DISTINCT TOP 3 sp.GIA
FROM SANPHAM sp
ORDER BY sp.GIA DESC
)










SELECT MASP, TENSP
FROM SANPHAM
WHERE GIA IN
(
SELECT  TOP 3 sp.GIA
FROM SANPHAM sp
ORDER BY sp.GIA DESC
) and nuocsx ='thai lan'

--30. In ra danh sách các sản phẩm (MASP, TENSP) do “Trung Quoc” sản xuất có giá bằng 1 trong 3 mức giá cao nhất
-- (của sản phẩm do “Trung Quoc” sản xuất).

SELECT MASP, TENSP
FROM SANPHAM
WHERE GIA IN
(
SELECT  DISTINCT TOP 3 sp.GIA
FROM SANPHAM sp
 where nuocsx='trung quoc' ORDER BY sp.GIA DESC
) and nuocsx ='trung quoc'









SELECT MASP, TENSP
FROM SANPHAM
WHERE GIA IN
(
SELECT  TOP 3 sp.GIA
FROM SANPHAM sp where nuocsx ='trung quoc'
ORDER BY sp.GIA DESC
) and nuocsx ='trung quoc'

--31. * In ra danh sách 3 khách hàng có doanh số cao nhất (sắp xếp theo kiểu xếp hạng)
select hoten, doanhso from khachhang where doanhso in( select top 3 doanhso from KHACHHANG order by doanhso desc) order by doanhso





SELECT top 3 *
FROM KHACHHANG 
order by DOANHSO desc

--32. Tính tổng số sản phẩm do “Trung Quoc” sản xuất.
select count(masp) from sanpham where nuocsx='trung quoc'




select count(masp) from sanpham where nuocsx='trung quoc' 


--33. Tính tổng số sản phẩm của từng nước sản xuất.

select nuocsx, count(masp) from sanpham  group by nuocsx



select nuocsx, count(masp) from sanpham group by nuocsx


--34. Với từng nước sản xuất, tìm giá bán cao nhất, thấp nhất, trung bình của các sản phẩm.
select nuocsx, max(gia) max , min(gia) min, avg(gia) TB from SANPHAM group by NUOCSX



select max(gia) max, min(gia) min, avg(gia) trungbinh, nuocsx from sanpham group by nuocsx

--35. Tính doanh thu bán hàng mỗi ngày.
select nghd, sum(trigia) from HOADON group by NGHD





select sum(trigia), nghd from hoadon group by nghd


--36. Tính tổng số lượng của từng sản phẩm bán ra trong tháng 10/2006.
select ct.masp, sum(ct.sl) from hoadon hd, cthd ct where hd.sohd=ct.sohd and month(nghd)=10 and year(nghd)=2006 group by ct.masp






select ct.masp, sum(ct.sl) from hoadon hd, cthd ct where hd.sohd=ct.SOHD and month(nghd)=10 and year(nghd)=2006 
group by ct.masp


--37. Tính doanh thu bán hàng của từng tháng trong năm 2006.

select month(nghd) thang, sum(trigia) from hoadon where year(nghd)=2006 group by month(nghd)




select month(nghd) thang, sum(trigia) doanhthu from hoadon where year(nghd) =2006 group by month(nghd)

--38. Tìm hóa đơn có mua ít nhất 4 sản phẩm khác nhau.
select sohd, count(masp) from cthd  group by sohd having count(masp) >=4




select sohd, count(distinct masp) masp from cthd group by sohd having count(distinct masp) >=4

SELECT *
FROM HOADON
WHERE
SOHD in
(
    SELECT HD.SOHD
    FROM (    SELECT ct.sohd, COUNT(CT.MASP) SL
              FROM CTHD ct
              GROUP BY CT.SOHD) HD
    WHERE HD.SL>=4
)

--39. Tìm hóa đơn có mua 3 sản phẩm do “Viet Nam” sản xuất (3 sản phẩm khác nhau).
select sohd ,count(ct.masp) from cthd ct,(select masp from sanpham where nuocsx='viet nam' ) a where ct.masp=a.masp
group by sohd having count(a.masp) >=3




select sohd, count(distinct ct.masp) masp from sanpham sp, cthd ct where ct.masp=sp.masp 
and nuocsx = 'viet nam'  group by sohd having count(distinct ct.masp) >=3



2
3
4
5
SELECT ct.SOHD, count(CT.MASP)
FROM CTHD ct, SANPHAM sp
WHERE ct.MASP = sp.MASP AND sp.NUOCSX='Viet Nam'
GROUP BY ct.SOHD
HAVING count(ct.masp)>=3





--40. Tìm khách hàng (MAKH, HOTEN) có số lần mua hàng nhiều nhất.
select makh from 
(select makh, count(sohd) sl  from hoadon where makh is not null group by makh) a 
where a.sl = (select max(a.sl) from (select makh, count(sohd) sl  from hoadon where makh is not null group by makh) a)

-

 select distinct hoten ,kh.makh, count(hd.makh) from hoadon hd, khachhang kh
  where  hd.makh=kh.makh group by hoten, kh.makh having count(hd.makh) = (select top 1 count(hd.makh) from hoadon hd, khachhang kh
  where  hd.makh=kh.makh group by hoten order by count(hd.makh) desc )

  SELECT TOP 1 HOADON.MAKH, KH.HOTEN, HOADON.MAKH
FROM HOADON, KHACHHANG kh
WHERE HOADON.MAKH is not null and HOADON.MAKH = KH.MAKH
GROUP BY HOADON.MAKH, KH.HOTEN
ORDER BY count(HOADON.MAKH) DESC
--41. Tháng mấy trong năm 2006, doanh số bán hàng cao nhất ?

select top 1 sum(trigia) doanhso , month(nghd) thang from hoadon where year(nghd)=2006
group by month(nghd) order by sum(trigia) desc



select top 1 sum(TRIGIA) doanhso , month(nghd) thang from hoadon 
where year(nghd)=2006 group by  month(nghd) order by sum(TRIGIA) desc




3
4
5
6
7
8
9
10
11
SELECT MONTH(hd2.NGHD)
FROM HOADON hd2
WHERE year(hd2.NGHD)=2006
GROUP BY MONTH(hd2.NGHD)
HAVING SUM(hd2.TRIGIA)>=ALL
(
   SELECT sum(hd.TRIGIA)
   FROM HOADON hd
   WHERE year(hd.NGHD) = 2006
   GROUP BY month(hd.NGHD)
)

--42. Tìm sản phẩm (MASP, TENSP) có tổng số lượng bán ra thấp nhất trong năm 2006.

select a.masp, tensp,  a.a_sl from sanpham sp, (select masp , sum(sl) a_sl from cthd ct, hoadon hd where ct.sohd=hd.sohd and year(nghd)=2006 group by masp) a
 where a.a_sl= (select min(a.a_sl) from
(select masp , sum(sl) a_sl from cthd ct, hoadon hd where ct.sohd=hd.sohd and year(nghd)=2006 group by masp) a) and a.masp = sp.masp













select  tensp , ct.masp, sum(sl) sl from sanpham sp ,cthd ct, hoadon hd
 where sp.masp=ct.masp and ct.SOHD=hd.sohd and year(nghd)=2006 group by ct.masp, tensp having sum(sl) =
(select top 1 sum(sl) from cthd ct , hoadon hd 
 where  ct.SOHD=hd.sohd and year(nghd)=2006 group by ct.masp order by sum(sl) )

 SELECT ct1.MASP, sp.TENSP
FROM CTHD ct1, HOADON hd1, SANPHAM sp 
WHERE ct1.SOHD = hd1.SOHD AND year(hd1.NGHD)=2006 and sp.MASP = ct1.MASP
GROUP BY ct1.MASP, sp.TENSP
HAVING sum(ct1.SL) <= ALL
(
    SELECT sum(ct.SL)
    FROM CTHD ct, HOADON hd 
    WHERE ct.SOHD = hd.SOHD AND year(hd.NGHD)=2006
    GROUP BY ct.MASP
)
--43. *Mỗi nước sản xuất, tìm sản phẩm (MASP,TENSP) có giá bán cao nhất.
select sp.masp, tensp, gia, a.NUOCSX from sanpham sp,(select nuocsx, max(gia) giamax from sanpham group by nuocsx )a where sp.gia = a.giamax
 and a.NUOCSX=sp.NUOCSX









select tensp, sp.NUOCSX, sp.GIA from sanpham sp, (select nuocsx, max(gia) giamax from sanpham group by nuocsx) a where

a.NUOCSX = sp.NUOCSX and sp.GIA=a.giamax


2
3
4
5
6
SELECT sp1.NUOCSX, sp1.MASP, sp1.TENSP
FROM SANPHAM sp1, 
    (SELECT sp.NUOCSX, max(sp.GIA) giamax
    FROM SANPHAM sp
    GROUP BY sp.NUOCSX) gia_QG
WHERE sp1.NUOCSX = gia_QG.NUOCSX and sp1.GIA=gia_QG.giamax

--44. Tìm nước sản xuất ít nhất 3 sản phẩm có giá bán khác nhau.



select nuocsx, count (a.slmasp) from (select gia, nuocsx, count(distinct masp) slmasp from sanpham group by gia, nuocsx) a 
group by a.nuocsx having count(a.slmasp) >=3


---45. *Trong 10 khách hàng có doanh số cao nhất, tìm khách hàng có số lần mua hàng nhiều nhất.

SELECT hd1.MAKH, DS1.HOTEN
FROM 
(
    SELECT TOP 10 kh1.MAKH, KH1.HOTEN
    FROM KHACHHANG kh1
    WHERE kh1.MAKH is not null
    ORDER BY kh1.DOANHSO DESC
) DS1, HOADON hd1
WHERE DS1.MAKH = hd1.MAKH
GROUP BY hd1.MAKH, DS1.HOTEN
 
HAVING COUNT(HD1.SOHD)>=
ALL(
 
    SELECT count(hd.SOHD)
    FROM 
    (
        SELECT TOP 10 kh.MAKH
        FROM KHACHHANG kh
        WHERE kh.MAKH is not null
        ORDER BY kh.DOANHSO DESC
    ) DS, HOADON hd
    WHERE DS.MAKH = hd.MAKH
    GROUP BY hd.MAKH 
)
