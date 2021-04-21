using System;

namespace Lab1CSharp.Domain.Validators
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(String message): base(message)
        {
        }
    }
}