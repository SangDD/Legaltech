-- Start of DDL Script for Package STOCK_DR.PKG_ALLCODE
-- Generated 6/20/2019 8:38:17 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_allcode
  IS
type tcursor is ref cursor;

PROCEDURE proc_allcode_getbycdname
(
p_cdname IN allcode.cdname % TYPE,
p_cursor out tcursor
);

PROCEDURE proc_allcode_gets
(
p_cdname IN allcode.cdname % TYPE,
p_cdtype IN allcode.cdtype % TYPE,
p_cursor out tcursor
);


PROCEDURE proc_allcode_getstatus
(
p_cursor out tcursor
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_allcode
IS

PROCEDURE proc_allcode_getbycdname
(
p_cdname IN allcode.cdname % TYPE,
p_cursor out tcursor
)
is
begin
    open p_cursor for select upper(a.cdname) as cdname, upper(a.cdtype) cdtype, a.cdval, a.content
    from allcode a
    where upper(a.cdname)= upper(p_cdname);
end;


PROCEDURE proc_allcode_gets
(
p_cdname IN allcode.cdname % TYPE,
p_cdtype IN allcode.cdtype % TYPE,
p_cursor out tcursor
)
is
begin
    open p_cursor for select upper(a.cdname) as cdname, upper(a.cdtype) cdtype, a.cdval, a.content
    from allcode a
    where upper(a.cdname)= upper(p_cdname) and upper(a.cdtype)= upper(p_cdtype);
end;

PROCEDURE proc_allcode_getstatus
(
p_cursor out tcursor
)
is
begin
    open p_cursor for select upper(a.cdname) as cdname, upper(a.cdtype) cdtype, a.cdval, a.content,
    case when (select status_step from dr_operator) = a.cdval then 1 else 0 end used,
    LEAD(cdval, 1, 1) OVER (ORDER BY cdval) AS next_cdval,
    LEAD(content, 1, 0) OVER (ORDER BY cdval) AS next_content
    from allcode a
    where upper(a.cdname)= 'OPERATOR' and upper(a.cdtype)= 'STATUS_STEP';
end;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_ALLCODE

-- Start of DDL Script for Package STOCK_DR.PKG_DR_ASS
-- Generated 6/20/2019 8:38:17 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_ass
  IS
type tcursor is ref cursor;


procedure proc_Autions_GetAll
(
    p_code in number,
    p_status in number,
    p_isalllot in number,
    p_datefrom in date,
    p_dateto in date,
    p_cursor out tcursor
);


PROCEDURE proc_Autions_GetCbo
(
    p_type in number,
    p_cursor out tcursor
);

FUNCTION Func_Cal_ExecQtty
(
    p_aution_id in number
) RETURN number;

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_ass
IS


procedure proc_Autions_GetAll
(
    p_code in number,
    p_status in number,
    p_isalllot in number,
    p_datefrom in date,
    p_dateto in date,
    p_cursor out tcursor
)
is
    str VARCHAR2(5000);
    strsq VARCHAR2(5000);
begin
    str:='';
    if(p_code > 0) then
        str:= str || ' and a.stock_id = ' || p_code;
    end if;
    if(p_status > -99) then
        str:= str || ' and a.status = ' || p_status;
    end if;
    if(p_isalllot >= 0) then
        str:= str || ' and a.is_all_lot = ' || p_isalllot;
    end if;
    --09/10/2015 - yeu cau 72758,72734
    strsq := 'select a.name,s.code as stockcode,s.stock_type,aa.content as stocktype_str,starting_price,
    trunc(auction_date) as auction_date,share_unit * quote_qtty as share_qtty,
    pkg_dr_ass.Func_Cal_ExecQtty(a.auction_id)*share_unit as share_unit,
    auction_time,ab.content as auction_time_str,status,ac.content as status_str,
    a.type,ad.content as type_str,a.con_auc,c.name as con_auc_str,deposit_rate,
    a.is_all_lot,(select content from allcode@ass where cdname=''ALL_LOT'' and cdval = a.is_all_lot and cdtype=''AUCTIONS'' )is_all_lot_str
    from auctions@ass a left join stocks@ass s on a.stock_id = s.stock_id and s.deleted =0
    left join allcode@ass aa on s.stock_type = aa.cdval and aa.cdname=''STOCK_TYPE'' and aa.cdtype=''STOCKS''
    left join allcode@ass ab on a.auction_time = ab.cdval and ab.cdname=''PROPERTY'' and ab.cdtype=''AUCTIONS''
    left join allcode@ass ac on a.status = ac.cdval and ac.cdname=''STATUS'' and ac.cdtype=''AUCTIONS''
    left join allcode@ass ad on a.type = ad.cdval and ad.cdname=''AUCTION_TYPE'' and ad.cdtype=''AUCTIONS''
    left join consultant_auction@ass c on a.con_auc = c.consultant_auction_id and c.deleted = 0
    where a.deleted = 0 and trunc(auction_date) between '''|| p_datefrom ||''' and ''' || p_dateto || ''''
    || str || ' order by a.name';
    --dbms_output.put_line(strsq);
    open p_cursor for strsq;

end;

PROCEDURE proc_Autions_GetCbo
(
    p_type in number,
    p_cursor out tcursor
)
is
begin
    if(p_type= 2) THEN --stock
        open p_cursor for select distinct stock_id as id,code as code
        from stocks@ass where deleted = 0 order by code asc;
    elsif(p_type= 4) THEN --trang thai dau gia
        open p_cursor for select distinct cdval as id,content as code
        from allcode@ass where cdname='STATUS' and cdtype='AUCTIONS' and cdval <> -1
        order by content asc;
    elsif(p_type= 9) THEN --hinh thuc dau gia
        open p_cursor for select distinct cdval as id,content as code
        from allcode@ass where cdname='ALL_LOT' and cdtype='AUCTIONS' order by content asc;
    end if;
end;

FUNCTION Func_Cal_ExecQtty(
    p_aution_id in number
)return NUMBER
is
    v_return number;
BEGIN
    select nvl(sum(exec_qtty),0) into v_return
    from orders_details@ass od join orders@ass o on od.order_id=o.order_id and od.deleted = 0
    where auction_id = p_aution_id
    group by o.auction_id;
    return v_return;
end;
END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_ASS

-- Start of DDL Script for Package STOCK_DR.PKG_DR_CORE
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_core
  IS
type tcursor is ref cursor;

status_no_order CONSTANT NUMBER(1) := 0;
status_un_match CONSTANT NUMBER(1) := 1;
status_path_match CONSTANT NUMBER(1) := 2;
status_all_match CONSTANT NUMBER(1) := 3;

type_board CONSTANT NUMBER(1) := 1;
type_symbol CONSTANT NUMBER(1) := 2;

NORC_QUOTE CONSTANT NUMBER(1) := 6;
NORC_NOMAL CONSTANT NUMBER(1) := 2;
NORC_REPLACE CONSTANT NUMBER(1) := 3;
NORC_CANCEL CONSTANT NUMBER(1) := 4;
NORC_EXECUTED CONSTANT NUMBER(1) := 5;
NORC_ICE_BERG CONSTANT NUMBER(1) := 8;
NORC_STOP_ORDER CONSTANT NUMBER(1) := 9;
NORC_MARKET_ORDER CONSTANT NUMBER(2) := 10;
NORC_REJECT_ORDER CONSTANT NUMBER(2) := 13;
NORC_DEAL CONSTANT NUMBER(1) := 7;

ORDER_STATUS_0 CONSTANT NUMBER(1) := 0;         --Trang thai lenh khong hieu luc
ORDER_STATUS_NORMAL CONSTANT NUMBER(1) := 1;    -- Trang thai lenh binh thuong
ORDER_STATUS_PEND CONSTANT NUMBER(1) := 2;      -- Trang thai lenh cho kiem soat
STOP_STATUS_ACTIVE CONSTANT NUMBER(1) := 3;     --TRANG THAI LENH STOP DA DC KICH HOAT
MARKET_STATUS_CONVERT CONSTANT NUMBER(1) := 4;--     Market da dc chuyen sang LO


PROCEDURE proc_allcode_core_getall
(
    p_cursor out tcursor
);

--trang thai bang / thi truong
PROCEDURE PROC_STATUS_BOARDS_GETALL
(
    p_board in number,
    p_market in number,
    p_cursor OUT tcursor
);

--trang thai CK
PROCEDURE PROC_STATUS_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_cursor OUT tcursor
);

--thong tin GD cua CK
PROCEDURE PROC_TRADED_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
);

--thong tin GD cua CK o TT_order
PROCEDURE PROC_TRADED_SYMBOL_GETALLS
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
);

--Lay bang/thi truong
PROCEDURE PROC_CBO_GETALL
(
    p_type in number,
    p_cursor OUT tcursor
);

--thong tin s? l?nh
PROCEDURE PROC_STATUS_ORDER_GETALL
(
    p_type in number,
    p_code in number,
    p_board in number,
    p_member in number,
    p_schedule in number,
    p_cursor OUT tcursor
);

--thong tin s? l?nh khop de huy KQGD
PROCEDURE PROC_STATUS_ORDER_GETS
(
    p_type in number,
    p_board in VARCHAR2,
    p_cursor OUT tcursor
);

--thong tin gia khop cuoi cung o so lenh theo chung khoan - tinh lai chi so
PROCEDURE PROC_SYMBOL_PRICE_GET
(
    p_type in number,
    p_index_id in number,
    p_cursor OUT tcursor
);

procedure proc_getlast_ordertime
(
    p_type in number,
    p_code in VARCHAR2,
    p_cursor OUT tcursor
);

PROCEDURE proc_order_member
(
    p_member in VARCHAR2,
    p_cursor OUT tcursor
);

PROCEDURE proc_sessinfo_getall
(
    p_cursor OUT tcursor
);

PROCEDURE proc_getboard_forcancel
(
    p_cursor OUT tcursor
);

PROCEDURE proc_change_session
(
    p_idboard in number,
    p_idsymbol in number,
    p_idschedule in number,
    p_type in number,
    p_last_action_time in date
);
--
--Lay order type code
FUNCTION func_get_OrderTypeCode(strId VARCHAR2) RETURN VARCHAR2;

FUNCTION func_get_OrderPrice(p_type number,strsql VARCHAR2) RETURN VARCHAR2;

FUNCTION func_get_Current(p_type number,p_id number) RETURN number;

--dangtq get data board info for main
PROCEDURE proc_get_board_main
(
    p_cursor OUT tcursor
);

-- get data symbol info for main
PROCEDURE proc_get_symbol_main
(
    p_cursor OUT tcursor
);

--
-- bussiness rule
PROCEDURE proc_Get_AllSymbol
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllBoard
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllTrading_Rule
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllTrading_Schedule
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllTrading_States
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllSymbolCalendar
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllPrice_Type
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllTick_Size
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllFoeign_Room
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_AllQuantity_Type
(
    p_cursor OUT tcursor
);
PROCEDURE proc_Get_AllMatch_Type
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_BuyMatch_Order
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_MatchPrice_Order
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_LastSeqConfirm
(
    p_type IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE Get_ExecOrderTem_ByTrad_Id
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_backup_ExecOrder_Auction
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_tem IN NUMBER
);

PROCEDURE proc_backup_AllExecOrder
(
    p_board IN VARCHAR2
);

PROCEDURE proc_Revert_ExecOrder_Auction
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_nguon IN NUMBER
);

PROCEDURE proc_backup_Order
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_tem IN NUMBER
);

PROCEDURE proc_Revert_Order
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_nguon IN NUMBER
);

PROCEDURE proc_get_schedule
(
    p_cursor OUT tcursor
);

PROCEDURE proc_get_pre_schedule
(
    p_trad_sche_id IN NUMBER,
    p_schdrule_id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_get_schedule_symbol
(
    p_cursor OUT tcursor
);

PROCEDURE proc_Get_all_symbol_Trading
(
    p_cursor OUT tcursor
);

PROCEDURE proc_update_buyMatch
(
    p_symbol IN VARCHAR2,
    p_buyMatch IN NUMBER
);

PROCEDURE proc_get_order_matching
(
    p_type in NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
);


PROCEDURE PROC_TT_ORDERS_INSERT
(
    p_org_order_id in NUMBER,--1
    p_floor_code in VARCHAR2,--2
    p_order_confirm_no in VARCHAR2,--3
    p_order_no   in VARCHAR2,  --4  --update theo orderno
    p_co_order_no   in VARCHAR2,--5
    p_org_order_no  in VARCHAR2,--6
    p_order_date  in Date,--7
    p_order_time  in VARCHAR2,--8
    p_member_id  in NUMBER,--9
    p_co_member_id  NUMBER,--10
    p_stock_id  in NUMBER,--11
    p_order_type  in NUMBER,--12
    p_priority  in NUMBER,--13
    p_oorb  in NUMBER,--14
    p_norp  in NUMBER,--15
    p_norc  in NUMBER,--16
    p_bore  in NUMBER,--17
    p_aori  in NUMBER,--18
    p_settlement_type  in NUMBER,--19
    p_dorf  in NUMBER,--20
    p_order_qtty  in NUMBER,--21
    p_order_price  in NUMBER,--22
    p_status  in NUMBER,--23
    p_quote_price  in NUMBER,--24
    p_state  in NUMBER,--25
    p_quote_time  in VARCHAR2 ,--26
    p_quote_qtty  in NUMBER ,--27
    p_exec_qtty  in NUMBER ,--28 hien dang dung trong TH sua lenh luu tam KL thay doi dua KL tong IO khi sua giam KL dinh
    p_correct_qtty  in NUMBER ,--29
    --30
    p_cancel_qtty  in NUMBER ,--30
    p_reject_qtty  in NUMBER ,--31
    p_reject_reason  in NUMBER ,--32 Co y nghia doi voi thang lenh reject
    p_account_no  in VARCHAR2 ,--33
    p_co_account_no  in VARCHAR2 ,--34
    p_broker_id  in NUMBER ,--35
    p_co_broker_id  in NUMBER  ,--36
    p_deleted  in NUMBER  ,        --37
    p_date_created  in Date  ,--38
    p_date_modified  in Date ,--39
    --40
    p_modified_by  in VARCHAR2 ,--40
    p_created_by  in VARCHAR2 ,--41
    p_telephone  in VARCHAR2  ,--42
    p_org_order_base  in VARCHAR2  ,--43
    p_settle_day  in NUMBER  ,--44 -- hien chi dung khi insert lenh khop thi = gia tri cua co_dorf
    p_aorc  in NUMBER  ,--45      --Hien dung de gui thang idmarket xuong luu db vi trg nay ko dung de = 0
    p_yieldmat  in NUMBER    ,--46
    p_clordid  in  VARCHAR2   , --47
    p_Noro  in  VARCHAR2  , --lo giao dichm 48
    p_ORDER_PRICE_STOP  in  NUMBER   , --49
    p_ORDER_QTTY_DISPLAY in   NUMBER  , --50
    p_SUB_ORDER_NO in  VARCHAR2  , --51
    p_Org_Order_Type in  NUMBER  , --52
    p_Trading_Schedule_Id  in  NUMBER , -- moi them trang thai cua tradingding_schedule 53
    p_CO_SUB_ORDER_NO in  VARCHAR2 ,  --54
    p_GroupName IN VARCHAR2, ---55
    p_NumSeqSave IN NUMBER   ,      --56
    p_Co_Bore IN NUMBER,   --- 57 . Add 07.2017
    p_Special_side IN VARCHAR2 , -- 58 Add 11.2017
    p_Special_type IN NUMBER,  -- 59 Add 11.2017
    p_Co_Special_type IN NUMBER,  -- 60 Add 12.2017
    p_Co_Special_side IN VARCHAR2, -- 61 Add 02.2018
    p_MMLINK_ORDERNO IN  VARCHAR2,  ---62 add 04.2018
    p_IsRisk_Manage IN  NUMBER,  ---63 add 04.2018
    p_CoIsRisk_Manage IN  NUMBER,  ---64 add 04.2018
    p_IsInDay IN VARCHAR2, ---65 add 04.2018
    p_CoIsInDay IN VARCHAR2 ---66 add 04.2018
--
);

PROCEDURE proc_get_symbol_in_tt_order
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE proc_delete_order_plus_tem
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER
);

PROCEDURE proc_get_total_exec_qtty
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE PROC_GET_ORDER_tem
(
    p_frm in number,--tu record
    p_to in number,--den record
    p_stock_id in number,--  ck
    p_board in number,-- bang
    p_member in number,-- thanh vien
    p_TradingSchedule_Id IN NUMBER,
    p_cursor OUT tcursor
);

PROCEDURE PROC_GET_COUNT_ORDER_TEM
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_return OUT number
);

PROCEDURE proc_checkIsReceive
(
    p_return OUT NUMBER
) ;

procedure  proc_checkEndsession
(
    p_type in number,
    p_code in VARCHAR2,
    p_return out varchar2
);

procedure proc_reject_order;

--Tinh l?i thong tin GD cua CK
PROCEDURE PROC_UPDATE_SYMBOL_INFO;
END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_core
IS

PROCEDURE proc_allcode_core_getall
(
    p_cursor out tcursor
)
IS
BEGIN
    open p_cursor for select * FROM allcode_core;
END ;

--tr?ng th?i bang / thi truong
PROCEDURE PROC_STATUS_BOARDS_GETALL
(
    p_board in number,
    p_market in number,
    p_cursor OUT tcursor
)
is
  str VARCHAR2(2000);
begin
    str :='';
    if(p_board > 0) then
        str := str || ' and A.id = '|| p_board;
    end if;
    if(p_market > 0)then
        str:=str  || ' and A.market_id = '|| p_market;
    end if;
    open p_cursor for
    'select A.*, nvl(B.klkttg_chan,0) klkttg_chan,nvl(B.gtkttg_chan,0) gtkttg_chan, nvl(B.klkttg_le,0) klkttg_le,
    nvl(B.gtkttg_le,0) gtkttg_le,nvl(B.klktt_chan,0) klktt_chan, nvl(B.gtktt_chan,0) gtktt_chan,
    nvl(B.klktt_le,0) klktt_le, nvl(B.gtktt_le,0) gtktt_le,nvl(B.tong_klgd,0) total_traded_qtty,
    nvl(B.tong_gtgd,0) total_traded_value
    from (select b.id,b.code as boardcode, m.code as marketcode,m.id as market_id,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then op.status_step
    when op.status_step = ''M'' then decode(tc.current_status,90,op.status_step,tc.current_status)
    when op.status_step = ''D'' then decode(is_backup,0,op.status_step,1,''5'')
    when op.status_step = ''E12'' then op.status_step
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then ab.content
    when op.status_step = ''M'' then decode(tc.current_status,90,ab.content,aa.content)
    when op.status_step = ''D'' then decode(is_backup,0,ab.content,1,''5'')
    when op.status_step = ''E12'' then ab.content
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status_str,decode(op.status_step,''M'',ts.code,''-'')as statecode,
    op.CURRENT_DATE as working_day,tsc.code as schedulecode, ts.inputallow,ts.matchallow,ts.sesstype,
    a.content as sesstype_str,ts.ordertype,pkg_dr_core.func_get_OrderTypeCode(ts.ordertype) as ordertype_str,
    tt.rdallow ||''|'' || tt.odallow as tradingtype,tdt.matchtype,
    (select content from allcode@hnxcore where cdname=''TS_MATCH_TYPE'' and cdtype=''MATCHTYPE'' and cdval = tdt.matchtype)matchtype_str
    from ts_boards@hnxcore  b left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted=0
    left join ts_operator_markets@hnxcore op on m.id = op.market_id
    left join ts_trading_calendars@hnxcore tc on b.id = tc.boardid and tc.is_workingday=1
    left join ts_trading_schedules@hnxcore tsc on tc.trading_schedule_id = tsc.id
    left join ts_trading_state@hnxcore ts on tsc.idstate = ts.id
    left join ts_match_type@hnxcore tdt on ts.matchtype = tdt.id
    left join allcode@hnxcore a on ts.sesstype = a.cdval  and a.cdname =''TS_TRADING_STATE'' and a.cdtype=''SESSTYPE''
    left join allcode@hnxcore aa on tc.current_status  = aa.cdval and aa.cdname =''TS_TRADING_CALENDAR'' and aa.cdtype=''CURRENT_STATUS''
    left join allcode@hnxcore ab on op.status_step  = ab.cdval and ab.cdname =''OPERATOR_MARKET'' and ab.cdtype=''CURRENT_STEP''
    left join ts_trading_type@hnxcore tt on ts.tradingtype = tt.id
    where b.deleted=0)A
    left join
    (select s.idboard,
    --tong klgd thoa thuan chan
    nvl(sum(CASE WHEN norp = 2 and noro = 1 AND norc = 7 AND tt.status in(4,5,6) THEN  order_qtty END),0)  klktt_chan,
    --tong gtgd thoa thuan chan
    nvl(sum(CASE WHEN norp = 2 and noro = 1 AND norc = 7 AND tt.status in(4,5,6) THEN order_qtty * order_price END),0) gtktt_chan ,
    --tong klgd thoa thuan le
    nvl(sum(CASE WHEN norp = 2 and noro = 2 AND norc = 7 AND tt.status in(4,5,6) THEN  order_qtty END),0)  klktt_le,
    --tong gtgd thoa thuan le
    nvl(sum(CASE WHEN norp = 2 and noro = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_qtty * order_price END),0) gtktt_le ,
    --tong klgd thong thuong chan
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 1 AND tt.norc = 5 THEN  tt.order_qtty END),0) klkttg_chan,
    -- tong gtgd thong thuong chan
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 1 AND tt.norc = 5 THEN tt.order_qtty * tt.order_price END),0) gtkttg_chan,
    --tong klgd thong thuong le
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 2 AND tt.norc = 5 THEN  tt.order_qtty END),0) klkttg_le,
    -- tong gtgd thong thuong le
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 2 AND tt.norc = 5 THEN tt.order_qtty * tt.order_price END),0) gtkttg_le,
    --tong klgd
    nvl(sum(CASE WHEN (tt.norp = 1 AND tt.norc = 5)or (norp = 2 and norc = 7 AND tt.status in(4,5,6)) THEN  tt.order_qtty END),0) as tong_klgd,
    --tong gtgd
    nvl(sum(CASE WHEN (tt.norp = 1 AND tt.norc = 5)or (norp = 2 and norc = 7 AND tt.status in(4,5,6)) THEN  tt.order_qtty * tt.order_price END),0) as tong_gtgd
    from view_tt_orders@hnxcore tt left join ts_symbol@hnxcore s on tt.stock_id = s.id
    group by s.idboard) B
    on A.id = B.idboard
    where 1=1 ' || str || ' order by A.boardcode, A.marketcode';

    EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--05/10/2015-Sua bo ck huy niem yet khoi ds theo y/c 72646
--06/10/2015-72653
PROCEDURE PROC_STATUS_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_cursor OUT tcursor
)
is
     str VARCHAR2(2000);
begin
    str :='';
    if(p_code > 0) then
        str := str || ' and s.id = '|| p_code;
    end if;
    if(p_board > 0)then
        str:=str  || ' and b.id = '|| p_board;
    end if;
    open p_cursor for
    'select s.symbol, s.name,s.idsectype,a.content as sectype, b.code as boardcode,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then op.status_step
    when op.status_step = ''M'' then decode(decode(s.idschedule,0,tc.current_status,sc.current_status),90,op.status_step,tc.current_status)
    when op.status_step = ''D'' then decode(is_backup,0,op.status_step,1,''5'')
    when op.status_step = ''E12'' then op.status_step
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then ab.content
    when op.status_step = ''M'' then decode(decode(s.idschedule,0,tc.current_status,sc.current_status),90,ab.content,aa.content)
    when op.status_step = ''D'' then decode(is_backup,0,ab.content,1,''5'')
    when op.status_step = ''E12'' then ab.content
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status_str,
    decode(op.status_step,''M'',ts.code,''-'')as statecode,op.CURRENT_DATE as working_day,tsc.code as schedulecode,
    ts.ordertype,pkg_dr_core.func_get_OrderTypeCode(ts.ordertype) as ordertype_str
    from ts_symbol@hnxcore s left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted=0
    left join ts_securities_type@hnxcore st on s.idsectype = st.id and st.deleted = 0
    left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted = 0
    left join ts_operator_markets@hnxcore op on m.id = op.market_id
    left join ts_symbol_calendar@hnxcore sc on s.id = sc.symbol_id and sc.deleted=0
    left join ts_trading_schedules@hnxcore tsc on sc.current_trading_schedule_id = tsc.id
    left join ts_trading_state@hnxcore ts on decode(sc.current_trading_state,0,tsc.idstate,sc.current_trading_state) = ts.id
    left join ts_trading_calendars@hnxcore tc on b.id = tc.boardid and tc.is_workingday = 1
    left join allcode@hnxcore a on st.type = a.cdval  and a.cdname =''TS_SERCURITIES_TYPE'' and a.cdtype=''TYPE''
    left join allcode@hnxcore aa on sc.current_status  = aa.cdval and aa.cdname =''TS_TRADING_CALENDAR'' and aa.cdtype=''CURRENT_STATUS''
    left join allcode@hnxcore ab on op.status_step  = ab.cdval and ab.cdname =''OPERATOR_MARKET'' and ab.cdtype=''CURRENT_STEP''
    left join ts_trading_type@hnxcore tt on ts.tradingtype = tt.id
    where s.deleted=0 and s.status not in (6,9) ' || str || ' order by s.symbol';
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

PROCEDURE PROC_CBO_GETALL
(
    p_type in number,
    p_cursor OUT tcursor
)
is
BEGIN
    if(p_type = 0 ) then --bang
        open p_cursor for select id, code from ts_boards@HNXCORE where deleted = 0 and status = 'A'
        order by code asc;
    elsif (p_type = 1) then --tt
        open p_cursor for select id, code from ts_markets@hnxcore where deleted = 0 and status = 'A'
        order by code asc;
    elsif (p_type = 2) then --ck
        open p_cursor for select id,symbol as code from ts_symbol@hnxcore where deleted = 0 and status not in (6,9)
        order by symbol asc;
    elsif (p_type = 3) then --member
        open p_cursor for select id, code_trade as code from ts_members@hnxcore
        where deleted = 0 and status = 'A' order by code_trade asc;
    elsif (p_type = 4) then --status
        open p_cursor for select cdval as id,content as code from allcode@hnxcore
        where cdname = 'TS_SYMBOL' and cdtype='STATUS' and cdval not in (6,9) order by content asc;
    elsif(p_type = 10) then --phien gd
        open p_cursor for select id, code from ts_trading_schedules@hnxcore
        where deleted = 0 and status = 'A' order by code asc;
    end if;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--thong tin GD cua CK lay tu bang thong tin ck
PROCEDURE PROC_TRADED_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for
    SELECT 'TT_SYMBOL_INFO' as sys,tt.symbol_date,tt.symbol, s.name,b.code as boardcode,s.idsectype, a.content as sectype,
    s.status,aa.content as status_str,s.listing_status, ab.content as listingsts,s.reference_status,ac.content as refstatus,
    nvl(s.basic_price,0)basic_price, nvl(s.celling_price,0)celling_price,nvl(s.floor_price,0)floor_price,
    nvl(s.parvalue,0)parvalue,tt.date_no,nvl(tt.open_price,0) open_price, nvl(tt.close_price,0)close_price, nvl(tt.avg_price,0)avg_price,
    nvl(tt.current_price,0)current_price,nvl(tt.current_qtty,0)current_qtty,nvl(tt.match_price,0)match_price,
    nvl(tt.match_qtty,0)match_qtty, nvl(tt.excute_rdlot_price,0)excute_rdlot_price,nvl(tt.klkchan_gannhat,0)klkchan_gannhat,
    nvl(tt.excute_oddlot_price,0)excute_oddlot_price,nvl(tt.klkle_gannhat,0)klkle_gannhat,nvl(tt.klkchan,0)klkchan,nvl(tt.gtkchan,0)gtkchan,
    nvl(tt.klkle,0)klkle,nvl(tt.gtkle,0)gtkle,nvl(tt.klkttg,0)klkttg,nvl(tt.gtkttg,0)gtkttg,nvl(tt.pt_match_price,0)pt_match_price,
    nvl(tt.pt_match_qtty,0)pt_match_qtty,nvl(tt.klktt,0)-nvl(tt.od_total_traded_qtty_pt,0) as klktt_chan,
    nvl(tt.gtktt,0)-nvl(tt.od_total_traded_value_pt,0) as gtktt_chan,nvl(tt.od_total_traded_qtty_pt,0) as klktt_le,
    nvl(tt.od_total_traded_value_pt,0) as gtktt_le,nvl(klktt,0)klktt, nvl(gtktt,0)gtktt
    from tt_symbol_info@hnxcore tt JOIN ts_symbol@hnxcore s ON s.id = tt.id_symbol
    left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted = 0
    left join ts_securities_type@hnxcore st on s.idsectype = st.id and st.deleted = 0
    left join allcode@hnxcore a on st.type = a.cdval  and a.cdname ='TS_SERCURITIES_TYPE' and a.cdtype='TYPE'
    left join allcode@hnxcore aa on s.status = aa.cdval  and aa.cdname ='TS_SYMBOL' and aa.cdtype='STATUS'
    left join allcode@hnxcore ab on s.listing_status = ab.cdval  and ab.cdname ='TS_SYMBOL' and ab.cdtype='LISTING_STATUS'
    left join allcode@hnxcore ac on s.listing_status = ac.cdval  and ac.cdname ='TS_SYMBOL' and ac.cdtype='REFERENCE_STATUS'
    where s.id like decode(p_code,-1,'%',p_code) and b.id like decode(p_board,-1,'%',p_board)
    and s.idsectype like decode(p_sectype,-1,'%',p_sectype) and s.status like decode(to_number(p_status),-1,'%',p_status)
    and s.listing_status like decode(p_listingsts,-1,'%',p_listingsts)
    and s.deleted=0 and s.status not in (6,9)
    order by tt.symbol_date,tt.symbol;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;


--thong tin GD cua CK lay tu so lenh
PROCEDURE PROC_TRADED_SYMBOL_GETALLS
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for SELECT 'TT_ORDERS' as sys, tt.symbol_date,to_char(s.symbol) as symbol, s.name,b.code as boardcode,s.idsectype,
    a.content as sectype,s.status,aa.content as status_str,s.listing_status,ab.content as listingsts,s.reference_status,
    ac.content as refstatus,s.basic_price, s.celling_price,s.floor_price ,s.parvalue ,s.date_no,
    nvl(tt.open_price,0) open_price,nvl(tt.close_price,0)close_price, nvl(tt.avg_price,0)avg_price,tt.current_price,
    tt.current_qtty,nvl(A.v_last_mtprice_nm,0) as match_price,nvl(A.v_last_mtqtty_nm,0) as match_qtty,
    nvl(A.v_last_mtprice_nm_rd,0) as EXCUTE_RDLOT_PRICE, nvl(A.v_last_mtqtty_nm_rd,0) as KLKCHAN_GANNHAT,
    nvl(A.v_last_mtprice_nm_od,0) as EXCUTE_ODDLOT_PRICE,nvl(A.v_last_mtqtty_nm_od,0) as KLKLE_GANNHAT,
    nvl(A.v_kl_khop_chan_15,0) as klkchan,nvl(A.v_gt_khop_chan_15,0) as gtkchan,nvl(A.v_kl_khop_le_25,0) as klkle,
    nvl(A.v_gt_khop_le_25,0) as gtkle,nvl(A.v_klkttg_15,0) as klkttg,nvl(A.v_gtkttg_15,0) as gtkttg,
    nvl(A.v_last_mtprice_pt,0) as pt_match_price,nvl(A.v_last_mtqtty_pt,0) as pt_match_qtty,
    nvl(A.v_klktt_25,0)-nvl(A.v_od_total_traded_qtty_pt,0) as klktt_chan,
    nvl(A.v_gtktt_25,0)-nvl(A.v_od_total_traded_value_pt,0) as gtktt_chan,nvl(A.v_od_total_traded_qtty_pt,0) as klktt_le,
    nvl(A.v_od_total_traded_value_pt,0) as gtktt_le, nvl(A.v_klktt_25,0) as klktt, nvl(A.v_gtktt_25,0) as gtktt,
    (fr.current_room - nvl(buymatch,0)) current_room
    FROM ( SELECT stock_id,
        -- tong khoi luong khop thoa thuan
        nvl(sum(CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN  order_qtty END),0)  v_klktt_25,
        -- tong gia tri khop thoa thuan
        nvl(sum(CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_qtty * order_price END),0) v_gtktt_25 ,
        --  tong khoi luong khop chan
        nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN  order_qtty END),0) v_kl_khop_chan_15,
        -- tong gia tri khop chan
        nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN order_qtty * order_price END),0) v_gt_khop_chan_15 ,
        -- tong khoi luong khop thong thuong ( lay tong kl khop cua tat ca cac lenh)
        nvl(sum(CASE WHEN norp = 1 AND norc = 5 THEN  order_qtty END),0)  v_klkttg_15,
        -- tong gia tri khop thong thuong ( lay tong gia tri khop cua tat ca cac lenh)
        nvl(sum(CASE WHEN norp = 1 AND norc = 5 THEN order_qtty * order_price END),0)  v_gtkttg_15 ,
        --  tong khoi luong khop le thong thuong
        nvl(sum(CASE WHEN tt.norp = 1 AND tt.noro = 2 AND tt.norc = 5 THEN  tt.order_qtty END),0) v_kl_khop_le_25,
        -- tong gia tri khop le thong thuong
        nvl(sum(CASE WHEN tt.norp = 1 AND tt.noro = 2 AND tt.norc = 5 THEN tt.order_qtty * tt.order_price END),0) v_gt_khop_le_25,
        -- tong khoi luong dat ban thoa thuan
        -- them deleted = 0 (ko tinh lenh xoa ngang) 12/02/2014
        nvl(SUM(CASE WHEN tt.NORC = 7  AND tt.deleted = 0 and tt.STATUS in(4,5,6) OR (tt.NORP = 2 AND tt.STATUS = 3 AND tt.deleted = 0)
        THEN tt.order_qtty END ),0) v_total_pt_offer_qtty,
        --tong kl khop cua thoa thuan lo le
        nvl(SUM(CASE WHEN tt.NORC = 7 and tt.STATUS in(4,5,6) AND tt.NORO = 2 THEN tt.order_qtty END ),0) v_od_total_traded_qtty_pt,
        --tong gt khop cua thoa thuan lo le
        nvl(SUM(CASE WHEN tt.NORC = 7 and tt.STATUS in(4,5,6) AND tt.NORO = 2 THEN tt.order_qtty*tt.order_price END ),0) v_od_total_traded_value_pt,
        --total_bid_qtty_od lo le va mua
        nvl(sum(
         CASE WHEN noro = 2 AND OORB = 1 AND (norc = 2 AND ORG_ORDER_BASE = 'L' )
             THEN  (order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)) end),0)  v_TOTAL_BID_QTTY_OD,
        --total_bid_qtty_od lo le va ban
        nvl(sum(
         CASE WHEN noro = 2 AND OORB = 2 AND (norc = 2 AND ORG_ORDER_BASE = 'L' )
             THEN  (order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)) end),0)  v_TOTAL_OFFER_QTTY_OD,
        --- v_total_bid_qtty lo chan mua
        nvl(sum(
         CASE WHEN noro = 1 AND  OORB = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
             OR (norc = 10 and ORG_ORDER_BASE ='M'))
             THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
         WHEN noro = 1 AND OORB = 1 AND (norc = 8 and ORG_ORDER_BASE ='I')
              THEN (order_qtty - nvl(cancel_qtty,0)) END),0)  v_total_bid_qtty,
        -- v_total_offer_qtty lo chan ban
        nvl(sum(
         CASE WHEN noro = 1 AND OORB = 2 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
             OR (norc = 10 and ORG_ORDER_BASE ='M'))
             THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
         WHEN noro = 1 AND OORB = 2 AND(norc = 8 and ORG_ORDER_BASE ='I')
             THEN (order_qtty - nvl(cancel_qtty,0)) END),0)  v_total_offer_qtty,
        --total_bid_qtty
        nvl(sum(case when noro = 1 AND  OORB = 1 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) v_total_bid_qtty_io_con,
        --total_offer_qtty
        nvl(sum(case when noro = 1 AND OORB = 2 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) v_total_offer_qtty_io_con,
        --gia khop gan nhat cua lenh thong thuong
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and stock_id = '||tt.stock_id),0) v_last_mtprice_nm,
        max(CASE WHEN NORC = 5 AND NORP = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 THEN order_time ELSE ' ' END) v_last_mtprice_nm,
        --kl khop gan nhat thong thuong
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and stock_id = '||tt.stock_id),0) v_last_mtqtty_nm,
         max( CASE WHEN NORC = 5 AND NORP = 1 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 THEN order_time ELSE ' ' END) v_last_mtqtty_nm,

        --gia khop gan nhat thong thuong lo le
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 2 and stock_id = '||tt.stock_id),0) v_last_mtprice_nm_od,
        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_time ELSE ' ' END) v_last_mtprice_nm_od,

        --kl khop gan nhat thong thuong lo le
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and noro = 2 and stock_id = '||tt.stock_id),0) v_last_mtqtty_nm_od,
        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_time ELSE ' ' END) v_last_mtqtty_nm_od,

        --gia khop gan nhat thong thuong lo chan
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||tt.stock_id),0) v_last_mtprice_nm_rd,
        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_time ELSE ' ' END) v_last_mtprice_nm_rd,

        --kl khop gan nhat thong thuong lo chan
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||tt.stock_id),0) v_last_mtqtty_nm_rd,
        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_time ELSE ' ' END) v_last_mtqtty_nm_rd,

        --gia khop gan nhat cua lenh thoa thuan
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 7 AND STATUS in(4,5,6) and stock_id = '||tt.stock_id),0) v_last_mtprice_pt,
        max(CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_time ELSE ' ' END)  v_last_mtprice_pt,

        --kl khop gan nhat cua lenh thoa thuan
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 7 AND STATUS in(4,5,6) and stock_id = '||tt.stock_id),0) v_last_mtqtty_pt,
        max(CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_time ELSE ' ' END)  v_last_mtqtty_pt,
        --buy match
        nvl(sum(case when (norp = 1 AND norc = 5  AND substr(co_account_no,4,1) = 'F')
        or (norp = 2 AND norc = 7 AND status IN (4,5,6) AND substr(co_account_no,4,1) = 'F' AND substr(account_no,4,1) <> 'F') then order_qtty end),0)buymatch
    from view_tt_orders@hnxcore tt where deleted = 0 GROUP BY tt.stock_id ) A
    right JOIN ts_symbol@hnxcore s ON s.id = A.stock_id and s.deleted =0 and s.status not in (6,9)
    join tt_symbol_info@hnxcore tt on s.symbol=tt.symbol
    join ts_foreign_room@hnxcore fr on s.symbol=fr.symbol
    left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted = 0
    left join ts_securities_type@hnxcore st on s.idsectype = st.id and st.deleted = 0
    left join allcode@hnxcore a on st.type = a.cdval  and a.cdname ='TS_SERCURITIES_TYPE' and a.cdtype='TYPE'
    left join allcode@hnxcore aa on s.status = aa.cdval  and aa.cdname ='TS_SYMBOL' and aa.cdtype='STATUS'
    left join allcode@hnxcore ab on s.listing_status = ab.cdval  and ab.cdname ='TS_SYMBOL' and ab.cdtype='LISTING_STATUS'
    left join allcode@hnxcore ac on s.listing_status = ac.cdval  and ac.cdname ='TS_SYMBOL' and ac.cdtype='REFERENCE_STATUS'
    where s.id like decode(p_code,-1,'%',p_code) and b.id like decode(p_board,-1,'%',p_board)
    and s.idsectype like decode(p_sectype,-1,'%',p_sectype) and s.status like decode(to_number(p_status),-1,'%',p_status)
    and s.listing_status like decode(p_listingsts,-1,'%',p_listingsts)
    and s.deleted=0 and s.status not in (6,9)
    order by symbol_date,s.symbol;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;
