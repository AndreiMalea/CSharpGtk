namespace Lab1CSharp.Domain.Validators
{
    public class ShowValidator : IValidator<Show>
    {
        public void Validate(Show e)
        {
            if (e == null || e.Artist == null || e.TicketNumber < 0)
                throw new ValidationException("Invalid show!");
        }
    }
}