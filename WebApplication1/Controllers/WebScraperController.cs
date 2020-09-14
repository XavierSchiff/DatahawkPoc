using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using DatahawkPoc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DatahawkPoc.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebScraperController : ControllerBase
    {
        private readonly ILogger<WebScraperController> _logger;

        public WebScraperController(ILogger<WebScraperController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsinReviews(CancellationToken cancellationToken, string asin = "B082XY23D5")
        {
            try
            {
                var reviewTaskList = new List<Task<IEnumerable<Review>>>();
                for (var pageNum = 1; pageNum < 6; pageNum++)
                {
                    reviewTaskList.Add(GetPageReviews(asin, pageNum, cancellationToken));
                }
                var completedReviews = await Task.WhenAll(reviewTaskList);

                return Ok(completedReviews
                    .SelectMany(r => r)
                    .OrderByDescending(r => r.ReviewDate));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while Scraping Reviews");
                return BadRequest(e);
            }
        }

        private async Task<IEnumerable<Review>> GetPageReviews(string asin, int pageNum, CancellationToken cancellationToken)
        {
            var url = $"https://www.amazon.com/product-reviews/{asin}/ref=cm_cr_arp_d_viewopt_srt?sortBy=recent&pageNumber={pageNum}";

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url, cancellationToken);

            return document
                .QuerySelectorAll("[data-hook=\"review\"]")
                .Select(r => new Review
                {
                    Asin = asin,
                    Rating = r.GetReviewRating(),
                    ReviewContent = r.GetReviewContent(),
                    ReviewDate = r.GetReviewDate(),
                    ReviewTitle = r.GetReviewTitle()
                });
        }

    }
}