--
--thong tin s? l?nh
PROCEDURE PROC_STATUS_ORDER_GETALL
(
    p_type in number,--loai: dat thong thuong/dat thoa thuan/ khop
    p_code in number,--  ck
    p_board in number,-- bang
    p_member in number,-- thanh vien
    p_schedule in number,--phien gd
    p_cursor OUT tcursor
)
is
    str VARCHAR2(5000);
begin
    str :='';
    if(p_code > 0) then
        str := str || ' and stock_id = '|| p_code;
    end if;
    if(p_board > 0) then
        str := str || ' and (select b.id from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) = '|| p_board;
    end if;
    --y/c 72802
    if(p_member > 0)then
        if(p_type = 0) then
            str:=str  || ' and decode(oorb, 1,co_member_id ,member_id) = '|| p_member;
        else
            str:=str  || ' and (co_member_id = '|| p_member || ' or member_id = '|| p_member ||')';
        end if;
    end if;
    if(p_schedule > 0)then
        str:=str  || ' and A.trading_schedule_id = '|| p_schedule;
    end if;
    if(p_type = 0) then --kl
        open p_cursor for ' select A.*, rownum as rowa from
        (SELECT order_id,a.member_id,
         m.name AS member_name, m.short_name,m.code_trade AS member_code,
         t.name AS stock_type,
        decode(oorb,1, co_account_no, account_no) account_no, a.order_time,
        s.symbol, b.code AS boardcode, ma.code  marketcode,a.order_type,
        odt.code order_type_str,org_order_type, a.order_no, a.org_order_no, order_date,
        odtg.code  AS org_order_type_str, norc,  oorb,  a1.content AS soorb,
        a.order_qtty, a.order_price ,trading_schedule_id,nvl(a.quote_price,0) quote_price ,
        a.quote_qtty, nvl(a.exec_qtty,0) exec_qtty,
        a.order_qtty_display,a.order_price_stop, a.reject_qtty, a.cancel_qtty, correct_qtty,
       tras.code  AS  trading_schedules, s.idsectype AS idsectype,
        t.TYPE AS StockType, decode(broker_id,0,co_broker_id,broker_id) broker_id,
        a.status,a.noro, a2.content  AS snoro,  mu.username AS broker, mu.type AS broker_type,
        a.SPECIAL_SIDE, a.Co_SPECIAL_SIDE,nvl(ISRISK_MANAGE,0) ISRISK_MANAGE, nvl(IS_INDAY,''N'') IS_INDAY,MMLINK_ORDERNO
        from view_tt_orders@hnxcore a
           JOIN ts_symbol@hnxcore s ON s.id = a.stock_id
          JOIN ts_members@hnxcore m ON  m.id = decode(a.oorb, 1,a.co_member_id ,a.member_id) and m.deleted = 0
          JOIN ts_securities_type@hnxcore t ON t.id = s.idsectype
          JOIN  ts_boards@hnxcore b on s.idboard = b.id
          JOIN ts_markets@hnxcore ma ON  ma.id = a.idmarket
          JOIN ts_order_type@hnxcore odt ON odt.id = a.order_type
          JOIN ts_order_type@hnxcore odtg ON  odtg.id = a.org_order_type
          JOIN allcode@hnxcore a1 ON a1.cdname = ''TT_ORDERS'' and a1.cdtype=''OORB'' and a1.cdval = a.oorb
          JOIN ts_trading_schedules@hnxcore tras ON tras.id = trading_schedule_id
          JOIN allcode@hnxcore a2 ON a2.cdname = ''TT_ORDERS'' and a2.cdtype=''NORO'' and a2.cdval = a.noro
          JOIN mt_users@hnxcore mu ON mu.id = decode(a.broker_id,0,a.co_broker_id,a.broker_id)
       where norp =1 and norc <> 5 and norc <> 13 and a.deleted = 0 ' || str || ' ORDER BY order_time, order_id) A';
    elsif(p_type = 1) then --tt
         open p_cursor for 'SELECT order_id,order_date, a.order_time,member_id,co_member_id,org_order_id,yieldmat,settle_day,
            a1. content AS settlement_type,m.name AS  member_name, t.name  AS stock_type,
             m1.code_trade AS member,account_no, co_account_no,m2.code_trade AS co_member,
              s.symbol , b.code AS boardcode, ma.code AS marketcode,
              broker_id, a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,aori,
              ts.code AS trading_schedules,a2.content AS saori,a3.content AS ssnorc, a4.content AS snoro,
               nvl(org_order_no, order_no) org_order_no,u1.username AS broker,u2.username AS co_broker,co_broker_id,norc,noro,
               a.SPECIAL_SIDE, a.Co_SPECIAL_SIDE,nvl(ISRISK_MANAGE,0) ISRISK_MANAGE, nvl(IS_INDAY,''N'') IS_INDAY,MMLINK_ORDERNO
            FROM tt_orders@hnxcore a
            JOIN ts_symbol@hnxcore s ON s.id = a.stock_id
            JOIN ts_boards@hnxcore b on s.idboard = b.id
            JOIN ts_markets@hnxcore ma ON ma.id = a.idmarket
            JOIN ts_members@hnxcore m ON  m.id = decode(oorb, 1,co_member_id ,member_id)
            JOIN  ts_securities_type@hnxcore t ON t.id=s.idsectype
            JOIN ts_members@hnxcore m1 ON m1.id = a.member_id
            JOIN ts_members@hnxcore m2 ON m2.id = a.co_member_id
            JOIN ts_trading_schedules@hnxcore ts ON ts.id = trading_schedule_id
            JOIN mt_users@hnxcore u1 ON u1.id = broker_id
            JOIN mt_users@hnxcore u2 ON u2.id = co_broker_id
            JOIN allcode@hnxcore a1 ON a1.cdname=''TT_ORDERS'' and a1.cdtype=''SETTLEMENT_TYPE'' and a1.cdval = a.settlement_type
            JOIN allcode@hnxcore a2 ON a2.cdname=''TT_ORDERS'' and a2.cdtype=''AORI'' and a2.cdval = a.aori
            JOIN allcode@hnxcore a3 ON a3.cdname=''TT_ORDERS'' and a3.cdtype=''STATUS'' and a3.cdval = a.status
            JOIN allcode@hnxcore a4 ON a4.cdname=''TT_ORDERS'' and a4.cdtype=''NORO'' and a4.cdval = a.noro
            where norp = 2 and norc <> 5 and deleted = 0  ' || str || ' ORDER BY order_time, order_id)A';
    else --khop
        open p_cursor for 'SELECT AA.*,rownum as rowa from
            (SELECT order_id,order_date, a.order_time, member_id,co_member_id,aori,broker_id,co_broker_id,norc,norp,noro,oorb,
            t.name AS  stock_type, account_no, co_account_no,
            m.code_trade AS member, m.name  member_name, m.short_name  short_name,
           -- odt.code order_type_str, odtg.code org_order_type_str,
            com.code_trade  co_member, s.symbol symbol,  b.code  boardcode,
            ma.code  marketcode,
            a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,
           s.idschedule idschedule, tras.code  trading_schedules,
            nvl(org_order_no, order_no) org_order_no,
            mu.username broker,comu.username co_broker,
           a1.content  saori ,a2.content snorc, a3.content snoro, a4.content snorp,
            nvl(a.reject_qtty,0) reject_qtty,nvl(a.exec_qtty,0) exec_qtty,nvl(cancel_qtty,0)cancel_qtty,
            a.SPECIAL_SIDE, a.Co_SPECIAL_SIDE,nvl(ISRISK_MANAGE,0) ISRISK_MANAGE, nvl(IS_INDAY,''N'') IS_INDAY,MMLINK_ORDERNO
            FROM VIEW_TT_ORDERS_EXEC@hnxcore a
            JOIN ts_symbol@hnxcore s ON s.id = a.stock_id
             JOIN  ts_boards@hnxcore b on s.idboard = b.id
             JOIN ts_markets@hnxcore ma ON  ma.id = a.idmarket
             JOIN ts_members@hnxcore m ON  m.id = a.member_id and m.deleted = 0
             JOIN ts_members@hnxcore com ON  com.id = a.co_member_id and com.deleted = 0
             JOIN ts_securities_type@hnxcore t ON t.id = s.idsectype
             jOIN ts_trading_schedules@hnxcore tras ON tras.id = a.trading_schedule_id
             JOIN mt_users@hnxcore mu ON mu.id = a.broker_id
             JOIN mt_users@hnxcore comu ON comu.id = a.co_broker_id
             JOIN allcode@hnxcore a1 ON a1.cdname = ''TT_ORDERS'' and a1.cdtype=''AORI'' and a1.cdval = a.AORI
             JOIN allcode@hnxcore a2 ON a2.cdname = ''TT_ORDERS'' and a2.cdtype=''NORC'' and a2.cdval = a.NORC
            JOIN allcode@hnxcore a3 ON a3.cdname = ''TT_ORDERS'' and a3.cdtype=''NORO'' and a3.cdval = a.NORO
             JOIN allcode@hnxcore a4 ON a4.cdname = ''TT_ORDERS'' and a4.cdtype=''NORP'' and a4.cdval = a.NORP
            where ((norc=5 and norp = 1) or (norp=2 and norc =7 and a.status in (4,5,6))) and a.deleted = 0 '  || str || ' ORDER BY order_time, order_id) AA ';
    end if;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--thong tin s? l?nh khop de huy KQGD
PROCEDURE PROC_STATUS_ORDER_GETS
(
    p_type in number,
    p_board in VARCHAR2,
    p_cursor OUT tcursor
)
is
    cursor curBoard is select * FROM
    (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) idboard FROM dual
                       CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0 )
     where idboard <> -1;
     --
     cursor curStep(v_board number) is select  object_id ,step
     from  dr_step_matching d where 1=1
     and (d.object_id = v_board or d.object_id in (select id from ts_symbol@hnxcore where idboard = v_board and deleted=0));
     --
     v_idcore varchar2(100);
     v_iddr VARCHAR2(100);
     v_count number;
begin
    v_idcore :='-1,';
    v_iddr :='-1,';
    for recBoard in curBoard LOOP
        for recStep in curStep(recBoard.idboard) LOOP
            --bang 0,1 => lay ben dr
            if(recStep.step = pkg_dr_step_matching.step_backup)then
                v_iddr := v_iddr || recStep.object_id || ',';
            --bang -1,2,3 lay ben core
            elsif(recStep.step = pkg_dr_step_matching.step_nothing
            or recStep.step = pkg_dr_step_matching.step_accept_matching
            or recStep.step = pkg_dr_step_matching.step_accept_newmatching) then
                v_idcore := v_idcore || recStep.object_id || ',';
            end if;
        end loop;
    end loop;
    dbms_output.put_line(v_iddr || '-' || v_idcore);
    if(p_type = 0) then --dat
        open p_cursor for select A.*,rownum as rowa from
        (select * from
         (SELECT order_id,member_id,(select m.name from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)member_name,
        (select t.name from ts_securities_type@hnxcore t join ts_symbol@hnxcore s on t.id=s.idsectype where s.id =  a.stock_id) stock_type,
         (select m.short_name from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)short_name,
        (select m.code_trade from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)member_code,
        decode(oorb,1, co_account_no, account_no) account_no, a.order_time,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =to_char(norc))ssnorc,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,a.order_type,
        (select o.code from ts_order_type@hnxcore o where o.deleted =0 and o.id = order_type) order_type_str,org_order_type,
        a.order_no, a.org_order_no, order_date,(select code from ts_order_type@hnxcore where id = org_order_type)org_order_type_str,norc,
        oorb,(select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='OORB' and cdval =oorb )soorb,
        a.order_qtty, a.order_price ,trading_schedule_id,nvl(a.quote_price,0) quote_price , a.quote_qtty, nvl(a.exec_qtty,0) exec_qtty,
        a.order_qtty_display,a.order_price_stop, a.reject_qtty, a.cancel_qtty, correct_qtty,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        (select idsectype from ts_symbol@hnxcore s where s.id =a.stock_id and s.deleted =0) idsectype,
        (select type from ts_securities_type@hnxcore st where st.id = (select idsectype from ts_symbol@hnxcore s where s.id =a.stock_id
        and s.deleted =0)  and st.deleted =0) StockType,decode(broker_id,0,co_broker_id,broker_id) broker_id,
        status,noro,(select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro)snoro,
        (select username from mt_users@hnxcore where id = decode(broker_id,0,co_broker_id,broker_id) and deleted =0) broker,
        (select m.type from mt_users@hnxcore m where m.deleted = 0 and m.id = decode(a.broker_id,0,a.co_broker_id,a.broker_id)) broker_type
         FROM tt_orders@hnxcore a
         where deleted = 0
         and  (stock_id in (select id from ts_symbol@hnxcore where idschedule = 0 and deleted=0 and idboard in
                          (SELECT to_number(trim(regexp_substr(v_idcore, '[^,]+', 1, LEVEL))) FROM dual
                           CONNECT BY instr(v_idcore, ',', 1, LEVEL - 1) > 0))
               or stock_id in (SELECT to_number(trim(regexp_substr(v_idcore, '[^,]+', 1, LEVEL))) FROM dual
                           CONNECT BY instr(v_idcore, ',', 1, LEVEL - 1) > 0))
        union
        SELECT order_id,member_id,(select m.name from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)member_name,
        (select t.name from ts_securities_type@hnxcore t join ts_symbol@hnxcore s on t.id=s.idsectype where s.id =  a.stock_id) stock_type,
         (select m.short_name from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)short_name,
        (select m.code_trade from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)member_code,
        decode(oorb,1, co_account_no, account_no) account_no, a.order_time,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =to_char(norc))ssnorc,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,a.order_type,
        (select o.code from ts_order_type@hnxcore o where o.deleted =0 and o.id = order_type) order_type_str,org_order_type,
        a.order_no, a.org_order_no, order_date,(select code from ts_order_type@hnxcore where id = org_order_type)org_order_type_str,norc,
        oorb,(select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='OORB' and cdval =oorb )soorb,
        a.order_qtty, a.order_price ,trading_schedule_id,nvl(a.quote_price,0) quote_price , a.quote_qtty, nvl(a.exec_qtty,0) exec_qtty,
        a.order_qtty_display,a.order_price_stop, a.reject_qtty, a.cancel_qtty, correct_qtty,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        (select idsectype from ts_symbol@hnxcore s where s.id =a.stock_id and s.deleted =0) idsectype,
        (select type from ts_securities_type@hnxcore st where st.id = (select idsectype from ts_symbol@hnxcore s where s.id =a.stock_id
        and s.deleted =0)  and st.deleted =0) StockType,decode(broker_id,0,co_broker_id,broker_id) broker_id,
        status,noro,(select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro)snoro,
        (select username from mt_users@hnxcore where id = decode(broker_id,0,co_broker_id,broker_id) and deleted =0) broker,
        (select m.type from mt_users@hnxcore m where m.deleted = 0 and m.id = decode(a.broker_id,0,a.co_broker_id,a.broker_id)) broker_type
        FROM tt_orders_tem a
        where deleted = 0
        and (stock_id in (select id from ts_symbol@hnxcore where idschedule = 0 and deleted=0 and idboard in
                         (SELECT to_number(trim(regexp_substr(v_iddr, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(v_iddr, ',', 1, LEVEL - 1) > 0))
            or stock_id in (SELECT to_number(trim(regexp_substr(v_iddr, '[^,]+', 1, LEVEL))) FROM dual
                           CONNECT BY instr(v_iddr, ',', 1, LEVEL - 1) > 0)))
        order by order_date, order_id)A;
    else --khop
        open p_cursor for select A.*,rownum as rowa from
        (select * from
        (SELECT order_id,order_date, a.order_time, member_id,co_member_id,aori,broker_id,co_broker_id,norc,norp,noro,oorb,
        (select t.name from ts_securities_type@hnxcore t join ts_symbol@hnxcore s on t.id=s.idsectype where s.id =  a.stock_id) stock_type,
        (select m.name from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)member_name,
        (select m.short_name from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)short_name,
        (select m.code_trade from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)member,   account_no, co_account_no,
        (select m.code_trade from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0)co_member,
        (select o.code from ts_order_type@hnxcore o where o.deleted =0 and o.id = order_type) order_type_str,
        (select code from ts_order_type@hnxcore where deleted =0  and id = org_order_type)org_order_type_str,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
        a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,
        nvl((select idschedule from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id),0) idschedule,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='AORI' and cdval =aori)saori ,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        nvl(org_order_no, order_no) org_order_no,(select username from mt_users@hnxcore where id = broker_id and deleted =0) broker,
        (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_broker,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =to_char(norc))snorc,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro)snoro,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORP' and cdval = norp)snorp,
        nvl(a.reject_qtty,0) reject_qtty,nvl(a.exec_qtty,0) exec_qtty,nvl(cancel_qtty,0)cancel_qtty
         FROM VIEW_TT_ORDERS_EXEC@hnxcore a
         where ((norc=5 and norp = 1) or (norp=2 and norc =7 and status in (4,5,6))) and deleted = 0
         and  (stock_id in (select id from ts_symbol@hnxcore where idschedule = 0 and deleted=0 and idboard in
                          (SELECT to_number(trim(regexp_substr(v_idcore, '[^,]+', 1, LEVEL))) FROM dual
                           CONNECT BY instr(v_idcore, ',', 1, LEVEL - 1) > 0))
               or stock_id in (SELECT to_number(trim(regexp_substr(v_idcore, '[^,]+', 1, LEVEL))) FROM dual
                           CONNECT BY instr(v_idcore, ',', 1, LEVEL - 1) > 0))
        union
        SELECT order_id,order_date, a.order_time, member_id,co_member_id,aori,broker_id,co_broker_id,norc,norp,noro,oorb,
        (select t.name from ts_securities_type@hnxcore t join ts_symbol@hnxcore s on t.id=s.idsectype where s.id =  a.stock_id) stock_type,
        (select m.name from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)member_name,
        (select m.short_name from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)short_name,
        (select m.code_trade from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)member,   account_no, co_account_no,
        (select m.code_trade from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0)co_member,
        (select o.code from ts_order_type@hnxcore o where o.deleted =0 and o.id = order_type) order_type_str,
        (select code from ts_order_type@hnxcore where deleted =0  and id = org_order_type)org_order_type_str,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
        a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,
        nvl((select idschedule from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id),0) idschedule,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='AORI' and cdval =aori)saori ,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        nvl(org_order_no, order_no) org_order_no,(select username from mt_users@hnxcore where id = broker_id and deleted =0) broker,
        (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_broker,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =to_char(norc))snorc,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro)snoro,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORP' and cdval = norp)snorp,
        nvl(a.reject_qtty,0) reject_qtty,nvl(a.exec_qtty,0) exec_qtty,nvl(cancel_qtty,0)cancel_qtty
        FROM tt_orders_plus_tem a
        where norc=5 and norp = 1 and deleted = 0
        and (stock_id in (select id from ts_symbol@hnxcore where idschedule = 0 and deleted=0 and idboard in
                         (SELECT to_number(trim(regexp_substr(v_iddr, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(v_iddr, ',', 1, LEVEL - 1) > 0))
            or stock_id in (SELECT to_number(trim(regexp_substr(v_iddr, '[^,]+', 1, LEVEL))) FROM dual
                           CONNECT BY instr(v_iddr, ',', 1, LEVEL - 1) > 0)))
        order by order_date, order_id)A;
    end if;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--thong tin gia khop cuoi cung o so lenh theo chung khoan - tinh lai chi so
PROCEDURE PROC_SYMBOL_PRICE_GET
(
    p_type in number,
    p_index_id in number,
    p_cursor OUT tcursor
)
is
begin
    if(p_type = -1) then --Lay all
        open p_cursor for
          SELECT s.id as stock_id,s.symbol as code,s.idsectype as STOCK_TYPE, t.name as STOCK_TYPE_str,s.date_no,s.status,
         (select content from allcode@hnxcore where cdname='TS_SYMBOL' and cdtype='STATUS' and cdval=to_char(s.status)) as STATUS_str,
         (select content from allcode@hnxcore where cdname='TS_SYMBOL' and cdtype='LISTING_STATUS' and cdval=to_char(s.listing_status)) as LISTING_STATUS_str,
         s.listing_status,'Core' as sys,
          --gia khop gan nhat cua lenh thong thuong lo chan
         -- pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as index_price,
          max( CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE ' ' END) AS index_price,
          --tong klgd kl / tong gtgd kl - l? ch?n
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN  order_qtty END),0) nm_total_traded_qtty,
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN order_qtty * order_price END),0) nm_total_traded_value,
          --tong klgd tt/ tong gtgd tt - l? ch?n
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty END ),0) pt_total_traded_qtty,
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty*tt.order_price END ),0) pt_total_traded_value,
          --tong kl dat mua th?ng thu?ng - lo chan
          nvl(sum(CASE WHEN noro = 1 AND  OORB = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
                    OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
                  WHEN noro = 1 AND OORB = 1 AND (norc = 8 and ORG_ORDER_BASE ='I') THEN (order_qtty - nvl(cancel_qtty,0)) END),0)
          - nvl(sum(case when noro = 1 AND  OORB = 1 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) as total_bid_qtty,
          -- tong kl dat mua th?ng thu?ng - lo chan
          nvl(sum(CASE WHEN noro = 1 AND OORB = 2 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
                    OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
                  WHEN noro = 1 AND OORB = 2 AND(norc = 8 and ORG_ORDER_BASE ='I') THEN (order_qtty - nvl(cancel_qtty,0)) END),0)
          - nvl(sum(case when noro = 1 AND OORB = 2 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) as total_offer_qtty
          from ts_symbol s 
          left join view_tt_orders tt ON s.id = tt.stock_id and tt.deleted =0
          left join ts_securities_type t on s.idsectype = t.id and t.deleted =0
          where s.deleted = 0 and s.status not in (6,9)
          GROUP BY s.id,s.symbol,s.idsectype,t.name,s.date_no,s.status, s.listing_status;
    else --chi lay nhung ck da giao dich vs gia cuoi cung
         open p_cursor for
         SELECT s.id as stock_id,s.symbol as code,
          --gia khop gan nhat cua lenh thong thuong lo chan
         -- pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as index_price,
          max(CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_price ELSE 0 END ) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE ' ' END) AS index_price,
          --gia khop DAU TIEN cua lenh thong thuong lo chan theo ck
          --pkg_dr_core.func_get_OrderPrice(-1,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as begin_idxprice,
          max(CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK FIRST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE '99:99:99.999999' END) AS begin_idxprice,
          --
          --pkg_dr_core.func_get_OrderPrice(2,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as order_time,
         max(CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE '00:00:00:000000' END  ) KEEP (DENSE_RANK FIRST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE '99:99:99:999999' END) AS order_time,
          --tong klgd kl / tong gtgd kl - l? ch?n
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN  order_qtty END),0) nm_total_traded_qtty,
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN order_qtty * order_price END),0) nm_total_traded_value,
          --tong klgd tt/ tong gtgd tt - l? ch?n
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty END ),0) pt_total_traded_qtty,
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty*tt.order_price END ),0) pt_total_traded_value,
          --tong kl dat mua th?ng thu?ng - lo chan
          nvl(sum(CASE WHEN noro = 1 AND  OORB = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
                    OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
                  WHEN noro = 1 AND OORB = 1 AND (norc = 8 and ORG_ORDER_BASE ='I') THEN (order_qtty - nvl(cancel_qtty,0)) END),0)
          - nvl(sum(case when noro = 1 AND  OORB = 1 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) as total_bid_qtty
          from ts_symbol s 
          join view_tt_orders tt ON s.id = tt.stock_id
          left join ts_securities_type t on s.idsectype = t.id and t.deleted =0
          where tt.deleted = 0 and s.deleted = 0
          GROUP BY s.id,s.symbol;
   end if;
     EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

PROCEDURE proc_order_member
(
    p_member in VARCHAR2,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for select m.id, m.code_trade as member_code, nvl(sum (count_nm_order),0) as cmsgInNM,
    nvl(sum (count_pt_order),0) as cmsgInPT,nvl(sum(count_nm_traded),0) as cmsgMatchNM,
    nvl(sum(count_pt_traded),0) as cmsgMatchPT
    from ts_members@hnxcore m left join
    (select member_id as member_id,(select m.code_trade from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)member_code,
        --so lenh dat thong thuong - lenh huy toan bo
        COUNT(CASE WHEN norp = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S' ) )
               OR (norc =8 and ORG_ORDER_BASE ='I') OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN order_id END)
        - COUNT(CASE WHEN norp = 1 AND((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S') and (order_qtty + correct_qtty  = cancel_qtty ) )
               OR (norc =8 and ORG_ORDER_BASE ='I' and (order_qtty + correct_qtty  = cancel_qtty )) ) THEN order_id END) as count_nm_order,
        --So lenh dat thoa thuan
        COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in (3,4,5,6) AND tt.deleted = 0 THEN order_id END) count_pt_order,
        --So lenh khop thong thuong
        COUNT (CASE WHEN norp = 1 AND norc = 5 THEN order_id END)  as count_nm_traded,
        --So lenh khop thoa thuan
        COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_id END) as count_pt_traded
    from view_tt_orders@hnxcore tt where member_id <> 0  and oorb =2
    group by member_id
    union
    select co_member_id as member_id,(select m.code from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0)member_code,
       --so lenh dat thong thuong - lenh huy toan bo
       COUNT(CASE WHEN norp = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S' ) )
              OR (norc =8 and ORG_ORDER_BASE ='I') OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN order_id END)
       - COUNT(CASE WHEN norp = 1 AND((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S') and (order_qtty + correct_qtty  = cancel_qtty ) )
              OR (norc =8 and ORG_ORDER_BASE ='I' and (order_qtty + correct_qtty  = cancel_qtty )) ) THEN order_id END) as count_nm_order,
       --So lenh dat thoa thuan
       COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in (3,4,5,6) AND tt.deleted = 0 THEN order_id END) count_pt_order,
       --So lenh khop thong thuong
       COUNT (CASE WHEN norp = 1 AND norc = 5 THEN order_id END)  as count_nm_traded,
       --So lenh khop thoa thuan
       COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_id END) as count_pt_traded
    from view_tt_orders@hnxcore tt where co_member_id <> 0  and oorb =1
    group by co_member_id ) on m.id = member_id
    where m.deleted = 0 and m.status ='A' and m.code_trade like decode(p_member,'','%',p_member)
    group by m.id, m.code_trade
    order by m.code_trade asc;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

PROCEDURE proc_sessinfo_getall
(
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for select idboard,decode(idsymbol,0,brcode,'-') as brcode,idsymbol, symbol,changetype,status,
    current_status,strCurrent,strLast,idLast,step_matching,(select content from allcode where cdname='STEP_MATCHING'
    and cdtype='STEP' and cdval = step_matching) process_status
    from
    (select b.id as idboard, b.code as brcode,0 as idsymbol, '-' symbol,sm.type as changetype,sm.step as step_matching,
    tc.current_status as status,case when op.status_step = 'B' or op.status_step = 'B1' then ab.content
     when op.status_step = 'M' then decode(tc.current_status,90,ab.content,aa.content)
     when op.status_step = 'D' then decode(is_backup,0,ab.content,1,'5')
     when op.status_step = 'E12' then ab.content
     when op.status_step like 'D%' then '6'
     when op.status_step like 'E%' then '7' end current_status,tsc.code as strCurrent,
     (select id from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
     from ts_trading_schedules@hnxcore where idschdrule = b.idschedule and deleted =0 and status ='A')
     and idschdrule = b.idschedule and deleted =0 and status ='A') as idLast,
     (select code from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
     from ts_trading_schedules@hnxcore where idschdrule = b.idschedule and deleted =0 and status ='A')
     and idschdrule = b.idschedule and deleted =0 and status ='A') as strLast
     from ts_boards@hnxcore  b left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted=0
     left join dr_step_matching sm on b.id = sm.object_id
     left join ts_operator_markets@hnxcore op on m.id = op.market_id
     left join ts_trading_calendars@hnxcore tc on b.id = tc.boardid and tc.is_workingday=1
     left join ts_trading_schedules@hnxcore tsc on tc.trading_schedule_id = tsc.id
     left join allcode@hnxcore aa on tc.current_status  = aa.cdval and aa.cdname ='TS_TRADING_CALENDAR' and aa.cdtype='CURRENT_STATUS'
     left join allcode@hnxcore ab on op.status_step  = ab.cdval and ab.cdname ='OPERATOR_MARKET' and ab.cdtype='CURRENT_STEP'
     where b.deleted=0
    union
    select nvl(b.id,0) as idboard,nvl(b.code,'-') as brcode,s.id as idsymbol,s.symbol,sm.type as changetype,sm.step as step_matching,
    sc.current_status as status,
    case when tsc.code = (select code from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
     from ts_trading_schedules@hnxcore where idschdrule = s.idschedule and deleted =0 and status ='A')
     and idschdrule = s.idschedule and deleted =0 and status ='A') and sc.current_status = 13 then aa.content
    when op.status_step = 'B' or op.status_step = 'B1' then ab.content
    when op.status_step = 'M' then decode(sc.current_status,90,ab.content,aa.content)
     when op.status_step = 'D' then decode(is_backup,0,ab.content,1,'5')
     when op.status_step = 'E12' then ab.content
     when op.status_step like 'D%' then '6'
     when op.status_step like 'E%' then '7' end current_status,tsc.code as strCurrent,
     (select id from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
     from ts_trading_schedules@hnxcore where idschdrule = s.idschedule and deleted =0 and status ='A')
     and idschdrule = s.idschedule and deleted =0 and status ='A') as idLast,
     (select code from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
     from ts_trading_schedules@hnxcore where idschdrule = s.idschedule and deleted =0 and status ='A')
     and idschdrule = s.idschedule and deleted =0 and status ='A') as strLast
     from ts_symbol@hnxcore s left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted=0
     left join dr_step_matching sm on s.id = sm.object_id
     left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted = 0
     left join ts_operator_markets@hnxcore op on m.id = op.market_id
     left join ts_symbol_calendar@hnxcore sc on s.id = sc.symbol_id and sc.deleted=0
     left join ts_trading_schedules@hnxcore tsc on sc.current_trading_schedule_id = tsc.id
     left join allcode@hnxcore aa on sc.current_status  = aa.cdval and aa.cdname ='TS_TRADING_CALENDAR' and aa.cdtype='CURRENT_STATUS'
     left join allcode@hnxcore ab on op.status_step = ab.cdval and ab.cdname ='OPERATOR_MARKET' and ab.cdtype='CURRENT_STEP'
     where s.deleted=0 and s.status not in (6,9) and s.idschedule > 0)
order by symbol, brcode;
  EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

PROCEDURE proc_getboard_forcancel
(
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for select b.id as idboard, b.code as brcode,sm.type as changetype,sm.step as step_matching,
    (select content from allcode where cdname='STEP_MATCHING' and cdtype='STEP' and cdval = sm.step) process_status,
    tc.current_status as status,case when op.status_step = 'B' or op.status_step = 'B1' then ab.content
     when op.status_step = 'M' then decode(tc.current_status,90,ab.content,aa.content)
     when op.status_step = 'D' then decode(is_backup,0,ab.content,1,'5')
     when op.status_step = 'E12' then ab.content
     when op.status_step like 'D%' then '6'
     when op.status_step like 'E%' then '7' end current_status,tsc.code as strCurrent,
     (select id from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
     from ts_trading_schedules@hnxcore where idschdrule = b.idschedule and deleted =0 and status ='A')
     and idschdrule = b.idschedule and deleted =0 and status ='A') as idLast,
     (select code from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
     from ts_trading_schedules@hnxcore where idschdrule = b.idschedule and deleted =0 and status ='A')
     and idschdrule = b.idschedule and deleted =0 and status ='A') as strLast
     from ts_boards@hnxcore  b left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted=0
     left join dr_step_matching sm on b.id = sm.object_id
     left join ts_operator_markets@hnxcore op on m.id = op.market_id
     left join ts_trading_calendars@hnxcore tc on b.id = tc.boardid and tc.is_workingday=1
     left join ts_trading_schedules@hnxcore tsc on tc.trading_schedule_id = tsc.id
     left join allcode@hnxcore aa on tc.current_status  = aa.cdval and aa.cdname ='TS_TRADING_CALENDAR' and aa.cdtype='CURRENT_STATUS'
     left join allcode@hnxcore ab on op.status_step  = ab.cdval and ab.cdname ='OPERATOR_MARKET' and ab.cdtype='CURRENT_STEP'
     where b.deleted=0
    order by  brcode;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;


PROCEDURE proc_change_session
(
    p_idboard in number,
    p_idsymbol in number,
    p_idschedule in number,
    p_type in number,
    p_last_action_time in date
)
is
    v_order_id number;
begin
    --Lay id
    select seq_tt_orders.NEXTVAL@hnxcore INTO v_order_id FROM dual;
    --1.la bang
    if(p_type = type_board) then
        --1.1.Cap nhat cho bang
        update ts_trading_calendars@hnxcore set trading_schedule_id = p_idschedule,
        current_status = 13,pre_status_pause =0,last_action_time = p_last_action_time
        where boardid = p_idboard and deleted = 0 and is_workingday=1;
        --
        update ts_operator_markets@hnxcore set status_step = 'M'
        where market_id = (select idmarket from ts_boards@hnxcore where deleted =0 and status='A' and id =p_idboard);
        --1.2.Cap nhat cho ck thuoc bang
        update ts_symbol_calendar@hnxcore SET CURRENT_TRADING_SCHEDULE_ID = p_idschedule,
        current_status = 13,CURRENT_TRADING_STATE = 0,pre_status_pause =0,last_action_time = p_last_action_time
        where symbol_id in (select id from ts_symbol@hnxcore where IDBOARD = p_idboard and nvl(IDSCHEDULE,0)=0 and deleted = 0);


        --1.4.danh dau la da chuyn vao ket thuc phien gd cuoi cung
        update dr_step_matching set step = pkg_dr_step_matching.step_end_session where object_id = p_idboard;
    --2.la chung khoan co phien rieng
    else
        --2.1. Cap nhat lai calendar cho ck
        update ts_symbol_calendar@hnxcore set current_trading_schedule_id = p_idschedule,
        current_status = 13,pre_status_pause =0,last_action_time = p_last_action_time
        where symbol_id = p_idsymbol and deleted = 0;


     --2.3.danh dau la da chuyn vao ket thuc phien gd cuoi cung
        update dr_step_matching set step = pkg_dr_step_matching.step_end_session where object_id = p_idsymbol;
    end if;
    --
    commit;
    EXCEPTION
    when others then
        ROLLBACK;
    raise;
end;


procedure proc_getlast_ordertime
(
    p_type in number,
    p_code in VARCHAR2,
    p_cursor OUT tcursor
)
is
begin
     if(p_type = -1) then --lay tg lon nhat trong tt_order_plus theo chi so cua etf
        open p_cursor for select distinct max(tt.order_time)order_time, max(tt.order_date) order_date
        from tt_orders_plus@hnxcore tt join ts_symbol@hnxcore ts on tt.stock_id = ts.id and ts.deleted = 0
        where tt.deleted = 0
        and ts.symbol in (select symbol_code from etf_pcf@hnxetf where etf_code = p_code);
    elsif(p_type=0) then --lay tg lon nhat trong tt_order_plus theo chi so
        open p_cursor for select distinct max(order_time)order_time, max(order_date) order_date
        from tt_orders_plus@hnxcore
        where deleted = 0
        and stock_id in (select bs.stock_id from idx_basket_info@hnxindex bs join idx_stocks_info@hnxindex st
            on bs.stock_id = st.stock_id where index_id = to_number(p_code) and is_calcindex = 1);
    else  --lay ngay min trong ts_operator_market
        open p_cursor for select distinct min(a.current_date) as working_date
        from ts_operator_markets@hnxcore a;
    end if;
end;

FUNCTION func_get_OrderTypeCode(strId VARCHAR2) RETURN VARCHAR2
is
codes VARCHAR2(1000);
BEGIN
     select  rtrim (xmlagg (xmlelement (e, code || '|')).extract ('//text()'), ',') into codes
     from  ts_order_type@hnxcore
     where to_char(id) in (select regexp_substr(trim(strId),'[^|]+', 1, level) from dual
     connect by regexp_substr(trim(strId), '[^|]+', 1, level) is not null )
     and deleted =0;
     select utl_i18n.unescape_reference(codes) into codes from dual;
     DBMS_OUTPUT.PUT_LINE(codes);
     RETURN rtrim(codes ,'|');
    EXCEPTION
    when others then
        RAISE;

END;

