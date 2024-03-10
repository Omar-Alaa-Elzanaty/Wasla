namespace Wasla.Model.Models
{
    public class Line
    {
        public int Id { get; set; }
        public int StartId { get; set; }
        public virtual Station Start { get; set; }
        public int EndId { get; set; }
        public virtual Station End { get; set; }
    }
}
