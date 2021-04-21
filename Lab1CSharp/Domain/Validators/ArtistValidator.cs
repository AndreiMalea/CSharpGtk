namespace Lab1CSharp.Domain.Validators
{
    public class ArtistValidator: IValidator<Artist>
    {
        public void Validate(Artist e)
        {
            if (e == null || e.Genre == null || e.Name == null)
                throw new ValidationException("Artist is invalid!");
        }
    }
}