--
FUNCTION func_get_OrderPrice(p_type number,strsql VARCHAR2) RETURN VARCHAR2
is
v_order_price number;
v_outval varchar2(500);
maxtime VARCHAR2(30);
mintime VARCHAR2(30);
BEGIN

     EXECUTE IMMEDIATE 'select nvl(max(order_time),'''') from view_tt_orders@hnxcore where deleted = 0 and ' || strsql ||'' into maxtime;
     if(p_type = 0) then --gia cuoi cung
        EXECUTE IMMEDIATE 'select nvl((select order_price from view_tt_orders@hnxcore
        where deleted = 0 and rownum = 1 and order_time = '''|| maxtime || '''and ' || strsql || '),0) from dual ' into v_order_price;
        v_outval := to_char(v_order_price) ;
     elsif (p_type = 1) then --kl
        EXECUTE IMMEDIATE 'select nvl((select order_qtty from view_tt_orders@hnxcore
        where deleted = 0 and rownum = 1 and order_time = '''|| maxtime || '''and ' || strsql || '),0) from dual ' into v_order_price;
        v_outval := to_char(v_order_price) ;
     elsif(p_type = -1) then  --gia dau tien (-1)
        EXECUTE IMMEDIATE 'select nvl(min(order_time),'''') from view_tt_orders@hnxcore where deleted = 0 and ' || strsql ||'' into mintime;
        --
        EXECUTE IMMEDIATE 'select nvl((select order_price from view_tt_orders@hnxcore
        where deleted = 0 and rownum = 1 and order_time = '''|| mintime || '''and ' || strsql || '),0) from dual ' into v_order_price;
        v_outval := to_char(v_order_price) ;
     elsif(p_type = 2) then --tg
        begin
            EXECUTE IMMEDIATE 'select nvl(min(order_time),''00:00:00:000000'') from view_tt_orders@hnxcore where deleted = 0 and ' || strsql ||'' into mintime;
            v_outval := mintime ;
            EXCEPTION
              WHEN OTHERS THEN
              v_outval :='00:00:00:000000';
        end;
     else --gia tri
        EXECUTE IMMEDIATE 'select nvl((select order_qtty*order_price from view_tt_orders@hnxcore
        where deleted = 0 and rownum = 1 and order_time = '''|| maxtime || '''and ' || strsql || '),0) from dual ' into v_order_price;
        v_outval := to_char(v_order_price) ;
     end if;
    RETURN v_outval;

END;
--Lay gia va kl khop du kien
FUNCTION func_get_Current(p_type number,p_id number) RETURN number
is
    v_return number;
    v_value number;
    v_count number;
    v_countmatch number;
    v_trading_schedule number;
begin
    --lay phien hien tai cua ck ra
    select current_trading_schedule_id into v_trading_schedule
    from ts_symbol_calendar@hnxcore where symbol_id = p_id;
    --Kiem tra xem phien hien tai co phai phien dk
    select count(*) into v_count
    from ts_trading_schedules@hnxcore tsc
    join ts_trading_state@hnxcore tst on tsc.idstate = tst.id and tst.deleted = 0
    join ts_match_type@hnxcore mt on tst.matchtype = mt.id and mt.deleted = 0
    where mt.matchtype='A' and tsc.id = v_trading_schedule;
    --Dem so lenh khop trong phien dk
    select count(*) into v_countmatch from tt_orders_plus
    where trading_schedule_id = v_trading_schedule and stock_id = p_id and deleted = 0;
    --Xet la lay gia hay kl khop du kien
    if(p_type = 0) then --gia
        if(v_count > 0) then --phien hien tai la pdk
            if(v_countmatch > 0) then --co lenh khop lay = gia khop bat ky
                select nvl(order_price,0) into v_value from tt_orders_plus@hnxcore
                where trading_schedule_id = v_trading_schedule and stock_id = p_id and deleted = 0 and rownum = 1;
            else --chua co lenh khop lay tu tt_symbol info
                select nvl(current_price,0) into v_value from tt_symbol_info@hnxcore where id = p_id;
            end if;
        else --ko phai phien dk lay tu tt_symbol info
            select nvl(current_price,0) into v_value from tt_symbol_info@hnxcore where id = p_id;
        end if;
    else --kl
        if(v_count > 0) then --phien hien tai la pdk
            if(v_countmatch > 0) then --co lenh khop lay = gia khop bat ky
                select sum(nvl(order_qtty,0)) into v_value from tt_orders_plus@hnxcore
                where trading_schedule_id = v_trading_schedule and stock_id = p_id and deleted = 0
                group by stock_id;
            else --chua co lenh khop lay tu tt_symbol info
                select nvl(current_qtty,0) into v_value from tt_symbol_info@hnxcore where id = p_id;
            end if;
        else --ko phai phien dk lay tu tt_symbol info
            select nvl(current_qtty,0) into v_value from tt_symbol_info@hnxcore where id = p_id;
        end if;
    end if;
    return v_value;
    EXCEPTION
    WHEN OTHERS THEN
    return 0;
end;
--dangtq get data board info for main
PROCEDURE proc_get_board_main
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT b.code,tc.working_day,tc.current_status,op.status_step,
        al.content AS current_status_name,tsh.code Session_Code,
        nvl(tt.total_traded_qtty_nm,0) AS qtty_nm,nvl(tt.TOTAL_TRADED_VALUE_NM,0) AS value_nm,
        nvl(tt.total_traded_qtty_pt,0) AS qtty_pt,nvl(tt.total_traded_value_pt,0) AS value_pt
        FROM ts_trading_calendars@hnxcore tc
        JOIN ts_boards@hnxcore b ON b.id = tc.boardid AND b.deleted = 0
        LEFT JOIN tt_boards_info@hnxcore tt ON tt.idboard = b.id
        LEFT JOIN ts_operator_markets@hnxcore op ON b.idmarket = op.market_id
        LEFT JOIN allcode@hnxcore al on tc.current_status = al.cdval and al.cdname ='TS_TRADING_CALENDAR' and al.cdtype = 'CURRENT_STATUS'
        LEFT JOIN ts_trading_schedules@hnxcore tsh ON tc.trading_schedule_id = tsh.id
        WHERE is_workingday = 1
        order by b.code;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_get_symbol_main
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT s.id,s.symbol,sc.current_status,al.content AS current_status_name,s.basic_price,s.CELLING_PRICE,
    s.FLOOR_PRICE,tsh.code Session_Code,nvl(tt.EXCUTE_RDLOT_PRICE,0) as LastPrice_NM,
    nvl(tt.KLKTTG,0) AS qtty_nm,nvl(tt.GTKTTG,0) AS value_nm,
    nvl(tt.KLKTT,0) AS qtty_pt,nvl(tt.GTKTT,0) AS value_pt
    FROM ts_symbol@hnxcore s
        LEFT JOIN ts_symbol_calendar@hnxcore sc ON sc.symbol_id = s.id
        LEFT JOIN tt_symbol_info@hnxcore tt ON tt.id_symbol = s.id
        LEFT JOIN allcode@hnxcore al on sc.current_status = al.cdval and al.cdname ='TS_TRADING_CALENDAR' and al.cdtype = 'CURRENT_STATUS'
        LEFT JOIN ts_trading_schedules@hnxcore tsh ON sc.CURRENT_TRADING_SCHEDULE_ID = tsh.id
     WHERE s.deleted = 0 AND s.status <> 6
     ORDER BY s.symbol;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

-- bussiness rule
PROCEDURE proc_Get_AllSymbol
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT s.id,symbol,idboard AS broadId,s.idtradingrule AS tradingRuleId,
    celling_price,floor_price,basic_price,open_price,close_price,
    match_price,PRIOR_CLOSE_PRICE AS prev_close_price
    FROM ts_symbol@HNXCORE s
    JOIN ts_boards@HNXCORE b ON b.id = s.idboard AND b.deleted = 0 AND b.status = 'A'
    WHERE s.deleted = 0 AND s.status NOT IN (6,9);
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllBoard
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT id,name,code,idmarket,status,idschedule,idtradingrule
    FROM ts_boards@HNXCORE WHERE deleted = 0;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllTrading_Rule
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT tr.id, tr.pricetype,tr.qttytype,tr.requesttype,tr.matchtype,
    tr.ordertype,tr.tradingtype ,tr.moneytype,tr.foreigntype
    FROM ts_trading_rules@HNXCORE tr
    --LEFT JOIN ts_price_type p ON p.id = tr.pricetype AND p.deleted = 0
    --LEFT JOIN ts_quantity_type q ON q.id = tr.qttytype AND q.deleted = 0
    WHERE tr.deleted = 0;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllTrading_Schedule
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT ts.id,ts.code,ts.idstate,ts.idschdrule
    FROM ts_trading_schedules@HNXCORE ts
    WHERE ts.deleted = 0;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllTrading_States
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT ts.id,ts.code,ts.sesstype,ts.matchtype,ts.ordertype
    FROM ts_trading_state@HNXCORE ts
    WHERE ts.deleted = 0 AND ts.status ='A';

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllSymbolCalendar
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT symbol_id,current_status,first_current_Status,Pre_Status_Pause,
    current_trading_schedule_id AS current_Trading_Schedule_Id,
    current_trading_state AS current_Trading_State_Id,
    first_trading_schedule_id AS first_trading_schedule_id
    FROM ts_symbol_calendar@HNXCORE;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllPrice_Type
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT pt.id,pt.code,pt.ticksizeid,pt.resvolume,pt.defticksize,pt.maxlimitprice,pt.minlimitprice
    FROM ts_price_type@HNXCORE pt
    WHERE pt.deleted = 0;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllTick_Size
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT id,fromvalue,tovalue,ticksize
    FROM ts_tick_size@HNXCORE
    WHERE deleted = 0;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllFoeign_Room
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT f.symbol,nvl(f.buy_match,0) buy_match,nvl(f.current_room,0) current_room
    FROM ts_foreign_room@HNXCORE f
    JOIN ts_symbol@HNXCORE s ON s.symbol = f.symbol AND s.deleted = 0
    WHERE f.deleted = 0 ORDER BY f.symbol;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllQuantity_Type
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT qt.id,qt.code,qt.tradelot
    FROM ts_quantity_type@HNXCORE qt
    WHERE qt.deleted = 0;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_AllMatch_Type
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT mt.id,mt.code,mt.matchtype,mt.aucntype AS uncntype
    FROM ts_match_type@HNXCORE mt
    WHERE mt.deleted = 0;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_BuyMatch_Order
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    IF p_type = type_board THEN
    OPEN p_cursor FOR
        SELECT sum(order_qtty) buy_match, stock_id FROM
        (
            SELECT order_qtty,stock_id
                FROM tt_orders@HNXCORE t
                JOIN ts_symbol@hnxcore s ON s.id = t.stock_id AND s.idboard = p_id AND idschedule = 0
                WHERE norp = 2 AND norc = 7 AND t.status IN (4,5,6)
                AND substr(co_account_no,4,1) = 'F' AND substr(account_no,4,1) <> 'F'
            UNION ALL
            SELECT order_qtty,stock_id
                FROM tt_orders_plus@HNXCORE t1
                JOIN ts_symbol@hnxcore s ON s.id = t1.stock_id AND s.idboard = p_id AND idschedule = 0
                WHERE norp = 1 AND norc = 5 AND substr(co_account_no,4,1) = 'F'
        )
        GROUP BY stock_id;
    ELSIF p_type = type_symbol THEN
    OPEN p_cursor FOR
        SELECT sum(order_qtty) buy_match, stock_id FROM
        (
            SELECT order_qtty,stock_id FROM tt_orders@HNXCORE
                WHERE norp = 2 AND norc = 7 AND status IN (4,5,6)
                AND substr(co_account_no,4,1) = 'F' AND substr(account_no,4,1) <> 'F' AND stock_id = p_id
            UNION ALL
            SELECT order_qtty,stock_id FROM tt_orders_plus@HNXCORE
                WHERE norp = 1 AND norc = 5  AND substr(co_account_no,4,1) = 'F' AND stock_id = p_id
        )
        GROUP BY stock_id;
    END IF;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_MatchPrice_Order
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    IF p_type = type_board THEN
        OPEN p_cursor FOR
        SELECT MAX(order_price) KEEP(DENSE_RANK LAST ORDER BY order_id) match_price,stock_id FROM
        (
            SELECT t.order_id, t.stock_id,t.order_price
            FROM tt_orders_plus@HNXCORE t
            JOIN ts_symbol@hnxcore s ON s.id = t.stock_id AND s.idboard = p_id AND idschedule = 0
            WHERE t.norp = 1 AND t.norc = 5 and t.noro = 1
        )
        GROUP BY stock_id;
    ELSIF p_type = type_symbol then

        OPEN p_cursor FOR
        SELECT MAX(order_price) KEEP(DENSE_RANK LAST ORDER BY order_id) match_price,stock_id FROM
        (
            SELECT order_id, stock_id,order_price
            FROM tt_orders_plus@HNXCORE
            WHERE norp = 1 AND norc = 5 and noro = 1 AND stock_id = p_id
        )
        GROUP BY stock_id;
    END IF;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_Get_LastSeqConfirm
(
    p_type IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    IF p_type = type_board THEN
        OPEN p_cursor FOR
        SELECT MAX(order_confirm_no) KEEP(DENSE_RANK LAST ORDER BY order_id) order_confirm_no,stock_id FROM
        (
            SELECT order_id, stock_id,order_confirm_no
            FROM tt_orders_plus@HNXCORE t
            JOIN ts_symbol@hnxcore s ON s.id = t.stock_id AND s.idboard = p_id AND idschedule = 0
            WHERE norp = 1 AND norc = 5 and noro = 1
        )
        GROUP BY stock_id;
    ELSIF p_type = type_symbol THEN
        OPEN p_cursor FOR
        SELECT MAX(order_confirm_no) KEEP(DENSE_RANK LAST ORDER BY order_id) order_confirm_no,stock_id FROM
        (
            SELECT order_id, stock_id,order_confirm_no
            FROM tt_orders_plus@HNXCORE
            WHERE norp = 1 AND norc = 5 and noro = 1 AND stock_id = p_id
        )
        GROUP BY stock_id;
    END IF;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

-- lay thong tin gia khop va tong KL khop o so lenh backup, de so sanh vs gia va kl cua minh khop lai
PROCEDURE Get_ExecOrderTem_ByTrad_Id
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    IF p_type = type_board THEN
        OPEN p_cursor FOR
        SELECT stock_id,nvl(symbol,'') symbol,exec_price,sum_exec_qtty
        FROM
        (
            SELECT stock_id, max(order_price) exec_price ,SUM(order_qtty) sum_exec_qtty
            FROM tt_orders_plus_tem
            WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id
            AND norc = 5 AND norp = 1
            AND stock_id IN
            (
                SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND idschedule = 0
            )
            GROUP BY stock_id
        )a
        LEFT JOIN ts_symbol@HNXCORE s ON s.id = a.stock_id AND s.idschedule = 0;
    ELSIF p_type = type_symbol THEN

        OPEN p_cursor FOR
        SELECT stock_id,nvl(symbol,'') symbol,exec_price,sum_exec_qtty
        FROM
        (
            SELECT stock_id, max(order_price) exec_price ,SUM(order_qtty) sum_exec_qtty
            FROM tt_orders_plus_tem
            WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id AND stock_id = p_id
            AND norc = 5 AND norp = 1
            GROUP BY stock_id
        )a
        LEFT JOIN ts_symbol@HNXCORE s ON s.id = a.stock_id AND s.idschedule <> 0;
    END IF;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

-- insert lenh vao DR tu he thong core
-- xoa lenh o core di
PROCEDURE proc_backup_ExecOrder_Auction
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_tem IN NUMBER
)
IS
BEGIN

    IF p_type = type_board THEN

       IF p_is_delete_tem = 1 THEN
            DELETE tt_orders_plus_tem t WHERE t.TRADING_SCHEDULE_ID = p_trad_sche_id
            AND t.stock_id IN
            (
                SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND s.idschedule = 0
            );
        END IF;

        INSERT INTO tt_orders_plus_tem
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders_plus@hnxcore
        WHERE(norc = 5 OR norc = 13)
        AND TRADING_SCHEDULE_ID = p_trad_sche_id
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        DELETE tt_orders_plus@hnxcore
        WHERE ORDER_ID IN
        (
            SELECT t.ORDER_ID
            FROM tt_orders_plus_tem t
            JOIN ts_symbol@hnxcore s ON s.id = t.stock_id AND s.idboard = p_id AND s.idschedule = 0
            WHERE t.TRADING_SCHEDULE_ID = p_trad_sche_id
        );
    ELSIF p_type = type_symbol THEN

        -- xoa tem
        IF p_is_delete_tem = 1 THEN
            DELETE tt_orders_plus_tem t WHERE t.TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;
        END IF;

        -- insert tem
        INSERT INTO tt_orders_plus_tem
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
       FROM tt_orders_plus@hnxcore t
        WHERE (t.norc = 5 OR t.norc = 13)
        AND t.TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;

        DELETE tt_orders_plus@hnxcore
        WHERE ORDER_ID IN
        (
            SELECT ORDER_ID
            FROM tt_orders_plus_tem
            WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id
        );
    END IF;
    COMMIT;
EXCEPTION
WHEN OTHERS THEN
    ROLLBACK;
    RAISE;
END;

--backup khi huy ket qua giao dich
PROCEDURE proc_backup_AllExecOrder
(
    p_board IN VARCHAR2
)
is
    v_count number;
    v_count_order number;
begin
    --Dem so lenh trong orders_plus
    select count(*) into v_count from  tt_orders_plus@hnxcore
    WHERE stock_id IN ( SELECT s.id FROM ts_symbol@hnxcore s
    WHERE s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and deleted = 0);
    --dem so lenh trong orders
    select count(*) into v_count_order from tt_orders@hnxcore
    WHERE stock_id IN ( SELECT s.id FROM ts_symbol@hnxcore s
    WHERE s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and deleted = 0);
    --Xu ly cho phan lenh dat
    if(v_count_order > 0) then
    --Xoa dl bang temp di
    DELETE tt_orders_tem t
    WHERE t.stock_id IN (SELECT s.id FROM ts_symbol@hnxcore s
    WHERE s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                       CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and s.deleted = 0);
    --Insert dl ms vao bang temp
    INSERT INTO tt_orders_tem
    (
       order_id, org_order_id, floor_code, order_confirm_no,
       order_no, co_order_no, org_order_no, order_date,
       order_time, member_id, co_member_id, order_type,
       priority, oorb, norp, norc, bore, aori,
       settlement_type, dorf, order_qtty, order_price, status,
       quote_price, state, quote_time, quote_qtty, exec_qtty,
       correct_qtty, cancel_qtty, reject_qtty, reject_reason,
       account_no, co_account_no, broker_id, co_broker_id,
       deleted, date_created, date_modified, modified_by,
       created_by, telephone, org_order_base, settle_day,
       aorc, yieldmat, order_qtty_display, order_price_stop,
       clordid, noro, stock_id, sub_order_no, org_order_type,
       trading_schedule_id, member_code_adv, co_dorf,
       current_trading_schedule_id, co_sub_order_no,
       real_correct_qtty, idmarket, group_name, numseq_save,
       co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
       co_isrisk_manage, is_inday, co_is_inday
    )
    SELECT order_id, org_order_id, floor_code, order_confirm_no,
       order_no, co_order_no, org_order_no, order_date,
       order_time, member_id, co_member_id, order_type,
       priority, oorb, norp, norc, bore, aori,
       settlement_type, dorf, order_qtty, order_price, status,
       quote_price, state, quote_time, quote_qtty, exec_qtty,
       correct_qtty, cancel_qtty, reject_qtty, reject_reason,
       account_no, co_account_no, broker_id, co_broker_id,
       deleted, date_created, date_modified, modified_by,
       created_by, telephone, org_order_base, settle_day,
       aorc, yieldmat, order_qtty_display, order_price_stop,
       clordid, noro, stock_id, sub_order_no, org_order_type,
       trading_schedule_id, member_code_adv, co_dorf,
       current_trading_schedule_id, co_sub_order_no,
       real_correct_qtty, idmarket, group_name, numseq_save,
       co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
       co_isrisk_manage, is_inday, co_is_inday
    FROM tt_orders@hnxcore
    WHERE stock_id IN ( SELECT s.id FROM ts_symbol@hnxcore s
    WHERE s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                       CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and s.deleted = 0);
    --Xoa dl ben tt_order ben Core
    DELETE tt_orders@hnxcore
    WHERE ORDER_ID IN (SELECT t.ORDER_ID FROM tt_orders_tem t
    JOIN ts_symbol@hnxcore s ON s.id = t.stock_id
    where s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                       CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0)  and s.deleted = 0);
    end if;

    --Xu ly cho phan lenh khop
    --Neu tt_orders_plus con dl khop cua bang nay thi phan tinh toan lenh khop phien dk chua bk dl
    --Thuc hien backup. Nguoc lai thi ko lam gi ca
    if(v_count > 0) then
       --Xoa dl bang plus temp di
       DELETE tt_orders_plus_tem t
       WHERE t.stock_id IN (SELECT s.id FROM ts_symbol@hnxcore s
       WHERE s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and s.deleted = 0);
       --Insert dl ms vao bang temp
       INSERT INTO tt_orders_plus_tem
       (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_bore, special_side, co_special_side, special_type,
            co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
       )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_bore, special_side, co_special_side, special_type,
            co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
       FROM tt_orders_plus@hnxcore
       WHERE stock_id IN ( SELECT s.id FROM ts_symbol@hnxcore s
       WHERE s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and s.deleted = 0);
       --Xoa dl ben tt_order_plus ben Core
       DELETE tt_orders_plus@hnxcore
       WHERE ORDER_ID IN (SELECT t.ORDER_ID FROM tt_orders_plus_tem t
       JOIN ts_symbol@hnxcore s ON s.id = t.stock_id
       where s.idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0)  and s.deleted = 0);
    end if;

    --Cap nhat lai step matching cho bang
    update dr_step_matching set step = pkg_dr_step_matching.step_cancel_allmatching
    where object_id in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0);

    --Cap nhat step matching cho ck thuoc bang
    update dr_step_matching set step = pkg_dr_step_matching.step_cancel_allmatching
    where object_id in (select id from ts_symbol@hnxcore where idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0));

    --Reset lai thong tin tt_symbol_info cho ck thuoc bang
    update tt_symbol_info@hnxcore set MAX_PRICE_BUY = 0,MAX_QTTY_BUY = 0,MAX_PRICE_SELL=0,MAX_QTTY_SELL = 0,MATCH_PRICE = 0,
    MATCH_QTTY=0,MATCH_VALUE = 0,PT_MATCH_PRICE=0,PT_MATCH_QTTY = 0,EXCUTE_RDLOT_PRICE = 0,EXCUTE_ODDLOT_PRICE = 0,
    KLKCHAN_GANNHAT = 0,KLKLE_GANNHAT = 0,GTKCHAN_GANNHAT = 0,GTKLE_GANNHAT = 0,CURRENT_PRICE = 0,CURRENT_QTTY = 0
    where id in (select id from ts_symbol@hnxcore where idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0)) ;
    update ts_symbol@hnxcore set match_price = 0 where id in (select id from ts_symbol@hnxcore where idboard in (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0)) ;
    commit;
end;

-- insert du lieu vao core tu DR
-- revert thi ko xoa nguon
PROCEDURE proc_Revert_ExecOrder_Auction
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_nguon IN NUMBER
)
IS
BEGIN

    IF p_type = type_board THEN

        -- xoa core
        DELETE tt_orders_plus@hnxcore
        WHERE (norc = 5 OR norc = 13) AND TRADING_SCHEDULE_ID = p_trad_sche_id
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        -- insert bang tem
        INSERT INTO tt_orders_plus@hnxcore
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders_plus_tem WHERE (norc = 5 OR norc = 13) AND TRADING_SCHEDULE_ID = p_trad_sche_id
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        IF p_is_delete_nguon = 1 THEN
            -- xoa bang tem
            DELETE tt_orders_plus_tem
            WHERE (norc = 5 OR norc = 13)
            AND TRADING_SCHEDULE_ID = p_trad_sche_id
            AND stock_id IN
            (
                SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND s.idschedule = 0
            );
        END IF;

    ELSIF p_type = type_symbol THEN
         -- xoa core
        DELETE tt_orders_plus@hnxcore
        WHERE (norc = 5 OR norc = 13) AND TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;

        -- insert bang tem vao bang that
        INSERT INTO tt_orders_plus@hnxcore
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders_plus_tem WHERE (norc = 5 OR norc = 13) AND TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;

        -- xoa bang tem
        IF p_is_delete_nguon = 1 THEN
            DELETE tt_orders_plus_tem WHERE (norc = 5 OR norc = 13) AND TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;
        END IF;
    END IF;
    COMMIT;
EXCEPTION
WHEN OTHERS THEN
    ROLLBACK;
    RAISE;
END;

PROCEDURE proc_backup_Order
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_tem IN NUMBER
)
IS
BEGIN

    IF p_type = type_board THEN

        -- so lenh khop
        IF p_is_delete_tem = 1 THEN
            DELETE tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id
            AND stock_id IN
            (
                SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
            );
        END IF;

        --25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        INSERT INTO tt_orders_plus_tem
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders_plus@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND norp = 1
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        DELETE tt_orders_plus@hnxcore
        WHERE ORDER_ID IN (SELECT ORDER_ID FROM tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id)
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        -- so lenh dat
        IF p_is_delete_tem = 1 THEN
            DELETE tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id
            AND stock_id IN
            (
                SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
            );
        END IF;

        --25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        INSERT INTO tt_orders_tem
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
            FROM tt_orders@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id  AND norp = 1
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        DELETE tt_orders@hnxcore
        WHERE ORDER_ID IN (SELECT ORDER_ID FROM tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id)
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );
    ELSIF p_type = type_symbol THEN

        -- so lenh khop
        IF p_is_delete_tem = 1 THEN
            DELETE tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;
        END IF;

        -- 25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        INSERT INTO tt_orders_plus_tem
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders_plus@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id AND norp = 1;

        DELETE tt_orders_plus@hnxcore
        WHERE stock_id = p_id
        AND ORDER_ID IN (SELECT ORDER_ID FROM tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id);

        -- so lenh dat
        IF p_is_delete_tem = 1 THEN
            DELETE tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;
        END IF;

        -- 25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        INSERT INTO tt_orders_tem
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT  order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id AND norp = 1;

        DELETE tt_orders@hnxcore
        WHERE ORDER_ID IN (SELECT ORDER_ID FROM tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id) AND stock_id = p_id;
    END IF;
    COMMIT;
EXCEPTION
WHEN OTHERS THEN
    ROLLBACK;
    RAISE;
END;

-- revert thi ko xoa nguon
PROCEDURE proc_Revert_Order
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_is_delete_nguon IN NUMBER
)
IS
BEGIN
    IF p_type = type_board THEN

        -- khop
        -- xoa core
        --25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        DELETE tt_orders_plus@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND norp = 1
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        -- insert core
        INSERT INTO tt_orders_plus@hnxcore
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        IF p_is_delete_nguon = 1 THEN
            -- xoa bang tem
            DELETE tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id
            AND stock_id IN
            (
                SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
            );
        END IF;

        -- so lenh dat

        -- xoa core
        --25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        DELETE tt_orders@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND norp = 1
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

         -- insert bang tem vao bang that
        INSERT INTO tt_orders@hnxcore
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT  order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        FROM tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
        );

        IF p_is_delete_nguon = 1 THEN
            -- xoa bang tem
            DELETE tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id
            AND stock_id IN
            (
                SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id  AND s.idschedule = 0
            );
        END IF;
    ELSIF p_type = type_symbol THEN

        -- khop
        -- xoa core
        --25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        DELETE tt_orders_plus@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id AND norp = 1;

        -- insert bang tem vao bang that
        INSERT INTO tt_orders_plus@hnxcore
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
       FROM tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;

        IF p_is_delete_nguon = 1 THEN
            -- xoa bang tem
            DELETE tt_orders_plus_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;
        END IF;

        -- so lenh dat
        --25/12/2015 dangtq saua chi lay lenh thong thuong
        -- yc cua em huong
        DELETE tt_orders@hnxcore
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id AND norp = 1;

         -- insert bang tem vao bang that
        INSERT INTO tt_orders@hnxcore
        (
            order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
        )
        SELECT  order_id, org_order_id, floor_code, order_confirm_no,
            order_no, co_order_no, org_order_no, order_date,
            order_time, member_id, co_member_id, order_type,
            priority, oorb, norp, norc, bore, aori,
            settlement_type, dorf, order_qtty, order_price, status,
            quote_price, state, quote_time, quote_qtty, exec_qtty,
            correct_qtty, cancel_qtty, reject_qtty, reject_reason,
            account_no, co_account_no, broker_id, co_broker_id,
            deleted, date_created, date_modified, modified_by,
            created_by, telephone, org_order_base, settle_day,
            aorc, yieldmat, order_qtty_display, order_price_stop,
            clordid, noro, stock_id, sub_order_no, org_order_type,
            trading_schedule_id, member_code_adv, co_dorf,
            current_trading_schedule_id, co_sub_order_no,
            real_correct_qtty, idmarket, group_name, numseq_save,
            co_Bore,special_side,co_special_side,special_type, co_special_type, mmlink_orderno, isrisk_manage,
            co_isrisk_manage, is_inday, co_is_inday
       FROM tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;

        IF p_is_delete_nguon = 1 THEN
            -- xoa bang tem
            DELETE tt_orders_tem WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;
        END IF ;
    END IF;
COMMIT;
EXCEPTION
WHEN OTHERS THEN
    ROLLBACK;
    RAISE;
END;

PROCEDURE proc_get_schedule
(
    p_cursor OUT tcursor
)
IS
BEGIN
-- 23/12 dangtq lay them thang current_status
OPEN p_cursor FOR
    SELECT b.id AS object_id,b.code AS code,1 AS type, ts.id AS trading_schedule_id,
    ts.code AS trading_schedule_code,mt.matchtype,ts.idschdrule AS Schdrule_Id,tc.current_status
    FROM ts_trading_calendars@HNXCORE tc
    JOIN ts_boards@HNXCORE b ON b.id = tc.boardid AND b.deleted = 0
    JOIN ts_trading_schedules@HNXCORE ts ON ts.id  = tc.trading_schedule_id AND ts.deleted = 0
    JOIN ts_trading_state@HNXCORE ste ON ste.id = ts.idstate AND ste.deleted = 0
    JOIN ts_match_type@HNXCORE mt ON mt.id = ste.matchtype AND mt.deleted = 0 --AND mt.matchtype ='A'
    WHERE tc.is_workingday = 1 ORDER BY b.code;

EXCEPTION
WHEN OTHERS THEN
RAISE;

END;

