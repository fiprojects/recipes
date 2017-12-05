namespace RecipesCore.Models
{
    public class TfIdfElement
    {
        public long Id { get; set; }
        
        public string Term { get; set; }

        public double TfIdf { get; set; }
    }
}
