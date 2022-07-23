using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BL.Delegates;
using Engine.BO;
using Classes;
using Engine.Constants;

namespace Classes;

public abstract class CustomContoller : ControllerBase
{
    public ControlAccessBL bl {get;}

    protected Delegates.CallbackExceptionMsg OnMissingProperty => SetErrorOnRequest;
    protected List<RequestError> ErrorsRequest {get; set;} = new List<RequestError>();    

    public CustomContoller() {
        bl = new ControlAccessBL(SetErrorOnRequest);
    }

    protected void SetErrorOnRequest(Exception ex, string msg) => ErrorsRequest.Add(
        new RequestError() {
            Info = msg,
            Exception = ex
        }
    );

    protected Result RequestResponse(
        Delegates.ActionResult action, 
        Delegates.ActionResult? action2 = null, 
        Delegates.ActionResult? action3 = null
    ) => RequestBlock(result => {
        if (result != null) {
            result.Data = action();

            if(action2 != null)
                result.Data2 = action2();

            if(action3 != null)
                result.Data3 = action3();
        }        
    });

    protected Result RequestResponse(Delegates.ActionResult_R action) => RequestBlock( 
        result => {
            if (action != null)
                result = action();
        }
    );

    private Result RequestBlock(Delegates.CallbackResult action) {
        Result result = new Result(){
            Status = C.OK
        };

        try {
            action(result);

            if(ErrorsRequest != null && ErrorsRequest.Count > 0){
                throw new Exception("Errores en request");
            }

            result.Message = C.COMPLETE;
        } catch (Exception ex) {
            ErrorResult(result, ex);
        }
        return result;
    }

    private void ErrorResult(Result result, Exception? ex = null) {
        result.Status = C.ERROR;        
        result.Data = RequestError.FormatErrors(ErrorsRequest);

        if(ex != null) {
            result.Message = ex.Message;            
        } else {
            result.Message = "Error doing something!";
        }
    }
    
}