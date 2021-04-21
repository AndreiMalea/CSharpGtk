namespace Lab1CSharp.Domain.Validators
{
    public class EmployeeValidator: IValidator<Employee>
    {
        public void Validate(Employee e)
        {
            if (e==null || e.Name == null || e.Office == null || e.Position == null)
                throw new ValidationException("Invalid employee!");
        }
    }
}