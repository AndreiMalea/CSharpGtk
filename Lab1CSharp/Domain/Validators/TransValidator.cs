namespace Lab1CSharp.Domain.Validators
{
    public class TransValidator: IValidator<Transaction>
    {
        public void Validate(Transaction e)
        {
            if (e == null || e.Client == null || e.TicketNumber <= 0)
                throw new ValidationException("Invalid transaction!");
        }
    }
}