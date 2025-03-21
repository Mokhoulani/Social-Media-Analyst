﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Abstractions;

[ApiController]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender; 
}
