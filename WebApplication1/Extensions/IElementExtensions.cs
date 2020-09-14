using System;
using System.Globalization;
using AngleSharp.Dom;

namespace DatahawkPoc.Extensions
{
    public static class IElementExtensions
    {
        public static string GetReviewRating(this IElement elem) => 
            elem.QuerySelector("[data-hook=\"review-star-rating\"]")
                ?.TextContent.Substring(0, 1);

        public static DateTime GetReviewDate(this IElement elem)
        {
            var fullString = elem.QuerySelector("[data-hook=\"review-date\"]")?.TextContent;
            var dateString = fullString?.Substring(fullString.IndexOf("on", StringComparison.Ordinal) + 3);

            return DateTime.TryParseExact(dateString, "MMMM d, yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date)
                ? date
                : DateTime.MinValue;
        }

        public static string GetReviewContent(this IElement elem) =>
            elem.QuerySelector("[data-hook=\"review-body\"]")?.TextContent.Trim();

        public static string GetReviewTitle(this IElement elem) =>
            elem.QuerySelector("[data-hook=\"review-title\"]")?.TextContent.Trim();
    }
}
