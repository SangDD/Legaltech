SELECT * FROM ts_operator_markets;
SELECT * from ts_trading_calendars WHERE is_workingday = 1 ;

SELECT sum(order_qtty * order_price) FROM tt_orders;
SELECT sum(order_qtty * order_price) FROM tt_orders_plus;
SELECT * FROM tt_orders;


