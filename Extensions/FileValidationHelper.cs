namespace MetreOr.Extensions
{
    public class FileValidationHelper
    {
        // Liste des types MIME d'image acceptés
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".bmp" };
        private readonly string[] _permittedMimeTypes = { "image/jpeg", "image/png", "image/bmp" };

        public bool IsImage(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }

            // Vérifie le type MIME
            if (!_permittedMimeTypes.Contains(file.ContentType))
            {
                return false;
            }

            // Vérifie l'extension de fichier
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_permittedExtensions.Contains(extension))
            {
                return false;
            }

            return true;
        }
    }
}
