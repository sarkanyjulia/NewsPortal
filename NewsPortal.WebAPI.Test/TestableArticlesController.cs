using Microsoft.AspNetCore.Mvc;
using NewsPortal.Persistence;
using NewsPortal.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPortal.WebAPI.Test
{
    public class TestableArticlesController : ArticlesController
    {
        public TestableArticlesController(NewsPortalContext context) : base(context)
        {
        }


        protected override int GetUserId()
        {
            return 1;
        }
    }
}
