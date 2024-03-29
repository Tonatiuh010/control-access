/*#######################  GET OWNERS CARD ###########################*/
DROP FUNCTION IF EXISTS GET_CARD_DETAIL;
DELIMITER //
CREATE FUNCTION GET_CARD_DETAIL (
	IN_NUMBER VARCHAR(20)
) RETURNS INT
DETERMINISTIC
BEGIN
	DECLARE VL_EMPLOYEE INT DEFAULT 0;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN 
		SET VL_EMPLOYEE = NULL;
    END;
	
    SELECT EMPLOYEE_ID 
    INTO VL_EMPLOYEE 
    FROM CARD C 
    WHERE C.NUMBER = IN_NUMBER;
    
    RETURN VL_EMPLOYEE;
END //
DELIMITER ;

/*#######################  GET_CHECK ###########################*/

DROP FUNCTION IF EXISTS GET_CHECK;
DELIMITER //
CREATE FUNCTION GET_CHECK(
	IN_EMPLOYEE INT,
    IN_TYPE_FLAG BOOL
) RETURNS DATETIME
DETERMINISTIC
BEGIN
	DECLARE VL_CHECK_DT DATETIME DEFAULT NULL;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN 
		SET VL_CHECK_DT = NULL;
    END;	
    
    SELECT 
		CASE WHEN IN_TYPE_FLAG THEN MAX(CHECK_DT) ELSE MIN(CHECK_DT) END INTO VL_CHECK_DT 
	FROM CARD_CHECK CC 
    WHERE 
		EMPLOYEE_ID = IN_EMPLOYEE
	AND DATE(CHECK_DT) = DATE(NOW())
    LIMIT 1;
    
    RETURN VL_CHECK_DT;
END // 
DELIMITER ;

/*######################### SET EMPLOYEE ##############################*/
DROP PROCEDURE IF EXISTS SET_EMPLOYEE;
DELIMITER // 
CREATE PROCEDURE SET_EMPLOYEE (
	IN IN_EMPLOYEE INT,
	IN IN_NAME VARCHAR(50),
    IN IN_LAST_NAME VARCHAR(50),
    IN IN_POSITION INT,
    IN IN_SHIFT INT,
    IN IN_IMG LONGTEXT,
    IN IN_USER VARCHAR(50),
    OUT OUT_RESULT VARCHAR(500)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_EMPLOYEE] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
    SET OUT_RESULT = 'OK';
    
	UPDATE EMPLOYEE SET
		FIRST_NAME = IN_NAME,
		LAST_NAME = IN_LAST_NAME,
		STATUS = 'ENABLED',
		UPDATED_ON = NOW(),
		UPDATED_BY = IN_USER,
		POSITION_ID = IN_POSITION,
		SHIFT = IN_SHIFT,
        IMAGE = IFNULL( UNHEX(IN_IMG), IMAGE )
	WHERE EMPLOYEE_ID = IN_EMPLOYEE;
    
	IF ROW_COUNT() = 0 THEN 
		INSERT INTO EMPLOYEE (
			EMPLOYEE_ID,
			FIRST_NAME,
            LAST_NAME,
            STATUS,
            CREATED_ON,
            CREATED_BY ,
            POSITION_ID,
            SHIFT
            ,IMAGE
        ) VALUES (
			IN_EMPLOYEE, -- IFNULL(IN_EMPLOYEE, GET_NEXT_VAL('CTL_ACCESS', 'EMPLOYEE')),
			IN_NAME,
            IN_LAST_NAME,
			'ENABLED',
            NOW(),
            IN_USER,
			IN_POSITION,
            IN_SHIFT
            ,UNHEX(IN_IMG)
        );
    END IF;
END //
DELIMITER ;

