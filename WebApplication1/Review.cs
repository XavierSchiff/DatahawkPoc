using System;

namespace DatahawkPoc
{
    public class Review
    {
        public string Asin { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewContent { get; set; }
        public string Rating { get; set; }
    }
}
