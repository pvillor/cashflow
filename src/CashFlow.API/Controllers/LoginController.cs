﻿using CashFlow.Application.UseCases.Login;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUseCase useCase,
        [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
