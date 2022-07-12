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

/* TEST PROCEDURES */

	SET @RESULT = '';
	CALL SET_EMPLOYEE(
			1,
			'TONATIUH',
			'LOPEZ RAMIREZ',
			NULL,
			NULL,
			'API_TEST',
			@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_DEPARTAMENT(
		1,
		'DEPTO 1',
		'D1',
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_JOB(
		1,
		'JOB',
		'TEST JOB',
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_CARD(
		1,
		'12345',
		1,
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_CARD_CHECK(		
		'12345',		
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_ACCESS_EMPLOYEE(		
		1,		
		1,
        'DISABLED',
		@RESULT
	);
	SELECT @RESULT;
    
    UPDATE EMPLOYEE_ACCESS_LEVEL SET
		STATUS = 'ENABLED'
	WHERE	
		EMPLOYEE_ID = 1
	AND ACCESS_LEVEL_ID = 1;
    