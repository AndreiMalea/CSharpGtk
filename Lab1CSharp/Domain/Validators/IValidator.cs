namespace Lab1CSharp.Domain.Validators
{
    public interface IValidator<E>
    {
        void Validate(E e);
    }
}