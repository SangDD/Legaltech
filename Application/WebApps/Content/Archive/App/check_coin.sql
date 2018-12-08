--SELECT * FROM mt_users where username = 'admin';

SELECT open_price,deleted,a.* from ts_symbol a WHERE symbol IN ('ETC-VND', 'ETC-BTC');
--UPDATE ts_symbol SET open_price = 224726.12 WHERE symbol =  'ETC-VND';
--UPDATE ts_symbol SET open_price = 0.00152537 WHERE symbol = 'ETC-BTC';

SELECT * FROM ts_queue_symbol WHERE code IN ('ETC-VND', 'ETC-BTC');
SELECT * from ts_symbol_calendar WHERE symbol_id IN (SELECT id from ts_symbol WHERE symbol IN ('ETC-VND', 'ETC-BTC'));
SELECT * FROM ts_foreign_room WHERE symbol IN ('ETC-VND', 'ETC-BTC');

--UPDATE ts_symbol SET deleted = 0 WHERE symbol IN ('ETC-VND', 'ETC-BTC');
