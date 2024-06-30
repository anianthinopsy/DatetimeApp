using Grpc.Core;

namespace GrpcService.Services;

public class CalculateDateService : DateService.DateServiceBase
{
    private readonly ILogger<CalculateDateService> _logger;

    public CalculateDateService(ILogger<CalculateDateService> logger)
    {
        _logger = logger;
    }
    
    public override Task<DateResponse> ResultDate(DateRequest request, ServerCallContext context)
    {
        try
        {
            DateTime currentDate = Convert.ToDateTime(request.Date);
            int dayOfMonth = request.DayOfMonth;
            bool adjust = request.Adjust;
            
            string calculatedDate = CalculateNextDate(currentDate, dayOfMonth, adjust);
            
            var response = new DateResponse
            {
                ResultDate = calculatedDate
            };

            return Task.FromResult(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred while processing the request."));
        }
    }
    
    private string CalculateNextDate(DateTime date, int dayOfMonth, bool adjust)
    {
        adjust = IsMost(dayOfMonth, date);

        if (adjust && dayOfMonth <= DateTime.DaysInMonth(date.Year, date.Month))
        {
            date = new DateTime(date.Year, date.Month, dayOfMonth,0, 0, 0, DateTimeKind.Utc);
        }
        else
        {
            date = new DateTime(date.Year, date.Month + 1, dayOfMonth, 0, 0, 0, DateTimeKind.Utc);
        
        }
        
        return date.ToString("yyyy-MM-dd");
    }

    private bool IsMost(int dayOfMonth, DateTime date)
    {
        bool isTrue;
        if (dayOfMonth > date.Day && dayOfMonth <= DateTime.DaysInMonth(date.Year, date.Month))
        {
            isTrue = true;
        }
        else
        {
            isTrue = false;
        }
        return isTrue;
    }
}