using Sabio.Web.Domain;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{

    [AllowAnonymous]
    [RoutePrefix("api/user")]
    public class ProfilesApiController : ApiController
    {
        private ISectionService _sectionService;
        private IUserDataService _userDataService;
        private IOfficeHourServices _officeHours;

        public ProfilesApiController(ISectionService SectionService,
                                         IUserDataService userDataService, 
                                         IOfficeHourServices OfficeHourServices)
        {
            _sectionService = SectionService;
            _officeHours = OfficeHourServices;
            _userDataService = userDataService;
        }

        [Route("current"), HttpGet]
        public HttpResponseMessage GetsUser()
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<UserInfo> response = new ItemResponse<UserInfo>();
            string UserId = UserService.GetCurrentUserId();
            response.Item = _userDataService.GetByUserId(UserId);
            return Request.CreateResponse(response);
        }

        [Route("settings"), HttpGet]
        public HttpResponseMessage Get()
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<UserSettings> response = new ItemsResponse<UserSettings>();
            response.Items = UserService.GetSettings();
            return Request.CreateResponse(response);
        }

        [Route("sectiondetails"), HttpGet]
        public HttpResponseMessage GetsUserSectionDetails()
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<Section> response = new ItemResponse<Section>();
            string UserId = UserService.GetCurrentUserId();
            response.Item = _sectionService.GetSectionDetailsByUserId(UserId);
            return Request.CreateResponse(response);
        }

        [Route("OfficeHours"), HttpGet]
        public HttpResponseMessage GetByDate()
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<OfficeHour> response = new ItemsResponse<OfficeHour>();
            response.Items =_officeHours.GetByDate();
            return Request.CreateResponse(response);
        }


    }
}
