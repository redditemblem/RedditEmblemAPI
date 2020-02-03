using RedditEmblemAPI.Models.Exceptions;

namespace RedditEmblemAPI.Models.Output
{
    public class HP
    {
        public int Current { get; set; }
        public int Maximum { get; set; }

        public HP(int current, int maximum)
        {
            this.Current = current;
            this.Maximum = maximum;
        }

        public HP(string current, string maximum)
        {
            int val;
            if (!int.TryParse(current, out val) || val < 0)
                throw new PositiveIntegerException("Current HP", current);
            this.Current = val;

            if (!int.TryParse(maximum, out val) || val < 0)
                throw new PositiveIntegerException("Maximum HP", maximum);
            this.Maximum = val;
        }
    }
}
