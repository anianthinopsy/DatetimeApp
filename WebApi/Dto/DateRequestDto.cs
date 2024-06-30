namespace WebApi.Dto;

public class DateRequestDto
{
    public DateTime Date { get; set; }
    public int DayOfMonth { get; set; }
    public bool Adjust { get; set; }
}