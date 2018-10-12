SELECT ORDER_ID,order_date, a.order_time, member_id, account_no, co_member_id, co_account_no, 
     (select b.idboard from ts_symbol b where b.deleted =0 and b.id = stock_id) boardid,a.idmarket AS marketid,
     stock_id, a.order_no, a.co_order_no, a.order_confirm_no, a.aori, a.order_qtty, a.order_price, trading_schedule_id,
     (select type from ts_securities_type st where st.id = (select idsectype from ts_symbol s where s.id =a.stock_id and s.deleted =0) and st.deleted =0) StockType,
     norp, broker_id, co_broker_id,noro,special_side,special_type,co_special_type 
 
 FROM VIEW_TT_ORDERS_EXEC a 
 where (a.norc=5 and a.norp = 1) or (norp=2 and norc =7 and status in (4,5,6)) and a.deleted = 0

