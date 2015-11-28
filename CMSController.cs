using Sabio.Web.Classes;
using Sabio.Web.Domain;
using Sabio.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    [RoutePrefix("CMS")]
    public class CMSController : BaseController
    {
        public CMSController(ISiteConfig config) : base(config)
        { }

        // GET: CMS
        [Route("Add")]
        [Route("{id:int}")]
        public ActionResult Index(int id = 0)
        {
            ItemViewModel<int> model = new ItemViewModel<int>();
            model.Item = id;
            return View(model);
        }

        [Route]
        public ActionResult List()
        {
            //ItemViewModel<ListOfCMS> model = new ItemViewModel<ListOfCMS>();
            return View();
        }
    }
}