/* TESTING PROCEDURE  
	SET @RESULT = '';
	CALL SET_EMPLOYEE(
			NULL,
			'TONATIUH',
			'LOPEZ RAMIREZ',
			NULL,
			NULL,
            '89504e470d0a1a0a0000000d4948445200000320000002580803000000ada872420000001974455874536f6674776172650041646f626520496d616765526561647971c9653c00000033504c5445ffffffcccccce5e5e5f2f2f2d9d9d9fcfcfcd6d6d6dfdfdff5f5f5cfcfcfe9e9e9d2d2d2efefeff9f9f9ecececdcdcdce2e2e2c763af2f00000a604944415478daecdd6b979a3a1880d18a780175f4ffffda73dae9bd0386248c21d9fb7bd762d1f7194c40fcf205000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000f8ea7ced08763d9b9896e2e8fa1d0bf59d489a70188fa63dce713c989fdaf3e806831e6fe82452b5ebc590a7b95c4d51bd978fbb014f777711a9756deef291e72262b55ea537ab8f5c2b9137d354611f063b1f85e80385b4b47d65a6f3b29955d7fadcfa23f73ac44abda6fd5d37cff3df56b7db5b8f8779ceef61ae6ab137cd6bd89bac4ab841b8ce0d43936587177bbd2e20b884b805829b217cc423bcabb99bae0aee8198e3f5b81762898e657ad54ec6783d27f3b5799e3259f37913f3b579337ffe3c6f17e43c7311767636ffbf3bf55f3b785222d87ef261687f6336ff7feb492267916953b7097ba76689deadc24a7536287398da2cef9c9a4a03f1d920cb672c81080481080481084420024120024120024120024120024120024120024120021188401088401008021108021108021108021108021108d50572d88f5dff43d7ed0f02118840bebb3efe7d01ddf17115880913c897db63eabd50c3e32610da0e64dfcfbe7db3df0b847603b9f54f5f50dbdf0442a3817441ef70ee04428b819c435f0d7f3c0b84e602b90ec1bf0330bc0984c6021917fd544627109a0a64e94f539d044243812cffe9b69340682690989f363c0984460289fb6ddc51203411c83ef2472daf02a181406e436420c34d20d41f48bf8bd50b84ea031977f14681507920872121904a3f640944203f9d76294e02a1ea406ebb343781507320a7c4404e02a1e2400ebb540781506f20637220a340a837906372204781506d20b75dba9b40a83590314320a340a835907b8640ee02a1d640860c810c02a1d2400ebb9d458840043215c83e4b209ffcb2c5ee20103e2790314b209fbb4a3fed8e0781f02981745902e93eb78fddfa850844201903797c721feb172210817c73cf1248ffd97dac5e884004f24dbfb1407e3d7abc6e210211c81603f9fdd1fc550b1188403618c89f5f5d59b3108108e483912bfc5993bf0f76c5420422908cbb58dd4bfa58b3108108646b817c74b15bad108108e49b6b9640de5ed4c77a85084420b38350dcb358538ba5950a118840de6509e4757dac55884004f2ee98a18fe30bfb58a9108108e4dd234320a757f6b14e21021148be55faf5a57dac52884004f22ec7570a0fafed638d42042290efd29fe7bdbfba8f150a118840b27dc6babebc8ffc850844203f5c12fbb814d047f6420422901fde1203194be82377210211c8cf657ada25643814d147e6420422904c9790b742fac85b884004f24bcaddf463317d642d442002f9e59c10c8b99c3e721622108104fca3977e1324e6cb8ed90a1188407e17fbd5f4beac3ef21522108164d8c95a71072bf6cbf2990a118840fe5c86c4fc0cc2702eae8f5c85084420c98514d947a642042290d4420aed234f210211486221c5f691a5108108e4df4296dc30bc94db478e420422900ff6b2c2bf1bd21f0aee2343210211c847c6c08f592b3ec19be765a8a9850844201fba85dc32ec6fa5f7915c88400432e1faec9ee165cd6f10e6ea23b510810864d2dbdc55e4b8ea6b46f3f5915888400432b79ff5f8f83272799cbf6ca58fb442042290278d8cf73f23b9dcc7f3cac79eb78fa4420422909031d98fdd57e3fe135e4f9dbd8f9442042290d2e4ef23a1108108a4813ee20b1188405ae823ba108108a4893e620b118840dae823b2108108a4913ee20a1188405ae923aa108108a4993e620a118840dae923a2108108a4a13e9617221081b4d4c7e242042290a6fa585a884004d2561f0b0b1188401aeb6359210211486b7d2c2a44200269ae8f2585084420edf5b1a0108108a4c13ec20b1188405aec23b8108108a4c93e420b118840daec23b0108108a4d13ec20a118840129d0f1bed23a810810824b18f21fa953aafee23a410810824b18fe81786bcbe8f804317884052fb882ca4843e9e1fba400492dc47542165f4f1f4d0052290f43e220a29a58f67872e108164e8637121e5f4f1e4d00522901c7d2c2ca4a43ee60f5d2002c9d2c7a242caea63f6d00522903c7d2c28a4b43ee60e5d2002c9d4477021e5f53173e8021148ae3e020b29b18fe943178840b2f5115448997dec76778108245720537d0414526a1fbb5e2002c914c8741f4f0b29b60f8108245720737d3c29a4dc3e0422904c81ccf7315b48c17d084420790279d6c74c2125f72110816409e4791f938514dd874004922390903e260a29bb0f8108244320617d7c5848e17d084420e98184f6f14121a5f7211081240712dec73f8514df874004921ac8923efe2aa4fc3e042290c44096f5f147211be8432002490b64691fbf15b2853e042290a44096f7f1b3904df4211081a40412d3c7f742b6d18740049210485c1fdf0ad9481f0211487c20b17dfc5fc856fa108840a20389ef6343042290c8409ae8432002890ca48d3e042290b8401ae9432002890aa4953e04229098409ae94320028908a49d3e042290e58134d4874004b2389096fa1088409606d2541f0211c8c240daea432002591648637d0844208b0269ad0f8108644920cdf52110812c08a4bd3e042290f0401aec432002090ea4c53e042290d0409aec432002090ca4cd3e042290b0401aed432002090aa4d53e04229090409aed4320020908a4dd3e042290e78134dc874004f2349096fb1088409e05d2741f0211c89340daee432002990fa4f13e042290d9405aef432002990da4df09442002118840108840108840108840108840108840108840108840042210810844200211884004221004221004221004221004221004221004221004221081084420021188400482400482400482400482400482400482400482400462c2042210810844200211884004824004824004824004824004c2660239ef1b7716884066024120021188401088401088401088401088401088401088401088400422100422100422108108048108048108048108048108048108048108844d1a27fe671f4ecd128f89d3383a3595fee91b0ece4db8c3e042dc5820bbbb7313eebe1348ad7ffba65f70737376c2dca65f86e43abc79c3cc3ba03a02ccbc2b6c305f9bd7fcabe05ef09a3936a433c6ebb1cbbb7d6763bc9eb3f9dabe8b395ecbc574f98c854f58956f521ae4b5d828afc2c924afe364b65c427001a9dec32cafc1f39eb538d8c85a630bcb6326d5d81be7fc3ca768ab175bbc8db89be8bc7c5ba0b265c8d14ce774b4005108fa5008fae09d3beaeea0336734db39789349b5ce3e66a57fbcf21d90aa2f2283114f31b87cd4be56ef3c7712ff74496775de80ebc96524e6e271ba9a9d56ecbbbbb79d2cd0df3b8f5e0100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000064f39f0003004d2901e05bed70070000000049454e44ae426082',
			'API_TEST',
			@RESULT
	);
	SELECT @RESULT;
*/

