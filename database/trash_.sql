USE CTL_ACCESS;
SHOW TABLES;

SELECT 
	CC.TIME_EXP, CC.CHECK_DT, CC.TYPE,
	E.FIRST_NAME, E.LAST_NAME
FROM 
	CARD_CHECK CC, 
	CARD C, 
	EMPLOYEE E
WHERE 
	CC.CARD_ID = C.CARD_ID
	AND E.EMPLOYEE_ID = C.EMPLOYEE_ID;
	
SELECT * from access_level;
SELECT * FROM CARD;
SELECT * from card_check;
select * from departament;
select * from employee;
select * from employee_accesS_level;
select * from job;
select * FROM POSITION;
SELECT * FROM SHIFT;