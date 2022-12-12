SET SERVEROUTPUT ON;

DECLARE cnt number;

BEGIN
    -- 判断时，username必须为全大写
	select count(1) into cnt FROM all_users where username='ASKRETRIVEUSER';
	IF cnt = 0 THEN
        execute immediate 'create user ASKRetriveUser identified by Ask2022';
        execute immediate 'grant create session to ASKRetriveUser';
        execute immediate 'grant connect to  ASKRetriveUser';
        execute immediate 'grant select any table to ASKRetriveUser';
	END IF;
    DBMS_OUTPUT.PUT_LINE('执行成功, success!');
END;
/

EXIT
