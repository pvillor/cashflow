﻿using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class InvalidLoginException : CashFlowException
{
    public InvalidLoginException() : base("Invalid e-mail or password.")
    {
    }

    public override int StatusCode => (int) HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
