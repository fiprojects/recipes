namespace RecipesCore.Models
{
    public class TfIdfModel
    {
        public long Id { get; set; }
        
        public Recipe Recipe { get; set; }
        
        public string Term { get; set; }
        
        public double TfIdf { get; set; }
    }
}