PROCEDURE proc_get_pre_schedule
(
    p_trad_sche_id IN NUMBER,
    p_schdrule_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
    v_count NUMBER;
    v_trading_schedule_id NUMBER;
    v_str_id VARCHAR2(100);

    v_sql VARCHAR2(2000);
    v_auction VARCHAR2(1);
    v_sql_condition VARCHAR2(100);
BEGIN

    v_str_id := '0';
    v_auction := 'A';
    v_sql_condition := '';

    --v_sql_condition := ' AND mt.matchtype ='''  || v_auction || '''';
    SELECT count(id) INTO v_count FROM ts_trading_schedules@HNXCORE WHERE idschdrule = p_schdrule_id AND id < p_trad_sche_id;

    IF v_count > 0 THEN
       SELECT nvl(id,0) into v_trading_schedule_id
       FROM
       (
           SELECT ROWNUM rn,a.* FROM
           (
               SELECT id,code FROM ts_trading_schedules@HNXCORE
               WHERE idschdrule = p_schdrule_id AND id < p_trad_sche_id
               ORDER BY id desc
           ) a
       )
       where rn = 1;
    END IF;

    v_str_id := TO_CHAR(v_trading_schedule_id);

    v_sql := 'SELECT ts.id AS trading_schedule_id ,ts.code trading_schedule_code,mt.matchtype
             FROM ts_trading_schedules@HNXCORE ts
             JOIN ts_trading_state@HNXCORE ste ON ste.id = ts.idstate AND ste.deleted = 0
             JOIN ts_match_type@HNXCORE mt ON mt.id = ste.matchtype AND mt.deleted = 0 ' || v_sql_condition || ' WHERE ts.id IN ('''|| v_str_id || ''')' ;

    dbms_output.put_line(v_sql);
    OPEN p_cursor FOR v_sql;

EXCEPTION
WHEN OTHERS THEN
RAISE;

END;

PROCEDURE proc_get_schedule_symbol
(
    p_cursor OUT tcursor
)
IS
BEGIN
-- 23/12 dangtq lay them thang current_status
OPEN p_cursor FOR
    SELECT s.id AS object_id,s.symbol AS code,2 AS type, ts.id AS trading_schedule_id,
    ts.code AS trading_schedule_code,mt.matchtype,ts.idschdrule AS Schdrule_Id,sc.current_status
    FROM ts_symbol_calendar@HNXCORE sc
    JOIN ts_symbol@HNXCORE s ON s.id = sc.symbol_id AND s.deleted = 0 and idschedule <> 0
    JOIN ts_trading_schedules@HNXCORE ts ON ts.id  = sc.current_trading_schedule_id AND ts.deleted = 0
    JOIN ts_trading_state@HNXCORE ste ON ste.id = ts.idstate AND ste.deleted = 0
    JOIN ts_match_type@HNXCORE mt ON mt.id = ste.matchtype AND mt.deleted = 0 --AND mt.matchtype ='A'
    ORDER BY symbol;

EXCEPTION
WHEN OTHERS THEN
RAISE;

END;

-- lay all chung khoan giao dich trong ngay
PROCEDURE proc_Get_all_symbol_Trading
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT tt.symbol,tt.id_symbol,b.id AS id_board,s.idschedule AS id_schedule
    FROM tt_symbol_info@HNXCORE tt
    JOIN ts_symbol@HNXCORE s ON s.id = tt.id_symbol AND s.deleted = 0 AND s.status NOT IN (6,9)
    JOIN ts_boards@HNXCORE b ON b.id = s.idboard AND b.deleted = 0 ORDER BY tt.symbol;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_update_buyMatch
(
    p_symbol IN VARCHAR2,
    p_buyMatch IN NUMBER
)
IS
BEGIN
    UPDATE ts_foreign_room@HNXCORE SET buy_match = p_buyMatch WHERE symbol = p_symbol;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

-- lay danh sach lenh de tam khop (dat + chua khop tu phien truoc)
PROCEDURE proc_get_order_matching
(
    p_type in NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN

    IF p_type = type_board THEN
        OPEN p_cursor FOR
        SELECT ORDER_ID, ORG_ORDER_ID, FLOOR_CODE, ORDER_CONFIRM_NO, ORDER_NO, CO_ORDER_NO,
                ORG_ORDER_NO, ORDER_DATE, ORDER_TIME, MEMBER_ID, CO_MEMBER_ID, ORDER_TYPE,
                PRIORITY, OORB, NORP, NORC, BORE, AORI, SETTLEMENT_TYPE, DORF, QUOTE_QTTY as ORDER_QTTY,
                STATUS, QUOTE_PRICE as ORDER_PRICE, STATE, QUOTE_TIME, REJECT_REASON, ACCOUNT_NO, CO_ACCOUNT_NO,
                BROKER_ID, CO_BROKER_ID, DELETED, DATE_CREATED, DATE_MODIFIED, MODIFIED_BY,
                CREATED_BY, TELEPHONE, SETTLE_DAY, AORC, YIELDMAT, ORDER_QTTY_DISPLAY,
                ORDER_PRICE_STOP, CLORDID, NORO, STOCK_ID, SUB_ORDER_NO, ORG_ORDER_TYPE,
                TRADING_SCHEDULE_ID, MEMBER_CODE_ADV, EXEC_QTTY, CORRECT_QTTY, CANCEL_QTTY,
                REJECT_QTTY, QUOTE_QTTY, ORG_ORDER_BASE, CO_DORF, CURRENT_TRADING_SCHEDULE_ID,IDMARKET ,
                (select m.code from ts_members@hnxcore m where m.id = member_id and m.deleted = 0) membercode,
                (select m.code from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0) co_membercode,
                (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
                (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
                (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
                (select o.code from ts_order_type@hnxcore o where o.deleted =0 and o.id = order_type) ordertype,
                (select code from ts_order_type@hnxcore where id = org_order_type)org_ordertype,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =norc) snorc,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='OORB' and cdval =oorb )soorb,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro) snoro,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='AORI' and cdval =aori) saori ,
                (select username from mt_users@hnxcore where id = broker_id and deleted =0) brokercode,
                (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_brokercode,
                (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
                special_type,co_special_type,special_side,co_special_side,ISRISK_MANAGE,CO_ISRISK_MANAGE,
                IS_INDAY,CO_IS_INDAY,MMLINK_ORDERNO

                FROM view_tt_orders@hnxcore tt
                WHERE deleted = 0 and norc NOT IN (3,4,5,13) and quote_qtty > 0
                AND stock_id IN
                (
                    SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND idschedule = 0
                )
                ORDER BY order_time DESC;
     ELSIF p_type = type_symbol THEN
        OPEN p_cursor FOR
                SELECT ORDER_ID, ORG_ORDER_ID, FLOOR_CODE, ORDER_CONFIRM_NO, ORDER_NO, CO_ORDER_NO,
                ORG_ORDER_NO, ORDER_DATE, ORDER_TIME, MEMBER_ID, CO_MEMBER_ID, ORDER_TYPE,
                PRIORITY, OORB, NORP, NORC, BORE, AORI, SETTLEMENT_TYPE, DORF, QUOTE_QTTY as ORDER_QTTY,
                STATUS, QUOTE_PRICE as ORDER_PRICE, STATE, QUOTE_TIME, REJECT_REASON, ACCOUNT_NO, CO_ACCOUNT_NO,
                BROKER_ID, CO_BROKER_ID, DELETED, DATE_CREATED, DATE_MODIFIED, MODIFIED_BY,
                CREATED_BY, TELEPHONE, SETTLE_DAY, AORC, YIELDMAT, ORDER_QTTY_DISPLAY,
                ORDER_PRICE_STOP, CLORDID, NORO, STOCK_ID, SUB_ORDER_NO, ORG_ORDER_TYPE,
                TRADING_SCHEDULE_ID, MEMBER_CODE_ADV, EXEC_QTTY, CORRECT_QTTY, CANCEL_QTTY,
                REJECT_QTTY, QUOTE_QTTY, ORG_ORDER_BASE, CO_DORF, CURRENT_TRADING_SCHEDULE_ID,IDMARKET ,
                (select m.code from ts_members@hnxcore m where m.id = member_id and m.deleted = 0) membercode,
                (select m.code from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0) co_membercode,
                (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
                (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
                (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
                (select o.code from ts_order_type@hnxcore o where o.deleted =0 and o.id = order_type) ordertype,
                (select code from ts_order_type@hnxcore where id = org_order_type)org_ordertype,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =norc) snorc,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='OORB' and cdval =oorb )soorb,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro) snoro,
                (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='AORI' and cdval =aori) saori ,
                (select username from mt_users@hnxcore where id = broker_id and deleted =0) brokercode,
                (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_brokercode,
                (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
                special_type,co_special_type,special_side,co_special_side,ISRISK_MANAGE,CO_ISRISK_MANAGE,
                IS_INDAY,CO_IS_INDAY,MMLINK_ORDERNO
                FROM view_tt_orders@hnxcore tt
                WHERE deleted = 0 AND norc NOT IN (3,4,5,13) and quote_qtty > 0
                AND stock_id = p_id
                ORDER BY order_time DESC;
     END IF;
end;

PROCEDURE PROC_TT_ORDERS_INSERT
(
    p_org_order_id in NUMBER,--1
    p_floor_code in VARCHAR2,--2
    p_order_confirm_no in VARCHAR2,--3
    p_order_no   in VARCHAR2,  --4  --update theo orderno
    p_co_order_no   in VARCHAR2,--5
    p_org_order_no  in VARCHAR2,--6
    p_order_date  in Date,--7
    p_order_time  in VARCHAR2,--8
    p_member_id  in NUMBER,--9
    p_co_member_id  NUMBER,--10
    p_stock_id  in NUMBER,--11
    p_order_type  in NUMBER,--12
    p_priority  in NUMBER,--13
    p_oorb  in NUMBER,--14
    p_norp  in NUMBER,--15
    p_norc  in NUMBER,--16
    p_bore  in NUMBER,--17
    p_aori  in NUMBER,--18
    p_settlement_type  in NUMBER,--19
    p_dorf  in NUMBER,--20
    p_order_qtty  in NUMBER,--21
    p_order_price  in NUMBER,--22
    p_status  in NUMBER,--23
    p_quote_price  in NUMBER,--24
    p_state  in NUMBER,--25
    p_quote_time  in VARCHAR2 ,--26
    p_quote_qtty  in NUMBER ,--27
    p_exec_qtty  in NUMBER ,--28 hien dang dung trong TH sua lenh luu tam KL thay doi dua KL tong IO khi sua giam KL dinh
    p_correct_qtty  in NUMBER ,--29
    --30
    p_cancel_qtty  in NUMBER ,--30
    p_reject_qtty  in NUMBER ,--31
    p_reject_reason  in NUMBER ,--32 Co y nghia doi voi thang lenh reject
    p_account_no  in VARCHAR2 ,--33
    p_co_account_no  in VARCHAR2 ,--34
    p_broker_id  in NUMBER ,--35
    p_co_broker_id  in NUMBER  ,--36
    p_deleted  in NUMBER  ,        --37
    p_date_created  in Date  ,--38
    p_date_modified  in Date ,--39
    --40
    p_modified_by  in VARCHAR2 ,--40
    p_created_by  in VARCHAR2 ,--41
    p_telephone  in VARCHAR2  ,--42
    p_org_order_base  in VARCHAR2  ,--43
    p_settle_day  in NUMBER  ,--44 -- hien chi dung khi insert lenh khop thi = gia tri cua co_dorf
    p_aorc  in NUMBER  ,--45      --Hien dung de gui thang idmarket xuong luu db vi trg nay ko dung de = 0
    p_yieldmat  in NUMBER    ,--46
    p_clordid  in  VARCHAR2   , --47
    p_Noro  in  VARCHAR2  , --lo giao dichm 48
    p_ORDER_PRICE_STOP  in  NUMBER   , --49
    p_ORDER_QTTY_DISPLAY in   NUMBER  , --50
    p_SUB_ORDER_NO in  VARCHAR2  , --51
    p_Org_Order_Type in  NUMBER  , --52
    p_Trading_Schedule_Id  in  NUMBER , -- moi them trang thai cua tradingding_schedule 53
    p_CO_SUB_ORDER_NO in  VARCHAR2 ,  --54
    p_GroupName IN VARCHAR2, ---55
    p_NumSeqSave IN NUMBER  ,      --56

    p_Co_Bore IN NUMBER,   --- 57 . Add 07.2017

    p_Special_side IN VARCHAR2 , -- 58 Add 11.2017
    p_Special_type IN NUMBER,  -- 59 Add 11.2017

    p_Co_Special_type IN NUMBER,  -- 60 Add 12.2017
    p_Co_Special_side IN VARCHAR2, -- 61 Add 02.2018

    p_MMLINK_ORDERNO IN  VARCHAR2,  ---62 add 04.2018

    p_IsRisk_Manage IN  NUMBER,  ---63 add 04.2018
    p_CoIsRisk_Manage IN  NUMBER,  ---64 add 04.2018

    p_IsInDay IN VARCHAR2, ---65 add 04.2018
    p_CoIsInDay IN VARCHAR2 ---66 add 04.2018
)
is
    v_id number;
    p_return  NUMBER ;
    v_numEfect NUMBER;
    v_count NUMBER;
begin
    SELECT seq_tt_orders.NEXTVAL@hnxcore INTO v_id FROM dual;

    --Voi thang sua co bien p_exec_qtty dung de luu tam KL sua thuc te cua lenh --> luu vao truong real_correct_qtty
    --voi lenh huy cua IO thi truong real_correct_qtty luu KL sua cua thang IO cha
    --Voi thang sua thi order_qtty = KL mong muon sua (p_correct_qtty), correct_qtty = KL chenh lech mong sua ( =  p_correct_qtty - p_order_qtty)
    IF(p_norc = NORC_REPLACE) THEN
        INSERT INTO TT_ORDERS_PLUS@hnxcore(order_id,org_order_id,floor_code,order_confirm_no,order_no,co_order_no,org_order_no,order_date,order_time,
        member_id,co_member_id,stock_id,order_type,priority,oorb,norp,norc,bore,aori,settlement_type,dorf,order_qtty,order_price,
        status,quote_price, state,quote_time, quote_qtty,exec_qtty,correct_qtty,cancel_qtty,reject_qtty,reject_reason, account_no,
        co_account_no,broker_id, co_broker_id, deleted,date_created,date_modified,modified_by,created_by,telephone,org_order_base ,
        aorc,yieldmat ,clordid ,noro,ORDER_PRICE_STOP,ORDER_QTTY_DISPLAY,SUB_ORDER_NO,ORG_ORDER_TYPE ,
        trading_schedule_id,current_trading_schedule_id,real_correct_qtty,idmarket,group_name,numseq_save,ISRISK_MANAGE, IS_INDAY,MMLINK_ORDERNO)

        values(v_id, 0,p_floor_code ,p_order_confirm_no ,p_order_no   ,p_co_order_no   ,p_org_order_no  ,p_order_date  ,p_order_time  ,
        p_member_id  , p_co_member_id ,p_stock_id ,p_order_type  ,p_priority ,p_oorb ,p_norp ,p_norc ,p_bore ,p_aori ,p_settlement_type ,p_dorf ,
        p_correct_qtty ,p_order_price,p_status,p_quote_price ,p_state  ,p_quote_time ,p_quote_qtty ,0  , p_correct_qtty - p_order_qtty ,p_cancel_qtty ,
        p_reject_qtty  ,p_reject_reason ,p_account_no ,p_co_account_no  ,p_broker_id  ,p_co_broker_id ,p_deleted ,p_date_created ,p_date_modified ,
        p_modified_by ,p_created_by ,p_telephone  ,p_org_order_base ,0  ,p_yieldmat ,p_clordid ,p_NorO,decode(p_org_order_base,'S',p_ORDER_PRICE_STOP,0),
        decode(p_org_order_base,'I',p_ORDER_QTTY_DISPLAY,0),p_SUB_ORDER_NO,p_Org_Order_Type,p_Trading_Schedule_Id,p_Trading_Schedule_Id,
        p_exec_qtty,p_aorc,p_GroupName,p_NumSeqSave, decode(p_oorb,2, p_IsRisk_Manage, p_CoIsRisk_Manage),decode(p_oorb,2,p_IsInDay,p_CoIsInDay),p_MMLINK_ORDERNO);
    ELSIF (p_norc = NORC_EXECUTED) THEN
        --Rieng thang khop co them thang co_dorf dung tam trong bien p_reject_reason khi call tu C++ xuong
         INSERT INTO TT_ORDERS_PLUS@hnxcore(order_id,org_order_id,floor_code,order_confirm_no,order_no,co_order_no,org_order_no,order_date,order_time,
        member_id,co_member_id,stock_id,order_type,priority,oorb,norp,norc,bore,aori,settlement_type,dorf,co_dorf,order_qtty,order_price,
        status,quote_price, state,quote_time, quote_qtty,exec_qtty,correct_qtty,cancel_qtty,reject_qtty,reject_reason, account_no,
        co_account_no,broker_id, co_broker_id, deleted,date_created,date_modified,modified_by,created_by,telephone,org_order_base ,
        aorc,yieldmat ,clordid ,noro,ORDER_PRICE_STOP,ORDER_QTTY_DISPLAY,SUB_ORDER_NO,ORG_ORDER_TYPE ,trading_schedule_id,
        current_trading_schedule_id,CO_SUB_ORDER_NO,idmarket,group_name,numseq_save,co_bore,special_type,co_special_type,special_side,co_special_side,
        ISRISK_MANAGE,CO_ISRISK_MANAGE, IS_INDAY, CO_IS_INDAY)

        VALUES (v_id, v_id,p_floor_code ,p_order_confirm_no ,p_order_no   ,p_co_order_no   ,p_org_order_no  ,p_order_date  ,p_order_time  ,
        p_member_id  , p_co_member_id ,p_stock_id ,p_order_type  ,p_priority ,p_oorb ,p_norp ,p_norc ,p_bore ,p_aori ,p_settlement_type ,p_dorf ,
        p_settle_day,p_order_qtty ,p_order_price,p_status,p_quote_price ,p_state  ,p_quote_time ,p_quote_qtty ,p_exec_qtty  ,p_correct_qtty ,p_cancel_qtty ,
        p_reject_qtty  ,0 ,p_account_no ,p_co_account_no  ,p_broker_id  ,p_co_broker_id ,p_deleted ,p_date_created ,p_date_modified ,
        p_modified_by ,p_created_by ,p_telephone  ,p_org_order_base ,0  ,p_yieldmat ,p_clordid ,p_NorO,p_ORDER_PRICE_STOP,
        p_ORDER_QTTY_DISPLAY,p_SUB_ORDER_NO,p_Org_Order_Type,p_Trading_Schedule_Id,p_Trading_Schedule_Id,p_CO_SUB_ORDER_NO,p_aorc,
        p_GroupName,p_NumSeqSave, p_Co_Bore,p_Special_type,p_Co_Special_type,p_Special_side,p_Co_Special_side,
        p_IsRisk_Manage,p_CoIsRisk_Manage,p_IsInDay ,p_CoIsInDay);
     ELSIF (p_norc = NORC_CANCEL OR p_norc = NORC_REJECT_ORDER) THEN
        INSERT INTO TT_ORDERS_PLUS@hnxcore(order_id,org_order_id,floor_code,order_confirm_no,order_no,co_order_no,org_order_no,order_date,order_time,
        member_id,co_member_id,stock_id,order_type,priority,oorb,norp,norc,bore,aori,settlement_type,dorf,order_qtty,order_price,
        status,quote_price, state,quote_time, quote_qtty,exec_qtty,correct_qtty,cancel_qtty,reject_qtty,reject_reason, account_no,
        co_account_no,broker_id, co_broker_id, deleted,date_created,date_modified,modified_by,created_by,telephone,org_order_base ,
        aorc,yieldmat ,clordid ,noro,ORDER_PRICE_STOP,ORDER_QTTY_DISPLAY,SUB_ORDER_NO,ORG_ORDER_TYPE ,
        trading_schedule_id,current_trading_schedule_id,real_correct_qtty,idmarket,group_name,numseq_save,ISRISK_MANAGE, IS_INDAY,MMLINK_ORDERNO)

        VALUES (v_id, v_id,p_floor_code ,p_order_confirm_no ,p_order_no   ,p_co_order_no   ,p_org_order_no  ,p_order_date  ,p_order_time  ,
        p_member_id  , p_co_member_id ,p_stock_id ,p_order_type  ,p_priority ,p_oorb ,p_norp ,p_norc ,p_bore ,p_aori ,p_settlement_type ,p_dorf ,
        p_order_qtty ,p_order_price,p_status,p_quote_price ,p_state  ,p_quote_time ,p_quote_qtty ,0  ,p_correct_qtty ,p_cancel_qtty ,
        p_reject_qtty  ,p_reject_reason ,p_account_no ,p_co_account_no  ,p_broker_id  ,p_co_broker_id ,p_deleted ,p_date_created ,p_date_modified ,
        p_modified_by ,p_created_by ,p_telephone  ,p_org_order_base ,0  ,p_yieldmat ,p_clordid ,p_NorO,decode(p_org_order_base,'S',p_ORDER_PRICE_STOP,0),
        decode(p_org_order_base,'I',p_ORDER_QTTY_DISPLAY,0),p_SUB_ORDER_NO,p_Org_Order_Type,p_Trading_Schedule_Id,
        p_Trading_Schedule_Id,p_exec_qtty,p_aorc,p_GroupName,p_NumSeqSave ,
        decode(p_oorb,2, p_IsRisk_Manage, p_CoIsRisk_Manage),decode(p_oorb,2,p_IsInDay,p_CoIsInDay),p_MMLINK_ORDERNO);
     ELSE
        INSERT INTO tt_orders@hnxcore(order_id,org_order_id,floor_code,order_confirm_no,order_no,co_order_no,org_order_no,order_date,order_time,
        member_id,co_member_id,stock_id,order_type,priority,oorb,norp,norc,bore,aori,settlement_type,dorf,order_qtty,order_price,
        status,quote_price, state,quote_time, quote_qtty,exec_qtty,correct_qtty,cancel_qtty,reject_qtty,reject_reason, account_no,
        co_account_no,broker_id, co_broker_id, deleted,date_created,date_modified,modified_by,created_by,telephone,org_order_base ,
        aorc,yieldmat ,clordid ,noro,ORDER_PRICE_STOP,ORDER_QTTY_DISPLAY,SUB_ORDER_NO,ORG_ORDER_TYPE, trading_schedule_id,current_trading_schedule_id,
        idmarket,group_name,numseq_save,special_side, special_type,MMLINK_ORDERNO,ISRISK_MANAGE, IS_INDAY)

        VALUES (v_id, v_id,p_floor_code ,p_order_confirm_no ,p_order_no   ,p_co_order_no   ,p_org_order_no  ,p_order_date  ,p_order_time  ,
        p_member_id  , p_co_member_id ,p_stock_id ,p_order_type  ,p_priority ,p_oorb ,p_norp ,p_norc ,p_bore ,p_aori ,p_settlement_type ,p_dorf ,
        p_order_qtty ,p_order_price,p_status,p_quote_price ,p_state  ,p_quote_time ,p_quote_qtty ,p_exec_qtty  ,p_correct_qtty ,p_cancel_qtty ,
        p_reject_qtty  ,p_reject_reason ,p_account_no ,p_co_account_no  ,p_broker_id  ,p_co_broker_id ,p_deleted ,p_date_created ,p_date_modified ,
        p_modified_by ,p_created_by ,p_telephone  ,p_org_order_base ,0  ,p_yieldmat ,p_clordid ,p_NorO,decode(p_org_order_base,'S',p_ORDER_PRICE_STOP,0),
        decode(p_org_order_base,'I',p_ORDER_QTTY_DISPLAY,0),p_SUB_ORDER_NO,p_Org_Order_Type,p_Trading_Schedule_Id,
        p_Trading_Schedule_Id,p_aorc,p_GroupName,p_NumSeqSave,
        decode(p_oorb,2, p_Special_side,p_Co_Special_side),p_Special_type,p_MMLINK_ORDERNO,
        decode(p_oorb,2, p_IsRisk_Manage, p_CoIsRisk_Manage),decode(p_oorb,2,p_IsInDay,p_CoIsInDay));
    END IF;


    --lenh cho khop van dung thang nay de update cho nhung lenh dac biet IO, SO, MO
   IF (p_norc = NORC_NOMAL) THEN
        IF (p_SUB_ORDER_NO IS NOT NULL ) THEN  --trong TH insert lenh con cua lenh Iceberg
            UPDATE tt_orders@hnxcore
            SET quote_qtty = quote_qtty - p_order_qtty,current_trading_schedule_id = p_Trading_Schedule_Id
            WHERE stock_id = p_stock_id AND  order_no =  p_order_no AND SUB_ORDER_NO IS NULL AND norc = NORC_ICE_BERG; -- AND quote_qtty > 0 bo thang nay di vi sua dinh IO cap nhat lai KL vao quote_qtty
        END IF;

        IF (p_state = 4) THEN --Lenh kich hoat cua thang STOP, cap nhat trang thai cua lenh stop, va KL cho khop ve 0
            UPDATE tt_orders@hnxcore SET status = STOP_STATUS_ACTIVE,quote_qtty = 0,current_trading_schedule_id = p_Trading_Schedule_Id
             WHERE stock_id = p_stock_id AND norc = NORC_STOP_ORDER AND order_no =  p_order_no AND status = ORDER_STATUS_NORMAL;
             --MARKET_STATUS_CONVERT
        ELSIF (p_state = 3) THEN
            UPDATE tt_orders@hnxcore SET status = MARKET_STATUS_CONVERT,quote_qtty = p_quote_qtty,current_trading_schedule_id = p_Trading_Schedule_Id  --Cho KL cho khop cua lenh Maret cha ve 0
             WHERE stock_id = p_stock_id AND norc = NORC_MARKET_ORDER AND order_no =  p_order_no AND status = ORDER_STATUS_NORMAL;
        END IF ;

   ELSIF (p_norc = NORC_EXECUTED) THEN
    --Thang nay dung de update gia thuc hien gan nhat, neu la khop lenh lo chan
       IF (p_Noro = 1) THEN
           UPDATE ts_symbol@hnxcore SET match_price = p_order_price WHERE id = p_stock_id;
       END IF ;

       --Cap nhat KL khop cua thang IO cha neu khop cua IO con
       IF(p_SUB_ORDER_NO IS NOT NULL) THEN
            UPDATE tt_orders@hnxcore
              SET exec_qtty = nvl(exec_qtty,0) + p_order_qtty
            WHERE  stock_id = p_stock_id AND order_no =  p_order_no AND  norc = NORC_ICE_BERG;
       END IF ;

       IF(p_CO_SUB_ORDER_NO IS NOT NULL) THEN
            UPDATE tt_orders@hnxcore
              SET exec_qtty = nvl(exec_qtty,0) + p_order_qtty
            WHERE  stock_id = p_stock_id AND order_no =  p_co_order_no AND norc = NORC_ICE_BERG;
       END IF ;

   END IF ;
    COMMIT WRITE NOWAIT ;
   EXCEPTION
    when others then
    p_return := -1;
    ROLLBACK ;
    RAISE;
end;

-- tam thoi chua dung
PROCEDURE proc_get_symbol_in_tt_order
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    IF p_type = type_board THEN

        OPEN p_cursor FOR
            SELECT s.symbol,a.stock_id AS id_symbol
            FROM
            (
                SELECT DISTINCT stock_id
                FROM tt_orders@hnxcore
                JOIN ts_symbol@hnxcore s ON s.id = stock_id AND s.idboard = p_id AND idschedule = 0
                WHERE current_trading_schedule_id = p_trad_sche_id
            ) a
            JOIN ts_symbol@hnxcore s ON s.id = a.stock_id;
    ELSIF p_type = type_symbol THEN
        OPEN p_cursor FOR
            SELECT s.symbol,a.stock_id AS id_symbol
            FROM
            (
                SELECT DISTINCT stock_id
                FROM tt_orders@hnxcore
                WHERE current_trading_schedule_id = p_trad_sche_id AND STOCK_ID = p_id
            ) a
            JOIN ts_symbol@hnxcore s ON s.id = a.stock_id;
    END IF;
EXCEPTION
WHEN OTHERS THEN
RAISE;

END;

PROCEDURE proc_delete_order_plus_tem
(
    p_type IN NUMBER,
    p_trad_sche_id IN NUMBER,
    p_id IN NUMBER
)
IS
BEGIN
    IF p_type = type_board THEN
        DELETE tt_orders_plus_tem t
        WHERE TRADING_SCHEDULE_ID = p_trad_sche_id
        AND t.stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND idschedule = 0
        );
    ELSIF p_type = type_symbol THEN
        DELETE tt_orders_plus_tem  WHERE TRADING_SCHEDULE_ID = p_trad_sche_id AND stock_id = p_id;
    END IF;

    COMMIT;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

-- lay tong kl khop cu va moi trong order tem
PROCEDURE proc_get_total_exec_qtty
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_cursor OUT tcursor
)
IS
v_total_exec_qtty NUMBER;
v_total_exec_qtty_tem NUMBER;
BEGIN

    IF p_type = type_board THEN
        SELECT nvl(SUM(order_qtty),0) sum_exec_qtty INTO v_total_exec_qtty
        FROM tt_orders_plus@HNXCORE
        WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND idschedule = 0
        );

        SELECT nvl(SUM(order_qtty),0) sum_exec_qtty INTO v_total_exec_qtty_tem
        FROM tt_orders_plus_tem
        WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND idschedule = 0
        );
    ELSIF p_type = type_symbol THEN
        SELECT nvl(SUM(order_qtty),0) sum_exec_qtty INTO v_total_exec_qtty
        FROM tt_orders_plus@HNXCORE
        WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id AND stock_id = p_id;

        SELECT nvl(SUM(order_qtty),0) sum_exec_qtty INTO v_total_exec_qtty_tem
        FROM tt_orders_plus_tem
        WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id AND stock_id = p_id;
    END IF;
    OPEN p_cursor FOR SELECT v_total_exec_qtty AS total_exec_qtty,v_total_exec_qtty_tem AS total_exec_qtty_tem FROM dual;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
RAISE;
END;

-- lay thong tin lenh khop cu de ket xuat
PROCEDURE PROC_GET_ORDER_tem
(
    p_frm in number,--tu record
    p_to in number,--den record
    p_stock_id in number,--  ck
    p_board in number,-- bang
    p_member in number,-- thanh vien
    p_TradingSchedule_Id IN NUMBER,
    p_cursor OUT tcursor
)
IS
    str_sql VARCHAR2(5000);
    str_condition VARCHAR2(5000);
BEGIN
    str_condition := ' AND norp = 1 AND norc = 5 ';

    str_condition := str_condition || ' AND TRADING_SCHEDULE_ID = ' || p_TradingSchedule_Id;

    IF (p_stock_id > 0) THEN
        str_condition := str_condition || ' AND stock_id = '|| p_stock_id;
     END IF ;

    IF (p_board > 0) THEN
        str_condition := str_condition || ' AND (select b.id from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id AND s.idschedule = 0) = '|| p_board;
    END IF ;

    IF (p_member > 0)THEN
       str_condition := str_condition  || ' AND (co_member_id = '|| p_member || ' or member_id = '|| p_member ||')';
    END IF ;


    str_sql :=  ' select * from (select A.*,rownum as rowa from
        (SELECT order_id,order_date, a.order_time, member_id,co_member_id,aori,broker_id,co_broker_id,norc,norp,noro,oorb,
        (select t.name from ts_securities_type@hnxcore t join ts_symbol@hnxcore s on t.id=s.idsectype where s.id =  a.stock_id) stock_type,
        (select m.name from ts_members@hnxcore m where m.id = decode(oorb, 1,co_member_id ,member_id) and m.deleted = 0)member_name,
        (select m.short_name from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)short_name,
        (select m.code_trade from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)member,   account_no, co_account_no,
        (select m.code_trade from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0)co_member,
        (select o.code from ts_order_type@hnxcore o where o.deleted =0 and o.id = order_type) order_type_str,
        (select code from ts_order_type@hnxcore where deleted =0  and id = org_order_type)org_order_type_str,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
        a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,
        nvl((select idschedule from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id),0) idschedule,
        (select content from allcode@hnxcore where cdname = ''TT_ORDERS'' and cdtype=''AORI'' and cdval =aori)saori ,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        nvl(org_order_no, order_no) org_order_no,(select username from mt_users@hnxcore where id = broker_id and deleted =0) broker,
        (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_broker,
        (select content from allcode@hnxcore where cdname = ''TT_ORDERS'' and cdtype=''NORC'' and cdval =to_char(norc))snorc,
        (select content from allcode@hnxcore where cdname = ''TT_ORDERS'' and cdtype=''NORO'' and cdval =noro)snoro,
        (select content from allcode@hnxcore where cdname = ''TT_ORDERS'' and cdtype=''NORP'' and cdval = norp)snorp,
        nvl(a.reject_qtty,0) reject_qtty,nvl(a.exec_qtty,0) exec_qtty,nvl(cancel_qtty,0)cancel_qtty
        FROM tt_orders_plus_tem a where 1 = 1 and norc = 5 ' || str_condition || ' ORDER BY Order_date, order_id)A)
        where rowa > ' || p_frm || ' and rowa <= ' || p_to;

    dbms_output.put_line(str_sql);
    OPEN p_cursor FOR  str_sql;

    -- dangtq sua
    /*OPEN p_cursor for 'select * from
        (select A.*, rownum as rowa from
        (SELECT order_id,order_date, a.order_time, (select m.code from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)member,   account_no, co_account_no,
        (select m.code from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0)co_member,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
        a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,
        (select content from allcode@hnxcore where cdname = ''TT_ORDERS'' and cdtype=''AORI'' and cdval =aori)aori ,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        nvl(org_order_no, order_no) org_order_no,(select username from mt_users@hnxcore where id = broker_id and deleted =0) broker,
        (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_broker,
        (select content from allcode@hnxcore where cdname = ''TT_ORDERS'' and cdtype=''NORC'' and cdval =to_char(norc))norc
        FROM tt_orders_plus_tem a where 1 = 1 ' || str_condition || ' ORDER BY Order_date, order_id)A)
        where rowa > ' || p_frm || ' and rowa <= ' || p_to;*/
END;

PROCEDURE PROC_GET_COUNT_ORDER_TEM
(
    p_type IN NUMBER,
    p_TradingSchedule_Id IN NUMBER,
    p_id IN NUMBER,
    p_return OUT number
)
IS
    v_count NUMBER;
BEGIN
    IF p_type = type_board THEN
        SELECT COUNT(*) INTO v_count FROM tt_orders_plus_tem
        WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id AND norc = 5
        AND stock_id IN
        (
            SELECT s.id FROM ts_symbol@hnxcore s WHERE s.idboard = p_id AND idschedule = 0
        );
    ELSIF p_type = type_symbol THEN
        SELECT COUNT(*) INTO v_count FROM tt_orders_plus_tem
        WHERE TRADING_SCHEDULE_ID = p_TradingSchedule_Id AND stock_id = p_id AND norc = 5;
    END IF;

    p_return := v_count;
END;

PROCEDURE proc_checkIsReceive
(
    p_return OUT NUMBER
) IS
v_out NUMBER ;
v_count NUMBER ;
BEGIN
    --Dem nhung thang thi truong dang giao dich
    SELECT count(a.market_id) INTO v_count FROM ts_operator_markets@hnxcore a
    JOIN ts_markets@hnxcore m ON a.market_id = m.id
    WHERE  m.status = 'A' AND a.status_step in ('B','B1','M');
    --Neu co thang giao dich thi cho DR xu ly
    IF (v_count > 0) THEN
        p_return := 1;
    ELSE --Khong co thang nao = M thi nghia la DR ko can xu ly
        p_return := -1;
    END IF ;
    EXCEPTION
    WHEN OTHERS THEN
        p_return := -1;
        RAISE ;
END ;


PROCEDURE proc_checkEndsession
(
    p_type in number,
    p_code in VARCHAR2,
    p_return out VARCHAR2
)
IS
    v_return VARCHAR2 (3000);
    v_allcode varchar2 (3000);
    v_code VARCHAR2(3000);
    v_step number;
    PcurResult tcursor;
    v_changetype NUMBER ;
    v_brcode VARCHAR2 (50);
    v_symbol VARCHAR2 (50);
    v_current_status  VARCHAR2 (5);
    ---
    CURSOR curResult(v_con VARCHAR2) is SELECT  * FROM
        (select to_char(b.code) as brcode, '-' symbol,sm.type as changetype,
        case when op.status_step = 'B' or op.status_step = 'B1' then op.status_step
         when op.status_step = 'M' then decode(tc.current_status,90,op.status_step,tc.current_status)
         when op.status_step = 'D' then decode(is_backup,0,op.status_step,1,'5')
         when op.status_step = 'E12' then op.status_step
         when op.status_step like 'D%' then '6'
         when op.status_step like 'E%' then '7' end current_status,tsc.code as strCurrent,
         (select code from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
         from ts_trading_schedules@hnxcore where idschdrule = b.idschedule and deleted =0 and status ='A')
         and idschdrule = b.idschedule and deleted =0 and status ='A') as strLast
         from ts_boards@hnxcore  b left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted=0
         left join dr_step_matching sm on b.id = sm.object_id
         left join ts_operator_markets@hnxcore op on m.id = op.market_id
         left join ts_trading_calendars@hnxcore tc on b.id = tc.boardid and tc.is_workingday=1
         left join ts_trading_schedules@hnxcore tsc on tc.trading_schedule_id = tsc.id
         where b.deleted=0
        union
        select '-' as brcode,s.symbol,sm.type as changetype,
        case when op.status_step = 'B' or op.status_step = 'B1' then op.status_step
        when op.status_step = 'M' then decode(sc.current_status,90,op.status_step,sc.current_status)
         when op.status_step = 'D' then decode(is_backup,0,op.status_step,1,'5')
         when op.status_step = 'E12' then op.status_step
         when op.status_step like 'D%' then '6'
         when op.status_step like 'E%' then '7' end current_status,tsc.code as strCurrent,
         (select code from ts_trading_schedules@hnxcore where finish_time = (select max(finish_time)
         from ts_trading_schedules@hnxcore where idschdrule = s.idschedule and deleted =0 and status ='A')
         and idschdrule = s.idschedule and deleted =0 and status ='A') as strLast
         from ts_symbol@hnxcore s left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted=0
         left join dr_step_matching sm on s.id = sm.object_id
         left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted = 0
         left join ts_operator_markets@hnxcore op on m.id = op.market_id
         left join ts_symbol_calendar@hnxcore sc on s.id = sc.symbol_id and sc.deleted=0
         left join ts_trading_schedules@hnxcore tsc on sc.current_trading_schedule_id = tsc.id
        where s.deleted=0 and s.status not in (6,9) and s.idschedule > 0)
        where decode(brcode,'-',symbol,brcode) in (SELECT trim(regexp_substr(rtrim(v_con,', '), '[^,]+', 1, LEVEL)) FROM dual
        CONNECT BY trim(instr(rtrim(v_con,', '), ',', 1, LEVEL - 1)) > 0)  ;
BEGIN
    v_return :='';
    if(p_type = 0) then --check chuyen tien trinh
        --2018 thang nay ko dung kieu cursor o tren dc do nheu CK phien rieng qua cursor ko chay


        OPEN PcurResult FOR  SELECT  * FROM
        (select to_char(b.code) as brcode, '-' symbol,sm.type as changetype,
        case when op.status_step = 'B' or op.status_step = 'B1' then op.status_step
         when op.status_step = 'M' then decode(tc.current_status,90,op.status_step,tc.current_status)
         when op.status_step = 'D' then decode(is_backup,0,op.status_step,1,'5')
         when op.status_step = 'E12' then op.status_step
         when op.status_step like 'D%' then '6'
         when op.status_step like 'E%' then '7' end current_status
         from ts_boards@hnxcore  b left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted=0
         left join dr_step_matching sm on b.id = sm.object_id
         left join ts_operator_markets@hnxcore op on m.id = op.market_id
         left join ts_trading_calendars@hnxcore tc on b.id = tc.boardid and tc.is_workingday=1
         left join ts_trading_schedules@hnxcore tsc on tc.trading_schedule_id = tsc.id
         where b.deleted=0
        union
        select '-' as brcode,s.symbol,sm.type as changetype,
        case when op.status_step = 'B' or op.status_step = 'B1' then op.status_step
        when op.status_step = 'M' then decode(sc.current_status,90,op.status_step,sc.current_status)
         when op.status_step = 'D' then decode(is_backup,0,op.status_step,1,'5')
         when op.status_step = 'E12' then op.status_step
         when op.status_step like 'D%' then '6'
         when op.status_step like 'E%' then '7' end current_status
         from ts_symbol@hnxcore s left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted=0
         left join dr_step_matching sm on s.id = sm.object_id
         left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted = 0
         left join ts_operator_markets@hnxcore op on m.id = op.market_id
         left join ts_symbol_calendar@hnxcore sc on s.id = sc.symbol_id and sc.deleted=0
         left join ts_trading_schedules@hnxcore tsc on sc.current_trading_schedule_id = tsc.id
        where s.deleted=0 and s.status not in (6,9) and s.idschedule > 0)
        where decode(brcode,'-',symbol,brcode) in (SELECT object_name FROM  dr_step_matching)  ;


         LOOP
            FETCH PcurResult  INTO v_brcode,v_symbol,v_changetype,v_current_status;
            EXIT WHEN  PcurResult%NOTFOUND ;
            select decode(v_changetype,type_board,v_brcode,v_symbol) into v_code from dual;

            select step into v_step from dr_step_matching where object_name = v_code;
            if(v_current_status not in ('D','E12','5','6','7'))  then
                if(v_step <> pkg_dr_step_matching.step_end_session) then
                    v_return := v_return || v_code || ', ' ;
                end if;
            end if;

        END  LOOP ;

    ELSE  --check chuyen phien
        for recRelt in curResult(p_code) loop
            select decode(recRelt.changetype,to_char(type_board),recRelt.brcode,recRelt.symbol) into v_code from dual;
            select step into v_step from dr_step_matching where object_name = v_code;
            if((recRelt.strCurrent = recRelt.strLast and recRelt.current_status in ('D','5','6','13'))
                or recRelt.current_status in ('E12','7')
                or v_step = pkg_dr_step_matching.step_end_session) then
                v_return := v_return || v_code || ', ' ;
                if(v_step <> pkg_dr_step_matching.step_end_session) then
                    update dr_step_matching set step = pkg_dr_step_matching.step_end_session
                    where object_name = v_code;
                end if;
                commit;
            end if;
        end loop;
    end if;
    p_return := nvl(rtrim(v_return ,', '),'-1');
    EXCEPTION
      WHEN OTHERS THEN
      p_return :='-2';
      RAISE;
end;

procedure proc_reject_order
is
begin
    INSERT INTO TT_ORDERS_PLUS@hnxcore(order_id,org_order_id,floor_code,order_confirm_no,order_no,co_order_no,org_order_no,order_date,order_time,
        member_id,co_member_id,stock_id,order_type,priority,oorb,norp,norc,bore,aori,settlement_type,dorf,order_qtty,order_price,
        status,quote_price, state,quote_time, quote_qtty,exec_qtty,correct_qtty,cancel_qtty,reject_qtty,reject_reason, account_no,
        co_account_no,broker_id, co_broker_id, deleted,date_created,date_modified,modified_by,created_by,telephone,org_order_base ,
        aorc,yieldmat ,clordid ,noro,ORDER_PRICE_STOP,ORDER_QTTY_DISPLAY,SUB_ORDER_NO,ORG_ORDER_TYPE ,
        trading_schedule_id,current_trading_schedule_id,real_correct_qtty,idmarket,group_name,numseq_save)
    select seq_tt_orders.NEXTVAL@hnxcore, order_id,floor_code,order_confirm_no,order_no,co_order_no,org_order_no,order_date,order_time,
        member_id,co_member_id,stock_id,order_type,priority,oorb,norp,13 as norc,bore,aori,settlement_type,dorf,0 as order_qtty,order_price,
        status,quote_price, state,quote_time, 0 as quote_qtty,0 as exec_qtty,correct_qtty,cancel_qtty,quote_qtty as reject_qtty,
        decode(org_order_base,'L',4,3)reject_reason, account_no,co_account_no,broker_id, co_broker_id, deleted,date_created,date_modified,
        modified_by,created_by,telephone,org_order_base ,0,yieldmat ,clordid ,noro,ORDER_PRICE_STOP,ORDER_QTTY_DISPLAY,SUB_ORDER_NO,
        ORG_ORDER_TYPE ,trading_schedule_id,current_trading_schedule_id,exec_qtty,idmarket,'',0
        FROM view_tt_orders@hnxcore
        WHERE deleted = 0 and norc not in (3,4,5,13) and quote_qtty > 0;
    EXCEPTION
      WHEN OTHERS THEN
      Rollback;
      Raise;
end;

--Tinh l?i thong tin GD cua CK
PROCEDURE PROC_UPDATE_SYMBOL_INFO
IS
BEGIN
    EXECUTE IMMEDIATE 'TRUNCATE TABLE TEMP_CURRENT_SYMBOL_PRICE';

    EXECUTE IMMEDIATE 'TRUNCATE TABLE TEMP_SYMBOL_INFO';

    -- TINH THONG TIN GIA GAN NHAT CUA CK CO PHIEN HIEN TAI LA DINH KY VA CO LENH KHOP
    INSERT INTO TEMP_CURRENT_SYMBOL_PRICE
     SELECT od.stock_id, max(od.order_price)  AS order_price,
     sum(od.order_qtty)  AS order_qtty
     from tt_orders_plus@hnxcore od
        JOIN  ts_symbol_calendar@hnxcore sc ON od.stock_id = sc.symbol_id AND od.trading_schedule_id =  sc.current_trading_schedule_id
        JOIN ts_trading_schedules@hnxcore tsc ON sc.current_trading_schedule_id = tsc.id
        join ts_trading_state@hnxcore tst on tsc.idstate = tst.id and tst.deleted = 0
        join ts_match_type@hnxcore mt on tst.matchtype = mt.id and mt.deleted = 0
        where mt.matchtype='A' AND od.norc = 5 AND norp = 1 GROUP BY od.stock_id;
     --- neu ko fai la DK hoac ko co lenh khop DK thi lay trong tt symbol info (tuong tu func: pkg_dr_core.func_get_Current)
    INSERT INTO TEMP_CURRENT_SYMBOL_PRICE
     SELECT id, nvl(current_price,0), nvl(current_qtty,0) FROM tt_symbol_info@hnxcore
     WHERE id NOT IN (SELECT stock_id FROM TEMP_CURRENT_SYMBOL_PRICE);

    INSERT INTO TEMP_SYMBOL_INFO
     select id as stock_id, symbol ,nvl(MAX_PRICE_BUY,0)MAX_PRICE_BUY,nvl(MAX_QTTY_BUY,0)MAX_QTTY_BUY,nvl(MAX_PRICE_SELL,0)MAX_PRICE_SELL,
      nvl(MAX_QTTY_SELL,0)MAX_QTTY_SELL,nvl(A.match_price,0)match_price,nvl(match_qtty,0)match_qtty,nvl(match_value,0)match_value,
      nvl(pt_match_price,0)pt_match_price,nvl(pt_match_qtty,0)pt_match_qtty,nvl(EXCUTE_RDLOT_PRICE,0)EXCUTE_RDLOT_PRICE,
      nvl(EXCUTE_ODDLOT_PRICE,0)EXCUTE_ODDLOT_PRICE,nvl(KLKCHAN_GANNHAT,0)KLKCHAN_GANNHAT,nvl(KLKLE_GANNHAT,0)KLKLE_GANNHAT,
      nvl(GTKCHAN_GANNHAT,0)GTKCHAN_GANNHAT,nvl(GTKLE_GANNHAT,0)GTKLE_GANNHAT,
      --gia khop du kien
      pr.order_price as CURRENT_PRICE,
      --kl khop du kien
      pr.order_qtty as CURRENT_QTTY
      from (SELECT stock_id,
        -- gia mua cho khop cao nhat (thong thuong - chan)
        nvl(max(CASE WHEN tt.norc not in (3,4,5,13) and norp = 1 AND noro = 1 and oorb=1 and tt.quote_qtty > 0 THEN  tt.quote_price END ),0) MAX_PRICE_BUY ,
        -- kl mua c? gia cho khop cao nhat (thong thuong - chan)
        nvl((select quote_qtty from view_tt_orders@hnxcore tt where deleted = 0 and quote_price = nvl((select max(quote_price)
        from view_tt_orders@hnxcore tt where deleted = 0 and tt.norc not in (3,4,5,13) and norp = 1 AND noro = 1 and oorb=1
        and stock_id = tt.stock_id and rownum=1 and tt.quote_qtty > 0 ),0) and stock_id = tt.stock_id and rownum=1 and tt.quote_qtty > 0),0)  MAX_QTTY_BUY ,
        -- gia mua cho khop cao nhat (thong thuong - chan)
        nvl(max(CASE WHEN tt.norc not in (3,4,5,13) and norp = 1 AND noro = 1 and oorb=2 and tt.quote_qtty > 0 THEN  tt.quote_price END ),0) MAX_PRICE_SELL ,
        -- kl ban co gia cho khop cao nhat (thong thuong - chan)
        nvl((select quote_qtty from view_tt_orders@hnxcore tt where deleted = 0 and quote_price = nvl((select max(quote_price)
        from view_tt_orders@hnxcore tt where deleted = 0 and tt.norc not in (3,4,5,13) and norp = 1 AND noro = 1 and oorb=2
        and stock_id = tt.stock_id and rownum=1 and tt.quote_qtty > 0),0) and stock_id = tt.stock_id and rownum=1 and tt.quote_qtty > 0),0) MAX_QTTY_SELL,
        --gia cua lenh khop thong thuong (chan+le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and stock_id = '||tt.stock_id),0) as match_price,
        max(CASE WHEN NORC = 5 AND NORP = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 THEN order_time ELSE ' ' END) as match_price,
        --kl cua lenh khop thong thuong (chan + le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and stock_id = '||tt.stock_id),0) as match_qtty,
        max( CASE WHEN NORC = 5 AND NORP = 1 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 THEN order_time ELSE ' ' END) as match_qtty,

        --gt cua lenh khop thong thuong (chan + le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(3,'NORC = 5 AND NORP = 1 and stock_id = '||tt.stock_id),0) as match_value,
        max(CASE WHEN NORC = 5 AND NORP = 1 THEN  order_qtty * order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 THEN order_time ELSE ' ' END)  as match_value,

       --gia cua lenh khop thoa thuan (chan+le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 7 and STATUS in(4,5,6) and NORP=2 and stock_id = '||tt.stock_id),0) as pt_match_price,
        max(CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) and NORP=2 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) and NORP=2 THEN order_time ELSE ' ' END)   as pt_match_price,
        --kl cua lenh khop thoa thuan (chan+le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 7 and STATUS in(4,5,6) and NORP=2 and stock_id = '||tt.stock_id),0) as pt_match_qtty,
        max(CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) and NORP=2 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) and NORP=2 THEN order_time ELSE ' ' END)  as pt_match_qtty,

        --gia cua lenh khop thong thuong (chan) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro=1 and stock_id = '||tt.stock_id),0) as EXCUTE_RDLOT_PRICE,
        max(CASE WHEN NORC = 5 AND NORP = 1 and noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 and noro = 1 THEN order_time ELSE ' ' END) as EXCUTE_RDLOT_PRICE,

        --gia cua lenh khop thong thuong (le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro=2 and stock_id = '||tt.stock_id),0) as EXCUTE_ODDLOT_PRICE,
        max(CASE WHEN NORC = 5 AND NORP = 1 and noro=2 THEN order_price ELSE 0 end) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 and noro=2 THEN order_time ELSE ' ' END) AS EXCUTE_ODDLOT_PRICE,

        --kl cua lenh khop thong thuong (chan) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and noro=1 and stock_id = '||tt.stock_id),0) as KLKCHAN_GANNHAT,
        max(CASE WHEN NORC = 5 AND NORP = 1 and noro=1 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 and noro=1 THEN order_time ELSE ' ' END) as KLKCHAN_GANNHAT,

        --kl cua lenh khop thong thuong (le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and noro=2 and stock_id = '||tt.stock_id),0) as KLKLE_GANNHAT,
        max(CASE WHEN NORC = 5 AND NORP = 1 and noro=2 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 and noro=2 THEN order_time ELSE ' ' END) AS KLKLE_GANNHAT,

        --gt cua lenh khop thong thuong (chan) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(3,'NORC = 5 AND NORP = 1 and noro=1 and stock_id = '||tt.stock_id),0) as GTKCHAN_GANNHAT,
        max(CASE WHEN NORC = 5 AND NORP = 1 and noro = 1 THEN order_qtty * order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 and noro=1 THEN order_time ELSE ' ' END) as GTKCHAN_GANNHAT,

        --gt cua lenh khop thong thuong (le) gan nhat
        --nvl(pkg_dr_core.func_get_OrderPrice(3,'NORC = 5 AND NORP = 1 and noro=2 and stock_id = '||tt.stock_id),0) as GTKLE_GANNHAT
        max(CASE WHEN NORC = 5 AND NORP = 1 and noro=2 THEN order_qtty * order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 and noro=2 THEN order_time ELSE ' ' END) AS GTKLE_GANNHAT
    FROM view_tt_orders@hnxcore tt
    WHERE deleted=0 GROUP BY stock_id)A
     RIGHT JOIN ts_symbol@hnxcore ts on A.stock_id = ts.id
     RIGHT JOIN TEMP_CURRENT_SYMBOL_PRICE pr ON pr.stock_id = a.stock_id
    WHERE ts.deleted = 0 order by symbol;

    merge INTO tt_symbol_info@hnxcore M
    USING (SELECT * FROM TEMP_SYMBOL_INFO) recInfo
    ON (M.ID = recInfo.stock_id)
    WHEN matched THEN UPDATE SET
       MAX_PRICE_BUY = recInfo.MAX_PRICE_BUY,
        MAX_QTTY_BUY = recInfo.MAX_QTTY_BUY,
        MAX_PRICE_SELL=recInfo.MAX_PRICE_SELL,MAX_QTTY_SELL = recInfo.MAX_QTTY_SELL,
        MATCH_PRICE = recInfo.MATCH_PRICE,MATCH_QTTY=recInfo.MATCH_QTTY,
        MATCH_VALUE = recInfo.MATCH_VALUE,PT_MATCH_PRICE=recInfo.PT_MATCH_PRICE,
        PT_MATCH_QTTY = recInfo.PT_MATCH_QTTY,EXCUTE_RDLOT_PRICE = recInfo.EXCUTE_RDLOT_PRICE,
        EXCUTE_ODDLOT_PRICE = recInfo.EXCUTE_ODDLOT_PRICE,KLKCHAN_GANNHAT = recInfo.KLKCHAN_GANNHAT,
        KLKLE_GANNHAT = recInfo.KLKLE_GANNHAT,
        GTKCHAN_GANNHAT = recInfo.GTKCHAN_GANNHAT,GTKLE_GANNHAT = recInfo.GTKLE_GANNHAT,
        CURRENT_PRICE = recInfo.CURRENT_PRICE,CURRENT_QTTY = recInfo.CURRENT_QTTY;

    merge INTO ts_symbol@hnxcore M
        USING (SELECT * FROM TEMP_SYMBOL_INFO) recInfo
        ON (M.ID = recInfo.stock_id)
        WHEN matched THEN  UPDATE SET  match_price = recInfo.MATCH_PRICE;


    COMMIT ;
    EXCEPTION
    WHEN OTHERS THEN
    ROLLBACK;
    raise;
END ;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_CORE

-- Start of DDL Script for Package STOCK_DR.PKG_DR_CORE_INFO
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_core_info
  IS
type tcursor is ref cursor;

status_no_order CONSTANT NUMBER(1) := 0;
status_un_match CONSTANT NUMBER(1) := 1;
status_path_match CONSTANT NUMBER(1) := 2;
status_all_match CONSTANT NUMBER(1) := 3;

type_board CONSTANT NUMBER(1) := 1;
type_symbol CONSTANT NUMBER(1) := 2;

NORC_QUOTE CONSTANT NUMBER(1) := 6;
NORC_NOMAL CONSTANT NUMBER(1) := 2;
NORC_REPLACE CONSTANT NUMBER(1) := 3;
NORC_CANCEL CONSTANT NUMBER(1) := 4;
NORC_EXECUTED CONSTANT NUMBER(1) := 5;
NORC_ICE_BERG CONSTANT NUMBER(1) := 8;
NORC_STOP_ORDER CONSTANT NUMBER(1) := 9;
NORC_MARKET_ORDER CONSTANT NUMBER(2) := 10;
NORC_REJECT_ORDER CONSTANT NUMBER(2) := 13;
NORC_DEAL CONSTANT NUMBER(1) := 7;

ORDER_STATUS_0 CONSTANT NUMBER(1) := 0;         --Trang thai lenh khong hieu luc
ORDER_STATUS_NORMAL CONSTANT NUMBER(1) := 1;    -- Trang thai lenh binh thuong
ORDER_STATUS_PEND CONSTANT NUMBER(1) := 2;      -- Trang thai lenh cho kiem soat
STOP_STATUS_ACTIVE CONSTANT NUMBER(1) := 3;     --TRANG THAI LENH STOP DA DC KICH HOAT
MARKET_STATUS_CONVERT CONSTANT NUMBER(1) := 4;--     Market da dc chuyen sang LO


--trang thai bang / thi truong
PROCEDURE PROC_STATUS_BOARDS_GETALL
(
    p_board in number,
    p_market in number,
    p_cursor OUT tcursor
);

--Lay bang/thi truong
PROCEDURE PROC_CBO_GETALL
(
    p_type in number,
    p_cursor OUT tcursor
);

--trang thai CK
PROCEDURE PROC_STATUS_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_cursor OUT tcursor
);

--thong tin GD cua CK
PROCEDURE PROC_TRADED_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
);

--thong tin GD cua CK o TT_order
PROCEDURE PROC_TRADED_SYMBOL_GETALLS
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
);

--thong tin s? l?nh
PROCEDURE PROC_STATUS_ORDER_GETALL
(
    p_type in number,
    p_code in number,
    p_board in number,
    p_member in number,
    p_schedule in number,
    p_cursor OUT tcursor
);

--thong tin s? l?nh khop de huy KQGD
PROCEDURE PROC_STATUS_ORDER_GETS
(
    p_symbol in VARCHAR2,
    p_board in VARCHAR2,
    p_cursor OUT tcursor
);

--thong tin gia khop cuoi cung o so lenh theo chung khoan - tinh lai chi so
PROCEDURE PROC_SYMBOL_PRICE_GET
(
    p_type in number,
    p_index_id in number,
    p_cursor OUT tcursor
);

procedure proc_getlast_ordertime
(
    p_type in number,
    p_code in VARCHAR2,
    p_cursor OUT tcursor
);

PROCEDURE proc_order_member
(
    p_member in VARCHAR2,
    p_cursor OUT tcursor
);

--
--Lay order type code
FUNCTION func_get_OrderTypeCode(strId VARCHAR2) RETURN VARCHAR2;

FUNCTION func_get_OrderPrice(p_type number,strsql VARCHAR2) RETURN VARCHAR2;

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_core_info
IS


--tr?ng th?i bang / thi truong
PROCEDURE PROC_STATUS_BOARDS_GETALL
(
    p_board in number,
    p_market in number,
    p_cursor OUT tcursor
)
is
  str VARCHAR2(2000);
begin
    str :='';
    if(p_board > 0) then
        str := str || ' and A.id = '|| p_board;
    end if;
    if(p_market > 0)then
        str:=str  || ' and A.market_id = '|| p_market;
    end if;
    open p_cursor for
    'select A.*, nvl(B.klkttg_chan,0) klkttg_chan,nvl(B.gtkttg_chan,0) gtkttg_chan, nvl(B.klkttg_le,0) klkttg_le,
    nvl(B.gtkttg_le,0) gtkttg_le,nvl(B.klktt_chan,0) klktt_chan, nvl(B.gtktt_chan,0) gtktt_chan,
    nvl(B.klktt_le,0) klktt_le, nvl(B.gtktt_le,0) gtktt_le,nvl(B.tong_klgd,0) total_traded_qtty,
    nvl(B.tong_gtgd,0) total_traded_value
    from (select b.id,b.code as boardcode, m.code as marketcode,m.id as market_id,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then op.status_step
    when op.status_step = ''M'' then decode(tc.current_status,90,op.status_step,tc.current_status)
    when op.status_step = ''D'' then decode(is_backup,0,op.status_step,1,''5'')
    when op.status_step = ''E12'' then op.status_step
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then ab.content
    when op.status_step = ''M'' then decode(tc.current_status,90,ab.content,aa.content)
    when op.status_step = ''D'' then decode(is_backup,0,ab.content,1,''5'')
    when op.status_step = ''E12'' then ab.content
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status_str,decode(op.status_step,''M'',ts.code,''-'')as statecode,
    op.CURRENT_DATE as working_day,tsc.code as schedulecode, ts.inputallow,ts.matchallow,ts.sesstype,
    a.content as sesstype_str,ts.ordertype,pkg_dr_core_info.func_get_OrderTypeCode(ts.ordertype) as ordertype_str,
    tt.rdallow ||''|'' || tt.odallow as tradingtype,tdt.matchtype,
    (select content from allcode@hnxcore where cdname=''TS_MATCH_TYPE'' and cdtype=''MATCHTYPE'' and cdval = tdt.matchtype)matchtype_str
    from ts_boards@hnxcore  b left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted=0
    left join ts_operator_markets op on m.id = op.market_id
    left join TS_TRADING_CALENDARS tc on b.id = tc.boardid and tc.is_workingday=1
    left join ts_trading_schedules@hnxcore tsc on tc.trading_schedule_id = tsc.id
    left join ts_trading_state@hnxcore ts on tsc.idstate = ts.id
    left join ts_match_type@hnxcore tdt on ts.matchtype = tdt.id
    left join allcode@hnxcore a on ts.sesstype = a.cdval  and a.cdname =''TS_TRADING_STATE'' and a.cdtype=''SESSTYPE''
    left join allcode@hnxcore aa on tc.current_status  = aa.cdval and aa.cdname =''TS_TRADING_CALENDAR'' and aa.cdtype=''CURRENT_STATUS''
    left join allcode@hnxcore ab on op.status_step  = ab.cdval and ab.cdname =''OPERATOR_MARKET'' and ab.cdtype=''CURRENT_STEP''
    left join ts_trading_type@hnxcore tt on ts.tradingtype = tt.id
    where b.deleted=0)A
    left join
    (select s.idboard,
    --tong klgd thoa thuan chan
    nvl(sum(CASE WHEN norp = 2 and noro = 1 AND norc = 7 AND tt.status in(4,5,6) THEN  order_qtty END),0)  klktt_chan,
    --tong gtgd thoa thuan chan
    nvl(sum(CASE WHEN norp = 2 and noro = 1 AND norc = 7 AND tt.status in(4,5,6) THEN order_qtty * order_price END),0) gtktt_chan ,
    --tong klgd thoa thuan le
    nvl(sum(CASE WHEN norp = 2 and noro = 2 AND norc = 7 AND tt.status in(4,5,6) THEN  order_qtty END),0)  klktt_le,
    --tong gtgd thoa thuan le
    nvl(sum(CASE WHEN norp = 2 and noro = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_qtty * order_price END),0) gtktt_le ,
    --tong klgd thong thuong chan
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 1 AND tt.norc = 5 THEN  tt.order_qtty END),0) klkttg_chan,
    -- tong gtgd thong thuong chan
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 1 AND tt.norc = 5 THEN tt.order_qtty * tt.order_price END),0) gtkttg_chan,
    --tong klgd thong thuong le
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 2 AND tt.norc = 5 THEN  tt.order_qtty END),0) klkttg_le,
    -- tong gtgd thong thuong le
    nvl(sum(CASE WHEN tt.norp = 1 and noro = 2 AND tt.norc = 5 THEN tt.order_qtty * tt.order_price END),0) gtkttg_le,
    --tong klgd
    nvl(sum(CASE WHEN (tt.norp = 1 AND tt.norc = 5)or (norp = 2 and norc = 7 AND tt.status in(4,5,6)) THEN  tt.order_qtty END),0) as tong_klgd,
    --tong gtgd
    nvl(sum(CASE WHEN (tt.norp = 1 AND tt.norc = 5)or (norp = 2 and norc = 7 AND tt.status in(4,5,6)) THEN  tt.order_qtty * tt.order_price END),0) as tong_gtgd
    from view_tt_orders tt left join ts_symbol s on tt.stock_id = s.id
    group by s.idboard) B
    on A.id = B.idboard
    where 1=1 ' || str || ' order by A.boardcode, A.marketcode';
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--Lay bang/thi truong
PROCEDURE PROC_CBO_GETALL
(
    p_type in number,
    p_cursor OUT tcursor
)
is
BEGIN
    if(p_type = 0 ) then --bang
        open p_cursor for select id, code from ts_boards@hnxcore where deleted = 0 and status = 'A'
        order by code asc;
    elsif (p_type = 1) then --tt
        open p_cursor for select id, code from ts_markets@hnxcore where deleted = 0 and status = 'A'
        order by code asc;
    elsif (p_type = 2) then --ck
        open p_cursor for select id,symbol as code from ts_symbol@hnxcore where deleted = 0 and status not in (6,9)
        order by symbol asc;
    elsif (p_type = 3) then --member
        open p_cursor for select id, code_trade as code from ts_members@hnxcore
        where deleted = 0 and status = 'A' order by code_trade asc;
    elsif (p_type = 4) then --status
        open p_cursor for select cdval as id,content as code from allcode@hnxcore
        where cdname = 'TS_SYMBOL' and cdtype='STATUS' and cdval not in (6,9) order by content asc;
    elsif(p_type = 10) then --phien gd
        open p_cursor for select id, code from ts_trading_schedules@hnxcore
        where deleted = 0 and status = 'A' order by code asc;
    end if;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--05/10/2015-Sua bo ck huy niem yet khoi ds theo y/c 72646
--06/10/2015-72653
PROCEDURE PROC_STATUS_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_cursor OUT tcursor
)
is
     str VARCHAR2(2000);
begin
    str :='';
    if(p_code > 0) then
        str := str || ' and s.id = '|| p_code;
    end if;
    if(p_board > 0)then
        str:=str  || ' and b.id = '|| p_board;
    end if;
    open p_cursor for
    'select s.symbol, s.name,s.idsectype,a.content as sectype, b.code as boardcode,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then op.status_step
    when op.status_step = ''M'' then decode(decode(s.idschedule,0,tc.current_status,sc.current_status),90,op.status_step,tc.current_status)
    when op.status_step = ''D'' then decode(is_backup,0,op.status_step,1,''5'')
    when op.status_step = ''E12'' then op.status_step
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status,
    case when op.status_step = ''B'' or op.status_step = ''B1'' then ab.content
    when op.status_step = ''M'' then decode(decode(s.idschedule,0,tc.current_status,sc.current_status),90,ab.content,aa.content)
    when op.status_step = ''D'' then decode(is_backup,0,ab.content,1,''5'')
    when op.status_step = ''E12'' then ab.content
    when op.status_step like ''D%'' then ''6''
    when op.status_step like ''E%'' then ''7'' end current_status_str,
    decode(op.status_step,''M'',ts.code,''-'')as statecode,op.CURRENT_DATE as working_day,tsc.code as schedulecode,
    ts.ordertype,pkg_dr_core_info.func_get_OrderTypeCode(ts.ordertype) as ordertype_str
    from ts_symbol@hnxcore s left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted=0
    left join ts_securities_type@hnxcore st on s.idsectype = st.id and st.deleted = 0
    left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted = 0
    left join ts_operator_markets op on m.id = op.market_id
    left join TS_SYMBOL_CALENDAR sc on s.id = sc.symbol_id and sc.deleted=0
    left join ts_trading_schedules@hnxcore tsc on sc.current_trading_schedule_id = tsc.id
    left join ts_trading_state@hnxcore ts on decode(sc.current_trading_state,0,tsc.idstate,sc.current_trading_state) = ts.id
    left join TS_TRADING_CALENDARS tc on b.id = tc.boardid and tc.is_workingday = 1
    left join allcode@hnxcore a on st.type = a.cdval  and a.cdname =''TS_SERCURITIES_TYPE'' and a.cdtype=''TYPE''
    left join allcode@hnxcore aa on sc.current_status  = aa.cdval and aa.cdname =''TS_TRADING_CALENDAR'' and aa.cdtype=''CURRENT_STATUS''
    left join allcode@hnxcore ab on op.status_step  = ab.cdval and ab.cdname =''OPERATOR_MARKET'' and ab.cdtype=''CURRENT_STEP''
    left join ts_trading_type@hnxcore tt on ts.tradingtype = tt.id
    where s.deleted=0 and s.status not in (6,9) ' || str || ' order by s.symbol';
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--thong tin GD cua CK lay tu bang thong tin ck
PROCEDURE PROC_TRADED_SYMBOL_GETALL
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for
    SELECT 'TT_SYMBOL_INFO' as sys,tt.symbol_date,tt.symbol, s.name,b.code as boardcode,s.idsectype, a.content as sectype,
    s.status,aa.content as status_str,s.listing_status, ab.content as listingsts,s.reference_status,ac.content as refstatus,
    nvl(s.basic_price,0)basic_price, nvl(s.celling_price,0)celling_price,nvl(s.floor_price,0)floor_price,
    nvl(s.parvalue,0)parvalue,tt.date_no,nvl(tt.open_price,0) open_price, nvl(tt.close_price,0)close_price, nvl(tt.avg_price,0)avg_price,
    nvl(tt.current_price,0)current_price,nvl(tt.current_qtty,0)current_qtty,nvl(tt.match_price,0)match_price,
    nvl(tt.match_qtty,0)match_qtty, nvl(tt.excute_rdlot_price,0)excute_rdlot_price,nvl(tt.klkchan_gannhat,0)klkchan_gannhat,
    nvl(tt.excute_oddlot_price,0)excute_oddlot_price,nvl(tt.klkle_gannhat,0)klkle_gannhat,nvl(tt.klkchan,0)klkchan,nvl(tt.gtkchan,0)gtkchan,
    nvl(tt.klkle,0)klkle,nvl(tt.gtkle,0)gtkle,nvl(tt.klkttg,0)klkttg,nvl(tt.gtkttg,0)gtkttg,nvl(tt.pt_match_price,0)pt_match_price,
    nvl(tt.pt_match_qtty,0)pt_match_qtty,nvl(tt.klktt,0)-nvl(tt.od_total_traded_qtty_pt,0) as klktt_chan,
    nvl(tt.gtktt,0)-nvl(tt.od_total_traded_value_pt,0) as gtktt_chan,nvl(tt.od_total_traded_qtty_pt,0) as klktt_le,
    nvl(tt.od_total_traded_value_pt,0) as gtktt_le,nvl(klktt,0)klktt, nvl(gtktt,0)gtktt
    from tt_symbol_info tt JOIN ts_symbol s ON s.id = tt.id_symbol
    left join ts_boards b on s.idboard = b.id and b.deleted = 0
    left join ts_securities_type st on s.idsectype = st.id and st.deleted = 0
    left join allcode_core  a on st.type = a.cdval  and a.cdname ='TS_SERCURITIES_TYPE' and a.cdtype='TYPE'
    left join allcode_core aa on s.status = aa.cdval  and aa.cdname ='TS_SYMBOL' and aa.cdtype='STATUS'
    left join allcode_core  ab on s.listing_status = ab.cdval  and ab.cdname ='TS_SYMBOL' and ab.cdtype='LISTING_STATUS'
    left join allcode_core ac on s.listing_status = ac.cdval  and ac.cdname ='TS_SYMBOL' and ac.cdtype='REFERENCE_STATUS'
    where s.id like decode(p_code,-1,'%',p_code) and b.id like decode(p_board,-1,'%',p_board)
    and s.idsectype like decode(p_sectype,-1,'%',p_sectype) and s.status like decode(to_number(p_status),-1,'%',p_status)
    and s.listing_status like decode(p_listingsts,-1,'%',p_listingsts)
    and s.deleted=0 and s.status not in (6,9)
    order by tt.symbol_date,tt.symbol;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;


--thong tin GD cua CK lay tu so lenh
PROCEDURE PROC_TRADED_SYMBOL_GETALLS
(
    p_code in number,
    p_board in number,
    p_sectype in number,
    p_status in VARCHAR2,
    p_listingsts in number,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor FOR
     SELECT 'TT_ORDERS' as sys, tt.symbol_date,to_char(s.symbol) as symbol, s.name,b.code as boardcode,s.idsectype,
    a.content as sectype,s.status,aa.content as status_str,s.listing_status,ab.content as listingsts,s.reference_status,
    ac.content as refstatus,s.basic_price, s.celling_price,s.floor_price ,s.parvalue ,s.date_no,
    nvl(tt.open_price,0) open_price,nvl(tt.close_price,0)close_price, nvl(tt.avg_price,0)avg_price,tt.current_price,
    tt.current_qtty,nvl(A.v_last_mtprice_nm,0) as match_price,nvl(A.v_last_mtqtty_nm,0) as match_qtty,
    nvl(A.v_last_mtprice_nm_rd,0) as EXCUTE_RDLOT_PRICE, nvl(A.v_last_mtqtty_nm_rd,0) as KLKCHAN_GANNHAT,
    nvl(A.v_last_mtprice_nm_od,0) as EXCUTE_ODDLOT_PRICE,nvl(A.v_last_mtqtty_nm_od,0) as KLKLE_GANNHAT,
    nvl(A.v_kl_khop_chan_15,0) as klkchan,nvl(A.v_gt_khop_chan_15,0) as gtkchan,nvl(A.v_kl_khop_le_25,0) as klkle,
    nvl(A.v_gt_khop_le_25,0) as gtkle,nvl(A.v_klkttg_15,0) as klkttg,nvl(A.v_gtkttg_15,0) as gtkttg,
    nvl(A.v_last_mtprice_pt,0) as pt_match_price,nvl(A.v_last_mtqtty_pt,0) as pt_match_qtty,
    nvl(A.v_klktt_25,0)-nvl(A.v_od_total_traded_qtty_pt,0) as klktt_chan,
    nvl(A.v_gtktt_25,0)-nvl(A.v_od_total_traded_value_pt,0) as gtktt_chan,nvl(A.v_od_total_traded_qtty_pt,0) as klktt_le,
    nvl(A.v_od_total_traded_value_pt,0) as gtktt_le, nvl(A.v_klktt_25,0) as klktt, nvl(A.v_gtktt_25,0) as gtktt,
    (fr.current_room - nvl(buymatch,0)) current_room
    from(SELECT stock_id,
        -- tong khoi luong khop thoa thuan
        nvl(sum(CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN  order_qtty END),0)  v_klktt_25,
        -- tong gia tri khop thoa thuan
        nvl(sum(CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_qtty * order_price END),0) v_gtktt_25 ,
        --  tong khoi luong khop chan
        nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN  order_qtty END),0) v_kl_khop_chan_15,
        -- tong gia tri khop chan
        nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN order_qtty * order_price END),0) v_gt_khop_chan_15 ,
        -- tong khoi luong khop thong thuong ( lay tong kl khop cua tat ca cac lenh)
        nvl(sum(CASE WHEN norp = 1 AND norc = 5 THEN  order_qtty END),0)  v_klkttg_15,
        -- tong gia tri khop thong thuong ( lay tong gia tri khop cua tat ca cac lenh)
        nvl(sum(CASE WHEN norp = 1 AND norc = 5 THEN order_qtty * order_price END),0)  v_gtkttg_15 ,
        --  tong khoi luong khop le thong thuong
        nvl(sum(CASE WHEN tt.norp = 1 AND tt.noro = 2 AND tt.norc = 5 THEN  tt.order_qtty END),0) v_kl_khop_le_25,
        -- tong gia tri khop le thong thuong
        nvl(sum(CASE WHEN tt.norp = 1 AND tt.noro = 2 AND tt.norc = 5 THEN tt.order_qtty * tt.order_price END),0) v_gt_khop_le_25,
        -- tong khoi luong dat ban thoa thuan
        -- them deleted = 0 (ko tinh lenh xoa ngang) 12/02/2014
        nvl(SUM(CASE WHEN tt.NORC = 7  AND tt.deleted = 0 and tt.STATUS in(4,5,6) OR (tt.NORP = 2 AND tt.STATUS = 3 AND tt.deleted = 0)
        THEN tt.order_qtty END ),0) v_total_pt_offer_qtty,
        --tong kl khop cua thoa thuan lo le
        nvl(SUM(CASE WHEN tt.NORC = 7 and tt.STATUS in(4,5,6) AND tt.NORO = 2 THEN tt.order_qtty END ),0) v_od_total_traded_qtty_pt,
        --tong gt khop cua thoa thuan lo le
        nvl(SUM(CASE WHEN tt.NORC = 7 and tt.STATUS in(4,5,6) AND tt.NORO = 2 THEN tt.order_qtty*tt.order_price END ),0) v_od_total_traded_value_pt,
        --total_bid_qtty_od lo le va mua
        nvl(sum(
         CASE WHEN noro = 2 AND OORB = 1 AND (norc = 2 AND ORG_ORDER_BASE = 'L' )
             THEN  (order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)) end),0)  v_TOTAL_BID_QTTY_OD,
        --total_bid_qtty_od lo le va ban
        nvl(sum(
         CASE WHEN noro = 2 AND OORB = 2 AND (norc = 2 AND ORG_ORDER_BASE = 'L' )
             THEN  (order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)) end),0)  v_TOTAL_OFFER_QTTY_OD,
        --- v_total_bid_qtty lo chan mua
        nvl(sum(
         CASE WHEN noro = 1 AND  OORB = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
             OR (norc = 10 and ORG_ORDER_BASE ='M'))
             THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
         WHEN noro = 1 AND OORB = 1 AND (norc = 8 and ORG_ORDER_BASE ='I')
              THEN (order_qtty - nvl(cancel_qtty,0)) END),0)  v_total_bid_qtty,
        -- v_total_offer_qtty lo chan ban
        nvl(sum(
         CASE WHEN noro = 1 AND OORB = 2 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
             OR (norc = 10 and ORG_ORDER_BASE ='M'))
             THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
         WHEN noro = 1 AND OORB = 2 AND(norc = 8 and ORG_ORDER_BASE ='I')
             THEN (order_qtty - nvl(cancel_qtty,0)) END),0)  v_total_offer_qtty,
        --total_bid_qtty
        nvl(sum(case when noro = 1 AND  OORB = 1 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) v_total_bid_qtty_io_con,
        --total_offer_qtty
        nvl(sum(case when noro = 1 AND OORB = 2 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) v_total_offer_qtty_io_con,


        --gia khop gan nhat cua lenh thong thuong
        max(CASE WHEN NORC = 5 AND NORP = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 THEN order_time ELSE ' ' END) v_last_mtprice_nm,

        --nvl(pkg_dr_core_info.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and stock_id = '||tt.stock_id),0) v_last_mtprice_nm,
        --kl khop gan nhat thong thuong
        max(CASE WHEN NORC = 5 AND NORP = 1 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 THEN order_time ELSE ' ' END) v_last_mtqtty_nm,

        --nvl(pkg_dr_core_info.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and stock_id = '||tt.stock_id),0) v_last_mtqtty_nm,
        --gia khop gan nhat thong thuong lo le

        --nvl(pkg_dr_core_info.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 2 and stock_id = '||tt.stock_id),0) v_last_mtprice_nm_od,

        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_time ELSE ' ' END) v_last_mtprice_nm_od,

        --kl khop gan nhat thong thuong lo le
        --nvl(pkg_dr_core_info.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and noro = 2 and stock_id = '||tt.stock_id),0) v_last_mtqtty_nm_od,
        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 2 THEN order_time ELSE ' ' END) v_last_mtqtty_nm_od,

        --gia khop gan nhat thong thuong lo chan
        --nvl(pkg_dr_core_info.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||tt.stock_id),0) v_last_mtprice_nm_rd,

        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_time ELSE ' ' END) v_last_mtprice_nm_rd,
        --kl khop gan nhat thong thuong lo chan
        --nvl(pkg_dr_core_info.func_get_OrderPrice(1,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||tt.stock_id),0) v_last_mtqtty_nm_rd,
        max(CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1  and noro = 1 THEN order_time ELSE ' ' END) v_last_mtqtty_nm_rd,

        --gia khop gan nhat cua lenh thoa thuan
       -- nvl(pkg_dr_core_info.func_get_OrderPrice(0,'NORC = 7 AND STATUS in(4,5,6) and stock_id = '||tt.stock_id),0) v_last_mtprice_pt,
        max(CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_time ELSE ' ' END)  v_last_mtprice_pt,

        --kl khop gan nhat cua lenh thoa thuan
       -- nvl(pkg_dr_core_info.func_get_OrderPrice(1,'NORC = 7 AND STATUS in(4,5,6) and stock_id = '||tt.stock_id),0) v_last_mtqtty_pt,

        max(CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_qtty ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 7 AND tt.STATUS in(4,5,6) THEN order_time ELSE ' ' END)  v_last_mtqtty_pt,

        --buy match
        nvl(sum(case when (norp = 1 AND norc = 5  AND substr(co_account_no,4,1) = 'F')
        or (norp = 2 AND norc = 7 AND status IN (4,5,6) AND substr(co_account_no,4,1) = 'F' AND substr(account_no,4,1) <> 'F') then order_qtty end),0)buymatch
    from view_tt_orders tt where deleted = 0 GROUP BY tt.stock_id ) A
    right JOIN ts_symbol s ON s.id = A.stock_id and s.deleted =0 and s.status not in (6,9)
    join tt_symbol_info tt on s.symbol=tt.symbol
    join ts_foreign_room@hnxcore fr on s.symbol=fr.symbol
    left join ts_boards b on s.idboard = b.id and b.deleted = 0
    left join ts_securities_type st on s.idsectype = st.id and st.deleted = 0
    left join allcode_core a on st.type = a.cdval  and a.cdname ='TS_SERCURITIES_TYPE' and a.cdtype='TYPE'
    left join allcode_core aa on s.status = aa.cdval  and aa.cdname ='TS_SYMBOL' and aa.cdtype='STATUS'
    left join allcode_core ab on s.listing_status = ab.cdval  and ab.cdname ='TS_SYMBOL' and ab.cdtype='LISTING_STATUS'
    left join allcode_core ac on s.listing_status = ac.cdval  and ac.cdname ='TS_SYMBOL' and ac.cdtype='REFERENCE_STATUS'
    where s.id like decode(p_code,-1,'%',p_code) and b.id like decode(p_board,-1,'%',p_board)
    and s.idsectype like decode(p_sectype,-1,'%',p_sectype) and s.status like decode(to_number(p_status),-1,'%',p_status)
    and s.listing_status like decode(p_listingsts,-1,'%',p_listingsts)
    and s.deleted=0 and s.status not in (6,9)
    order by symbol_date,s.symbol;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;
--
--thong tin s? l?nh
PROCEDURE PROC_STATUS_ORDER_GETALL
(
    p_type in number,--loai: dat thong thuong/dat thoa thuan/ khop
    p_code in number,--  ck
    p_board in number,-- bang
    p_member in number,-- thanh vien
    p_schedule in number,--phien gd
    p_cursor OUT tcursor
)
is
    str VARCHAR2(5000);
BEGIN
    str :='';
    if(p_code > 0) then
        str := str || ' and stock_id = '|| p_code;
    end if;
    if(p_board > 0) then
        str := str || ' and (select b.id from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) = '|| p_board;
    end if;
    --y/c 72802
    if(p_member > 0)then
        if(p_type = 0) then
            str:=str  || ' and decode(oorb, 1,co_member_id ,member_id) = '|| p_member;
        else
            str:=str  || ' and (co_member_id = '|| p_member || ' or member_id = '|| p_member ||')';
        end if;
    end if;
    if(p_schedule > 0)then
        str:=str  || ' and A.trading_schedule_id = '|| p_schedule;
    end if;
    if(p_type = 0) then --kl
        open p_cursor for '
        select A.*, rownum as rowa from
        (SELECT order_id,a.member_id,
         m.name AS member_name, m.short_name,m.code_trade AS member_code,
         t.name AS stock_type,
        decode(oorb,1, co_account_no, account_no) account_no, a.order_time,
        s.symbol, b.code AS boardcode, ma.code  marketcode,a.order_type,
        odt.code order_type_str,org_order_type, a.order_no, a.org_order_no, order_date,
        odtg.code  AS org_order_type_str, norc,  oorb,  a1.content AS soorb,
        a.order_qtty, a.order_price ,trading_schedule_id,nvl(a.quote_price,0) quote_price ,
        a.quote_qtty, nvl(a.exec_qtty,0) exec_qtty,
        a.order_qtty_display,a.order_price_stop, a.reject_qtty, a.cancel_qtty, correct_qtty,
       tras.code  AS  trading_schedules, s.idsectype AS idsectype,
        t.TYPE AS StockType, decode(broker_id,0,co_broker_id,broker_id) broker_id,
        a.status,a.noro, a2.content  AS snoro,  mu.username AS broker, mu.type AS broker_type,
        a.SPECIAL_SIDE, a.Co_SPECIAL_SIDE,nvl(ISRISK_MANAGE,0) ISRISK_MANAGE, nvl(IS_INDAY,''N'') IS_INDAY,MMLINK_ORDERNO
        from view_tt_orders a
           JOIN ts_symbol s ON s.id = a.stock_id
          JOIN ts_members m ON  m.id = decode(a.oorb, 1,a.co_member_id ,a.member_id) and m.deleted = 0
          JOIN ts_securities_type t ON t.id = s.idsectype
          JOIN  ts_boards b on s.idboard = b.id
          JOIN ts_markets ma ON  ma.id = a.idmarket
          JOIN ts_order_type odt ON odt.id = a.order_type
          JOIN ts_order_type odtg ON  odtg.id = a.org_order_type
          JOIN allcode_core a1 ON a1.cdname = ''TT_ORDERS'' and a1.cdtype=''OORB'' and a1.cdval = a.oorb
          JOIN ts_trading_schedules tras ON tras.id = trading_schedule_id
          JOIN allcode_core a2 ON a2.cdname = ''TT_ORDERS'' and a2.cdtype=''NORO'' and a2.cdval = a.noro
          JOIN mt_users mu ON mu.id = decode(a.broker_id,0,a.co_broker_id,a.broker_id)
       where norp =1 and norc <> 5 and norc <> 13 and a.deleted = 0 ' || str || '  ORDER BY order_id) A';

    elsif(p_type = 1) then --tt
         open p_cursor for 'select A.*,rownum as rowa from
        (SELECT order_id,order_date, a.order_time,member_id,co_member_id,org_order_id,yieldmat,settle_day,
            a1. content AS settlement_type,m.name AS  member_name, t.name  AS stock_type,
             m1.code_trade AS member,account_no, co_account_no,m2.code_trade AS co_member,
              s.symbol , b.code AS boardcode, ma.code AS marketcode,
              broker_id, a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,aori,
              ts.code AS trading_schedules,a2.content AS saori,a3.content AS ssnorc, a4.content AS snoro,
               nvl(org_order_no, order_no) org_order_no,u1.username AS broker,u2.username AS co_broker,co_broker_id,norc,noro,
               a.SPECIAL_SIDE, a.Co_SPECIAL_SIDE,nvl(ISRISK_MANAGE,0) ISRISK_MANAGE, nvl(IS_INDAY,''N'') IS_INDAY,MMLINK_ORDERNO
            FROM tt_orders@hnxcore a
            JOIN ts_symbol@hnxcore s ON s.id = a.stock_id
            JOIN ts_boards@hnxcore b on s.idboard = b.id
            JOIN ts_markets@hnxcore ma ON ma.id = a.idmarket
            JOIN ts_members@hnxcore m ON  m.id = decode(oorb, 1,co_member_id ,member_id)
            JOIN  ts_securities_type@hnxcore t ON t.id=s.idsectype
            JOIN ts_members@hnxcore m1 ON m1.id = a.member_id
            JOIN ts_members@hnxcore m2 ON m2.id = a.co_member_id
            JOIN ts_trading_schedules@hnxcore ts ON ts.id = trading_schedule_id
            JOIN mt_users@hnxcore u1 ON u1.id = broker_id
            JOIN mt_users@hnxcore u2 ON u2.id = co_broker_id
            JOIN allcode@hnxcore a1 ON a1.cdname=''TT_ORDERS'' and a1.cdtype=''SETTLEMENT_TYPE'' and a1.cdval = a.settlement_type
            JOIN allcode@hnxcore a2 ON a2.cdname=''TT_ORDERS'' and a2.cdtype=''AORI'' and a2.cdval = a.aori
            JOIN allcode@hnxcore a3 ON a3.cdname=''TT_ORDERS'' and a3.cdtype=''STATUS'' and a3.cdval = a.status
            JOIN allcode@hnxcore a4 ON a4.cdname=''TT_ORDERS'' and a4.cdtype=''NORO'' and a4.cdval = a.noro
            where norp = 2 and norc <> 5 and deleted = 0   ' || str || ' ORDER BY order_date,order_id) A';
    else --khop
        open p_cursor for 'SELECT AA.*,rownum as rowa from
            (SELECT order_id,order_date, a.order_time, member_id,co_member_id,aori,broker_id,co_broker_id,norc,norp,noro,oorb,
            t.name AS  stock_type, account_no, co_account_no,
            m.code_trade AS member, m.name  member_name, m.short_name  short_name,
           -- odt.code order_type_str, odtg.code org_order_type_str,
            com.code_trade  co_member, s.symbol symbol,  b.code  boardcode,
            ma.code  marketcode,
            a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,
           s.idschedule idschedule, tras.code  trading_schedules,
            nvl(org_order_no, order_no) org_order_no,
            mu.username broker,comu.username co_broker,
           a1.content  saori ,a2.content snorc, a3.content snoro, a4.content snorp,
            nvl(a.reject_qtty,0) reject_qtty,nvl(a.exec_qtty,0) exec_qtty,nvl(cancel_qtty,0)cancel_qtty,
            a.SPECIAL_SIDE, a.Co_SPECIAL_SIDE,nvl(ISRISK_MANAGE,0) ISRISK_MANAGE, nvl(IS_INDAY,''N'') IS_INDAY,MMLINK_ORDERNO
            FROM VIEW_TT_ORDERS_EXEC a
            JOIN ts_symbol s ON s.id = a.stock_id
             JOIN  ts_boards b on s.idboard = b.id
             JOIN ts_markets ma ON  ma.id = a.idmarket
             JOIN ts_members m ON  m.id = a.member_id and m.deleted = 0
             JOIN ts_members com ON  com.id = a.co_member_id and com.deleted = 0
             JOIN ts_securities_type t ON t.id = s.idsectype
             jOIN ts_trading_schedules tras ON tras.id = a.trading_schedule_id
             JOIN mt_users mu ON mu.id = a.broker_id
             JOIN mt_users comu ON comu.id = a.co_broker_id
             JOIN allcode_core a1 ON a1.cdname = ''TT_ORDERS'' and a1.cdtype=''AORI'' and a1.cdval = a.AORI
             JOIN allcode_core a2 ON a2.cdname = ''TT_ORDERS'' and a2.cdtype=''NORC'' and a2.cdval = a.NORC
            JOIN allcode_core a3 ON a3.cdname = ''TT_ORDERS'' and a3.cdtype=''NORO'' and a3.cdval = a.NORO
             JOIN allcode_core a4 ON a4.cdname = ''TT_ORDERS'' and a4.cdtype=''NORP'' and a4.cdval = a.NORP
            where ((norc=5 and norp = 1) or (norp=2 and norc =7 and a.status in (4,5,6))) and a.deleted = 0 ' || str || ' ORDER BY Order_date, order_id) AA ';

    END  IF ;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
END ;

--thong tin s? l?nh khop de huy KQGD
PROCEDURE PROC_STATUS_ORDER_GETS
(
    p_symbol in VARCHAR2,
    p_board in VARCHAR2,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for select A.*,rownum as rowa from
        (select * from
        (SELECT order_id,order_date, a.order_time, member_id,co_member_id,broker_id,co_broker_id,
        (select m.code_trade from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)member,   account_no, co_account_no,
        (select m.code_trade from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0)co_member,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
        a.order_no, a.co_order_no, a.order_confirm_no,a.order_qtty, a.order_price,trading_schedule_id,
        nvl((select idschedule from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id),0) idschedule,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='AORI' and cdval =aori)saori ,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        nvl(org_order_no, order_no) org_order_no,(select username from mt_users@hnxcore where id = broker_id and deleted =0) broker,
        (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_broker,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =to_char(norc))snorc,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro)snoro,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORP' and cdval = norp)snorp
        FROM VIEW_TT_ORDERS_EXEC a
        where ((norc=5 and norp = 1) or (norp=2 and norc =7 and status in (4,5,6))) and deleted = 0
        and (stock_id in (SELECT to_number(trim(regexp_substr(p_symbol, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_symbol, ',', 1, LEVEL - 1) > 0)
            or stock_id in (select id from ts_symbol@hnxcore where idboard in
                         (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and idschedule = 0))

        UNION

        SELECT order_id,order_date, b.order_time, member_id,co_member_id,broker_id,co_broker_id,
        (select m.code_trade from ts_members@hnxcore m where m.id = member_id and m.deleted = 0)member,   account_no, co_account_no,
        (select m.code_trade from ts_members@hnxcore m where m.id = co_member_id and m.deleted = 0)co_member,
        (select s.symbol from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id)symbol,
        (select b.code from ts_symbol@hnxcore s join ts_boards@hnxcore b on s.idboard=b.id  where s.deleted =0 and s.id = stock_id) boardcode,
        (select m.code from ts_markets@hnxcore m where m.deleted =0 and m.id = idmarket)marketcode,
        b.order_no, b.co_order_no, b.order_confirm_no,b.order_qtty, b.order_price,trading_schedule_id,
        nvl((select idschedule from ts_symbol@hnxcore s where s.deleted =0 and s.id = stock_id),0) idschedule,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='AORI' and cdval =aori)aori ,
        (select s.code from ts_trading_schedules@hnxcore s where s.deleted =0 and s.id = trading_schedule_id)trading_schedules,
        nvl(org_order_no, order_no) org_order_no,(select username from mt_users@hnxcore where id = broker_id and deleted =0) broker,
        (select username from mt_users@hnxcore where id = co_broker_id and deleted =0) co_broker,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORC' and cdval =to_char(norc))norc,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORO' and cdval =noro)noro,
        (select content from allcode@hnxcore where cdname = 'TT_ORDERS' and cdtype='NORP' and cdval = norp)norp
        FROM tt_orders_plus_tem b
        where norc=5 and norp = 1 and deleted = 0
        and (stock_id in (SELECT to_number(trim(regexp_substr(p_symbol, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_symbol, ',', 1, LEVEL - 1) > 0)
            or stock_id in (select id from ts_symbol@hnxcore where idboard in
                         (SELECT to_number(trim(regexp_substr(p_board, '[^,]+', 1, LEVEL))) FROM dual
                          CONNECT BY instr(p_board, ',', 1, LEVEL - 1) > 0) and idschedule = 0)))
        order by order_date, order_id)A;
          EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

--thong tin gia khop cuoi cung o so lenh theo chung khoan - tinh lai chi so
PROCEDURE PROC_SYMBOL_PRICE_GET
(
    p_type in number,
    p_index_id in number,
    p_cursor OUT tcursor
)
is
begin
    if(p_type = -1) then --Lay all
        open p_cursor for
          SELECT s.id as stock_id,s.symbol as code,s.idsectype as STOCK_TYPE, s.date_no,s.status,
         (select name from ts_securities_type where s.idsectype = id and deleted =0) as STOCK_TYPE_str,
         (select content from allcode@hnxcore where cdname='TS_SYMBOL' and cdtype='STATUS' and cdval=to_char(s.status)) as STATUS_str,
         (select content from allcode@hnxcore where cdname='TS_SYMBOL' and cdtype='LISTING_STATUS' and cdval=to_char(s.listing_status)) as LISTING_STATUS_str,
         s.listing_status,'Core' as sys,
          --gia khop gan nhat cua lenh thong thuong lo chan
           max(CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE ' ' END) index_price,

          --pkg_dr_core_info.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as index_price,

          --tong klgd kl / tong gtgd kl - l? ch?n
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN  order_qtty END),0) nm_total_traded_qtty,
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN order_qtty * order_price END),0) nm_total_traded_value,
          --tong klgd tt/ tong gtgd tt - l? ch?n
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty END ),0) pt_total_traded_qtty,
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty*tt.order_price END ),0) pt_total_traded_value,
          --tong kl dat mua th?ng thu?ng - lo chan
          nvl(sum(CASE WHEN noro = 1 AND  OORB = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
                    OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
                  WHEN noro = 1 AND OORB = 1 AND (norc = 8 and ORG_ORDER_BASE ='I') THEN (order_qtty - nvl(cancel_qtty,0)) END),0)
          - nvl(sum(case when noro = 1 AND  OORB = 1 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) as total_bid_qtty,
          -- tong kl dat mua th?ng thu?ng - lo chan
          nvl(sum(CASE WHEN noro = 1 AND OORB = 2 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
                    OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
                  WHEN noro = 1 AND OORB = 2 AND(norc = 8 and ORG_ORDER_BASE ='I') THEN (order_qtty - nvl(cancel_qtty,0)) END),0)
          - nvl(sum(case when noro = 1 AND OORB = 2 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) as total_offer_qtty
          from ts_symbol s left join view_tt_orders tt ON s.id = tt.stock_id and tt.deleted =0
          where s.deleted = 0 and s.status not in (6,9)
          GROUP BY s.id,s.symbol,s.idsectype,s.date_no,s.status, s.listing_status;
    else --chi lay nhung ck da giao dich vs gia cuoi cung
         open p_cursor for
         SELECT s.id as stock_id,s.symbol as code,
          --gia khop gan nhat cua lenh thong thuong lo chan
          --pkg_dr_core_info.func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as index_price,
           max(CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK LAST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE ' ' END) AS index_price,
          --gia khop DAU TIEN cua lenh thong thuong lo chan theo ck
          --pkg_dr_core_info.func_get_OrderPrice(-1,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as begin_idxprice,
           max(CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_price ELSE 0 END) KEEP (DENSE_RANK FIRST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE '99:99:99.999999' END) AS begin_idxprice,
          --
          --pkg_dr_core_info.func_get_OrderPrice(2,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id) as order_time,
          max(CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE '00:00:00:000000' END) KEEP (DENSE_RANK FIRST ORDER BY CASE WHEN NORC = 5 AND NORP = 1 AND noro = 1 THEN order_time ELSE '99:99:99.999999' END) AS order_time,

          --tong klgd kl / tong gtgd kl - l? ch?n
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN  order_qtty END),0) nm_total_traded_qtty,
          nvl(sum(CASE WHEN noro = 1 AND norc = 5 THEN order_qtty * order_price END),0) nm_total_traded_value,
          --tong klgd tt/ tong gtgd tt - l? ch?n
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty END ),0) pt_total_traded_qtty,
          nvl(SUM(CASE WHEN NORC = 7 and tt.STATUS in(4,5,6) AND NORO = 1 THEN order_qtty*tt.order_price END ),0) pt_total_traded_value,
          --tong kl dat mua th?ng thu?ng - lo chan
          nvl(sum(CASE WHEN noro = 1 AND  OORB = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S'))
                    OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN  order_qtty + nvl(correct_qtty,0)- nvl(cancel_qtty,0)
                  WHEN noro = 1 AND OORB = 1 AND (norc = 8 and ORG_ORDER_BASE ='I') THEN (order_qtty - nvl(cancel_qtty,0)) END),0)
          - nvl(sum(case when noro = 1 AND  OORB = 1 AND norc = 2 and ORG_ORDER_BASE ='I' then cancel_qtty end), 0) as total_bid_qtty
          from ts_symbol s join view_tt_orders tt ON s.id = tt.stock_id
          where tt.deleted = 0 and s.deleted = 0
          GROUP BY s.id,s.symbol;
   end if;
     EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

PROCEDURE proc_order_member
(
    p_member in VARCHAR2,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for select m.id, m.code_trade as member_code, nvl(sum (count_nm_order),0) as cmsgInNM,
    nvl(sum (count_pt_order),0) as cmsgInPT,nvl(sum(count_nm_traded),0) as cmsgMatchNM,
    nvl(sum(count_pt_traded),0) as cmsgMatchPT
    from ts_members m left join
    (select member_id as member_id,(select m.code_trade from ts_members m where m.id = member_id and m.deleted = 0)member_code,
        --so lenh dat thong thuong - lenh huy toan bo
        COUNT(CASE WHEN norp = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S' ) )
               OR (norc =8 and ORG_ORDER_BASE ='I') OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN order_id END)
        - COUNT(CASE WHEN norp = 1 AND((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S') and (order_qtty + correct_qtty  = cancel_qtty ) )
               OR (norc =8 and ORG_ORDER_BASE ='I' and (order_qtty + correct_qtty  = cancel_qtty )) ) THEN order_id END) as count_nm_order,
        --So lenh dat thoa thuan
        COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in (3,4,5,6) AND tt.deleted = 0 THEN order_id END) count_pt_order,
        --So lenh khop thong thuong
        COUNT (CASE WHEN norp = 1 AND norc = 5 THEN order_id END)  as count_nm_traded,
        --So lenh khop thoa thuan
        COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_id END) as count_pt_traded
    FROM  view_tt_orders tt where member_id <> 0  and oorb =2
    GROUP  BY  member_id
    UNION
    SELECT co_member_id as member_id,(select m.code from ts_members m where m.id = co_member_id and m.deleted = 0)member_code,
       --so lenh dat thong thuong - lenh huy toan bo
       COUNT(CASE WHEN norp = 1 AND ((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S' ) )
              OR (norc =8 and ORG_ORDER_BASE ='I') OR (norc = 10 and ORG_ORDER_BASE ='M')) THEN order_id END)
       - COUNT(CASE WHEN norp = 1 AND((norc = 2 AND (ORG_ORDER_BASE = 'L' or ORG_ORDER_BASE = 'S') and (order_qtty + correct_qtty  = cancel_qtty ) )
              OR (norc =8 and ORG_ORDER_BASE ='I' and (order_qtty + correct_qtty  = cancel_qtty )) ) THEN order_id END) as count_nm_order,
       --So lenh dat thoa thuan
       COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in (3,4,5,6) AND tt.deleted = 0 THEN order_id END) count_pt_order,
       --So lenh khop thong thuong
       COUNT (CASE WHEN norp = 1 AND norc = 5 THEN order_id END)  as count_nm_traded,
       --So lenh khop thoa thuan
       COUNT (CASE WHEN norp = 2 AND norc = 7 AND tt.status in(4,5,6) THEN order_id END) as count_pt_traded
    FROM view_tt_orders tt where co_member_id <> 0  and oorb =1
    GROUP by co_member_id ) on m.id = member_id
    WHERE m.deleted = 0 and m.status ='A' and m.code_trade like decode(p_member,'','%',p_member)
    GROUP BY m.id, m.code_trade
    ORDER BY m.code_trade asc;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
END ;

procedure proc_getlast_ordertime
(
    p_type in number,
    p_code in VARCHAR2,
    p_cursor OUT tcursor
)
is
begin
     if(p_type = -1) then --lay tg lon nhat trong tt_order_plus theo chi so cua etf
        open p_cursor for select distinct max(tt.order_time)order_time, max(tt.order_date) order_date
        from tt_orders_plus tt join ts_symbol@hnxcore ts on tt.stock_id = ts.id and ts.deleted = 0
        where tt.deleted = 0
        and ts.symbol in (select symbol_code from etf_pcf@hnxetf where etf_code = p_code);
    elsif(p_type=0) then --lay tg lon nhat trong tt_order_plus theo chi so
        open p_cursor for select distinct max(order_time)order_time, max(order_date) order_date
        from tt_orders_plus
        where deleted = 0
        and stock_id in (select bs.stock_id from idx_basket_info@hnxindex bs join idx_stocks_info@hnxindex st
            on bs.stock_id = st.stock_id where index_id = to_number(p_code) and is_calcindex = 1);
    else  --lay ngay min trong ts_operator_market
        open p_cursor for select distinct min(a.current_date) as working_date
        from ts_operator_markets a;
    end if;
      EXCEPTION  WHEN OTHERS  THEN
        RAISE ;
end;

FUNCTION func_get_OrderTypeCode(strId VARCHAR2) RETURN VARCHAR2
is
codes VARCHAR2(1000);
BEGIN
     select  rtrim (xmlagg (xmlelement (e, code || '|')).extract ('//text()'), ',') into codes
     from  ts_order_type@hnxcore
     where to_char(id) in (select regexp_substr(trim(strId),'[^|]+', 1, level) from dual
     connect by regexp_substr(trim(strId), '[^|]+', 1, level) is not null )
     and deleted =0;
     select utl_i18n.unescape_reference(codes) into codes from dual;
     DBMS_OUTPUT.PUT_LINE(codes);
     RETURN rtrim(codes ,'|');
    EXCEPTION
    when others then
        RAISE;

END;

--
FUNCTION func_get_OrderPrice(p_type number,strsql VARCHAR2) RETURN VARCHAR2
is
v_order_price number;
v_outval varchar2(500);
maxtime VARCHAR2(30);
mintime VARCHAR2(30);
BEGIN

     EXECUTE IMMEDIATE 'select nvl(max(order_time),'''') from view_tt_orders where deleted = 0 and ' || strsql ||'' into maxtime;
     if(p_type = 0) then --gia cuoi cung
        EXECUTE IMMEDIATE 'select nvl((select order_price from view_tt_orders
        where deleted = 0 and rownum = 1 and order_time = '''|| maxtime || '''and ' || strsql || '),0) from dual ' into v_order_price;
        v_outval := to_char(v_order_price) ;
     elsif (p_type = 1) then --kl
        EXECUTE IMMEDIATE 'select nvl((select order_qtty from view_tt_orders
        where deleted = 0 and rownum = 1 and order_time = '''|| maxtime || '''and ' || strsql || '),0) from dual ' into v_order_price;
        v_outval := to_char(v_order_price) ;
     elsif(p_type = -1) then  --gia dau tien (-1)
        EXECUTE IMMEDIATE 'select nvl(min(order_time),'''') from view_tt_orders where deleted = 0 and ' || strsql ||'' into mintime;
        --
        EXECUTE IMMEDIATE 'select nvl((select order_price from view_tt_orders
        where deleted = 0 and rownum = 1 and order_time = '''|| mintime || '''and ' || strsql || '),0) from dual ' into v_order_price;
        v_outval := to_char(v_order_price) ;
     else
        begin
            EXECUTE IMMEDIATE 'select nvl(min(order_time),''00:00:00:000000'') from view_tt_orders where deleted = 0 and ' || strsql ||'' into mintime;
            v_outval := mintime ;
            EXCEPTION
              WHEN OTHERS THEN
              v_outval :='00:00:00:000000';
        end;
     end if;
    RETURN v_outval;

END;
--

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_CORE_INFO

-- Start of DDL Script for Package STOCK_DR.PKG_DR_ETF
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_etf
  IS
  TYPE tcursor IS ref cursor;

procedure proc_etf_info
(
    p_code in VARCHAR2,
    p_cursor out tcursor
);


PROCEDURE proc_ETF_GetCbo
(
    p_type in number,
    p_cursor out tcursor
);

PROCEDURE proc_ETF_PCF_GETS
(
    p_etf_code in VARCHAR2,
    p_cursor out tcursor
);

procedure proc_etf_update
(
  p_etf_code IN VARCHAR2,
  p_time IN VARCHAR2,
  p_inav IN number ,
  p_datecaculator In date ,
  p_timecaculator in VARCHAR2
);

PROCEDURE proc_get_etf_main
(
    p_cursor OUT tcursor
);

procedure get_etf_status
(
    p_cursor OUT tcursor
);

procedure get_etf_inav
(
    p_etf_code IN VARCHAR2,
    p_rownum in number,
    p_cursor OUT tcursor
);
END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_etf
IS


procedure proc_etf_info
(
    p_code in VARCHAR2,
    p_cursor out tcursor
)
is
begin
    open p_cursor for
    select etf_code,etf_name,swaplot,e.status, a.content as status_str,indexcode,time_interval,e.type_caculator,
    aa.content as type_caculator_str,nav_value,te_weekly,trading_date,iscaculator,pcf_date,basic_price,celling_price,
    floor_price,close_price,is_cal_nav,is_update_symbol_price,total_units_outstanding,hnx_nav_value,change_units,
    e.option_rec_pcf,ab.content as option_rec_pcf_str,nvl((select inav from etf_inav@hnxetf where etf_code = e.etf_code
    and timecaculator = (select distinct max(timecaculator) from etf_inav@hnxetf where etf_code = e.etf_code)),0) as inav_value
    from etf_info@hnxetf e
    left join allcode@hnxetf a on e.status = a.cdval and a.cdname ='ETF_INFO' and a.cdtype='STATUS'
    left join allcode@hnxetf aa on e.type_caculator = aa.cdval and aa.cdname ='TYPE_CAL_INAV' and aa.cdtype='TYPE'
    left join allcode@hnxetf ab on e.option_rec_pcf = ab.cdval and ab.cdname ='ETF_INFO' and ab.cdtype='OPTION_REC_PCF'
    where deleted = 0 and status not in (6,9) and etf_code like decode(p_code,'','%',p_code)
    order by etf_code;
end;

PROCEDURE proc_ETF_GetCbo
(
    p_type in number,
    p_cursor out tcursor
)
is
begin
    if(p_type= 8) THEN
        open p_cursor for select distinct 0 as id,etf_code as code
        from etf_info@hnxetf where deleted = 0 and status not in (6,9)
        order by etf_code asc;
    end if;
end;


PROCEDURE proc_ETF_PCF_GETS
(
    p_etf_code in VARCHAR2,
    p_cursor out tcursor
)
is
begin
    open p_cursor for select  etf_code, symbol_code, symbol_amount,symbol_price,symbol_exchange,tradedate
    from etf_pcf@hnxetf where etf_code = p_etf_code order by etf_code,symbol_code;
end;

procedure proc_etf_update
(
  p_etf_code IN VARCHAR2,
  p_time IN VARCHAR2,
  p_inav IN number ,
  p_datecaculator In date ,
  p_timecaculator in VARCHAR2
)
is
    v_id number;
    v_seq number;
    v_timecal VARCHAR2(30);
    v_timeinterval number;
begin
    select nvl(time_interval,0) into v_timeinterval from etf_info@hnxetf where etf_code= p_etf_code;
    if(p_time is not null and p_time > '00:00:00:000000') then
        delete etf_inav@hnxetf where timecaculator >= p_time and etf_code= p_etf_code;
        v_timecal := p_time;
    else
        v_timecal := '09:00:00:000000';
    end if;
    --
    SELECT seq_Etf_Inav.nextval@hnxetf INTO v_id from dual;
    SELECT nvl(max(seqrecvetf),0)+ 1 INTO v_seq from etf_inav@hnxetf;
    INSERT INTO Etf_Inav@hnxetf(id, etf_code, time, inav , datecaculator,timecaculator ,seqrecvetf)
    VALUES (v_id,p_etf_code, to_char(p_datecaculator,'yyyymmdd') || '-' || v_timecal, ROUND(p_inav,6),p_datecaculator,v_timecal ,v_seq);
       COMMIT;
  EXCEPTION
  WHEN OTHERS THEN
  RAISE;
END;


PROCEDURE proc_get_etf_main
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
         SELECT  e.etf_code,e.swaplot,nvl(e.nav_value,0) Nav,  nvl((select inav from etf_inav@hnxetf
        where etf_code = e.etf_code and timecaculator = (select distinct max(timecaculator)
        from etf_inav@hnxetf where etf_code = e.etf_code)),0) as inav
        FROM etf_info@HNXETF e
        WHERE e.deleted = 0
        ORDER BY etf_code;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

procedure get_etf_status
(
    p_cursor OUT tcursor
)
is
begin
     open p_cursor for select trading_date as working_date,
     (select content from allcode where cdname='ETF_SYSTEM'
    and cdtype = 'STATUS' and cdval = c.status) as current_status
    from etf_operator@hnxetf c where is_workingday=1;
end;

procedure get_etf_inav
(
    p_etf_code IN VARCHAR2,
    p_rownum in number,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for  SELECT * FROM (
        SELECT ROWNUM AS r,a.*
        FROM
        (
            SELECT etf_code,timecaculator,inav
            FROM etf_inav@hnxetf
            WHERE etf_code = p_etf_code
            ORDER BY timecaculator asc
        )a
    ) WHERE r > p_rownum ;
end;
END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_ETF

-- Start of DDL Script for Package STOCK_DR.PKG_DR_ETF_INFO
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_etf_info
  IS
  TYPE tcursor IS ref cursor;

procedure proc_etf_info
(
    p_code in VARCHAR2,
    p_cursor out tcursor
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_etf_info
IS


procedure proc_etf_info
(
    p_code in VARCHAR2,
    p_cursor out tcursor
)
is
begin
    open p_cursor for
    select etf_code,etf_name,swaplot,e.status, a.content as status_str,indexcode,time_interval,e.type_caculator,
    aa.content as type_caculator_str,nav_value,te_weekly,trading_date,iscaculator,pcf_date,basic_price,celling_price,
    floor_price,close_price,is_cal_nav,is_update_symbol_price,total_units_outstanding,hnx_nav_value,change_units,
    e.option_rec_pcf,ab.content as option_rec_pcf_str,nvl((select inav from etf_inav where etf_code = e.etf_code
    and timecaculator = (select distinct max(timecaculator) from etf_inav where etf_code = e.etf_code)),0) as inav_value
    from etf_info@hnxetf e
    left join allcode@hnxetf a on e.status = a.cdval and a.cdname ='ETF_INFO' and a.cdtype='STATUS'
    left join allcode@hnxetf aa on e.type_caculator = aa.cdval and aa.cdname ='TYPE_CAL_INAV' and aa.cdtype='TYPE'
    left join allcode@hnxetf ab on e.option_rec_pcf = ab.cdval and ab.cdname ='ETF_INFO' and ab.cdtype='OPTION_REC_PCF'
    where deleted = 0 and status not in (6,9) and etf_code like decode(p_code,'','%',p_code)
    order by etf_code;
end;


END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_ETF_INFO

-- Start of DDL Script for Package STOCK_DR.PKG_DR_INDEX
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_index
  IS
TYPE tcursor IS REF CURSOR ;

procedure proc_IndexInfo_GetAll
(
    p_index in number,
    p_type in number,
    p_cursor out tcursor
);

procedure proc_IndexDetails_GetAll
(
    p_cursor out tcursor
);

procedure proc_StockIndex_GetAll
(
    p_code in number,
    p_board in number,
    p_index in number,
    p_cursor out tcursor
);

procedure proc_Index_GetCbo
(
    p_type in number,
    p_cursor out tcursor
);

PROCEDURE PROC_BASKET_GETBYINDEX
 (
 p_index_id in number,
 p_cursor OUT TCursor
 );

 PROCEDURE PROC_STOCK_INDEXNAME
 (
 p_index_id in number,
 p_cursor OUT TCursor
 );

 procedure proc_update_indexinfo
(
    p_index_id in number,
    p_floor_code in VARCHAR2,
    p_trading_date in date,
    p_total_stock in number,
    p_index_val in number,
    p_time in VARCHAR2,
    p_base_divide in number,
    p_traded_qtty in number,
    p_traded_value in number,
    p_prior_index_val in number,
    p_chg_index in number,
    p_pct_index in number,
    p_traded_qtty_pt in number,
    p_traded_value_pt in number,
    p_open_index in number,
    p_lowest_index in number,
    p_highest_index in number,
    p_tri_val in number,
    p_chg_tri in number,
    p_pct_tri in number,
    p_highest_tri in number,
    p_lowest_tri in number,
    p_open_tri in number,
    p_tri_code in VARCHAR2,
    p_dpi_code in VARCHAR2,
    p_ffmc in number
);

procedure proc_update_stock
(
    p_stock_id in number,
    p_code in VARCHAR2,
    p_session in number,
    p_index_price in number,
    p_total_offer_qtty in number,
    p_total_bid_qtty in number,
    p_nm_total_qtty in number,
    p_nm_total_value in number,
    p_pt_total_qtty in number,
    p_pt_total_value in number
);

PROCEDURE proc_get_indexinfo
(
    p_time in VARCHAR2,
    p_indexid in number,
    p_cursor OUT tcursor
);

PROCEDURE proc_get_index_main
(
    p_cursor OUT tcursor
);

PROCEDURE get_index_detail_main
(
    p_floor_code IN VARCHAR2,
    p_cursor OUT tcursor
);

PROCEDURE get_index_detail_row_main
(
    p_floor_code IN VARCHAR2,
    p_rownum IN NUMBER,
    p_cursor OUT tcursor
);

procedure get_index_status
(
    p_cursor OUT tcursor
);
END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_index
IS

procedure proc_IndexInfo_GetAll
(
    p_index in number,
    p_type in number,
    p_cursor out tcursor
)
is
    str VARCHAR2(5000);
begin
    str:= '';
    if(p_index > 0) then
        str := str || ' and idx.index_id =' || p_index;
    end if;
    if(p_type >= 0) then
        str := str || ' and idx.type_index =' || p_type;
    end if;
    --
    open p_cursor for
    'select index_id,floor_code,name,trading_date,total_stock,session_no,round(base_divide,15)base_divide,base_date,type_index,
    a.content as type_index_str,used_capped_ratio,method_cap,aa.content as method_cap_str, param_cap_id ,c.param_name as param_name ,
    used_free_float,round(prior_index_val,10)prior_index_val,round(chg_index,10)chg_index ,round(pct_index,10)pct_index,
    round(current_index,10)current_index,base_index,round(highest_index,10) highest_index,round(lowest_index,10)lowest_index,
    total_qtty,total_value,pt_total_qtty ,pt_total_value,boardcode,is_calculate_tri,round(tri_value,10)tri_value,
    round(prev_tri_value,10)prev_tri_value,round(highest_tri,10)highest_tri,round(lowest_tri,10)lowest_tri,
    round(change_tri,10)change_tri, round(pct_tri,10)pct_tri, basedate_tri,basepoint_tri,tri_code,is_calculate_dpi,
    round(dpi_value,10)dpi_value, round(ff_marketcap,15)ff_marketcap, round(prev_ff_marketcap,15)prev_ff_marketcap,
    round(prev_dpi,10)prev_dpi,basedate_dpi,basepoint_dpi, dpi_code, method_calulated,
    round(base_divide_fisher,10)base_divide_fisher,round(open_index,10) open_index, round(open_tri,10) open_tri,
    ''Index'' sys,ab.content as method_calcstr
    from idx_index_info@hnxindex idx
    left join allcode@hnxindex a on idx.type_index = a.cdval and a.cdname=''INDEX_INFO'' and a.cdtype=''IDX_INDEX_INFO''
    left join allcode@hnxindex aa on idx.method_cap = aa.cdval and aa.cdname=''METHOD_CALC''
    left join allcode@hnxindex ab on idx.method_calulated = ab.cdval and ab.cdname=''CAL_METHOD''
    left join idx_cap_ratio_param@hnxindex c on idx.param_cap_id = c.param_id
    where is_calculated = 1 and idx.deleted = 0' || str || ' order by floor_code asc';
end;

procedure proc_IndexDetails_GetAll
(
    p_cursor out tcursor
)
is
begin
    open p_cursor for select distinct index_id from idx_index_info_details@hnxindex
    where time != '00:00:00:000000';
end;

procedure proc_StockIndex_GetAll
(
    p_code in number,
    p_board in number,
    p_index in number,
    p_cursor out tcursor
)
is
    str VARCHAR2(5000);
begin
    str :='';
    if(p_code > 0) then
        str := str || ' and s.stock_id =' || p_code;
    end if;
    if(p_board > 0) then
        str := str || ' and s.board_id =' || p_board;
    end if;
     if(p_index > 0) then
        str := str || ' and s.stock_id in (select stock_id from idx_basket_info@hnxindex where index_id =' || p_index ||')';
    end if;
    open p_cursor for
    'select s.stock_id,s.code,s.name,s.trading_date,t.id as STOCK_TYPE, t.name as STOCK_TYPE_str,s.TOTAL_LISTING_QTTY,s.TOTAL_LIMIT_QTTY,s.FREE_FLOAT_RATE,s.BASIC_PRICE,s.CEILING_PRICE,
    s.FLOOR_PRICE,s.INDEX_PRICE,s.TOTAL_OFFER_QTTY,s.TOTAL_BID_QTTY,s.IS_CALCINDEX,s.DATE_NO,s.NM_TOTAL_TRADED_QTTY,s.NM_TOTAL_TRADED_VALUE,
    s.PT_TOTAL_TRADED_QTTY,s.PT_TOTAL_TRADED_VALUE,s.STATUS, a.content as STATUS_str,s.ORIGIN_FREE_FLOAT_RATE,s.LISTING_DATE,s.CANCEL_LISTING_DATE,s.TOTAL_REQ_QTTY,
    s.BOARD_ID, b.code as boardcode,s.LISTING_STATUS, aa.content as LISTING_STATUS_str,s.BEGIN_IDXPRICE, func_get_name_index@hnxindex(s.stock_id,0) indexcode,
    ''Index'' as sys
    from idx_stocks_info@hnxindex s
    left join idx_boards@hnxindex b on s.board_id = b.id and b.deleted = 0
    left join idx_stocks_type@hnxindex t on s.stock_type = t.id and t.deleted =0
    left join allcode@hnxindex a on a.cdval = s.status and a.cdname=''STATUS'' and a.cdtype=''STS_STOCKS_INFO''
    left join allcode@hnxindex aa on aa.cdval = s.LISTING_STATUS and aa.cdname=''LISTTING_STATUS'' and aa.cdtype=''STS_STOCKS_INFO''
    where s.deleted = 0 and s.stock_type=2 and s.status not in (6,9) and s.stock_exchange = ''HNX''
    ' || str || 'order by code asc';
end;


procedure proc_Index_GetCbo
(
    p_type in number,
    p_cursor out tcursor
)
is
begin
    if(p_type = 0) then
        open p_cursor for select id, code from idx_boards@hnxindex
        where deleted = 0 and status ='A'
        order by code asc;
    elsif(p_type = 2) then
        open p_cursor for select stock_id as id, code from idx_stocks_info@hnxindex
        where deleted = 0 and status not in (6,9) and stock_exchange = 'HNX'  and stock_type=2
        order by code asc;
    elsif(p_type = 5)  then
        open p_cursor for select index_id as id, floor_code as code
        from idx_index_info@hnxindex
        where deleted = 0 and is_calculated = 1 order by floor_code asc;
    elsif(p_type = 6) then
        open p_cursor for select cdval as id, content as code
        from allcode@hnxindex
        where cdname='INDEX_INFO' and cdtype='IDX_INDEX_INFO'
        order by cdval asc;
    elsif(p_type = 7) then
        open p_cursor for select id, name as code
        from idx_stocks_type@hnxindex
        where deleted = 0 and status='A'
        order by name asc;
    end if;
end;

 PROCEDURE PROC_BASKET_GETBYINDEX
 ( p_index_id in number,
 p_cursor OUT TCursor)
 IS

 BEGIN
    OPEN p_cursor FOR  SELECT b.stock_id,s.code,s.IS_CALCINDEX,s.money_dividend,
        b.index_id,round(b.TOTAL_CEILING_QTTY,15) TOTAL_CEILING_QTTY,s.nm_total_traded_qtty,
        s.nm_total_traded_value,s.pt_total_traded_qtty, s.pt_total_traded_value,
        decode(s.IS_CALCINDEX,1,s.begin_idxprice,s.index_price_backup) as index_price
        FROM idx_basket_info@hnxindex b  LEFT JOIN idx_stocks_info@hnxindex s  ON b.stock_id = s.stock_id
    WHERE b.index_id = p_index_id;

 END ;

--lay ten ch? s? theo ch?ng khoan////tam thoi chua dung ham nay
 PROCEDURE PROC_STOCK_INDEXNAME
 (
    p_index_id in number,
    p_cursor OUT TCursor
 )
 is
 begin
    if(p_index_id > 0) then
        open p_cursor for select s.stock_id, s.code,func_get_name_index@hnxindex(s.stock_id,0) indexcode
        from idx_stocks_info@hnxindex s
        where s.deleted = 0 and s.stock_id in (select stock_id from idx_basket_info@hnxindex where index_id = p_index_id);
    else
        open p_cursor for select s.stock_id, s.code,func_get_name_index@hnxindex(s.stock_id,0) indexcode
        from idx_stocks_info@hnxindex s
        where s.deleted = 0;
    end if;
 end;

 procedure proc_update_indexinfo
(
    p_index_id in number,
    p_floor_code in VARCHAR2,
    p_trading_date in date,
    p_total_stock in number,
    p_index_val in number,
    p_time in VARCHAR2,
    p_base_divide in number,
    p_traded_qtty in number,
    p_traded_value in number,
    p_prior_index_val in number,
    p_chg_index in number,
    p_pct_index in number,
    p_traded_qtty_pt in number,
    p_traded_value_pt in number,
    p_open_index in number,
    p_lowest_index in number,
    p_highest_index in number,
    p_tri_val in number,
    p_chg_tri in number,
    p_pct_tri in number,
    p_highest_tri in number,
    p_lowest_tri in number,
    p_open_tri in number,
    p_tri_code in VARCHAR2,
    p_dpi_code in VARCHAR2,
    p_ffmc in number
)
is
    p_base  number;
    p_prev_ffmc  number;
    p_session  number;
    p_highest number;
    p_lowest number;
    p_high_tri number;
    p_low_tri number;
    v_idboard varchar2(100);
begin
    --xu ly details truoc
    if(p_time is not null and p_time > '00:00:00:000000') then
        dbms_output.put_line('vao day roi');
        --xoa het lenh thua ben index di
        delete idx_index_info_details@hnxindex a where a.time >= p_time and a.index_id = p_index_id;
        --
        select base_index,session_no,prev_ff_marketcap into p_base, p_session,p_prev_ffmc
        from idx_index_info@hnxindex where deleted = 0 and index_id = p_index_id;
        --
        INSERT INTO idx_index_info_details@hnxindex (INDEX_DETAILS_ID,INDEX_ID,FLOOR_CODE,TRADING_DATE,TIME,TOTAL_STOCK,
                  ADVANCES,NOCHANGE,DECLINES,UP_VOLUME,NOCHANGE_VOLUME,DOWN_VOLUME,TOTAL_QTTY,TOTAL_VALUE,PRIOR_INDEX_VAL,
                  CHG_INDEX,PCT_INDEX,CURRENT_INDEX,BASE_INDEX,HIGHEST_INDEX,LOWEST_INDEX,BASE_DIVIDE,SESSION_NO,
                  PT_TOTAL_QTTY,PT_TOTAL_VALUE,open_index,TRI_VALUE ,CHANGE_TRI ,PCT_TRI ,HIGHEST_TRI ,LOWEST_TRI ,
                  OPEN_TRI,tri_code, dpi_code,close_tri,close_index,ff_marketcap,prev_ff_marketcap)
        VALUES (SEQ_IDX_INDEX_INFO_DETAILS.nextval@hnxindex,p_index_id,p_floor_code,p_trading_date,p_time,p_total_stock,0,NULL,NULL,
              NULL,NULL,NULL,p_traded_qtty,p_traded_value,p_prior_index_val,p_chg_index,p_pct_index,
              p_index_val,p_base,p_highest_index,p_lowest_index,p_base_divide,p_session,p_traded_qtty_pt,
              p_traded_value_pt,p_open_index,p_tri_val,p_chg_tri,p_pct_tri,p_highest_tri,p_lowest_tri,
              p_open_tri,p_tri_code, p_dpi_code,p_tri_val,p_index_val,p_ffmc,p_prev_ffmc);
    end if;
     --Xu ly update index_info sau
     select max(CURRENT_INDEX), min(CURRENT_INDEX) , max(TRI_VALUE), min(TRI_VALUE)
     into p_highest, p_lowest , p_high_tri,p_low_tri
     from idx_index_info_details@hnxindex
     where index_id = p_index_id;
     --
    update idx_index_info@hnxindex
        set CURRENT_INDEX = p_index_val,time = p_time ,TOTAL_QTTY = p_traded_qtty,ff_marketcap=p_ffmc,
        TOTAL_VALUE = p_traded_value,CHG_INDEX = p_chg_index,close_index=p_index_val,
        PCT_INDEX = p_pct_index,PT_TOTAL_QTTY = p_traded_qtty_pt,PT_TOTAL_VALUE= p_traded_value_pt,
        --open_index = p_open_index,
        lowest_index = p_lowest,highest_index = p_highest,
        TRI_VALUE = p_tri_val,CHANGE_TRI = p_chg_tri,PCT_TRI = p_pct_tri,close_tri = p_tri_val,
        HIGHEST_TRI = p_high_tri,LOWEST_TRI = p_low_tri,OPEN_TRI = p_open_tri
    where index_id = p_index_id;
    --Xu ly cap nhat thong tin cho core
    begin
        select  rtrim (xmlagg (xmlelement (e, id || ',')).extract ('//text()'), ',') into v_idboard
        from ts_boards@hnxcore where upper(name_index) = upper(p_floor_code) and deleted = 0 and status ='A';
        EXCEPTION
           WHEN OTHERS  THEN
           v_idboard := '-1';
    end;
    --
    --dbms_output.put_line(v_idboard);
    if(v_idboard <> '-1' and v_idboard is not null) then
    --dbms_output.put_line('vao day roi');
        update tt_boards_info@hnxcore set current_index = round(p_index_val,6),changeindex_point=round(p_chg_index,6),
        changeindex_percent = round(p_pct_index,6),prior_market_index=round(p_prior_index_val,6),market_index=round(p_index_val,6),
        highest_index=p_highest,lowest_index = p_lowest,prv_prior_market_index=round(p_prior_index_val,6)
        where idboard in (SELECT to_number(trim(regexp_substr(v_idboard, '[^,]+', 1, LEVEL))) FROM dual
                       CONNECT BY instr(v_idboard, ',', 1, LEVEL - 1) > 0);
    --dbms_output.put_line('update roi');
    end if;
    --
    commit;
    --dbms_output.put_line('commit roi');
    EXCEPTION
    WHEN OTHERS THEN
    ROLLBACK;
end;

PROCEDURE proc_get_indexinfo
(
    p_time in VARCHAR2,
    p_indexid in number,
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for select
    (select nvl(max(a.current_index),0) from idx_index_info_details@hnxindex a where a.time < p_time
    and index_id=p_indexid and a.time !='00:00:00:000000')as highest_index,
    (select nvl(min(a.current_index),0) from idx_index_info_details@hnxindex a where a.time < p_time
    and index_id=p_indexid and a.time !='00:00:00:000000')as lowest_index,
    (select nvl(max(a.tri_value),0) from idx_index_info_details@hnxindex a where a.time < p_time
    and index_id=p_indexid and a.time !='00:00:00:000000')as HIGHEST_TRI,
    (select nvl(min(a.tri_value),0) from idx_index_info_details@hnxindex a where a.time < p_time
    and index_id=p_indexid and a.time !='00:00:00:000000')as LOWEST_TRI
    from dual;
end;
procedure proc_update_stock
(
    p_stock_id in number,
    p_code in VARCHAR2,
    p_session in number,
    p_index_price in number,
    p_total_offer_qtty in number,
    p_total_bid_qtty in number,
    p_nm_total_qtty in number,
    p_nm_total_value in number,
    p_pt_total_qtty in number,
    p_pt_total_value in number
)
is
    v_beginidx_price number;
    v_object number;
begin
    update idx_stocks_info@hnxindex set date_no = p_session, 
    	index_price = case when nvl(p_index_price,0) <=0 then begin_idxprice else p_index_price end,
    	total_offer_qtty =p_total_offer_qtty,
    total_bid_qtty=p_total_bid_qtty,nm_total_traded_qtty = p_nm_total_qtty,nm_total_traded_value=p_nm_total_value,
    pt_total_traded_qtty = p_pt_total_qtty, pt_total_traded_value=p_pt_total_value
    where stock_id = p_stock_id;
    --lay bang/ck da huy kq giao dich
    select count(*) into v_object from dr_step_matching
    where step = pkg_dr_step_matching.step_cancel_allmatching and (object_id = p_stock_id
    or object_id = (select board_id from idx_stocks_info@hnxindex where stock_id = p_stock_id));
    if(v_object > 0) then
        --lay gia tinh cs dau ngay cua ck
        select begin_idxprice into v_beginidx_price from idx_stocks_info@hnxindex
        where deleted = 0 and stock_id = p_stock_id;
        --Cap nhat lai gia tcs dau ngay
        update idx_stocks_info@hnxindex set index_price = v_beginidx_price where stock_id = p_stock_id;
    end if;
end;

PROCEDURE proc_get_index_main
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
        SELECT floor_code,current_index,chg_index,pct_index
        FROM idx_index_info@HNXINDEX
        WHERE deleted = 0
        ORDER BY floor_code;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE get_index_detail_main
(
    p_floor_code IN VARCHAR2,
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
        SELECT floor_code,TIME,current_index
        FROM idx_index_info_details@HNXINDEX
        WHERE floor_code = p_floor_code
        ORDER BY index_details_id;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE get_index_detail_row_main
(
    p_floor_code IN VARCHAR2,
    p_rownum IN NUMBER,
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT * FROM (
        SELECT ROWNUM AS r,a.*
        FROM
        (
            SELECT floor_code,TIME,current_index
            FROM idx_index_info_details@HNXINDEX
            WHERE floor_code = p_floor_code
            ORDER BY index_details_id
        )a
    ) WHERE r > p_rownum ;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

procedure get_index_status
(
    p_cursor OUT tcursor
)
is
begin
    open p_cursor for select working_date, (select content from allcode@hnxindex where cdname='SYSTEM_STATUS'
    and cdtype = 'INDEX_SYSTEM' and cdval = c.current_status) as current_status
    from idx_calendar@hnxindex c where is_workingday=1 and deleted = 0;
end;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_INDEX

-- Start of DDL Script for Package STOCK_DR.PKG_DR_INDEX_INFO
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_index_info
  IS
TYPE tcursor IS REF CURSOR ;

procedure proc_Index_GetCbo
(
    p_type in number,
    p_cursor out tcursor
);

procedure proc_IndexInfo_GetAll
(
    p_index in number,
    p_type in number,
    p_cursor out tcursor
);

procedure proc_IndexDetails_GetAll
(
    p_cursor out tcursor
);

procedure proc_StockIndex_GetAll
(
    p_code in number,
    p_board in number,
    p_index in number,
    p_cursor out tcursor
);

PROCEDURE PROC_BASKET_GETBYINDEX
 (
 p_index_id in number,
 p_cursor OUT TCursor
 );

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_index_info
IS

procedure proc_Index_GetCbo
(
    p_type in number,
    p_cursor out tcursor
)
is
begin
    if(p_type = 0) then
        open p_cursor for select id, code from idx_boards@hnxindex
        where deleted = 0 and status ='A'
        order by code asc;
    elsif(p_type = 2) then
        open p_cursor for select stock_id as id, code from idx_stocks_info
        where deleted = 0 and status not in (6,9) and stock_exchange = 'HNX'  and stock_type=2
        order by code asc;
    elsif(p_type = 5)  then
        open p_cursor for select index_id as id, floor_code as code
        from idx_index_info
        where deleted = 0 and is_calculated = 1 order by floor_code asc;
    elsif(p_type = 6) then
        open p_cursor for select cdval as id, content as code
        from allcode@hnxindex
        where cdname='INDEX_INFO' and cdtype='IDX_INDEX_INFO'
        order by cdval asc;
    elsif(p_type = 7) then
        open p_cursor for select id, name as code
        from idx_stocks_type@hnxindex
        where deleted = 0 and status='A'
        order by name asc;
    end if;
end;

procedure proc_IndexInfo_GetAll
(
    p_index in number,
    p_type in number,
    p_cursor out tcursor
)
is
    str VARCHAR2(5000);
begin
    str:= '';
    if(p_index > 0) then
        str := str || ' and idx.index_id =' || p_index;
    end if;
    if(p_type >= 0) then
        str := str || ' and idx.type_index =' || p_type;
    end if;
    --
    open p_cursor for
    'select index_id,floor_code,name,trading_date,total_stock,session_no,round(base_divide,15)base_divide,base_date,type_index,
    a.content as type_index_str,used_capped_ratio,method_cap,aa.content as method_cap_str, param_cap_id ,c.param_name as param_name ,
    used_free_float,round(prior_index_val,10)prior_index_val,round(chg_index,10)chg_index ,round(pct_index,10)pct_index,
    round(current_index,10)current_index,base_index,round(highest_index,10) highest_index,round(lowest_index,10)lowest_index,
    total_qtty,total_value,pt_total_qtty ,pt_total_value,boardcode,is_calculate_tri,round(tri_value,10)tri_value,
    round(prev_tri_value,10)prev_tri_value,round(highest_tri,10)highest_tri,round(lowest_tri,10)lowest_tri,
    round(change_tri,10)change_tri, round(pct_tri,10)pct_tri, basedate_tri,basepoint_tri,tri_code,is_calculate_dpi,
    round(dpi_value,10)dpi_value, round(ff_marketcap,15)ff_marketcap, round(prev_ff_marketcap,15)prev_ff_marketcap,
    round(prev_dpi,10)prev_dpi,basedate_dpi,basepoint_dpi, dpi_code, method_calulated,
    round(base_divide_fisher,10)base_divide_fisher,round(open_index,10) open_index, round(open_tri,10) open_tri,
    ''Index'' sys,ab.content as method_calcstr
    from idx_index_info idx
    left join allcode@hnxindex a on idx.type_index = a.cdval and a.cdname=''INDEX_INFO'' and a.cdtype=''IDX_INDEX_INFO''
    left join allcode@hnxindex aa on idx.method_cap = aa.cdval and aa.cdname=''METHOD_CALC''
    left join allcode@hnxindex ab on idx.method_calulated = ab.cdval and ab.cdname=''CAL_METHOD''
    left join idx_cap_ratio_param@hnxindex c on idx.param_cap_id = c.param_id
    where is_calculated = 1 and idx.deleted = 0' || str || ' order by floor_code asc';
end;

procedure proc_IndexDetails_GetAll
(
    p_cursor out tcursor
)
is
begin
    open p_cursor for select distinct index_id from idx_index_info_details@hnxindex
    where time != '00:00:00:000000';
end;

procedure proc_StockIndex_GetAll
(
    p_code in number,
    p_board in number,
    p_index in number,
    p_cursor out tcursor
)
is
    str VARCHAR2(5000);
begin
    str :='';
    if(p_code > 0) then
        str := str || ' and s.stock_id =' || p_code;
    end if;
    if(p_board > 0) then
        str := str || ' and s.board_id =' || p_board;
    end if;
     if(p_index > 0) then
        str := str || ' and s.stock_id in (select stock_id from idx_basket_info@hnxindex where index_id =' || p_index ||')';
    end if;
    open p_cursor for
    'select s.stock_id,s.code,s.name,s.trading_date,t.id as STOCK_TYPE, t.name as STOCK_TYPE_str,s.TOTAL_LISTING_QTTY,s.TOTAL_LIMIT_QTTY,s.FREE_FLOAT_RATE,s.BASIC_PRICE,s.CEILING_PRICE,
    s.FLOOR_PRICE,s.INDEX_PRICE,s.TOTAL_OFFER_QTTY,s.TOTAL_BID_QTTY,s.IS_CALCINDEX,s.DATE_NO,s.NM_TOTAL_TRADED_QTTY,s.NM_TOTAL_TRADED_VALUE,
    s.PT_TOTAL_TRADED_QTTY,s.PT_TOTAL_TRADED_VALUE,s.STATUS, a.content as STATUS_str,s.ORIGIN_FREE_FLOAT_RATE,s.LISTING_DATE,s.CANCEL_LISTING_DATE,s.TOTAL_REQ_QTTY,
    s.BOARD_ID, b.code as boardcode,s.LISTING_STATUS, aa.content as LISTING_STATUS_str,s.BEGIN_IDXPRICE, func_get_name_index@hnxindex(s.stock_id,0) indexcode,
    ''Index'' as sys
    from idx_stocks_info s
    left join idx_boards@hnxindex b on s.board_id = b.id and b.deleted = 0
    left join idx_stocks_type@hnxindex t on s.stock_type = t.id and t.deleted =0
    left join allcode@hnxindex a on a.cdval = s.status and a.cdname=''STATUS'' and a.cdtype=''STS_STOCKS_INFO''
    left join allcode@hnxindex aa on aa.cdval = s.LISTING_STATUS and aa.cdname=''LISTTING_STATUS'' and aa.cdtype=''STS_STOCKS_INFO''
    where s.deleted = 0 and s.stock_type=2 and s.status not in (6,9) and s.stock_exchange = ''HNX''
    ' || str || 'order by code asc';
end;

PROCEDURE PROC_BASKET_GETBYINDEX
 ( p_index_id in number,
 p_cursor OUT TCursor)
 IS

 BEGIN
    OPEN p_cursor FOR  SELECT b.stock_id,s.code,s.IS_CALCINDEX,s.money_dividend,
        b.index_id,round(b.TOTAL_CEILING_QTTY,15) TOTAL_CEILING_QTTY,s.nm_total_traded_qtty,
        s.nm_total_traded_value,s.pt_total_traded_qtty, s.pt_total_traded_value,
        decode(s.IS_CALCINDEX,1,s.begin_idxprice,s.index_price_backup) as index_price
        FROM idx_basket_info@hnxindex b  LEFT JOIN idx_stocks_info s  ON b.stock_id = s.stock_id
    WHERE b.index_id = p_index_id;

 END ;


END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_INDEX_INFO

-- Start of DDL Script for Package STOCK_DR.PKG_DR_LOGFIX
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_logfix
  IS
type tcursor is ref cursor;

-- Enter package declarations as shown below
--Trader info
PROCEDURE proc_Trader_Info_GetAll
(
    p_member in VARCHAR2,
    p_cursor out tcursor
);

PROCEDURE proc_Trader_Info_Gets
(
    p_member in VARCHAR2,
    p_cursor out tcursor
);

PROCEDURE proc_Trader_GetCbo
(
    p_type in number,
    p_cursor out tcursor
);

PROCEDURE proc_get_logfix_main
(
    p_cursor OUT tcursor
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_logfix
IS
--
PROCEDURE proc_Trader_Info_GetAll
(
    p_member in VARCHAR2,
    p_cursor out tcursor
)
is
begin
    open p_cursor for SELECT AA.member_code, AA.msgSucLogIn,AA.msgSucLogOut,
        BB.msgIn,CC.msgOut, CC.msgReject, BB.msgOrdersNM -  CC.msgRejectNM AS msgInNM,
        BB.msgOrdersPT - CC.msgRejectPT AS msgInPT,CC.msgFirmNM,CC.msgFirmPT,
        CC.msgMatchNM,CC.msgMatchPT FROM
        (SELECT g.member_code,
        COUNT (CASE WHEN s.msgtype='A' and s.submsgtype ='A' and s.msgtypenavi = 'O' THEN s.secid END ) AS msgSucLogIn,
        COUNT (CASE WHEN s.msgtype='5' and s.submsgtype ='5' and s.msgtypenavi = 'O' THEN s.secid END ) AS msgSucLogOut
        FROM stp_securities_gateway@hnxstp g JOIN stp_msglog_session@hnxstp s ON s.secid=g.member_code
        WHERE g.status = 1  and member_code like decode('','','%','') group BY g.member_code) AA,
        (SELECT g.member_code, COUNT (CASE WHEN  o.msgtypenavi ='I' THEN o.secid END ) AS msgIn,
        COUNT (CASE WHEN  o.msgtypenavi ='I' and o.msgtype = 'D' THEN o.secid END ) AS msgOrdersNM,
         COUNT (CASE WHEN  o.msgtypenavi ='I' and o.msgtype = 's'  THEN o.secid END ) AS msgOrdersPT
        FROM stp_securities_gateway@hnxstp g
        JOIN  stp_msglog_orders@hnxstp o ON o.secid=g.member_code
        WHERE g.status = 1 and member_code like decode('','','%','') group BY g.member_code) BB,
        (SELECT g.member_code,
        COUNT (CASE WHEN r.msgtypenavi ='O' THEN r.secid END ) AS msgOut,
        COUNT (CASE WHEN r.msgtype ='3' THEN r.secid END ) AS msgReject,
        COUNT (CASE WHEN r.msgtype ='3' and submsgtype='D' AND errdef not like '-7%' THEN r.secid END ) AS msgRejectNM,
        COUNT (CASE WHEN r.msgtype ='3' and submsgtype='s' AND errdef not like '-7%' THEN r.secid END ) AS msgRejectPT,
        COUNT (CASE WHEN r.msgtype = '8' and r.msgcontent like '%150=0%' AND r.msgcontent like '%39=A%' THEN r.secid END ) AS msgFirmNM,
        COUNT (CASE WHEN  r.msgtype = 's' and r.msgcontent like '%549=1%' THEN r.secid END ) AS msgFirmPT,
        COUNT (CASE WHEN r.msgtype = '8' and r.msgcontent like '%150=3%' and r.msgcontent like '%39=2%' and r.msgcontent not like '%11=%'  THEN r.secid END ) AS msgMatchNM,
        COUNT (CASE WHEN r.msgtype = '8' and r.msgcontent like '%150=3%' and r.msgcontent like '%39=2%' and r.msgcontent like '%11=%' THEN r.secid END ) AS msgMatchPT
        FROM stp_securities_gateway@hnxstp g
        JOIN stp_msglog_reports@hnxstp r ON r.secid=g.member_code
        WHERE g.status = 1 and member_code like decode('','','%','') group BY g.member_code) CC
        WHERE AA.member_code = BB.member_code AND BB.member_code = CC.member_code
         order by AA.member_code asc;
end;

--S?a theo dung urd , lenh dat lay ca hop le va ko hop le/ theo bug 72766
PROCEDURE proc_Trader_Info_Gets
(
    p_member in VARCHAR2,
    p_cursor out tcursor
)
is
begin
    open p_cursor for select distinct g.member_code,g.name,
    (select count(*) from stp_msglog_session@hnxstp s where s.msgtype='A' and s.submsgtype ='A' and s.msgtypenavi = 'O' and s.traderid=g.name) msgSucLogIn,
    (select count(*) from stp_msglog_session@hnxstp s where s.msgtype='5' and s.submsgtype ='5' and s.msgtypenavi = 'O' and s.traderid=g.name ) msgSucLogOut,
    (select count(*) from stp_msglog_orders@hnxstp r where r.msgtypenavi ='I' and r.traderid=g.name) msgIn,
    (select count(*) from stp_msglog_reports@hnxstp r where r.msgtypenavi ='O' and r.traderid=g.name) msgOut,
    (select count(*) from stp_msglog_reports@hnxstp r where r.msgtype ='3' and r.traderid=g.name ) msgReject,
    (select count(*) from stp_msglog_orders@hnxstp o where o.msgtypenavi ='I' and o.msgtype = 'D' and o.traderid=g.name) as msgInNM,
    (select count(*) from stp_msglog_orders@hnxstp o where o.msgtypenavi ='I' and o.msgtype = 's' and o.traderid=g.name) as msgInPT,
   (select count(*) from stp_msglog_reports@hnxstp r where r.msgtype = '8' and r.msgcontent like '%150=0%'
    and r.msgcontent like '%39=A%' and r.traderid=g.name) as msgFirmNM,
   (select count(*) from stp_msglog_reports@hnxstp r where r.msgtype = 's' and r.msgcontent like '%549=1%'
     and r.traderid=g.name) as msgFirmPT,
    (select count(*) from stp_msglog_reports@hnxstp r where r.msgtype = '8' and r.msgcontent like '%150=3%'
     and r.msgcontent like '%39=2%' and r.msgcontent not like '%11=%' and r.traderid=g.name) as msgMatchNM,
    (select count(*) from stp_msglog_reports@hnxstp r where r.msgtype = '8' and r.msgcontent like '%150=3%'
     and r.msgcontent like '%39=2%' and r.msgcontent like '%11=%' and r.traderid=g.name) as msgMatchPT
    from stp_securities_gateway@hnxstp g
    where g.status = 1  and member_code like decode(p_member,'','%',p_member)
    order by g.member_code asc;
end;


PROCEDURE proc_Trader_GetCbo
(
    p_type in number,
    p_cursor out tcursor
)
is
begin
    if(p_type= 3) THEN
        open p_cursor for select distinct 0 as id,member_code as code
        from stp_securities_gateway@hnxstp where status = 1 order by member_code asc;
    end if;
END;

PROCEDURE proc_get_logfix_main
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR
    SELECT member_code, sum(Msg_In) Msg_In,SUM(Msg_Out) Msg_Out,SUM (Logins) Logins,SUM (LogOuts) LogOuts
    FROM (
         SELECT member_code,nvl(T_MSG_ORDERS,0) AS Msg_In,nvl(t_msg_reports,0) AS Msg_Out,logins AS Logins,logouts AS LogOuts
         FROM stp_secgateway_info@hnxstp
    )
    GROUP BY member_code
    ORDER BY member_code;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;
END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_LOGFIX

-- Start of DDL Script for Package STOCK_DR.PKG_DR_OPERATOR
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_operator
  IS

TYPE tcursor IS REF Cursor ;
status_step_start CONSTANT VARCHAR2(1) := '1';
status_step_info CONSTANT VARCHAR2(1) := '2';
status_step_endsession CONSTANT VARCHAR2(1) := '4';
status_step_result CONSTANT VARCHAR2(1) := '5';
status_step_end CONSTANT VARCHAR2(1) := '6';

PROCEDURE proc_update_operator
(
    p_current_date IN DATE,
    p_status_step IN dr_operator.status_step % type
);

PROCEDURE proc_get_operator
(
    p_cursor OUT tcursor
);

PROCEDURE proc_search_by_string
(
    p_sql in VARCHAR2,
    p_cursor out tcursor
);

PROCEDURE proc_get_step_matching
(
    p_cursor OUT tcursor
);

PROCEDURE proc_update_step_matching
(
    p_object_name IN dr_step_matching.object_name % TYPE,
    p_object_id IN dr_step_matching.object_id % TYPE,
    p_step IN dr_step_matching.step % TYPE
);

PROCEDURE proc_insert_step_matching
(
    p_object_name IN dr_step_matching.object_name % TYPE,
    p_object_id IN dr_step_matching.object_id % TYPE,
    p_type IN dr_step_matching.TYPE % TYPE,
    p_step IN dr_step_matching.step % TYPE
);

PROCEDURE get_step_byObject
(
    p_object_id IN dr_step_matching.object_id % TYPE,
    p_type IN dr_step_matching.TYPE % TYPE,
    p_cursor OUT tcursor
);

procedure proc_backup_core
(
    p_return out number
);

procedure proc_backup_index
(
    p_return out number
);

procedure proc_backup_etf
(
    p_return out number
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_operator
IS

PROCEDURE proc_update_operator
(
    p_current_date IN DATE,
    p_status_step IN dr_operator.status_step % type
)
IS
    v_return number;
    CURSOR  curIndex IS  SELECT  index_id, floor_code, trading_date,'15:00:00:000000', total_stock,
      advances, nochange, declines,up_volume, nochange_volume, down_volume, total_qtty,total_value, prior_index_val,
      chg_index, pct_index,current_index, base_index, highest_index, lowest_index,base_divide, session_no,
      open_index, close_index,pt_total_qtty, pt_total_value,ff_marketcap,prev_ff_marketcap, prev_tri_value,
      open_tri, close_tri,highest_tri, lowest_tri, change_tri, pct_tri,
      is_calculate_tri, tri_value, tri_code, dpi_code
      FROM idx_index_info@hnxindex where deleted=0 and is_calculated = 1;
BEGIN
    UPDATE dr_operator a SET a.current_date = trunc(p_current_date), a.status_step = p_status_step;
    --Neu la bat dau xu ly tien trinh thi  truncate cac bang dl di (1=>2)
    IF (p_status_step = status_step_info) THEN

        EXECUTE IMMEDIATE 'DELETE tt_orders_tem WHERE 1 = 1';
        EXECUTE IMMEDIATE 'DELETE tt_orders_plus_tem WHERE 1 = 1';
    --Neu ket thuc tien trinh thi chuyen tu 6=>1 va reset step matching
    ELSIF (p_status_step = status_step_start) THEN

        UPDATE  dr_step_matching set step = -1;
        --neu la chuyen sang tu ktp sang bao cao ket qua thi ms insert dl vao details (4=>5)
    ELSIF p_status_step = status_step_result THEN

        --Xoa,Insert lenh vao index_info_details vs time = 15:00:00:00000 lay tu idx_index_info (y/c 20151130)
        delete idx_index_info_details@hnxindex a where a.time = '15:00:00:000000';
        for recIndex in curIndex loop
            INSERT INTO idx_index_info_details@hnxindex(index_details_id, index_id, floor_code, trading_date,time, total_stock,
              advances, nochange, declines,up_volume, nochange_volume, down_volume, total_qtty,total_value, prior_index_val,
              chg_index, pct_index,current_index, base_index, highest_index, lowest_index,base_divide, session_no,
              open_index, close_index,pt_total_qtty, pt_total_value,ff_marketcap,prev_ff_marketcap, prev_tri_value,
              open_tri, close_tri,highest_tri, lowest_tri, change_tri, pct_tri,
              is_calculate_tri, tri_value, tri_code, dpi_code)
             VALUES ((SELECT nvl(max(index_details_id),0) + 1 FROM idx_index_info_details@hnxindex),recIndex.index_id,recIndex.floor_code,
              recIndex.trading_date, '15:00:00:000000', recIndex.total_stock, recIndex.advances, recIndex.nochange,
              recIndex.declines, recIndex.up_volume, recIndex.nochange_volume, recIndex.down_volume,recIndex.total_qtty,
              recIndex.total_value, recIndex.prior_index_val, recIndex.chg_index,recIndex.pct_index, recIndex.current_index,
              recIndex.base_index, recIndex.highest_index,recIndex.lowest_index, recIndex.base_divide, recIndex.session_no,
              recIndex.open_index,recIndex.close_index,recIndex.pt_total_qtty,recIndex.pt_total_value,recIndex.ff_marketcap,
              recIndex.prev_ff_marketcap, recIndex.prev_tri_value, recIndex.open_tri, recIndex.close_tri,
              recIndex.highest_tri, recIndex.lowest_tri, recIndex.change_tri, recIndex.pct_tri,
              recIndex.is_calculate_tri, recIndex.tri_value,recIndex.tri_code, recIndex.dpi_code);
        END LOOP;
    END IF;

    COMMIT;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_get_operator
(
    p_cursor OUT tcursor
)
IS
    v_count number;
    v_datecore date;
BEGIN
    select count(*) into v_count from dr_operator;
    select distinct min(a.current_date) into v_datecore from ts_operator_markets@hnxcore a;
    if(v_count = 0) then
        insert into dr_operator(current_date,status_step)
        values (trunc(v_datecore),status_step_start);
    end if;
    --
    OPEN p_cursor FOR
    SELECT a.*,(select content from allcode where cdname='OPERATOR' and cdtype='STATUS_STEP' and cdval = a.status_step) as sys_status
    FROM dr_operator a
    order by a.status_step asc;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_search_by_string
(
    p_sql in VARCHAR2,
    p_cursor out tcursor
)
is
BEGIN
    open p_cursor for p_sql;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_update_step_matching
(
    p_object_name IN dr_step_matching.object_name % TYPE,
    p_object_id IN dr_step_matching.object_id % TYPE,
    p_step IN dr_step_matching.step % TYPE
)
IS
BEGIN

    UPDATE dr_step_matching SET step = p_step
    WHERE object_name = p_object_name AND object_id = p_object_id;

COMMIT;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_get_step_matching
(
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR SELECT * FROM dr_step_matching;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE proc_insert_step_matching
(
    p_object_name IN dr_step_matching.object_name % TYPE,
    p_object_id IN dr_step_matching.object_id % TYPE,
    p_type IN dr_step_matching.TYPE % TYPE,
    p_step IN dr_step_matching.step % TYPE
)
IS
BEGIN

    INSERT INTO dr_step_matching(object_name,object_id,TYPE,step)
    VALUES(p_object_name,p_object_id,p_type, p_step);


COMMIT;
EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

PROCEDURE get_step_byObject
(
    p_object_id IN dr_step_matching.object_id % TYPE,
    p_type IN dr_step_matching.TYPE % TYPE,
    p_cursor OUT tcursor
)
IS
BEGIN
    OPEN p_cursor FOR SELECT * FROM dr_step_matching WHERE TYPE = p_type AND object_id = p_object_id;
EXCEPTION
WHEN OTHERS THEN
RAISE;
END;

procedure proc_backup_core
(
    p_return out number
)
IS
    v_return number;
BEGIN

    --1.1. Thong tin so lenh dat
    delete tt_orders;
    insert into tt_orders(order_id, org_order_id, floor_code, order_confirm_no,order_no, co_order_no, org_order_no, order_date,
       order_time, member_id, co_member_id, order_type,priority, oorb, norp, norc, bore, aori,
       settlement_type, dorf, order_qtty, order_price, status,quote_price, state, quote_time, quote_qtty, exec_qtty,
       correct_qtty, cancel_qtty, reject_qtty, reject_reason,account_no, co_account_no, broker_id, co_broker_id,
       deleted, date_created, date_modified, modified_by,created_by, telephone, org_order_base, settle_day,
       aorc, yieldmat, order_qtty_display, order_price_stop,clordid, noro, stock_id, sub_order_no, org_order_type,
       trading_schedule_id, member_code_adv, co_dorf,current_trading_schedule_id, co_sub_order_no,
       real_correct_qtty, idmarket, group_name, numseq_save)
    SELECT order_id, org_order_id, floor_code, order_confirm_no,order_no, co_order_no, org_order_no, order_date,
       order_time, member_id, co_member_id, order_type,priority, oorb, norp, norc, bore, aori,
       settlement_type, dorf, order_qtty, order_price, status,quote_price, state, quote_time, quote_qtty, exec_qtty,
       correct_qtty, cancel_qtty, reject_qtty, reject_reason,account_no, co_account_no, broker_id, co_broker_id,
       deleted, date_created, date_modified, modified_by,created_by, telephone, org_order_base, settle_day,
       aorc, yieldmat, order_qtty_display, order_price_stop,clordid, noro, stock_id, sub_order_no, org_order_type,
       trading_schedule_id, member_code_adv, co_dorf,current_trading_schedule_id, co_sub_order_no,
       real_correct_qtty, idmarket, group_name, numseq_save
    FROM tt_orders@hnxcore;

    --1.2. Thong tin so lenh khop
    delete tt_orders_plus;
    insert into tt_orders_plus (order_id, org_order_id, floor_code, order_confirm_no,order_no, co_order_no, org_order_no, order_date,
       order_time, member_id, co_member_id, order_type,priority, oorb, norp, norc, bore, aori,
       settlement_type, dorf, order_qtty, order_price, status,quote_price, state, quote_time, quote_qtty, exec_qtty,
       correct_qtty, cancel_qtty, reject_qtty, reject_reason,account_no, co_account_no, broker_id, co_broker_id,
       deleted, date_created, date_modified, modified_by,created_by, telephone, org_order_base, settle_day,
       aorc, yieldmat, order_qtty_display, order_price_stop,clordid, noro, stock_id, sub_order_no, org_order_type,
       trading_schedule_id, member_code_adv, co_dorf,current_trading_schedule_id, co_sub_order_no,
       real_correct_qtty, idmarket, group_name, numseq_save)
    SELECT order_id, org_order_id, floor_code, order_confirm_no,order_no, co_order_no, org_order_no, order_date,
       order_time, member_id, co_member_id, order_type,priority, oorb, norp, norc, bore, aori,
       settlement_type, dorf, order_qtty, order_price, status,quote_price, state, quote_time, quote_qtty, exec_qtty,
       correct_qtty, cancel_qtty, reject_qtty, reject_reason,account_no, co_account_no, broker_id, co_broker_id,
       deleted, date_created, date_modified, modified_by,created_by, telephone, org_order_base, settle_day,
       aorc, yieldmat, order_qtty_display, order_price_stop,clordid, noro, stock_id, sub_order_no, org_order_type,
       trading_schedule_id, member_code_adv, co_dorf,current_trading_schedule_id, co_sub_order_no,
       real_correct_qtty, idmarket, group_name, numseq_save
    FROM tt_orders_plus@hnxcore;

    --1.3. Thong tin van hanh
    delete ts_operator_markets;
    insert into ts_operator_markets(market_id, current_date, status_step, is_backup,is_send_close_price, is_send_basic_price)
    SELECT market_id, a.current_date, status_step, is_backup,is_send_close_price, is_send_basic_price
    FROM ts_operator_markets@hnxcore a;

    --1.4. Thong tin trading_calendar
    delete ts_trading_calendars;
    insert into ts_trading_calendars(id, boardid, working_day, name, is_beginday,is_endday, is_openmarket, is_closemarket, is_netting,
       is_reciveinfo, is_hasopenclose, current_status,session_num, is_workingday, next_date, previous_date,
       processrunning, notes, deleted, created_by,created_date, modified_by, modified_date,
       trading_schedule_id, pre_status_pause, last_action_time)
    SELECT id, boardid, working_day, name, is_beginday,is_endday, is_openmarket, is_closemarket, is_netting,
       is_reciveinfo, is_hasopenclose, current_status,session_num, is_workingday, next_date, previous_date,
       processrunning, notes, deleted, created_by,created_date, modified_by, modified_date,
       trading_schedule_id, pre_status_pause, last_action_time
    FROM ts_trading_calendars@hnxcore where is_workingday = 1 and deleted = 0;

    --1.5. Thong tin ck co giao dich rieng
    delete ts_symbol_calendar;
    insert into ts_symbol_calendar(id, symbol_id, current_status,current_trading_schedule_id, deleted,current_trading_state,
        pre_status_pause, last_action_time,first_trading_schedule_id, first_current_status)
    SELECT id, symbol_id, current_status,current_trading_schedule_id, deleted,current_trading_state,
        pre_status_pause, last_action_time,first_trading_schedule_id, first_current_status
    FROM ts_symbol_calendar@hnxcore where deleted = 0;

    --1.6. Thong tin chung khoan
    delete tt_symbol_info;
    insert into tt_symbol_info(id, symbol_date, symbol, open_price, close_price,avg_price, status, cdttg, kldttg, ckttg, klkttg,
       gtkttg, cdtt, kldtt, gtdtt, cktt, klktt, gtktt,cdtd, kldtd, cktd, klktd, gtktd, cdmg, kldmg,
       ckmg, klkmg, gtkmg, cdchan, kldchan, ckchan, klkchan, gtkchan, cdle, kldle, ckle, klkle, gtkle,
       id_symbol, action_time, max_price_execute,min_price_execute, max_price_buy, max_price_sell,
       max_qtty_buy, max_qtty_sell, date_no, trading_unit,adjust_qtty, adjust_rate, divident_rate, prior_price,
       prior_close_price, is_calcindex, is_determinecl,index_price, prev_prior_price, prev_prior_close_price,
       highest_price, lowest_price, total_offer_qtty,total_bid_qtty, match_qtty, match_value, pt_match_qtty,
       pt_match_price, current_price, current_qtty,excute_rdlot_price, excute_oddlot_price, klkchan_gannhat,
       klkle_gannhat, gtkchan_gannhat, gtkle_gannhat,max_price_excute_pt, min_price_excute_pt,
       buy_foreign_traded_qtty, sell_foreign_traded_qtty,buy_foreign_traded_value, sell_foreign_traded_value,
       buy_foreign_qtty_pt, buy_foreign_value_pt,sell_foreign_qtty_pt, sell_foreign_value_pt, nm_bid_count,
       nm_offer_count, pt_bid_count, pt_offer_count,total_pt_bid_qtty, total_pt_offer_qtty,
       od_max_price_excute_nm, od_min_price_excute_nm,od_max_price_excute_pt, od_min_price_excute_pt,
       od_total_traded_qtty_pt, od_total_traded_value_pt,match_price, nm_bid_count_od, nm_offer_count_od,
       total_bid_qtty_od, total_offer_qtty_od, seqrecvmw,idboard)
    SELECT id, symbol_date, symbol, open_price, close_price,avg_price, status, cdttg, kldttg, ckttg, klkttg,
       gtkttg, cdtt, kldtt, gtdtt, cktt, klktt, gtktt,cdtd, kldtd, cktd, klktd, gtktd, cdmg, kldmg,
       ckmg, klkmg, gtkmg, cdchan, kldchan, ckchan, klkchan, gtkchan, cdle, kldle, ckle, klkle, gtkle,
       id_symbol, action_time, max_price_execute,min_price_execute, max_price_buy, max_price_sell,
       max_qtty_buy, max_qtty_sell, date_no, trading_unit,adjust_qtty, adjust_rate, divident_rate, prior_price,
       prior_close_price, is_calcindex, is_determinecl,index_price, prev_prior_price, prev_prior_close_price,
       highest_price, lowest_price, total_offer_qtty,total_bid_qtty, match_qtty, match_value, pt_match_qtty,
       pt_match_price, current_price, current_qtty,excute_rdlot_price, excute_oddlot_price, klkchan_gannhat,
       klkle_gannhat, gtkchan_gannhat, gtkle_gannhat,max_price_excute_pt, min_price_excute_pt,
       buy_foreign_traded_qtty, sell_foreign_traded_qtty,buy_foreign_traded_value, sell_foreign_traded_value,
       buy_foreign_qtty_pt, buy_foreign_value_pt,sell_foreign_qtty_pt, sell_foreign_value_pt, nm_bid_count,
       nm_offer_count, pt_bid_count, pt_offer_count,total_pt_bid_qtty, total_pt_offer_qtty,
       od_max_price_excute_nm, od_min_price_excute_nm,od_max_price_excute_pt, od_min_price_excute_pt,
       od_total_traded_qtty_pt, od_total_traded_value_pt,match_price, nm_bid_count_od, nm_offer_count_od,
       total_bid_qtty_od, total_offer_qtty_od, seqrecvmw,idboard
    FROM tt_symbol_info@hnxcore;

    --1.7. Thong tin chung khoan
    delete ts_symbol;
    insert into ts_symbol(id, symbol, isincode, name, idsectype, idissuer,idindustry, idboard, dvrtype, idtradingrule, parvalue,
       issueqtty, freefloat, fundqtty, listingqtty,listingdate, delistingdate, issuedate, maturitydate,
       interestrate, prevpaydate, nextpaydate, terms,statedcoupon, frequency, expmonth, contractsize,
       settmultiplier, exerciseprice, righttype, nearspread,farspread, adjusttype, lasttradingday, settmethod,
       random_end, prolong_allow, notes, deleted, created_by, created_date, modified_by, modified_date, celling_price,
       floor_price, basic_price, status, idschedule,match_price, totalreq, open_price, close_price,
       listing_status, reference_status, idcalendar, flag_25,prev_basic_price, about_delistingdate, date_no,
       prior_close_price, prior_open_price)
    SELECT id, symbol, isincode, name, idsectype, idissuer,idindustry, idboard, dvrtype, idtradingrule, parvalue,
       issueqtty, freefloat, fundqtty, listingqtty,listingdate, delistingdate, issuedate, maturitydate,
       interestrate, prevpaydate, nextpaydate, terms,statedcoupon, frequency, expmonth, contractsize,
       settmultiplier, exerciseprice, righttype, nearspread,farspread, adjusttype, lasttradingday, settmethod,
       random_end, prolong_allow, notes, deleted, created_by, created_date, modified_by, modified_date, celling_price,
       floor_price, basic_price, status, idschedule,match_price, totalreq, open_price, close_price,
       listing_status, reference_status, idcalendar, flag_25,prev_basic_price, about_delistingdate, date_no,
       prior_close_price, prior_open_price
   FROM ts_symbol@hnxcore where deleted = 0;
   --1.8. Thong tin loai chung khoan
   delete ts_securities_type;
   insert into ts_securities_type(id, name, short_name, type, idboard, idtradingrule, status, notes, deleted, created_by, created_date,
       modified_by, modified_date, is_check_room)
   SELECT id, name, short_name, type, idboard, idtradingrule, status, notes, deleted, created_by, created_date,
       modified_by, modified_date, 0 is_check_room -- thu sua
   FROM ts_securities_type@hnxcore where deleted = 0;

  -- 2108 them mot so bang lay lai tu core sang de view bao cao cho no nhanh
  delete ALLCODE_CORE;
  INSERT INTO allcode_core (CDNAME,CDTYPE,CDVAL,CONTENT,LSTODR)
  SELECT CDNAME,CDTYPE,CDVAL,CONTENT,LSTODR FROM allcode@hnxcore;

  ---   MT_USERS
  DELETE MT_USERS;
  INSERT INTO MT_USERS (ID,TRADERID,USERNAME,FULLNAME,TYPE,PASSWORD,IP_ADDRESS,
  GROUP_ID,STATUS,LASTLOGIN,ONLINESTS,NOTES,DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,
  MODIFIED_DATE,IS_RESET_PASS,LAST_UPDATE_PASS,ACTIVE_DATE,EXPIRE_DATE,IS_RE_SET_RIGHT,
  MACHINE_LOGIN)
  SELECT ID,TRADERID,USERNAME,FULLNAME,TYPE,PASSWORD,IP_ADDRESS,
  GROUP_ID,STATUS,LASTLOGIN,ONLINESTS,NOTES,DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,
  MODIFIED_DATE,IS_RESET_PASS,LAST_UPDATE_PASS,ACTIVE_DATE,EXPIRE_DATE,IS_RE_SET_RIGHT,
  MACHINE_LOGIN FROM MT_USERS@hnxcore;

  ---   TS_TRADING_SCHEDULES
  DELETE TS_TRADING_SCHEDULES;
  INSERT INTO TS_TRADING_SCHEDULES (ID,NAME,CODE,START_TIME,FINISH_TIME,IDSCHDRULE,IDSTATE,AORM,
  SESSION_NO,STATUS,EXECUTED_BY,NOTES,DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,MODIFIED_DATE)
  SELECT ID,NAME,CODE,START_TIME,FINISH_TIME,IDSCHDRULE,IDSTATE,AORM,
  SESSION_NO,STATUS,EXECUTED_BY,NOTES,DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,MODIFIED_DATE
  FROM TS_TRADING_SCHEDULES@hnxcore;

  ---   TS_MEMBERS
  DELETE TS_MEMBERS;
  INSERT INTO TS_MEMBERS (ID,NAME,SHORT_NAME,CODE,CODE_TRADE,TYPE,DORF_FLAG,BORF_FLAG,
  ADDRESS,PHONE,FAX,TELEX,CAPITAL,CAPITAL_RULE,STATUS,DELETED,CREATE_BY,CREATE_DATE,
  MODIFIED_BY,MODIFIED_DATE)
  SELECT ID,NAME,SHORT_NAME,CODE,CODE_TRADE,TYPE,DORF_FLAG,BORF_FLAG,
  ADDRESS,PHONE,FAX,TELEX,CAPITAL,CAPITAL_RULE,STATUS,DELETED,CREATE_BY,CREATE_DATE,
  MODIFIED_BY,MODIFIED_DATE FROM TS_MEMBERS@hnxcore;

  ---   TS_MARKETS
  DELETE TS_MARKETS;
  INSERT INTO TS_MARKETS (ID,CODE,NAME,IDEXCHANGE,IDCBRULE,STATUS,IDCALENDAR,NOTE,
  DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,MODIFIED_DATE,NUM_TRADING)
  SELECT ID,CODE,NAME,IDEXCHANGE,IDCBRULE,STATUS,IDCALENDAR,NOTE,
  DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,MODIFIED_DATE,NUM_TRADING FROM TS_MARKETS@hnxcore;

  ---   TS_BOARDS
  DELETE TS_BOARDS;
  INSERT INTO TS_BOARDS (ID,NAME,SHORT_NAME,CODE,IDMARKET,IDCBRULE,STATUS,IDSCHEDULE,
  IDTRADINGRULE,FLOOR_ALLOW,REMOTE_ALLOW,ONLINE_ALLOW,PROLONG_ALLOW,RANDOM_END,DELETED,
  CREATE_BY,CREATE_DATE,MODIFIED_BY,MODIFIED_DATE,PROLONG_MATCH,PROLONG_TIME,NUM_NON_TRADING
  ,NAME_INDEX,DEF_TRADINGRULE_25,DEF_TRADINGRULE_NYM,DEF_TRADINGRULE_SAP_HUY_NY,
  DEF_SCHEDULE_RULE_25,DEF_SCHEDULE_RULE_NYM,DEF_SCHEDULE_RULE_SAP_HUY_NY,DATE_NO)
  SELECT ID,NAME,SHORT_NAME,CODE,IDMARKET,IDCBRULE,STATUS,IDSCHEDULE,
  IDTRADINGRULE,FLOOR_ALLOW,REMOTE_ALLOW,ONLINE_ALLOW,PROLONG_ALLOW,RANDOM_END,DELETED,
  CREATE_BY,CREATE_DATE,MODIFIED_BY,MODIFIED_DATE,PROLONG_MATCH,PROLONG_TIME,NUM_NON_TRADING
  ,NAME_INDEX,DEF_TRADINGRULE_25,DEF_TRADINGRULE_NYM,DEF_TRADINGRULE_SAP_HUY_NY,
  DEF_SCHEDULE_RULE_25,DEF_SCHEDULE_RULE_NYM,DEF_SCHEDULE_RULE_SAP_HUY_NY,DATE_NO
  FROM TS_BOARDS@hnxcore;

  ---   TS_ORDER_TYPE
  DELETE TS_ORDER_TYPE;
  INSERT INTO TS_ORDER_TYPE (ID,CODE,NAME,INPUTCODE,BASE,PRIORITY,IS_CONVERT,CONVERT_COND,PRICE_REQ,
  PRICECALC,STOPCOND,DEFCANCCOND,DEFTIMECOND,DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,
  MODIFIED_DATE,ODCORRALLOW,ODCANCALLOW,CONVERT_TYPE,DISPLAYQTTYMIN)
  SELECT ID,CODE,NAME,INPUTCODE,BASE,PRIORITY,IS_CONVERT,CONVERT_COND,PRICE_REQ,
  PRICECALC,STOPCOND,DEFCANCCOND,DEFTIMECOND,DELETED,CREATED_BY,CREATED_DATE,MODIFIED_BY,
  MODIFIED_DATE,ODCORRALLOW,ODCANCALLOW,CONVERT_TYPE,DISPLAYQTTYMIN
  FROM TS_ORDER_TYPE@hnxcore;


  ---   TS_FOREIGN_ROOM
  DELETE TS_FOREIGN_ROOM;
  INSERT INTO TS_FOREIGN_ROOM (SYMBOL,FOREIGNRATE,DELETED,CREATED_BY,CREATED_DATE,
  MODIFIED_BY,MODIFIED_DATE,CURRENT_ROOM,BUY_MATCH,SELL_MATCH,ISINCODE,TOTAL_ROOM,FRGROOM_DATE)
  SELECT SYMBOL,FOREIGNRATE,DELETED,CREATED_BY,CREATED_DATE,
  MODIFIED_BY,MODIFIED_DATE,CURRENT_ROOM,BUY_MATCH,SELL_MATCH,ISINCODE,TOTAL_ROOM,FRGROOM_DATE
  FROM TS_FOREIGN_ROOM@hnxcore;




    commit;

    p_return := 1;

EXCEPTION
WHEN OTHERS THEN
     ROLLBACK ;
     pkg_log.log_error('proc_backup_core' || ': ' || SQLCODE || SQLERRM , 'proc_backup_core');
     p_return := -1;
     RAISE;
end;

procedure proc_backup_index
(
    p_return out number
)
IS
    v_return number;
BEGIN

    --1.1. Thong tin ch? s?
    DELETE idx_index_info;
    insert into idx_index_info(index_id, short_name, name, description, floor_code,trading_date, time, total_stock, advances, nochange,
       declines, up_volume, nochange_volume, down_volume,total_qtty, total_value, prior_index_val, chg_index,
       pct_index, current_index, base_index, highest_index,lowest_index, deleted, base_divide, session_no,
       date_created, date_modified, modified_by, created_by,type_index, is_calculated, type_calculated,
       time_interval, used_free_float, used_capped_ratio,method_calulated, industry_id, open_index, close_index,
       param_cap_id, param_cap_key, method_cap, base_date,base_divide_fisher, pt_total_qtty, pt_total_value,
       boardcode, is_calculate_tri, tri_value, return_one_day,return_mtd, return_qtd, return_ytd, is_calculate_dpi,
       dpi_value, ff_marketcap, prev_ff_marketcap,prev_tri_value, open_tri, close_tri, highest_tri,
       lowest_tri, change_tri, pct_tri, prev_dpi,basedate_tri, basepoint_tri, basedate_dpi, basepoint_dpi,
       return_one_day_pri, return_mtd_pri, return_qtd_pri,return_ytd_pri, notraded, up_ceiling, down_floor,
       is_scale_idx_normal, tri_code, dpi_code,annualized_1y_pri, annualized_3y_pri, annualized_5y_pri,
       annualized_1y_tri, annualized_3y_tri, annualized_5y_tri)
    SELECT index_id, short_name, name, description, floor_code,trading_date, time, total_stock, advances, nochange,
       declines, up_volume, nochange_volume, down_volume,total_qtty, total_value, prior_index_val, chg_index,
       pct_index, current_index, base_index, highest_index,lowest_index, deleted, base_divide, session_no,
       date_created, date_modified, modified_by, created_by,type_index, is_calculated, type_calculated,
       time_interval, used_free_float, used_capped_ratio,method_calulated, industry_id, open_index, close_index,
       param_cap_id, param_cap_key, method_cap, base_date,base_divide_fisher, pt_total_qtty, pt_total_value,
       boardcode, is_calculate_tri, tri_value, return_one_day,return_mtd, return_qtd, return_ytd, is_calculate_dpi,
       dpi_value, ff_marketcap, prev_ff_marketcap,prev_tri_value, open_tri, close_tri, highest_tri,
       lowest_tri, change_tri, pct_tri, prev_dpi,basedate_tri, basepoint_tri, basedate_dpi, basepoint_dpi,
       return_one_day_pri, return_mtd_pri, return_qtd_pri,return_ytd_pri, notraded, up_ceiling, down_floor,
       is_scale_idx_normal, tri_code, dpi_code,annualized_1y_pri, annualized_3y_pri, annualized_5y_pri,
       annualized_1y_tri, annualized_3y_tri, annualized_5y_tri
    FROM idx_index_info@hnxindex where deleted = 0 and is_calculated = 1;

    --1.2. Thong tin chung khoan
    DELETE idx_stocks_info;
    insert into idx_stocks_info(stock_id, trading_date, time, code, stock_type,total_listing_qtty, total_limit_qtty, adjust_qtty,
       adjust_limit_qtty, free_float_rate, basic_price,ceiling_price, floor_price, open_price, close_price,
       average_price, index_price, total_offer_qtty,total_bid_qtty, prior_price, prior_close_price, name,
       parvalue, floor_code, is_calcindex, date_no,nm_total_traded_qtty, nm_total_traded_value,
       pt_total_traded_qtty, pt_total_traded_value, deleted,date_created, date_modified, modified_by, created_by,
       status, flag, origin_free_float_rate, industry_code,prev_index_price, listing_date, cancel_listing_date,
       index_price_change, prev_basic_price, total_req_qtty,recive_index_price, recive_basic_price, index_price_backup,
       board_id, flag_25, stock_exchange, listing_status,money_dividend, hsx_idstocks, reference_status,
       free_float_rate_old, total_req_qtty_core, begin_idxprice)
    SELECT stock_id, trading_date, time, code, stock_type,total_listing_qtty, total_limit_qtty, adjust_qtty,
       adjust_limit_qtty, free_float_rate, basic_price,ceiling_price, floor_price, open_price, close_price,
       average_price, index_price, total_offer_qtty,total_bid_qtty, prior_price, prior_close_price, name,
       parvalue, floor_code, is_calcindex, date_no,nm_total_traded_qtty, nm_total_traded_value,
       pt_total_traded_qtty, pt_total_traded_value, deleted,date_created, date_modified, modified_by, created_by,
       status, flag, origin_free_float_rate, industry_code,prev_index_price, listing_date, cancel_listing_date,
       index_price_change, prev_basic_price, total_req_qtty,recive_index_price, recive_basic_price, index_price_backup,
       board_id, flag_25, stock_exchange, listing_status,money_dividend, hsx_idstocks, reference_status,
       free_float_rate_old, total_req_qtty_core, begin_idxprice
    FROM idx_stocks_info@hnxindex where deleted = 0;
    --

    COMMIT;
    p_return := 1;

EXCEPTION
WHEN OTHERS THEN
    ROLLBACK ;
    pkg_log.log_error('proc_backup_index' || ': ' || SQLCODE || SQLERRM , 'proc_backup_index');
    p_return := -1;
    RAISE;
END;

procedure proc_backup_etf
(
    p_return out number
)
IS
    v_return number;
BEGIN

    --1.Thong tin inav
    delete etf_inav;

    INSERT INTO etf_inav( id, etf_code, time, inav, datecaculator,timecaculator, seqrecvetf)
    SELECT id, etf_code, time, inav, datecaculator,timecaculator, seqrecvetf
    FROM etf_inav@hnxetf
    --
    COMMIT ;
    p_return := 1;

EXCEPTION
WHEN OTHERS THEN
    ROLLBACK ;
    pkg_log.log_error('proc_backup_etf' || ': ' || SQLCODE || SQLERRM , 'proc_backup_etf');
    p_return := -1;
    RAISE;
END;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_OPERATOR

-- Start of DDL Script for Package STOCK_DR.PKG_DR_STEP_MATCHING
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_dr_step_matching
  IS
type tcursor is ref cursor;

step_nothing CONSTANT NUMBER(1) := -1;
step_backup CONSTANT NUMBER(1) := 0;
step_cancel_matching CONSTANT NUMBER(1) := 1;
step_accept_matching CONSTANT NUMBER(1) := 2;
step_accept_newmatching CONSTANT NUMBER(1) := 3;
step_cancel_allmatching CONSTANT NUMBER(1) := 4;
step_end_session CONSTANT NUMBER(1) := 7;

PROCEDURE PROC_STEPMATCHING_INSERT
(p_return out number);


END;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_dr_step_matching
IS

PROCEDURE PROC_STEPMATCHING_INSERT
(p_return out number)
IS
    cursor cur_info is
    select * from
    (select b.id as idboard, b.code as brcode,0 as idsymbol, '-' symbol
        from ts_boards@hnxcore  b left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted=0
        left join ts_operator_markets@hnxcore op on m.id = op.market_id
        left join ts_trading_calendars@hnxcore tc on b.id = tc.boardid and tc.is_workingday=1
        left join ts_trading_schedules@hnxcore tsc on tc.trading_schedule_id = tsc.id
        where b.deleted=0
    union
    select nvl(b.id,0) as idboard,nvl(b.code,'-') as brcode,s.id as idsymbol,s.symbol
        from ts_symbol@hnxcore s left join ts_boards@hnxcore b on s.idboard = b.id and b.deleted=0
        left join ts_securities_type@hnxcore st on s.idsectype = st.id and st.deleted = 0
        left join ts_markets@hnxcore m on b.idmarket = m.id and m.deleted = 0
        left join ts_operator_markets@hnxcore op on m.id = op.market_id
        left join ts_symbol_calendar@hnxcore sc on s.id = sc.symbol_id and sc.deleted=0
        left join ts_trading_schedules@hnxcore tsc on sc.current_trading_schedule_id = tsc.id
        where s.deleted=0 and s.status not in (6,9) and s.idschedule > 0)A
    order by  idboard, idsymbol;

    v_count number;
    v_step NUMBER(2);
BEGIN
      -- xoa bang luu action thuc hien di
    --Kiem tra neu he thong o buoc 1 + 2 thi xoa s?ch bang nay di insert lai
    SELECT status_step INTO v_step FROM  dr_operator WHERE ROWNUM = 1;
    IF v_step < 3 THEN
        DELETE dr_step_matching;
    END IF ;

    FOR recInfo in cur_info LOOP
        if(recInfo.idsymbol > 0) then
            select count(*) into v_count from dr_step_matching where upper(object_name) = upper(recInfo.symbol)
            and object_id = recInfo.idsymbol;
            if(v_count = 0) then
                insert into dr_step_matching(object_name, object_id, type, step)
                values(upper(recInfo.symbol),recInfo.idsymbol,pkg_dr_core.type_symbol,-1);
            END IF ;
        ELSE
            select count(*) into v_count from dr_step_matching where upper(object_name) = upper(recInfo.brcode)
            and object_id = recInfo.idboard;
            if(v_count = 0) then
                insert into dr_step_matching(object_name, object_id, type, step)
                values(upper(recInfo.brcode),recInfo.idboard,pkg_dr_core.type_board,-1);
            end if;
        end if;
    end loop;

    p_return :=1;
    EXCEPTION
    when others then
        p_return := -1;
    raise;
end;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_DR_STEP_MATCHING

-- Start of DDL Script for Package STOCK_DR.PKG_FUNCTION
-- Generated 6/20/2019 8:38:18 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_function
  IS
  type tcursor is ref cursor;

PROCEDURE proc_function_getall
(
  p_cursor out tcursor
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_function
IS

PROCEDURE proc_function_getall
(
  p_cursor out tcursor
)
is
begin
    open p_cursor for SELECT a.funcid, a.funcname, a.funccontent, a.funcparent, a.funcimage,a.functag
    FROM function a;
end;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_FUNCTION

-- Start of DDL Script for Package STOCK_DR.PKG_LOG
-- Generated 6/20/2019 8:38:19 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_log
  IS

/*-------------------------CREATE BY SANGDD-----------------------------------
 * CREATE BY SANGDD :
 * CREATE DATE :28-03-2014
 * MUC DICH : LUU LOG UNG DUNG HE THONG
 *----------------------------------------------------------------------------*/


  PROCEDURE LOGMSG(P_MSG IN NVARCHAR2 );

  PROCEDURE LOG(P_TYPE IN VARCHAR2,P_MSG IN VARCHAR2 );

  PROCEDURE log_error(P_MSG IN VARCHAR2, p_procName IN VARCHAR2);


END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_log
IS

/* CREATE BY SANGDD :
 * CREATE DATE :28-03-2014
 * MUC DICH : LUU LOG UNG DUNG HE THONG
 */

  -- HAM LUU LOG HE THONG PHUC VU LUU VET
  PROCEDURE LOGMSG(P_MSG IN NVARCHAR2 )
  IS
   P_SYSDATE DATE ;
   V_SEQLOG NUMBER ;
  BEGIN
   P_SYSDATE :=SYSDATE ;
    INSERT INTO WLOG(MSG,DATES) VALUES(P_MSG,P_SYSDATE);
    COMMIT ;
      EXCEPTION
      WHEN others THEN
          RAISE ;
  END ;


  PROCEDURE LOG(P_TYPE IN VARCHAR2,P_MSG IN VARCHAR2 )
  IS
   P_SYSDATE DATE ;
   V_SEQLOG NUMBER ;
   V_MSG VARCHAR2(2000) DEFAULT '';
  BEGIN
    V_MSG:=SUBSTR(P_MSG,1,2000);
    P_SYSDATE :=SYSDATE ;
    INSERT INTO WLOG(MSG,DATES,TYPE) VALUES(P_MSG,P_SYSDATE,P_TYPE);
    COMMIT ;
      EXCEPTION
      WHEN others THEN
          RAISE ;
  END ;


  PROCEDURE log_error(P_MSG IN VARCHAR2, p_procName IN VARCHAR2)
  IS
   V_MSG VARCHAR2(2000) DEFAULT '';
  BEGIN
      V_MSG:=SUBSTR(P_MSG,1,2000);

     INSERT INTO log_error
            VALUES (SYSDATE, V_MSG,p_procName);

    COMMIT ;
  EXCEPTION
      WHEN others THEN
          RAISE ;
  END ;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_LOG

-- Start of DDL Script for Package STOCK_DR.PKG_TEST
-- Generated 6/20/2019 8:38:19 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_test
  IS
type tcursor is ref cursor;
--

PROCEDURE calc_inav
(
p_from_time IN VARCHAR2,p_to_time IN VARCHAR2,
p_etfcode in varchar2
);

FUNCTION func_get_OrderPrice(p_type number,strsql VARCHAR2, p_time varchar2) RETURN number;

end;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_test
IS
type tcursor is ref cursor;
--

PROCEDURE calc_inav
(
 p_from_time IN VARCHAR2,p_to_time IN VARCHAR2,p_etfcode in varchar2
)
is
    cursor curPcf is select symbol_price,symbol_amount ,symbol_code
        from etf_pcf@hnxetf
        where upper(etf_code) = upper(p_etfcode);
    --
    v_index varchar2(30);
    v_swaplot number;
    v_priceCore number;
    v_totalvalue number;
    v_money number;
    v_inav number;
    v_return number;

    v_time_cal VARCHAR2(30);
    v_date DATE ;

begin
    select indexcode, swaplot into v_index , v_swaplot
    from etf_info@hnxetf where upper(etf_code) = upper(p_etfcode);
    v_totalvalue := 0;
    v_return := 0;

    v_date := trunc(SYSDATE);
    v_time_cal := p_from_time;

    WHILE v_time_cal < p_to_time LOOP
        FOR recPcf in curPcf LOOP
             v_priceCore := 0;
            if(recPcf.symbol_code != '$$$') then
                begin
                select func_get_OrderPrice(0,'NORC = 5 AND NORP = 1 and noro = 1 and stock_id = '||s.id , v_time_cal) into v_priceCore
                from ts_symbol@hnxcore s left join view_tt_orders@hnxcore tt ON s.id = tt.stock_id
                where tt.deleted = 0 and s.deleted = 0
                and s.symbol = recPcf.symbol_code group by s.id;
                EXCEPTION
                    WHEN OTHERS  THEN
                        v_priceCore := 0;
                    END;
                if(v_priceCore > 0) then
                    v_totalvalue := v_totalvalue + (recPcf.symbol_amount*v_priceCore);
                else
                    v_totalvalue := v_totalvalue + (recPcf.symbol_amount*recPcf.symbol_price);
                end if;
            else
                v_money := recPcf.symbol_amount;
            END if;
        END LOOP ;

        IF (v_swaplot > 0) THEN
        v_return := (v_totalvalue + v_money) / v_swaplot;
        END IF;

        --v_time_cal Add them 15 giay
        SELECT to_char(TO_DATE(v_time_cal,'HH:MI:SS') + 15/86400,'HH:MI:SS') INTO v_time_cal FROM dual;

    --PKG_ETF_INAV.proc_etf_inav_insert_new(p_etfcode, to_char (SYSDATE,'YYYYMMDD-HH24:MM:SS'), v_return,v_date, v_time_cal,0);

    END LOOP ;

    --return v_return;
END ;


FUNCTION func_get_OrderPrice(p_type number,strsql VARCHAR2, p_time varchar2) RETURN number
is
v_order_price number;
maxtime VARCHAR2(30);
v_str VARCHAr2(1000);
BEGIN
     EXECUTE IMMEDIATE 'select nvl(max(order_time),'''') from tt_orders_plus@hnxcore where deleted = 0
     and ' || strsql ||' and order_time <= ''' || p_time ||'''' into maxtime;
     if(p_type=0) then --gia
        EXECUTE IMMEDIATE 'select nvl((select order_price from tt_orders_plus@hnxcore
     where deleted = 0 and rownum = 1 and order_time = '''|| maxtime || '''and ' || strsql || '),0) from dual ' into v_order_price;
     else
        EXECUTE IMMEDIATE 'select nvl((select order_qtty from tt_orders_plus@hnxcore
     where deleted = 0 and rownum = 1 and order_time = '''|| maxtime || '''and ' || strsql || '),0) from dual ' into v_order_price;
     end if;
    RETURN v_order_price;
    EXCEPTION
    when others then
        RAISE;

