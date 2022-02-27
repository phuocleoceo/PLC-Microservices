using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

public class ValidationException : ApplicationException
{
    public ValidationException() : base("Validation failures")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        // Từ list các ValidationFailure, Group theo tên và error mess
        // Chuyển thành Dict có Key là failureKey và Value là mảng các failure
        Errors = failures.GroupBy(c => c.PropertyName, c => c.ErrorMessage)
                    .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
