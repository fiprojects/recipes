namespace RecipesCore.Critiquing
{
    public class Choice
    {
        public string Text { get; }

        public Evaluator Evaluator { get; }

        public Choice(string text, Evaluator evaluator)
        {
            Text = text;
            Evaluator = evaluator;
        }
    }
}