END;
END;
/


-- End of DDL Script for Package STOCK_DR.PKG_TEST

-- Start of DDL Script for Package STOCK_DR.PKG_USER_INFO
-- Generated 6/20/2019 8:38:19 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_user_info
IS
type tcursor is ref cursor;
--1.Insert
PROCEDURE proc_user_info_insert
(
p_username IN user_info.username % TYPE,
p_password IN user_info.password % TYPE,
p_fullname IN user_info.fullname % TYPE,
p_status IN user_info.status % TYPE,
p_activedate IN user_info.activedate % TYPE,
p_lastlogin IN user_info.lastlogin % TYPE,
p_note IN user_info.note % TYPE,
p_return out number
);
--1.End

--2.Update

PROCEDURE proc_user_info_update
(
p_username IN user_info.username % TYPE,
p_fullname IN user_info.fullname % TYPE,
p_status IN user_info.status % TYPE,
p_note IN user_info.note % TYPE
);
--2.end


--3.GetAll
PROCEDURE proc_user_info_getall
(
p_cursor out tcursor
);

--4.get by id

PROCEDURE proc_user_info_getbyid
(
  p_username IN user_info.username % TYPE,
  p_cursor out tcursor
);
--4.end

PROCEDURE proc_user_update_pass
(
  p_username IN user_info.username % TYPE,
  p_password IN user_info.password % TYPE,
  p_activedate IN user_info.activedate % TYPE,
  p_return out number
);

