namespace Lab1CSharp.Domain.Validators
{
    public class OfficeValidator: IValidator<Office>
    {
        public void Validate(Office e)
        {
            if (e == null || e.Location == null)
                throw new ValidationException("Invalid office!");
        }
    }
}