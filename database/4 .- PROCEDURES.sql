/*######################### SET ACCESS LEVEL ##############################*/

DROP PROCEDURE IF EXISTS SET_ACCESS_LEVEL;
DELIMITER //
CREATE PROCEDURE SET_ACCESS_LEVEL (
	IN 		IN_ACCESS_ID	INT,
	IN 	 	IN_NAME			VARCHAR(20),
    IN 		IN_USER 		VARCHAR(90),
    OUT		OUT_RESULT		VARCHAR(500)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_ACCESS_LEVEL] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
	SET OUT_RESULT = 'OK';
    
    UPDATE ACCESS_LEVEL SET
		NAME = IN_NAME,
        STATUS = 'ENABLED',
        UPDATED_ON = NOW(),
        UPDATED_BY = IN_USER
	WHERE 
		ACCESS_LEVEL_ID = IN_ACCESS_ID;
	
    IF ROW_COUNT() = 0 THEN 
		INSERT ACCESS_LEVEL (
			ACCESS_LEVEL_ID,
			NAME,
            STATUS,
            CREATED_ON,
            CREATED_BY
        ) VALUES (
			IN_ACCESS_ID,
            IN_NAME,
            'ENABLED',
            NOW(),
            IN_USER
        );
    END IF;
    
END //
DELIMITER ;

/* TESTING PROCEDURE
	SET @RESULT = '';
    CALL SET_ACCESS_LEVEL(
		1,
        'G1',
        'API_TEST',
        @RESULT
    );
    SELECT @RESULT;
*/

/*######################### SET SHIFT ##############################*/
DROP PROCEDURE IF EXISTS SET_SHIFT;
DELIMITER //
CREATE PROCEDURE SET_SHIFT(
	IN 	IN_SHIFT		INT,
    IN 	IN_NAME			VARCHAR(50),
    IN 	IN_CLOCK_IN 	TIME,
    IN  IN_CLOCK_OUT	TIME,
    IN 	IN_LUNCH 		TIME,
    IN 	IN_DAY 			INT,
    IN 	IN_USER 		VARCHAR(40),
    OUT	OUT_RESULT 		VARCHAR(500)
) BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_SHIFT] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
	SET OUT_RESULT = 'OK';
    
    UPDATE SHIFT SET
		NAME = IN_NAME,
        CLOCK_IN = IN_CLOCK_IN,
        CLOCK_OUT = IN_CLOCK_OUT,
        LUNCH_TIME = IN_LUNCH,
        DAY_COUNT = IN_DAY,
        STATUS = 'ENABLED',
        UPDATED_ON = NOW(),
        UPDATED_BY = IN_USER
	WHERE 
		SHIFT_ID = IN_SHIFT;
    
    IF ROW_COUNT() = 0 THEN
		INSERT INTO SHIFT (
			SHIFT_ID,
            NAME,
            CLOCK_IN,
            CLOCK_OUT,
            LUNCH_TIME,
            DAY_COUNT,
            STATUS,
            CREATED_ON,
            CREATED_BY
        ) VALUES (
			IN_SHIFT,
            IN_NAME,
            IN_CLOCK_IN,
            IN_CLOCK_OUT,
            IN_LUNCH,
            IN_DAY,
            'ENABLED',
            NOW(),
            IN_USER
        );
    END IF;
END //
DELIMITER ;

/* TESTING PROCEDURE
	SET @RESULT = '';
    CALL SET_SHIFT(
		1,
        '4 X 3',
        maketime(8, 00, 00), 
        maketime(18, 00, 00),         
        maketime(12, 00, 00),
        0, 
        'API_TEST',
        @RESULT
    );
    SELECT @RESULT;
*/

/*######################### SET POSITION ##############################*/

DROP PROCEDURE IF EXISTS SET_POSITION;
DELIMITER //
CREATE PROCEDURE SET_POSITION (
	IN 	IN_POSITION INT,
    IN 	IN_NAME VARCHAR(50),
    IN 	IN_DEPTO INT,
    IN 	IN_JOB 	INT,
    IN 	IN_USER	VARCHAR(40),
    OUT OUT_RESULT VARCHAR(500)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_POSITION] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
	SET OUT_RESULT = 'OK';
    
    UPDATE POSITION SET 
		NAME = IN_NAME,
        DEPARTAMENT_ID = IN_DEPTO,
        JOB_ID = IN_JOB,
        STATUS = 'ENABLED',
        UPDATED_ON = NOW(),
        UPDATED_BY = IN_USER
	WHERE 
		POSITION_ID = IN_POSITION;
        
    IF ROW_COUNT() = 0 THEN 
		INSERT INTO POSITION (
			POSITION_ID,
            NAME,
            DEPARTAMENT_ID,
            JOB_ID,
            STATUS,
            CREATED_ON,
            CREATED_BY
        ) VALUES (
			IN_POSITION,
            IN_NAME,
            IN_DEPTO,
            IN_JOB,
            'ENABLED',
            NOW(),
            IN_USER
        );        
    END IF;
END //
DELIMITER ;

/* TESTING PROCEDURE 
	SET @RESULT = '';
    CALL SET_POSITION(
		1,
        'BASE PLANT',        
		1,
		1,
		'API_TEST',	
        @RESULT
    );
    SELECT @RESULT;
*/