PROCEDURE proc_user_setdate
(
  p_type IN number,
  p_username IN user_info.username % TYPE,
  p_date IN date,
  p_return out number
);

PROCEDURE proc_user_deleted
(
  p_username IN user_info.username % TYPE,
  p_return out number
);

PROCEDURE proc_user_updateflag
(
  p_username IN user_info.username % TYPE,
  p_islogin in number
);

END;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_user_info
IS
--1.Insert
PROCEDURE proc_User_info_insert
(
p_username IN user_info.username % TYPE,
p_password IN user_info.password % TYPE,
p_fullname IN user_info.fullname % TYPE,
p_status IN user_info.status % TYPE,
p_activedate IN user_info.activedate % TYPE,
p_lastlogin IN user_info.lastlogin % TYPE,
p_note IN user_info.note % TYPE,
p_return out number
)
is
 v_id number; --n?u chya ?ung, vi?t l?i bi?n truy?n vao;
begin
    Select nvl(max(userid),0)+ 1 into v_id from user_info;
    insert into user_info( userid, username, password, fullname, status, activedate, lastlogin, note,islogin)
    values(v_id, p_username, p_password, p_fullname, p_status, trunc(p_activedate), p_lastlogin, p_note,0);
    p_return := v_id;
    EXCEPTION
    when others then
        p_return := -1;
    raise;
