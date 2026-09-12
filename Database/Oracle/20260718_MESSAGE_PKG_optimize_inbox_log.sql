-- Optimize MESSAGE_PKG.GetInboxBySearchLogFile while preserving its API signature.
-- Database: ATFM@PDBORCL
-- Date: 2026-07-18
-- TOTAL_RECORDS is calculated on the first page; subsequent pages reuse the UI cache.

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ATFM"."MESSAGE_PKG" AS

PROCEDURE SP_INSERT_QLBINBOX(P_IN_ID number,P_ER_ID number, P_NT_ID number, P_IN_STT varchar2, P_IN_TIME date, P_IN_NUM varchar2
	    , P_IN_MSG varchar2, P_IN_CONT varchar2,P_RowNumMap number,P_DATETIMEMDB varchar,P_OUT out number) as
  nChk number;
  BEGIN
   --sys_extract_utc(systimestamp)
	insert into T_QLBINBOX(IN_ID,ER_ID,NT_ID,IN_STT,in_time,IN_NUM,IN_MSG,IN_CONT,RowNumMap,DATETIMEMDB)
	values(
	P_IN_ID,P_ER_ID,P_NT_ID,P_IN_STT,sys_extract_utc(systimestamp),P_IN_NUM,P_IN_MSG,P_IN_CONT,p_RowNumMap,to_date(P_DATETIMEMDB,'dd-mm-yyyy hh24:mi:ss')
	);
	commit;
	P_OUT:=1;

    EXCEPTION
	WHEN OTHERS THEN
	 PROCESS_PKG.ADD_ERROR_LOG('SP_INSERT_QLBINBOX',
				SQLCODE,
				SUBSTR(SQLERRM, 1, 200));
	P_OUT:= -1;
  END SP_INSERT_QLBINBOX;

  PROCEDURE GroupAddress_GetAll(P_OUT out T_CURSOR) AS
  BEGIN
	open P_OUT for
	    /*select
	      RTRIM(LISTAGG(a.ADDRESS || ' ') WITHIN GROUP(ORDER BY 1),' ') Address
	      , b.GROUP_NAME
	      ,b.ID
			     from  M_G_A_D a
			inner join M_G_A_M b ON a.ID = b.id
			where a.G_A_M_ID  not in(1,2)
		    group by a.G_A_M_ID,b.GROUP_NAME,b.ID;
	    */
	    select * from   M_G_A_M Where GR_TYPE=1 order by GROUP_NAME ;
  END GroupAddress_GetAll;

  PROCEDURE GroupAddress_GetAll4(P_OUT out T_CURSOR) AS
  BEGIN
	open P_OUT for

	    select * from   M_G_A_M Where GR_TYPE=4 order by GROUP_NAME ;
  END GroupAddress_GetAll4;

  PROCEDURE GroupAddress_GetAll_Air(P_OUT out T_CURSOR) AS
  BEGIN
	open P_OUT for
	    /*select
	      RTRIM(LISTAGG(a.ADDRESS || ' ') WITHIN GROUP(ORDER BY 1),' ') Address
	      , b.GROUP_NAME
	      ,b.ID
			     from  M_G_A_D a
			inner join M_G_A_M b ON a.ID = b.id
			where a.G_A_M_ID  not in(1,2)
		    group by a.G_A_M_ID,b.GROUP_NAME,b.ID;
	    */
	    select * from   M_G_A_M Where GR_TYPE=3 order by GROUP_NAME ;
  END GroupAddress_GetAll_Air;

  PROCEDURE QlbOutBox_Insert(P_NT_ID number,P_OT_GTIME date DEFAULT sysdate,
			    P_OT_STIME date DEFAULT sysdate, P_OT_NUM number,
			    P_OT_CONT varchar2,P_OUT out number) AS
    GTIME date:= P_OT_GTIME;
    STIME date:= P_OT_STIME;
  BEGIN
    if GTIME is null then
	GTIME:= sys_extract_utc(systimestamp);
    end if;
    if STIME is null then
	STIME:= sys_extract_utc(systimestamp);
    end if;
	--quynx -20022024
	--insert into T_SENDINBOX (FlightDate,Content)
	--values (GTIME,P_OT_CONT);
	 --commit;
	insert into T_QlbOutBox(NT_ID, OT_GTIME, OT_STIME, OT_NUM, OT_CONT)
	    values (
	    P_NT_ID
	    ,GTIME
	    ,STIME
	    ,P_OT_NUM,upper(P_OT_CONT));
	    commit;
	P_OUT:=1;
	EXCEPTION
	WHEN OTHERS THEN
	P_OUT:= -1;
  END QlbOutBox_Insert;

  PROCEDURE DayFlightNotPerm_GetAll(P_OUT out T_CURSOR) as
    begin
	open P_OUT for
	select * from (
	    select  rownum as rnum, T_REALPLAN_LETTER.* from T_REALPLAN_LETTER
		--where to_date(LETTERNBR_PK,'dd-mm-yyyy') = to_date('18-08-2015','dd-mm-yyyy')
		    --and FLIGHT_ID is null
		    ) a
	    where a.rnum between 1 and 30;
    end DayFlightNotPerm_GetAll;

  PROCEDURE DayFlightNotPerm_GetHistory(P_Value varchar2 ,P_OUT out T_CURSOR) as
    begin
	open P_OUT for
	    select * from T_REALPLAN_LETTER
		where FLIGHTNBR = P_Value;
    end DayFlightNotPerm_GetHistory;
    PROCEDURE DayFlightNotPerm_GetBySearch(
			P_NBR varchar2
			,P_LETTER_TYPE varchar2
			,P_FLIGHTNBR varchar2
			,P_REGISTRATION varchar2
			,P_FROM_AIRP varchar2
			,P_TO_AIRP varchar2
			,P_ETD varchar2
			,P_ETA varchar2
			,P_ATD varchar2
			,P_ATA varchar2
			,P_VIA varchar2
			,P_CRAFT_TYPE varchar2
			,p_rowStart number
			,p_rowFinish number
			,p_ROUTE varchar2
			,p_ROUTE_TT varchar2
			,p_HasPerm number
			, P_OUT out T_CURSOR
			) is
	begin
	    open P_OUT for
		select * from(
		select rownum as rnum,a.* from (
		    select row_number() over (PARTITION by FLIGHTNBR order by T_REALPLAN_LETTER.LETTERNBR_PK) as IsNotPerm,T_REALPLAN_LETTER.* from T_REALPLAN_LETTER
			where (P_NBR is null or P_NBR='' or UPPER(NBR) like '%'|| UPPER(P_NBR)||'%' )
			and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(LETTER_TYPE) like '%'|| UPPER(P_LETTER_TYPE)||'%' )
			and (P_FLIGHTNBR is null or P_FLIGHTNBR='' or UPPER(FLIGHTNBR) like '%'|| UPPER(P_FLIGHTNBR)||'%' )
			and (P_REGISTRATION is null or P_REGISTRATION='' or UPPER(REGISTRATION) like '%'|| UPPER(P_REGISTRATION)||'%' )
			and (P_FROM_AIRP is null or P_FROM_AIRP='' or UPPER(FROM_AIRP) = UPPER(P_FROM_AIRP))
			and (P_TO_AIRP is null or P_TO_AIRP='' or UPPER(TO_AIRP)= UPPER(P_TO_AIRP) )
			and (P_REGISTRATION is null or P_REGISTRATION='' or UPPER(REGISTRATION) like '%'|| UPPER(P_REGISTRATION)||'%' )
			and (P_CRAFT_TYPE is null or P_CRAFT_TYPE='' or UPPER(CRAFT_TYPE)= UPPER(P_CRAFT_TYPE) )
			and (p_ROUTE is null or p_ROUTE='' or UPPER(ROUTE) like '%'|| UPPER(p_ROUTE)||'%' )
			and (p_ROUTE_TT is null or p_ROUTE_TT='' or UPPER(ROUTE_TT) like '%'|| UPPER(p_ROUTE_TT)||'%')
			and (P_VIA is null or P_VIA='' or UPPER(VIA) like '%'|| UPPER(P_VIA)||'%')
			--and HasPerm = p_HasPerm
			and T_REALPLAN_LETTER.ISFIR <> 3
			and T_REALPLAN_LETTER.FLIGHT_ID is null
			and to_date(T_REALPLAN_LETTER.FLIGHTDATE) = to_date(sysdate)
		) a where IsNotPerm = 1
		) where rnum between p_rowStart and p_rowFinish;
	end DayFlightNotPerm_GetBySearch;
	FUNCTION compareHour(p_value in varchar2) return number is
	ax varchar2(6):='';
    begin
	if p_value is null or p_value='' or is_numeric(regexp_replace(p_value, '[^0-9]'))=0 then
	    RETURN 0;
	end if;
	return to_number(SUBSTR(lpad(SUBSTR(regexp_replace(p_value, '[^0-9]'),1,6), 6,'0'),3,6));
	EXCEPTION
	  WHEN value_error
	  THEN
	    RETURN 0;
    end compareHour;

     PROCEDURE GroupAddress_GetbyID(p_value varchar2,P_OUT out T_CURSOR) is
     begin
	open P_OUT for
	    select * from M_G_A_D where G_A_M_ID= p_value order by Address ;
     end GroupAddress_GetbyID;
     Procedure CheckMessageICAO4444(p_Content varchar2, p_out out varchar2) is
	bChk BOOLEAN;
	bCheck BOOLEAN;
	vErr1 varchar2(4000):='vErr1';
	vErr2 varchar2(4000):='vErr2';
	begin
	    bCheck:= REALPLAN.CHECK_ERROR_ICAO4444(REALPLAN.Return_ContentFPL(bChk,p_Content,vErr1),'FPL',p_out);
	    Exception
	    when others then
	    null;
	end CheckMessageICAO4444;
    Procedure GetSttQlbInbox(p_Date date, p_out out number) is
	begin
	select max(rownummap) into p_out
	    FROM   t_qlbinbox where to_date(in_time,'dd-mm-yyyy') = to_date( p_date,'dd-mm-yyyy');
	if p_out is null then
	    p_out:=0;
	end if;
	end GetSttQlbInbox;

    procedure sp_ReadAcess(p_Date varchar2, p_value varchar2,p_out out number)
    is
      V_CHECK NUMBER;
    begin
    SELECT COUNT(0) INTO V_CHECK FROM m_HISACCESS  WHERE FileName = p_value;
    IF (V_CHECK = 0) THEN
     insert into m_HISACCESS(Ngaythuchien,FileName) values(p_Date,p_value);
      commit;
       p_out:=1;
       ELSE
      p_out := -1;
    END IF;


       EXCEPTION
	WHEN OTHERS THEN
	P_OUT:= -1;
    end sp_ReadAcess;

    procedure sp_ViewAcess(p_date varchar2,P_OUT out T_CURSOR)
    is

    begin
    open P_OUT for
	    select * from m_HISACCESS;
    end sp_ViewAcess;
    Procedure spGetContentMessage(p_letterPk date, p_out out varchar2) is
	begin
	    select Text into p_out from T_Realplan_Letter where Letternbr_Pk = p_letterPk;
	end spGetContentMessage;
    Function updateStatusMessage_WhenSend(p_date date, p_type varchar2, p_part_no number) return number is
	p_listId clob;
	begin
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', false);
	    select LISTFLIGHTID into p_listId from T_Plan_Message
		where FLIGHTDATE = p_date
		    --and PART_NO =p_part_no
		    --QUYNX-15042021
		    and ID = p_part_no
		    and MESS_TYPE = p_type;
	    update T_Day_Flights
		set Ismessage = 1
		    where flight_id in (select column_value from table(split_string(p_listId, ',')));
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
	return 1;
	Exception
	when others then
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
	    return -1;
	end updateStatusMessage_WhenSend;

     Function updateStatusMessage_WhenSendID(p_id number,p_date date, p_type varchar2, p_part_no number) return number is
	p_listId clob;
	begin
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', false);
	    select LISTFLIGHTID into p_listId from T_Plan_Message
		where FLIGHTDATE = p_date
		    and PART_NO =p_part_no and MESS_TYPE = p_type;
	    update T_Day_Flights
		set Ismessage = 1
		    where flight_id in (select column_value from table(split_string(p_listId, ',')));
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
	return 1;
	Exception
	when others then
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
	    return -1;
	end updateStatusMessage_WhenSendID;

     procedure Khb_SendAll_MessageAll(p_vn varchar2,p_ld varchar2,p_of varchar2,p_Origin varchar2,p_header varchar2,p_date date, p_ToAdd clob,p_out out number) is
	--vFromAdd varchar2(50):= to_char(sysdate, 'ddhh24mi')|| ' VVVVZGZX';
	vFromAdd varchar2(50):= to_char(sys_extract_utc(systimestamp), 'ddhh24mi')|| ' VVGLSLBT';
	vToAdd_HVN clob:='';vToAdd_LD clob:=''; vToAdd_OVER clob:='';
	nKq number:=1;
	slp BOOLEAN;
	hvn t_plan_message%rowtype; ld t_plan_message%rowtype; ov t_plan_message%rowtype;
	/*QUYNX - 072024*/
	cursor c_HVN is select * from t_plan_message where to_date(flightdate,'dd-mm-yyyy')= to_date(p_date,'dd-mm-yyyy') and Mess_Type = 'HVN MESSAGE' and status IN(0,1) order by part_no;
	cursor c_LD is select * from t_plan_message where  to_date(flightdate,'dd-mm-yyyy')= to_date(p_date,'dd-mm-yyyy') and Mess_Type = 'LANDING FLIGHTS' and status IN(0,1) order by part_no;
	cursor c_OVER is select * from t_plan_message where  to_date(flightdate,'dd-mm-yyyy')= to_date(p_date,'dd-mm-yyyy') and Mess_Type = 'OVER FLIGHTS' and status IN(0,1) order by part_no;
	begin
	    /*select RTRIM(LISTAGG(address || ',') WITHIN GROUP(ORDER BY 1),',') into vToAdd_HVN from  M_G_A_D where G_A_M_ID =1 and id<9000;
	    select RTRIM(LISTAGG(address || ',') WITHIN GROUP(ORDER BY 1),',') into vToAdd_LD from  M_G_A_D where G_A_M_ID=2 and id<9500;
	    select RTRIM(LISTAGG(address || ',') WITHIN GROUP(ORDER BY 1),',') into vToAdd_OVER from  M_G_A_D where G_A_M_ID=2 and id<9500;
	    --setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', false);
	    */
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', false);
	    vToAdd_HVN:= 'VVGLYFYX'||chr(10);
	    vToAdd_LD:= 'VVGLYFYX'||chr(10);
	    vToAdd_OVER:= 'VVGLYFYX'||chr(10);

	    if (p_vn='1') then
		open c_HVN;
		loop
		    FETCH c_HVN INTO hvn;
		    EXIT WHEN c_HVN%NOTFOUND;
		    --nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), null, vToAdd_HVN, hvn.CONTENT);
		    nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), p_Origin, p_ToAdd, hvn.CONTENT);

		    if nKq = 1 then
			update t_day_flights
			    set ismessage = 1
				where flight_id in (select column_value from table(split_string(hvn.listflightid,',')));
			update t_plan_message set status =1  where id = hvn.Id;
		    end if;
		    sleep(2000);
		end loop;
		close c_HVN;
	    end if;
	    if (p_ld='1') then

		open c_LD;
		loop
		    FETCH c_LD INTO ld;
		    EXIT WHEN c_LD%NOTFOUND;
		    --nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), null, vToAdd_LD, ld.CONTENT);
		    nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), p_Origin, p_ToAdd, ld.CONTENT);

		    if nKq = 1 then
			update t_day_flights
			    set ismessage = 1
				where flight_id in (select column_value from table(split_string(ld.listflightid,',')));
			update t_plan_message set status =1  where id = ld.Id;
			commit;
		    end if;
		    sleep(2000);
		end loop;
		close c_LD;
	    end if;

	     if (p_of='1') then

		open c_OVER;
		loop
		    Fetch c_OVER into ov;
		    exit when c_OVER%notFound;
		    --nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), null, vToAdd_OVER, ov.CONTENT);
		    nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), p_Origin, p_ToAdd, ov.CONTENT);
		    if nKq = 1 then
			update t_day_flights
			    set ismessage = 1
				where flight_id in (select column_value from table(split_string(ov.listflightid,',')));
			update t_plan_message set status = 1  where id = ov.Id;
		    end if;
		    sleep(2000);
		end loop;
		close c_OVER;

	     end if;
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
	    commit;
	    p_out:=1;

	    exception
	    when others then
		setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
		p_out:=-1;
		rollback;
	end Khb_SendAll_MessageAll;


     /* dia chi gui origin mac dinh: VVVVZGZX
      * p_Header default 'FF'; Neu trong phan dia chi co heade thi lay header theo dia chi gui
       p_out:= insertMessBy_Address_Content(p_Header, p_Origin, p_ToAdd, p_content);
      */
    procedure Khb_SendAll_Message(p_header varchar2,p_date date, p_out out number) is
	--vFromAdd varchar2(50):= to_char(sysdate, 'ddhh24mi')|| ' VVVVZGZX';
	vFromAdd varchar2(50):= to_char(sys_extract_utc(systimestamp), 'ddhh24mi')|| ' VVGLSLBT';
	vToAdd_HVN clob:='';vToAdd_LD clob:=''; vToAdd_OVER clob:='';
	nKq number:=1;
	slp BOOLEAN;
	hvn t_plan_message%rowtype; ld t_plan_message%rowtype; ov t_plan_message%rowtype;
	cursor c_HVN is select * from t_plan_message where to_date(flightdate,'dd-mm-yyyy')= to_date(p_date,'dd-mm-yyyy') and Mess_Type = 'HVN MESSAGE' and status=0 order by part_no;
	cursor c_LD is select * from t_plan_message where  to_date(flightdate,'dd-mm-yyyy')= to_date(p_date,'dd-mm-yyyy') and Mess_Type = 'LANDING FLIGHTS' and status=0 order by part_no;
	cursor c_OVER is select * from t_plan_message where  to_date(flightdate,'dd-mm-yyyy')= to_date(p_date,'dd-mm-yyyy') and Mess_Type = 'OVER FLIGHTS' and status=0 order by part_no;
	begin
	    /*select RTRIM(LISTAGG(address || ',') WITHIN GROUP(ORDER BY 1),',') into vToAdd_HVN from  M_G_A_D where G_A_M_ID =1 and id<9000;
	    select RTRIM(LISTAGG(address || ',') WITHIN GROUP(ORDER BY 1),',') into vToAdd_LD from  M_G_A_D where G_A_M_ID=2 and id<9500;
	    select RTRIM(LISTAGG(address || ',') WITHIN GROUP(ORDER BY 1),',') into vToAdd_OVER from  M_G_A_D where G_A_M_ID=2 and id<9500;
	    --setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', false);
	    */
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', false);
	    vToAdd_HVN:= 'VVGLYFYX'||chr(10);
	    vToAdd_LD:= 'VVGLYFYX'||chr(10);
	    vToAdd_OVER:= 'VVGLYFYX'||chr(10);
	    open c_HVN;
	    loop
		FETCH c_HVN INTO hvn;
		EXIT WHEN c_HVN%NOTFOUND;
		nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), null, vToAdd_HVN, hvn.CONTENT);
		if nKq = 1 then
		    update t_day_flights
			set ismessage = 1
			    where flight_id in (select column_value from table(split_string(hvn.listflightid,',')));
		    update t_plan_message set status =1  where id = hvn.Id;
		end if;
		sleep(2000);
	    end loop;
	    close c_HVN;

	    open c_LD;
	    loop
		FETCH c_LD INTO ld;
		EXIT WHEN c_LD%NOTFOUND;
		nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), null, vToAdd_LD, ld.CONTENT);
		if nKq = 1 then
		    update t_day_flights
			set ismessage = 1
			    where flight_id in (select column_value from table(split_string(ld.listflightid,',')));
		    update t_plan_message set status =1  where id = ld.Id;
		    commit;
		end if;
		sleep(2000);
	    end loop;
	    close c_LD;
	    open c_OVER;
	    loop
		Fetch c_OVER into ov;
		exit when c_OVER%notFound;
		nKq:= insertMessBy_Address_Content(nvl(p_header,'FF'), null, vToAdd_OVER, ov.CONTENT);
		if nKq = 1 then
		    update t_day_flights
			set ismessage = 1
			    where flight_id in (select column_value from table(split_string(ov.listflightid,',')));
		    update t_plan_message set status = 1  where id = ov.Id;
		end if;
		sleep(2000);
	    end loop;
	    close c_OVER;
	    setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
	    commit;
	    p_out:=1;

	    exception
	    when others then
		setStatusTrigger('DAY_FLIGHTS_TRIGGE_HIS', true);
		p_out:=-1;
		rollback;
	end Khb_SendAll_Message;


     /* dia chi gui origin mac dinh: VVVVZGZX
      * p_Header default 'FF'; Neu trong phan dia chi co heade thi lay header theo dia chi gui
       p_out:= insertMessBy_Address_Content(p_Header, p_Origin, p_ToAdd, p_content);
      */
    Function insertMessBy_Address_Content(p_Header varchar2, p_origin varchar2, p_ToAdd clob, p_content varchar2) return number is
	vAdd varchar2(4000):= '';
	vContent varchar2(4000);
	nChem number:=1;
	nC number:=1;
	nKq number;
	vHeader varchar2(20):=p_Header;



	cursor c is select * from table(split_string(trim(replace(RTrim(p_ToAdd),' ',',')),','));
	begin



	    savepoint BAK;
	    if p_ToAdd is null then
		    null;
		    return -1;
	    end if;
	    if substr(p_ToAdd, 3,1) = ' ' then
		vHeader:=substr(trim(p_ToAdd), 1,2);
	    end if;
	    for rc in c
	    loop
		if length(to_char(rc.column_value)) is null or length(to_char(rc.column_value)) = 2 then
		    goto nt;
		end if;
		if mod(nC,7)>0 then
		    vAdd:= trim(vAdd || ' ' || trim(rc.column_value));
		else
		    --vAdd:= trim(vAdd || ' ' || trim(rc.column_value) || chr(10));
		    if mod(nC, 21)=0 then
			vAdd:= trim(vAdd || ' ' || trim(rc.column_value));
		    else
			vAdd:= trim(vAdd || ' ' || trim(rc.column_value) || chr(10));
		    end if;
		    --QUYNX-042025
		    --vAdd:= trim(vAdd || ' ' || trim(rc.column_value));
		end if;


		nChem:=nChem+1;
		if mod(nC, 21)=0 then
		    vContent:= nvl(vHeader,'FF') ||' '|| replace(vAdd, chr(10) || ' ', chr(10))
				--|| chr(10) || to_char(sys_extract_utc(systimestamp), 'ddhh24mi') || ' ' || nvl(p_origin,'VVVVZGZX') || chr(10)
				|| chr(10) || to_char(sys_extract_utc(systimestamp), 'ddhh24mi') || ' ' || nvl(p_origin,'VVGLSLBT') || chr(10)
				|| p_content;
		    QlbOutBox_Insert(0, sys_extract_utc(systimestamp),sys_extract_utc(systimestamp), null, upper(vContent), nKq);
		    /*insert into qlboutbox@BDCU
			(OT_ID, NT_ID,	OT_GTIME,   OT_STIME,	OT_SENT,    OT_NUM, OT_CONT)
		    values( null,   null,   null,	null,	    'N',	null,	upper( vContent ));*/
		    vAdd:='';
		    nChem:=1; nC:=0; vContent:='';
		end if;
		nC:=nC+1;
		<<nt>>
		null;
	    end loop;

	    --insert into t_log values('QUYNX0420251',p_ToAdd,nC);

	    if nChem >1 then
		vContent:= nvl(vHeader,'FF') ||' '|| replace(vAdd, chr(10) || ' ', chr(10))
			|| chr(10) || to_char(sys_extract_utc(systimestamp), 'ddhh24mi') || ' ' || nvl(p_origin,'VVGLSLBT') || chr(10)
			|| p_content;
		QlbOutBox_Insert(0, sys_extract_utc(systimestamp),sys_extract_utc(systimestamp), null,upper(vContent), nKq);
		/*insert into qlboutbox@BDCU
			(OT_ID, NT_ID,	OT_GTIME,   OT_STIME,	OT_SENT,    OT_NUM, OT_CONT)
		    values( null,   null,   null,	null,	    'N',	null,	upper( vContent));*/
	    end if;
	    commit;
	    return 1;
	    exception
	    when others then
	    rollback to BAK;
	    return -1;
	end insertMessBy_Address_Content;
    Function insertOutbox_AMHS(p_Header varchar2, p_origin varchar2, p_ToAdd clob, p_content varchar2) return number is
	vAdd varchar2(4000):= '';
	vContent varchar2(4000);
	nChem number:=1;
	nC number:=1;
	nKq number;
	nIdOutbox number;
	cursor c is select * from table(split_string(trim(replace(p_ToAdd,' ',',')),','));
	begin
	    savepoint BAK;
	    select count(0)+1 into nC from outbox_oracle;
	    -- P_HEADER cua luong AMHS nhan messType tu MessManagement.aspx va duoc
	    -- luu vao SUBJECT. Gia tri khac GG dung priority mac dinh 0.
	    insert into outbox_oracle(ID,ATTACH,CONTENT,FROM_ADDRESS,PRIORITY,SUBJECT,TIME)
		values (nC, 0, p_content, (select nvl(ADDRESS,'AXXCA') from address_oracle where name = 'VVVVZGZX' and scheme='C' and rownum<2)
		    , (case when upper(trim(p_Header)) = 'GG' then 1 else 0 end),
		      nvl(trim(p_Header), 'object'), sysdate);
	    insert into Outbox_address_Oracle(address, outbox_oracle_id)
	    select  address,nC from table(split_string(trim(replace(p_ToAdd,' ',',')),','))
	    inner join Address_Oracle on column_value = name and scheme<>'C';
	    commit;
	    return 1;
	    exception
	    when others then
	    rollback to BAK;
	    return -1;
	end insertOutbox_AMHS;
    Procedure message_send_one(p_Header varchar2, p_ToAdd clob, p_content varchar2, p_out out number) is
	begin
	    p_out:= insertMessBy_Address_Content(p_Header, null, Rtrim(p_ToAdd), p_content);

	  --  p_out:= insertOutbox_AMHS(p_Header, null, p_ToAdd, p_content);

	    /*
	    *
	    * AFTNmessage_send_one_origin
	    *
	    */


	end message_send_one;

     Procedure message_send_one_origin(p_Origin varchar2,p_Header varchar2, p_ToAdd clob, p_content varchar2, p_out out number) is
	begin
	    p_out:= insertMessBy_Address_Content(p_Header, p_Origin, p_ToAdd, p_content);
	  --  p_out:= insertOutbox_AMHS(p_Header, null, p_ToAdd, p_content);
	    /*
	    *
	    * AFTNmessage_send_one_origin
	    *
	    */


	end message_send_one_origin;

    Procedure message_send_one_amhs(p_Origin varchar2,p_Header varchar2, p_ToAdd clob, p_content varchar2, p_out out number) is
	begin
	    --p_out:= insertMessBy_Address_Content(p_Header, p_Origin, p_ToAdd, p_content);
	    p_out:= insertOutbox_AMHS(p_Header, null, p_ToAdd, p_content);
	    /*
	    *
	    * AFTNmessage_send_one_origin
	    *
	    */


	end message_send_one_amhs;

    Procedure getGroupAddress_Ref(p_out out T_CURSOR) is
	begin
	    open p_out for
		select a.*, B.Group_Name from M_G_A_D a
		    inner join M_G_A_M b on a.g_a_m_id = b.id
		    where b.GR_TYPE=1
		    ;
	end getGroupAddress_Ref;
    PROCEDURE realplan_letter_get_page(P_PAGE_SIZE IN int,
					 P_PAGE_INDEX IN INT,
					 P_LETTERNBR_PK   DATE,
					 P_NBR		  VARCHAR2,
					 P_LETTER_TYPE	  VARCHAR2 ,
					P_STATUS	 VARCHAR2,
					--P_FLIGHT_ID	   NUMBER ,
					P_FLIGHTNBR	 VARCHAR2,
					P_REGISTRATION	 VARCHAR2 ,
					P_FROM_AIRP	 VARCHAR2 ,
					P_TO_AIRP	 VARCHAR2 ,
					P_ETD		 VARCHAR2,
					P_ETA		 VARCHAR2 ,
					P_ATD		 VARCHAR2 ,
					P_ATA		 VARCHAR2,
					P_VIA		 VARCHAR2 ,
					P_CRAFT_TYPE	 VARCHAR2 ,
					P_FLIGHTDATE	 DATE	,
					P_CODE		 VARCHAR2 ,
					--P_ISFIR	   NUMBER   ,
					P_ROUTE 	 VARCHAR2 ,
					P_ROUTE_TT	 VARCHAR2 ,
					--P_FLIGHT_ID_CUS  NUMBER  ,
					--P_ISSELECT	   NUMBER ,
					 P_OUT_CURSOR OUT T_CURSOR) IS
	    P_FIRST_INDEX int;
	    P_LAST_INDEX int;
    BEGIN
	    P_LAST_INDEX := P_PAGE_SIZE * (P_PAGE_INDEX );
	    P_FIRST_INDEX := P_LAST_INDEX - P_PAGE_SIZE;
	    Open P_OUT_CURSOR for  SELECT * FROM
		(select u.* ,rownum as rnum from
		    (select a.*,count(*) over() as Record_Sum
			from T_REALPLAN_LETTER a where
			(P_NBR is null or P_NBR='' or UPPER(a.NBR)= UPPER(P_NBR) )
			and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(a.LETTER_TYPE)like '%'||UPPER(P_LETTER_TYPE)||'%' )
			and  (P_STATUS is null or P_STATUS='' or UPPER(a.STATUS)= UPPER(P_STATUS) )
			and (p_FLIGHTNBR is null or p_FLIGHTNBR='' or UPPER(a.FLIGHTNBR) like '%'||UPPER(p_FLIGHTNBR)||'%' )
			and  (P_REGISTRATION is null or P_REGISTRATION='' or UPPER(a.REGISTRATION)= UPPER(P_REGISTRATION) )
			and  (P_FROM_AIRP is null or P_FROM_AIRP='' or UPPER(a.FROM_AIRP)= UPPER(P_FROM_AIRP) )
			and  (P_TO_AIRP is null or P_TO_AIRP='' or UPPER(a.TO_AIRP)= UPPER(P_TO_AIRP) )
			and  (P_ETD is null or P_ETD='' or UPPER(a.ETD)= UPPER(P_ETD) )
			and  (P_ETA is null or P_ETA='' or UPPER(a.ETA)= UPPER(P_ETA) )
			and  (P_ATD is null or P_ATD='' or UPPER(a.ATD)= UPPER(P_ATD) )
			and  (P_ATA is null or P_ATA='' or UPPER(a.ATA)= UPPER(P_ATA) )
			and  (P_VIA is null or P_VIA='' or UPPER(a.VIA)= UPPER(P_VIA) )
			and  (P_CRAFT_TYPE is null or P_CRAFT_TYPE='' or UPPER(a.CRAFT_TYPE)= UPPER(P_CRAFT_TYPE) )
			and  (P_ROUTE is null or P_ROUTE='' or UPPER(a.ROUTE)= UPPER(P_ROUTE) )
			and  (P_ROUTE_TT is null or P_ROUTE_TT='' or UPPER(a.ROUTE_TT)= UPPER(P_ROUTE_TT) )
			and  (P_FLIGHTDATE is null or P_FLIGHTDATE='' or to_date(a.LETTERNBR_PK) = to_date(P_FLIGHTDATE))
			) u order by u.LETTERNBR_PK desc) where rnum between P_FIRST_INDEX and	P_LAST_INDEX;
    END realplan_letter_get_page;
    PROCEDURE realplan_letter_HasChange(P_PAGE_SIZE IN int,
				 P_PAGE_INDEX IN INT,
				 P_LETTERNBR_PK   DATE,
				 P_NBR		  VARCHAR2,
				P_LETTER_TYPE	 VARCHAR2 ,
				P_STATUS	 VARCHAR2,
				--P_FLIGHT_ID	   NUMBER ,
				P_FLIGHTNBR	 VARCHAR2,
				P_REGISTRATION	 VARCHAR2 ,
				P_FROM_AIRP	 VARCHAR2 ,
				P_TO_AIRP	 VARCHAR2 ,
				P_ETD		 VARCHAR2,
				P_ETA		 VARCHAR2 ,
				P_ATD		 VARCHAR2 ,
				P_ATA		 VARCHAR2,
				P_VIA		 VARCHAR2 ,
				P_CRAFT_TYPE	 VARCHAR2 ,
				P_FLIGHTDATE	 DATE	,
				P_CODE		 VARCHAR2 ,
				--P_ISFIR	   NUMBER   ,
				P_ROUTE 	 VARCHAR2 ,
				P_ROUTE_TT	 VARCHAR2 ,
				--P_FLIGHT_ID_CUS  NUMBER  ,
				--P_ISSELECT	   NUMBER ,
				 P_OUT_CURSOR OUT T_CURSOR) is
	    P_FIRST_INDEX int;
	    P_LAST_INDEX int;
    begin
	    P_LAST_INDEX := P_PAGE_SIZE * (P_PAGE_INDEX );
	    P_FIRST_INDEX := P_LAST_INDEX - P_PAGE_SIZE;
	    open P_OUT_CURSOR for select * from
		(select u.* ,rownum as rnum from
		    (select b.*,count(*) over() as Record_Sum
			from t_day_flights_goingon a
			inner join T_Realplan_Letter b on a.id = b.flight_id_cus
			    where a.haschange = 1
			    and (P_NBR is null or P_NBR='' or UPPER(a.NBR)= UPPER(P_NBR) )
			    --and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(b.LETTER_TYPE)= UPPER(P_LETTER_TYPE) )
			    and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(b.LETTER_TYPE)like '%'||UPPER(P_LETTER_TYPE)||'%' )
			    and (P_STATUS is null or P_STATUS='' or UPPER(a.STATUS)= UPPER(P_STATUS) )
			    --and  (P_FLIGHT_ID is null or P_FLIGHT_ID='' or UPPER(a.FLIGHT_ID)= UPPER(P_FLIGHT_ID) )
			    and (p_FLIGHTNBR is null or p_FLIGHTNBR='' or UPPER(a.FLIGHTNBR) like '%'||UPPER(p_FLIGHTNBR)||'%' )
			    and  (P_REGISTRATION is null or P_REGISTRATION='' or UPPER(a.REGISTRATION)= UPPER(P_REGISTRATION) )
			    and  (P_FROM_AIRP is null or P_FROM_AIRP='' or UPPER(a.FROM_AIRP)= UPPER(P_FROM_AIRP) )
			    and  (P_TO_AIRP is null or P_TO_AIRP='' or UPPER(a.TO_AIRP)= UPPER(P_TO_AIRP) )
			    and  (P_ETD is null or P_ETD='' or UPPER(a.ETD)= UPPER(P_ETD) )
			    and  (P_ETA is null or P_ETA='' or UPPER(a.ETA)= UPPER(P_ETA) )
			    and  (P_ATD is null or P_ATD='' or UPPER(a.ATD)= UPPER(P_ATD) )
			    and  (P_ATA is null or P_ATA='' or UPPER(a.ATA)= UPPER(P_ATA) )
			    and  (P_VIA is null or P_VIA='' or UPPER(a.VIA)= UPPER(P_VIA) )
			    and  (P_CRAFT_TYPE is null or P_CRAFT_TYPE='' or UPPER(a.CRAFT_TYPE)= UPPER(P_CRAFT_TYPE) )
			    and  (P_ROUTE is null or P_ROUTE='' or UPPER(a.ROUTE)= UPPER(P_ROUTE) )
			    and  (P_ROUTE_TT is null or P_ROUTE_TT='' or UPPER(a.ROUTE_TT)= UPPER(P_ROUTE_TT) )
			    --and  (P_FLIGHT_ID_CUS is null or P_FLIGHT_ID_CUS='' or UPPER(a.FLIGHT_ID_CUS)= UPPER(P_FLIGHT_ID_CUS) )
			    --and  (P_FLIGHTDATE is null or P_FLIGHTDATE='' or a.FLIGHTDATE = to_date(P_FLIGHTDATE,'dd-mm-yyyy'))
			    and  (P_FLIGHTDATE is null or P_FLIGHTDATE='' or to_date(a.LETTERNBR_PK) = to_date(P_FLIGHTDATE))
		    )u order by u.LETTERNBR_PK desc) where rnum between P_FIRST_INDEX and  P_LAST_INDEX;
	end realplan_letter_HasChange;
    PROCEDURE GET_LETTER_TYPE(P_OUT_CURSOR OUT T_CURSOR) IS
	BEGIN
	    OPEN P_OUT_CURSOR FOR
		SELECT distinct(LETTER_TYPE) FROM T_Realplan_Letter;
	END GET_LETTER_TYPE;
    PROCEDURE UPDATE_REALPLAN_LETTER(P_USER varchar2,P_IS_CHECK number,P_ID number,P_RETURN_CODE OUT NUMBER) IS
    BEGIN
	UPDATE T_REALPLAN_LETTER set IS_CHECK = P_IS_CHECK , USER_CHECK=P_USER where IS_CHECK = 0 and ID = P_ID;
	commit;
	P_RETURN_CODE := P_ID;
    END UPDATE_REALPLAN_LETTER;
    PROCEDURE MessNotCheck(P_PAGE_SIZE IN int,
				 P_PAGE_INDEX IN INT,
				 P_LETTERNBR_PK   DATE,
				 P_NBR		  VARCHAR2,
				P_LETTER_TYPE	 VARCHAR2 ,
				P_STATUS	 VARCHAR2,
				--P_FLIGHT_ID	   NUMBER ,
				P_FLIGHTNBR	 VARCHAR2,
				P_REGISTRATION	 VARCHAR2 ,
				P_FROM_AIRP	 VARCHAR2 ,
				P_TO_AIRP	 VARCHAR2 ,
				P_ETD		 VARCHAR2,
				P_ETA		 VARCHAR2 ,
				P_ATD		 VARCHAR2 ,
				P_ATA		 VARCHAR2,
				P_VIA		 VARCHAR2 ,
				P_CRAFT_TYPE	 VARCHAR2 ,
				P_FLIGHTDATE	 DATE	,
				P_CODE		 VARCHAR2 ,
				--P_ISFIR	   NUMBER   ,
				P_ROUTE 	 VARCHAR2 ,
				P_ROUTE_TT	 VARCHAR2 ,
				--P_FLIGHT_ID_CUS  NUMBER  ,
				--P_ISSELECT	   NUMBER ,
				 P_OUT_CURSOR OUT T_CURSOR) IS
	     P_FIRST_INDEX int;
	    P_LAST_INDEX int;

    BEGIN
	P_LAST_INDEX := P_PAGE_SIZE * (P_PAGE_INDEX );
	    P_FIRST_INDEX := P_LAST_INDEX - P_PAGE_SIZE;
	    Open P_OUT_CURSOR for  SELECT * FROM
		(select u.* ,rownum as rnum from
		    (select a.*,count(*) over() as Record_Sum
			from T_REALPLAN_LETTER a where
			(P_NBR is null or P_NBR='' or UPPER(a.NBR)= UPPER(P_NBR) )
			--and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(a.LETTER_TYPE)= UPPER(P_LETTER_TYPE) )
			and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(a.LETTER_TYPE)like '%'||UPPER(P_LETTER_TYPE)||'%' )
			and  (P_STATUS is null or P_STATUS='' or UPPER(a.STATUS)= UPPER(P_STATUS) )
			--and  (P_FLIGHT_ID is null or P_FLIGHT_ID='' or UPPER(a.FLIGHT_ID)= UPPER(P_FLIGHT_ID) )
			and (p_FLIGHTNBR is null or p_FLIGHTNBR='' or UPPER(a.FLIGHTNBR) like '%'||UPPER(p_FLIGHTNBR)||'%' )
			and  (P_REGISTRATION is null or P_REGISTRATION='' or UPPER(a.REGISTRATION)= UPPER(P_REGISTRATION) )
			and  (P_FROM_AIRP is null or P_FROM_AIRP='' or UPPER(a.FROM_AIRP)= UPPER(P_FROM_AIRP) )
			and  (P_TO_AIRP is null or P_TO_AIRP='' or UPPER(a.TO_AIRP)= UPPER(P_TO_AIRP) )
			and  (P_ETD is null or P_ETD='' or UPPER(a.ETD)= UPPER(P_ETD) )
			and  (P_ETA is null or P_ETA='' or UPPER(a.ETA)= UPPER(P_ETA) )
			and  (P_ATD is null or P_ATD='' or UPPER(a.ATD)= UPPER(P_ATD) )
			and  (P_ATA is null or P_ATA='' or UPPER(a.ATA)= UPPER(P_ATA) )
			and  (P_VIA is null or P_VIA='' or UPPER(a.VIA)= UPPER(P_VIA) )
			and  (P_CRAFT_TYPE is null or P_CRAFT_TYPE='' or UPPER(a.CRAFT_TYPE)= UPPER(P_CRAFT_TYPE) )
			and  (P_ROUTE is null or P_ROUTE='' or UPPER(a.ROUTE)= UPPER(P_ROUTE) )
			and  (P_ROUTE_TT is null or P_ROUTE_TT='' or UPPER(a.ROUTE_TT)= UPPER(P_ROUTE_TT) )
			--and  (P_FLIGHT_ID_CUS is null or P_FLIGHT_ID_CUS='' or UPPER(a.FLIGHT_ID_CUS)= UPPER(P_FLIGHT_ID_CUS) )
			--and  (P_FLIGHTDATE is null or P_FLIGHTDATE='' or a.FLIGHTDATE = to_date(P_FLIGHTDATE,'dd-mm-yyyy'))
			and  (P_FLIGHTDATE is null or P_FLIGHTDATE='' or to_date(a.LETTERNBR_PK) = to_date(P_FLIGHTDATE))
			and a.IS_CHECK = 0
			) u order by u.LETTERNBR_PK desc) where rnum between P_FIRST_INDEX and	P_LAST_INDEX;
       END MessNotCheck;
    PROCEDURE MessIsCheck(P_PAGE_SIZE IN int,
				 P_PAGE_INDEX IN INT,
				 P_LETTERNBR_PK   DATE,
				 P_NBR		  VARCHAR2,
				P_LETTER_TYPE	 VARCHAR2 ,
				P_STATUS	 VARCHAR2,
				--P_FLIGHT_ID	   NUMBER ,
				P_FLIGHTNBR	 VARCHAR2,
				P_REGISTRATION	 VARCHAR2 ,
				P_FROM_AIRP	 VARCHAR2 ,
				P_TO_AIRP	 VARCHAR2 ,
				P_ETD		 VARCHAR2,
				P_ETA		 VARCHAR2 ,
				P_ATD		 VARCHAR2 ,
				P_ATA		 VARCHAR2,
				P_VIA		 VARCHAR2 ,
				P_CRAFT_TYPE	 VARCHAR2 ,
				P_FLIGHTDATE	 DATE	,
				P_CODE		 VARCHAR2 ,
				--P_ISFIR	   NUMBER   ,
				P_ROUTE 	 VARCHAR2 ,
				P_ROUTE_TT	 VARCHAR2 ,
				--P_FLIGHT_ID_CUS  NUMBER  ,
				--P_ISSELECT	   NUMBER ,
				 P_OUT_CURSOR OUT T_CURSOR) IS
	     P_FIRST_INDEX int;
	    P_LAST_INDEX int;
	BEGIN
	    P_LAST_INDEX := P_PAGE_SIZE * (P_PAGE_INDEX );
	    P_FIRST_INDEX := P_LAST_INDEX - P_PAGE_SIZE;
	    Open P_OUT_CURSOR for  SELECT * FROM
		(select u.* ,rownum as rnum from
		    (select a.*,count(*) over() as Record_Sum
			from T_REALPLAN_LETTER a where
			(P_NBR is null or P_NBR='' or UPPER(a.NBR)= UPPER(P_NBR) )
			--and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(a.LETTER_TYPE)= UPPER(P_LETTER_TYPE) )
			and (P_LETTER_TYPE is null or P_LETTER_TYPE='' or UPPER(a.LETTER_TYPE)like '%'||UPPER(P_LETTER_TYPE)||'%' )
			and  (P_STATUS is null or P_STATUS='' or UPPER(a.STATUS)= UPPER(P_STATUS) )
			--and  (P_FLIGHT_ID is null or P_FLIGHT_ID='' or UPPER(a.FLIGHT_ID)= UPPER(P_FLIGHT_ID) )
			and (p_FLIGHTNBR is null or p_FLIGHTNBR='' or UPPER(a.FLIGHTNBR) like '%'||UPPER(p_FLIGHTNBR)||'%' )
			and  (P_REGISTRATION is null or P_REGISTRATION='' or UPPER(a.REGISTRATION)= UPPER(P_REGISTRATION) )
			and  (P_FROM_AIRP is null or P_FROM_AIRP='' or UPPER(a.FROM_AIRP)= UPPER(P_FROM_AIRP) )
			and  (P_TO_AIRP is null or P_TO_AIRP='' or UPPER(a.TO_AIRP)= UPPER(P_TO_AIRP) )
			and  (P_ETD is null or P_ETD='' or UPPER(a.ETD)= UPPER(P_ETD) )
			and  (P_ETA is null or P_ETA='' or UPPER(a.ETA)= UPPER(P_ETA) )
			and  (P_ATD is null or P_ATD='' or UPPER(a.ATD)= UPPER(P_ATD) )
			and  (P_ATA is null or P_ATA='' or UPPER(a.ATA)= UPPER(P_ATA) )
			and  (P_VIA is null or P_VIA='' or UPPER(a.VIA)= UPPER(P_VIA) )
			and  (P_CRAFT_TYPE is null or P_CRAFT_TYPE='' or UPPER(a.CRAFT_TYPE)= UPPER(P_CRAFT_TYPE) )
			and  (P_ROUTE is null or P_ROUTE='' or UPPER(a.ROUTE)= UPPER(P_ROUTE) )
			and  (P_ROUTE_TT is null or P_ROUTE_TT='' or UPPER(a.ROUTE_TT)= UPPER(P_ROUTE_TT) )
			--and  (P_FLIGHT_ID_CUS is null or P_FLIGHT_ID_CUS='' or UPPER(a.FLIGHT_ID_CUS)= UPPER(P_FLIGHT_ID_CUS) )
			--and  (P_FLIGHTDATE is null or P_FLIGHTDATE='' or a.FLIGHTDATE = to_date(P_FLIGHTDATE,'dd-mm-yyyy'))
			and  (P_FLIGHTDATE is null or P_FLIGHTDATE='' or to_date(a.LETTERNBR_PK) = to_date(P_FLIGHTDATE))
			and a.IS_CHECK=1
			) u order by u.LETTERNBR_PK desc) where rnum between P_FIRST_INDEX and	P_LAST_INDEX;
	END MessIsCheck;
    Procedure getTimeMdb_To_Inbox(p_date date, p_out out varchar2) is
	begin
	    select to_char(max(DATETIMEMDB),'dd-mm-yyyy hh24:mi:ss') into p_out from T_Qlbinbox where to_char(in_time,'dd-mm-yyyy') = to_char(sys_extract_utc(systimestamp),'dd-mm-yyyy');
	    if p_out is null then
		p_out:= to_char(sys_extract_utc(systimestamp),'dd-mm-yyyy');
	    end if;
	    exception
	    when others then
		p_out:= to_char(sys_extract_utc(systimestamp),'dd-mm-yyyy');
	end getTimeMdb_To_Inbox;

    PROCEDURE GetInboxBySearch(
    p_start    NUMBER,
    p_end      NUMBER,
    p_fromDate DATE,
    p_toDate   DATE,
    p_nbr      VARCHAR2,
    p_origin   VARCHAR2,
    p_content  VARCHAR2,
    p_out      OUT t_cursor
) IS
BEGIN
    OPEN p_out FOR
	WITH base AS
	(
	    SELECT
		r.nbr,
		'DVKL' AS type,
		r.id,
		r.letternbr_pk,
		ROW_NUMBER() OVER (
		    ORDER BY r.letternbr_pk DESC
		) AS rn,
		COUNT(*) OVER () AS total_records
	    FROM t_receive_log r
	    WHERE r.letternbr_pk >= p_fromDate
	      AND r.letternbr_pk < p_toDate + 1
	      AND (
		    p_nbr IS NULL
		    OR TRIM(p_nbr) IS NULL
		    OR UPPER(TRIM(r.nbr)) = UPPER(TRIM(p_nbr))
		  )
	      AND (
		    p_origin IS NULL
		    OR TRIM(p_origin) IS NULL
		    OR UPPER(r.from_pl) LIKE
		       '%' || UPPER(TRIM(p_origin)) || '%'
		  )
	      AND (
		    p_content IS NULL
		    OR TRIM(p_content) IS NULL
		    OR UPPER(r.content) LIKE
		       '%' || UPPER(TRIM(p_content)) || '%'
		  )
	)
	SELECT
	    nbr,
	    type,
	    id,
	    letternbr_pk,
	    total_records
	FROM base
	WHERE rn BETWEEN p_start AND p_end
	ORDER BY rn;
