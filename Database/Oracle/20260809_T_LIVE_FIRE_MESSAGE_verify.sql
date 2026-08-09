SELECT object_name, object_type, status
  FROM user_objects
 WHERE object_name IN
       ('T_LIVE_FIRE_MESSAGE', 'SEQ_T_LIVE_FIRE_MESSAGE', 'LIVE_FIRE_MESSAGE_PKG')
 ORDER BY object_type, object_name;

SELECT index_name, column_name, column_position
  FROM user_ind_columns
 WHERE table_name = 'T_LIVE_FIRE_MESSAGE'
 ORDER BY index_name, column_position;

SELECT line, position, text
  FROM user_errors
 WHERE name = 'LIVE_FIRE_MESSAGE_PKG'
 ORDER BY sequence;
