using Sabio.Web.Domain;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Web.Services;
using Sabio.Web.Models;
using Glimpse.AspNet.Tab;
using Sabio.Web.Models.Requests;
using Sabio.Web.Services.Interfaces;

namespace Sabio.Web.Controllers.Api
{

    [RoutePrefix("api/Comments")]
    public class CommentsAPIController : ApiController
    {
        private ICommentsService _commentsService;
        //private IUserService _userService;

        public CommentsAPIController(ICommentsService CommentsService/*, IUserService UserService*/)
        {
            _commentsService = CommentsService;
            //_userService = UserService;
        }

        [Route, HttpPost]
        public HttpResponseMessage Add(CommentAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();
            string userId = UserService.GetCurrentUserId();
            response.Item = _commentsService.Insert(model, userId);

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Edit(CommentUpdateRequest model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SucessResponse response = new SucessResponse();
            _commentsService.Update(model);
            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage Get(int id)
        {
            ItemResponse<Comment> response = new ItemResponse<Comment>();

            response.Item = _commentsService.Get(id);

            return Request.CreateResponse(response);
        }


        [Route("{ownerId:int}/{ownerTypeId:int}"), HttpGet]
        [Route("{ownerId:int}/{ownerTypeId:int}/{parentId:int}")]
        public HttpResponseMessage GetList(int ownerId, int ownerTypeId, int parentId = 0)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<Comment> response = new ItemsResponse<Comment>();

            response.Items = _commentsService.GetList(ownerId, ownerTypeId, parentId);

            return Request.CreateResponse(response);

        }

        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            SucessResponse response = new SucessResponse();

            _commentsService.Delete(id);

            return Request.CreateResponse(response);
        }

    }

}

