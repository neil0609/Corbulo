using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/OfficeHour")]
    public class OfficeHoursAPIController : ApiController
    {

        private IOfficeHourServices _officeHourServices;

        public OfficeHoursAPIController(IOfficeHourServices OfficeHourServices)
        {
            this._officeHourServices = OfficeHourServices;
        }


        [AllowAnonymous]
        [Route("Add"), HttpPost]
        public HttpResponseMessage Add(OfficeHourAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();
            string userId = UserService.GetCurrentUserId();
            response.Item = _officeHourServices.Add(model, userId);

            return Request.CreateResponse(response);
        }

        [Route("{id:int}/Edit"), HttpPut]
        public HttpResponseMessage Edit(OfficeHourUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SucessResponse response = new SucessResponse();
            string userId = UserService.GetCurrentUserId();
            _officeHourServices.Update(model, userId);
            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<OfficeHour> response = new ItemResponse<OfficeHour>();
            response.Item = _officeHourServices.Get(id);
            return Request.CreateResponse(response);
        }

        [Route, HttpGet]
        public HttpResponseMessage Get()
        {
            ItemsResponse<OfficeHour> response = new ItemsResponse<OfficeHour>();
            response.Items = _officeHourServices.GetList();
            return Request.CreateResponse(response);
        }

        [Route("{id:int}/Delete"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            SucessResponse response = new SucessResponse();

            _officeHourServices.Delete(id);

            return Request.CreateResponse(response);
        }

    }
}
