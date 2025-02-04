using SmartFactorySample.DataReception.Application.Common.Enums;
using SmartFactorySample.DataReception.Application.Dtos;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.CreateTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.DeleteTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.UpdateTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Queries.GetTagInfo;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.WebUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ApiControllerBase
    {
        public TagController()
        {

        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return new JsonResult(BadRequest(ex));
            }
        }

        private BaseResponseDto<T> CreateResponse<T>(T dto)
        {
            return new BaseResponseDto<T>()
            {
                Dtos = new List<T>() { dto },
                IsSuccessful = true,
                StatusCode = HttpStatusCodeEnum.OK
            };
        }

        [HttpPost]
        [Route("CreateTag")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagInfoCommand command)
        {
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("DeleteTag")]
        public async Task<IActionResult> DeleteTag([FromBody] DeleteTagInfoCommand command)
        {
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("UpdateTag")]
        public async Task<IActionResult> UpdateTag([FromBody] UpdateTagInfoCommand command)
        {
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("GetTagInfo")]
        public async Task<IActionResult> GetTagInfo([FromBody] GetTagInfoQuery command)
        {
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }
    }
}
