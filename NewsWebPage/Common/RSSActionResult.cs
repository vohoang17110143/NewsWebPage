using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace NewsWebPage.Common
{
    public class RSSActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public RSSActionResult()
        {
        }

        public RSSActionResult(SyndicationFeed feed)
        {
            this.Feed = feed;
        }

        public void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";
            Rss20FeedFormatter formatter = new Rss20FeedFormatter(this.Feed);
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.ContentType))
            {
                formatter.WriteTo(writer);
            }
        }
    }
}