end;
--1.End

--2.Update

PROCEDURE proc_user_info_update
(
p_username IN user_info.username % TYPE,
p_fullname IN user_info.fullname % TYPE,
p_status IN user_info.status % TYPE,
p_note IN user_info.note % TYPE
)
is
begin
    update user_info set fullname = p_fullname, status = p_status, note = p_note
    where upper(username) = upper(p_username);
    commit;
    EXCEPTION
    WHEN OTHERS THEN
    ROLLBACK;
    RAISE;
end;
--2End

--3.GelAll

PROCEDURE proc_user_info_getall
(
p_cursor out tcursor
)
IS
 BEGIN
 OPEN p_cursor FOR
 SELECT userid, username, password, fullname, status, trunc(activedate) activedate, lastlogin, note,islogin,
 (select content from allcode where status = cdval and cdname='USER_INFO' and cdtype='STATUS') as statusstr
 from user_info;
END;

--4.GelbyId

PROCEDURE proc_user_info_getbyid
(
  p_username IN user_info.username % TYPE,
  p_cursor out tcursor
)
IS
BEGIN
    open p_cursor for
    SELECT userid, username, password, fullname, status, trunc(activedate) activedate, lastlogin, note,islogin,
     (select content from allcode where status = cdval and cdname='USER_INFO' and cdtype='STATUS') as statusstr
    from USER_INFO
    where upper(username) = upper(p_username);
