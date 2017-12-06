namespace RecipesCore.Services
{
    public interface IImageService
    {
        byte[] GetImage(string directory, long id);

        byte[] GetThumbnail(string directory, long id);

        void BuildCache(string directory);

        string BuildCache(string directory, long id);

        void BuildThumbnails(string directory);

        string BuildThumbnail(string directory, long id);
    }
}