END GetInboxBySearch;
    Procedure GetInboxDetailBy(p_type varchar, p_id number, p_out out t_cursor) is
	nC number:=0;
	begin

	 select count(*) into nC from
	     T_Receive_Log where id = p_id;


	 --open p_out for
		 --  select LETTERNBR_PK, NBR,CONTENT,FROM_PL,ID,ORIGIN from T_Receive_Log where id = p_id;--T_Receive_Logfile


	 if (nC = 0) then
	    --insert into t_log values ('LOGINBOX1','',nC);commit;
	   open p_out for
		   select LETTERNBR_PK, NBR,CONTENT,FROM_PL,ID,ORIGIN from t_receive_logfile where id = p_id;--T_Receive_Logfile
	 else
	    --insert into t_log values ('LOGINBOX2','',nC);commit;
	     open p_out for
		   select LETTERNBR_PK, NBR,CONTENT,FROM_PL,ID,ORIGIN from t_receive_log where id = p_id;--T_Receive_Logfile
	end if;
	/*
	    if p_type = 'DVKL' then--T_Receive_Logfile
		open p_out for
		    select LETTERNBR_PK, NBR,CONTENT,FROM_PL,ID,ORIGIN from T_Receive_Log where id = p_id;
	    else
		open p_out for
		    select LETTERNBR_PK,NBR,CONTENT,ID from T_Notam_Inbox where id = p_id;

	    end if; */
	end GetInboxDetailBy;


	Procedure GetInboxBySearchLogFile(p_start number, p_end number
                            , p_fromDate date, p_toDate date, p_nbr varchar2, p_origin varchar2
                            , p_content varchar2, p_out out t_cursor) is
        v_total_records number;
    begin
        -- Chỉ đếm khi tải trang đầu. Các trang sau sử dụng tổng số đã lưu trên giao diện.
        if nvl(p_start, 1) <= 1 then
            select count(*)
              into v_total_records
              from T_Receive_Logfile r
             where r.letternbr_pk >= p_fromDate
               and r.letternbr_pk < p_toDate + 1
               and (
                    trim(p_nbr) is null
                    or upper(trim(r.nbr)) = upper(trim(p_nbr))
                   )
               and (
                    trim(p_origin) is null
                    or upper(r.from_pl) like '%' || upper(trim(p_origin)) || '%'
                   )
               and (
                    trim(p_content) is null
                    or upper(r.content) like '%' || upper(trim(p_content)) || '%'
                   );
        else
            v_total_records := null;
        end if;

        open p_out for
            select nbr,
                   type,
                   id,
                   letternbr_pk,
                   v_total_records as total_records
              from (
                    select ordered_rows.*,
                           rownum as rn
                      from (
                            select r.nbr,
                                   'DVKL' as type,
                                   r.id,
                                   r.letternbr_pk
                              from T_Receive_Logfile r
                             where r.letternbr_pk >= p_fromDate
                               and r.letternbr_pk < p_toDate + 1
                               and (
                                    trim(p_nbr) is null
                                    or upper(trim(r.nbr)) = upper(trim(p_nbr))
                                   )
                               and (
                                    trim(p_origin) is null
                                    or upper(r.from_pl) like '%' || upper(trim(p_origin)) || '%'
                                   )
                               and (
                                    trim(p_content) is null
                                    or upper(r.content) like '%' || upper(trim(p_content)) || '%'
                                   )
                             order by r.letternbr_pk desc
                           ) ordered_rows
                     where rownum <= p_end
                   )
             where rn >= p_start
             order by rn;
    end GetInboxBySearchLogFile;
    Procedure GetInboxDetailByLogFile(p_type varchar, p_id number, p_out out t_cursor) is
	begin
	 open p_out for
		   select LETTERNBR_PK, NBR,CONTENT,FROM_PL,ID,ORIGIN from T_Receive_Logfile where id = p_id;--T_Receive_Logfile

	end GetInboxDetailByLogFile;




    Procedure GetOutboxBySearch(p_start number, p_end number
			    , p_fromDate date, p_toDate date, p_nbr varchar2, p_origin varchar2
			    , p_content varchar2, p_out out t_cursor) is
	begin
	   open p_out for
	    select * from(
		select NBR, ID, LETTERNBR_PK from T_Send_Logfile
		    where LETTERNBR_PK between p_fromDate and p_toDate+1
			and (p_nbr is null or p_nbr ='' or upper(trim(nbr))=upper(trim(p_nbr)))
			and (p_origin is null or p_origin ='' or upper(trim(FROM_PL)) like '%'||upper(trim(p_origin))||'%')
			and (p_content is null or p_content ='' or upper(trim(content))like '%' ||upper(trim(p_content))||'%')
		) order by LETTERNBR_PK desc;
	end GetOutboxBySearch;

	Procedure GetOutByExport(p_date date, p_out out t_cursor) is
	begin
	   open p_out for
	    --select * from T_QlbOutBox where OT_CONT is not null order by ID asc;
	    select * from (select * from T_QlbOutBox where OT_CONT is not null order by ID asc) where rownum =1;
	end GetOutByExport;

	 Procedure DeleteOutByExport(p_id number, p_out out varchar2) is
	  begin
	     -- select to_char(max(DATETIMEMDB),'dd-mm-yyyy hh24:mi:ss') into p_out from T_Qlbinbox where to_char(in_time,'dd-mm-yyyy') = to_char(sys_extract_utc(systimestamp),'dd-mm-yyyy');
	  insert into T_SEND_LOGFILE(letternbr_pk,NBR,content,Type,FROM_PL)
	     select OT_STIME,ID,OT_CONT,'','' from T_QlbOutBox where id = p_id;

	   delete from T_QlbOutBox where id = p_id;

	   commit;

	   p_out:='1';
	    exception
	    when others then
		p_out:= '0';
	  end DeleteOutByExport;
    Procedure GetOutboxDetailBy(p_type varchar, p_id number, p_out out t_cursor) is
	begin
		open p_out for
		    select LETTERNBR_PK,NBR,CONTENT,ID from T_Send_Logfile where id = p_id;--T_Send_Logfile
	end GetOutboxDetailBy;

     PROCEDURE Get_MessageDetailAirPort(p_date varchar2, P_MESSTYPE varchar2, P_OUT_CURSOR out T_CURSOR) is
	begin
	    open P_OUT_CURSOR for
		select	MESS_TYPE,PART_NO,CONTENT from T_PLAN_MESSAGE_New
		  where to_char(FLIGHTDATE,'dd/mm/yyyy')=p_date

		  and MESS_TYPE = P_MESSTYPE
		   and LENGTH(MESS_TYPE)=4
		  order by MESS_TYPE asc,PART_NO asc;
	end Get_MessageDetailAirPort;
END MESSAGE_PKG;
/
