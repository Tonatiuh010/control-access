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

	SET @RESULT = '';
	CALL SET_DEPARTAMENT(
		NULL,
		'DEPTO 1',
		'D1',
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_JOB(
		NULL,
		'JOB',
		'TEST JOB',
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;

	SET @RESULT = '';
    CALL SET_ACCESS_LEVEL(
		NULL,
        'G1',
        'API_TEST',
        @RESULT
    );
    SELECT @RESULT;
	
    SET @RESULT = '';
    CALL SET_SHIFT(
		NULL,
        '4 X 3',
        maketime(8, 00, 00), 
        maketime(18, 00, 00),         
        maketime(12, 00, 00),
        0,
        'API_TEST',        
        @RESULT
    );
    SELECT @RESULT;
    
    SET @RESULT = '';
    CALL SET_POSITION(
		NULL,
        'BASE PLANT',        
		1,
		1,
		'API_TEST',	
        @RESULT
    );
    SELECT @RESULT;

	SET @RESULT = '';
	CALL SET_EMPLOYEE(
			1,
			'TONATIUH',
			'LOPEZ RAMIREZ',
			NULL,
			1,
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
		'1',		
		'1',
        'DISABLED',
		@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_DOWN_EMPLOYEE(		
		'1',				
        'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
    
    SET @RESULT = '';
	CALL SET_DOWN_CARD(		
		1,				
        'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