end;

PROCEDURE proc_user_update_pass
(
  p_username IN user_info.username % TYPE,
  p_password IN user_info.password % TYPE,
  p_activedate IN user_info.activedate % TYPE,
  p_return out number
)
is
begin
    update user_info set password = p_password, activedate = trunc(p_activedate)
    where upper(username) = upper(p_username);
    commit;
    p_return := 1;
     EXCEPTION
    when others then
        p_return := -1;
    raise;
end;

PROCEDURE proc_user_setdate
(
  p_type IN number,
  p_username IN user_info.username % TYPE,
  p_date IN date,
  p_return out number
)
is
begin
    if(p_type = 0) then
        update user_info set lastlogin = p_date
        where upper(username) = upper(p_username);
    else
        update user_info set activedate = trunc(p_date)
        where upper(username) = upper(p_username);
    end if;
    commit;
    p_return := 1;
    EXCEPTION
    when others then
        p_return := -1;
    raise;
end;

PROCEDURE proc_user_deleted
(
  p_username IN user_info.username % TYPE,
  p_return out number
)
is
    v_id number;
begin
    select nvl(userid,0) into v_id from user_info where upper(username) = upper(p_username);
    --
    delete user_info where upper(username) = upper(p_username);
    --
    delete user_right where userid = v_id;
    --
    commit;
    p_return := 1;
    EXCEPTION
    when others then
        ROLLBACK;
        p_return := -1;
    raise;
end;

PROCEDURE proc_user_updateflag
(
  p_username IN user_info.username % TYPE,
  p_islogin in number
)
is
begin
    update user_info set islogin = p_islogin
    where upper(username) = upper(p_username);
    commit;
    EXCEPTION
    when others then
        ROLLBACK;
    raise;
end;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_USER_INFO

-- Start of DDL Script for Package STOCK_DR.PKG_USER_RIGHT
-- Generated 6/20/2019 8:38:19 AM from STOCK_DR@PM01_DR

CREATE OR REPLACE 
PACKAGE pkg_user_right
IS
type tcursor is ref cursor;
--1.Insert
PROCEDURE proc_user_right_insert
(
    p_userid IN user_right.userid % TYPE,
    p_funcid IN user_right.funcid % TYPE,
    p_used IN user_right.used % TYPE
);
--1.End

--2.Update

PROCEDURE proc_user_right_update
(
    p_userid IN user_right.userid % TYPE,
    p_funcid IN user_right.funcid % TYPE,
    p_used IN user_right.used % TYPE
);
--2.end

--4.get by id

PROCEDURE proc_user_right_getbyid
(
  p_username IN user_info.username % TYPE,
  p_cursor out tcursor
);
--4.end
END;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_user_right
IS
--1.Insert
PROCEDURE proc_User_right_insert
(
    p_userid IN user_right.userid % TYPE,
    p_funcid IN user_right.funcid % TYPE,
    p_used IN user_right.used % TYPE
)
is
begin
    insert into user_right(userid, funcid, used)
    values(p_userid, p_funcid, p_used);
    EXCEPTION
    when others then
        ROLLBACK;
    raise;
end;
--1.End

--2.Update

PROCEDURE proc_user_right_update
(
    p_userid IN user_right.userid % TYPE,
    p_funcid IN user_right.funcid % TYPE,
    p_used IN user_right.used % TYPE
)
is
    v_count number;
begin
    select count(*) into v_count from user_right where userid = p_userid and funcid = p_funcid;
    if(v_count > 0) then
        update user_right set used = p_used
        where userid = p_userid and funcid = p_funcid;
        commit;
    else
        insert into user_right(userid, funcid, used)
        values(p_userid, p_funcid, p_used);
    end if;
    EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
    RAISE;
end;
--2End


--4.GelbyId

PROCEDURE proc_user_right_getbyid
(
  p_username IN user_info.username % TYPE,
  p_cursor out tcursor
)
IS
BEGIN
    open p_cursor for SELECT a.userid, a.funcid, a.used, b.funcname,u.username,b.funcparent
    from user_info u join USER_RIGHT a on u.userid = a.userid
    join function b on a.funcid = b.funcid
    where upper(u.username)  = upper(p_username);
end;

END;
/


-- End of DDL Script for Package STOCK_DR.PKG_USER_RIGHT

