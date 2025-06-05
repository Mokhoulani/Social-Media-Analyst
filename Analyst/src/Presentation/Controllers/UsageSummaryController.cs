using Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
public class UsageSummaryController(ISender sender) : ApiController(sender)
{

}