/*######################### SET DEPARTAMENT ##############################*/
DROP PROCEDURE IF EXISTS SET_DEPARTAMENT;
DELIMITER //
CREATE PROCEDURE SET_DEPARTAMENT (
	IN IN_DEPTO INT,
    IN IN_NAME VARCHAR(50),
    IN IN_CODE VARCHAR(20),
    IN IN_USER VARCHAR(40),
    OUT OUT_RESULT VARCHAR(500)
) BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_DEPARTAMENT] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
    SET OUT_RESULT = 'OK';
	
	UPDATE DEPARTAMENT SET
		NAME = IN_NAME,
		CODE = IN_CODE,
		STATUS = 'ENABLED',
		UPDATED_ON = NOW(),
		UPDATED_BY = IN_USER
	WHERE DEPARTAMENT_ID = IN_DEPTO;
    
	IF ROW_COUNT() = 0 THEN 
		INSERT INTO DEPARTAMENT (
			DEPARTAMENT_ID,
			NAME,
            CODE,
            STATUS,
            CREATED_ON,
            CREATED_BY 
		) VALUES (
			IN_DEPTO, -- IFNULL(IN_DEPTO, GET_NEXT_VAL('CTL_ACCESS', 'DEPARTAMENT')),
			IN_NAME,
            IN_CODE,
			'ENABLED',
            NOW(),
            IN_USER
        );
    END IF;
END //
DELIMITER ;

/* TESTING PROCEDURE  
	SET @RESULT = '';
	CALL SET_DEPARTAMENT(
		1,
		'DEPTO 1',
		'D1',
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
*/

