using System;
using Engine.BO;

namespace Engine.BL.Delegates;

public static class Delegates {
    // Errors
    public delegate void CallbackException(Exception ex);
    public delegate void CallbackExceptionMsg(Exception ex, string msg);

    // Callbacks
    public delegate object ActionResult();
    public delegate Result ActionResult_R();
    public delegate void CallbackResult(Result result);
}