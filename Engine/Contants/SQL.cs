using System;

namespace Engine.Constants {
    public class SQL {

        public const string DB_ACC = "CTL_ACCESS";
        public const string SET_CARD = "SET_CARD";
        public const string SET_CARD_CHECK = "SET_CARD_CHECK";
        public const string SET_DEPARTAMENT = "SET_DEPARTAMENT";
        public const string SET_EMPLOYEE = "SET_EMPLOYEE";
        public const string SET_JOB = "SET_JOB";

        public const string WEIRD_QRY = @"
            SELECT 
                CC.TIME_EXP, CC.CHECK_DT, CC.TYPE,
                E.FIRST_NAME, E.LAST_NAME
            FROM 
                CARD_CHECK CC, 
                CARD C, 
                EMPLOYEE E
            WHERE 
                CC.CARD_ID = C.CARD_ID
                AND E.EMPLOYEE_ID = C.EMPLOYEE_ID
        ";

    }
}