/*######################### SET JOB ##############################*/
DROP PROCEDURE IF EXISTS SET_JOB;
DELIMITER // 
CREATE PROCEDURE SET_JOB (
	IN IN_JOB INT,
	IN IN_NAME VARCHAR(50),
    IN IN_DESCRIPTION TEXT,
    IN IN_USER VARCHAR(50),
    OUT OUT_RESULT VARCHAR(500)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_JOB] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;    
    SET OUT_RESULT = 'OK';
        
	UPDATE JOB SET
		NAME = IN_NAME,
		DESCRIPTION = IN_DESCRIPTION,
		STATUS = 'ENABLED',
		UPDATED_ON = NOW(),
		UPDATED_BY = IN_USER
	WHERE JOB_ID = IN_JOB;
        
	IF ROW_COUNT() = 0 THEN 
		INSERT INTO JOB (
			JOB_ID,
			NAME,
            DESCRIPTION,
            STATUS,
            CREATED_ON,
            CREATED_BY
        ) VALUES (
			IN_JOB,-- IFNULL(IN_JOB, GET_NEXT_VAL('CTL_ACCESS', 'JOB')),
			IN_NAME,
            IN_DESCRIPTION,
			'ENABLED',
            NOW(),
            IN_USER
        );
    END IF;
END //
DELIMITER ;

/* TESTING PROCEDURE  
	SET @RESULT = '';
	CALL SET_JOB(
		NULL,
		'JOB',
		'TEST JOB',
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
*/

/*######################### SET CARD ##############################*/
DROP PROCEDURE IF EXISTS SET_CARD;
DELIMITER // 
CREATE PROCEDURE SET_CARD (
	IN IN_CARD_ID INT,
	IN IN_NUMBER VARCHAR(20),
    IN IN_EMPLOYEE INT,
    IN IN_USER VARCHAR(50),
    OUT OUT_RESULT VARCHAR(500)
) BEGIN 
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_CARD] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    
    SET OUT_RESULT = 'OK';
    
	UPDATE CARD SET
		NUMBER = IFNULL(IN_NUMBER, NUMBER),
		EMPLOYEE_ID = IFNULL( IN_EMPLOYEE, EMPLOYEE_ID),
		STATUS = 'ENABLED',
		UPDATED_ON = NOW(),
		UPDATED_BY = IN_USER
	WHERE CARD_ID = IN_CARD_ID;
    
	IF ROW_COUNT() = 0 THEN 
		INSERT INTO CARD (
			CARD_ID,
            NUMBER,
            EMPLOYEE_ID,
            STATUS,
            CREATED_ON,
            CREATED_BY
        ) VALUES (
			IN_CARD_ID,-- IFNULL(IN_CARD_ID, GET_NEXT_VAL('CTL_ACCESS', 'CARD')),
			IN_NUMBER,
            IN_EMPLOYEE,
			'ENABLED',
            NOW(),
            IN_USER
        );
    END IF;
END //
DELIMITER ;

/* TESTING PROCEDURE  
	SET @RESULT = '';
	CALL SET_CARD(
		NULL,
		'12345',
		1,
		'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
*/

/*######################### SET CARD_CHECK ##############################*/
DROP PROCEDURE IF EXISTS SET_CARD_CHECK;
DELIMITER // 
CREATE PROCEDURE SET_CARD_CHECK (
	IN IN_NUMBER VARCHAR(20),
    IN IN_DEVICE INT,
    IN IN_USER VARCHAR(50),
    OUT OUT_RESULT VARCHAR(500)
) BEGIN 
	DECLARE VL_EMPLOYEE INT DEFAULT GET_CARD_DETAIL(IN_NUMBER);
    DECLARE VL_TYPE VARCHAR(10) DEFAULT NULL;
	DECLARE VL_CARD INT;
    DECLARE VL_CHECK_DT DATETIME DEFAULT NOW();
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_CARD_CHECK] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
	SET OUT_RESULT = 'OK';
	SELECT CARD_ID INTO VL_CARD FROM CARD C WHERE C.EMPLOYEE_ID = VL_EMPLOYEE;
    
    IF VL_CARD IS NOT NULL THEN  
		SET VL_CHECK_DT = NOW();
		SET VL_TYPE = IF( VL_CHECK_DT > GET_CHECK(VL_EMPLOYEE, FALSE), 'OUT', 'IN');
        
        IF VERIFY_ACCESS(VL_EMPLOYEE, IN_DEVICE) = 1 THEN 
			INSERT INTO CARD_CHECK 
			(
				EMPLOYEE_ID,
				TIME_EXP,
				CHECK_DT,
				TYPE,
				CARD_ID,
				STATUS,
				CREATED_ON,
				CREATED_BY,
                DEVICE_ID
			) VALUES (
				VL_EMPLOYEE,
				DATE_FORMAT(VL_CHECK_DT, '%H%i%s'),
				VL_CHECK_DT,
				VL_TYPE,
				VL_CARD,
				'ENABLED',
				NOW(),
				IN_USER,
                IN_DEVICE
			);
			
			CALL SET_DEVICE(IN_DEVICE, NULL, TRUE, IN_USER, OUT_RESULT);
            
		ELSE 
			CALL SET_DEVICE(IN_DEVICE, NULL, FALSE, IN_USER, OUT_RESULT);
			SET OUT_RESULT = 'Empleado no autorizado';
        END IF;		
	ELSE
		CALL SET_DEVICE(IN_DEVICE, NULL, FALSE, IN_USER, OUT_RESULT);
		SET OUT_RESULT = 'Tarjeta no encontrada, o tarjeta en desuso.';
	END IF;
