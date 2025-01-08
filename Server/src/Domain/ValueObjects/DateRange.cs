using Domain.Base;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public class DateRange : BaseValueObject
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
            throw new DomainException("Start date must be before end date.");
        StartDate = startDate;
        EndDate = endDate;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }

    public override string ToString() => $"{StartDate} - {EndDate}";
}