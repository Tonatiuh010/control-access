using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BL.Delegates;
using Engine.BO;
using Classes;
using Engine.Constants;

namespace Classes;

public abstract class CustomController : ControllerBase
{
    public ControlAccessBL bl {get;}
    public Uri Uri => new ($"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}");
    public string DomainUrl => $"{Uri.Scheme}://{Uri.Authority}";

    protected Delegates.CallbackExceptionMsg OnMissingProperty => SetErrorOnRequest;
    protected List<RequestError> ErrorsRequest {get; set;} = new List<RequestError>();    

    public CustomController() {
        bl = new ControlAccessBL(SetErrorOnRequest)
        {
            //DomainUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}"
        };        
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
        Delegates.ActionResult? action3 = null,
        Delegates.ActionResult? action4 = null
    ) => RequestBlock(result => {
        if (result != null) {
            result.Data = action();

            if(action2 != null)
                result.Data2 = action2();

            if (action3 != null)
                result.Data3 = action3();

            if (action4 != null)
                result.Data4 = action4();

            result.Message = C.COMPLETE;
            
        }

        return result;
    });

    protected Result RequestResponse(Delegates.ActionResult_R action) => RequestBlock( 
        result => {
            if (action != null)
            {
                var res = action();
                result = res ?? new Result() { Status = "ERROR", Message = "Not Result Founded!!!" };
            }

            return result;
        }
    );

    private Result RequestBlock(Delegates.CallbackResult action) {
        bl.DomainUrl = DomainUrl;
        Result result = new(){
            Status = C.OK
        };

        try {
            result = action(result);

            if(ErrorsRequest != null && ErrorsRequest.Count > 0){
                throw new Exception("Errores en request");
            }

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

    public static T? GetItem<T>(List<T> list, string? emptyMsg = null)
    {
        if (list != null && list.Count > 0)
        {
            return list[0];
        }

        return default;
    }
    
}