END //
DELIMITER ;

/* TESTING PROCEDURE  
	CALL SET_CARD_CHECK(		
		'76235467',		
        4,
		'API_TEST',
		@RESULT
	);
*/

/*#########################  SET_ACCESS_EMPLOYEE #########################*/
DROP PROCEDURE IF EXISTS SET_ACCESS_EMPLOYEE;
DELIMITER //
CREATE PROCEDURE SET_ACCESS_EMPLOYEE (
	IN IN_EMPLOYEE INT,
    IN IN_ACCESS INT,
    IN IN_STATUS VARCHAR(30),
    IN IN_USER 	VARCHAR(50),
	OUT OUT_RESULT VARCHAR(500)
) BEGIN 
	DECLARE VL_EXISTS INT DEFAULT 0;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_ACCESS_EMPLOYEE] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
	SET OUT_RESULT = 'OK';
	
	UPDATE EMPLOYEE_ACCESS_LEVEL SET
		STATUS = IN_STATUS,
        UPDATED_ON = NOW(),
        UPDATED_BY = IN_USER
	WHERE	
		EMPLOYEE_ID = IN_EMPLOYEE
	AND ACCESS_LEVEL_ID = IN_ACCESS;
    
    IF ROW_COUNT() = 0 THEN 
		INSERT INTO EMPLOYEE_ACCESS_LEVEL (
			EMPLOYEE_ID,
            ACCESS_LEVEL_ID,
            STATUS,
            CREATED_ON,
            CREATED_BY
        ) VALUES (
			IN_EMPLOYEE,
            IN_ACCESS,
            IN_STATUS,
            NOW(),
            IN_USER
        );
	END IF;					  
END //
DELIMITER ;

/* TESTING PROCEDURE
	SET @RESULT = '';
	CALL SET_ACCESS_EMPLOYEE(		
		'1',		
		'1',
        'DISABLED',
        'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
*/	

/*#########################  SET_DOWN_EMPLOYEE #########################*/
DROP PROCEDURE IF EXISTS SET_DOWN_EMPLOYEE;
DELIMITER //
CREATE PROCEDURE SET_DOWN_EMPLOYEE(
	IN IN_EMPLOYEE INT,    
    IN IN_USER VARCHAR(40),
	OUT OUT_RESULT VARCHAR(500)
) BEGIN 	
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_DOWN_EMPLOYEE] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;		
    SET OUT_RESULT = 'OK';
    
	UPDATE EMPLOYEE 
	SET 
		STATUS = 'DISABLED',
		UPDATED_BY = IN_USER,
		UPDATED_ON = NOW()
	WHERE
		EMPLOYEE_ID = IN_EMPLOYEE;			
END //
DELIMITER ;

/* TESTING PROCEDURE
	SET @RESULT = '';
	CALL SET_DOWN_EMPLOYEE(		
		'1',				
        'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
*/	

/*#########################  SET_DOWN_CARD #########################*/
DROP PROCEDURE IF EXISTS SET_DOWN_CARD;
DELIMITER //
CREATE PROCEDURE SET_DOWN_CARD(
	IN IN_CARD_ID INT,    
    IN IN_USER VARCHAR(40),
	OUT OUT_RESULT VARCHAR(500)
) BEGIN 	
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
		GET DIAGNOSTICS CONDITION 1 @SQL_STATUS = RETURNED_SQLSTATE, @ERR_MSG = MESSAGE_TEXT;    
		SET OUT_RESULT := CONCAT('ERROR -> ON [SET_DOWN_DOWN] ',  @SQL_STATUS, ' - ', @ERR_MSG);
	END;
    SET OUT_RESULT = 'OK';
    
	UPDATE CARD 
	SET 	
        EMPLOYEE_ID = NULL,
		UPDATED_BY = IN_USER,
		UPDATED_ON = NOW(),
        STATUS = 'DISABLED'
	WHERE
		CARD_ID = IN_CARD_ID;        
END //
DELIMITER ;

/* TESTING PROCEDURE
	SET @RESULT = '';
	CALL SET_DOWN_CARD(		
		1,				
        'API_TEST',
		@RESULT
	);
	SELECT @RESULT;
*/
