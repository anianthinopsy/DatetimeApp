using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]/[action]")]
public class DateController : ControllerBase
{
    private readonly DateService.DateServiceClient _dateServiceClient;

    public DateController(DateService.DateServiceClient dateServiceClient)
    {
        _dateServiceClient = dateServiceClient;
    }

    [HttpPost]
    [ActionName("calculateDate")]
    public async Task<IActionResult> GetNextDate([FromBody]DateRequestDto dateRequestDto)
    {
        if (dateRequestDto.DayOfMonth > 31 || dateRequestDto.DayOfMonth == 0)
        {
            return BadRequest("Invalid data sent to server. The number of day must be less than 31 and more than 0");
        }
        else
        {
            var grpcRequest = new DateRequest
            {
                Date = dateRequestDto.Date.ToString("yyyy-MM-dd"),
                DayOfMonth = dateRequestDto.DayOfMonth,
                Adjust = true
            };
            try
            {
                DateResponse grpcResponse = await _dateServiceClient.ResultDateAsync(grpcRequest);

                return Ok(new
                {
                    grpcResponse.ResultDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"gRPC error: {ex.Message}");
            }
        }
    }
}