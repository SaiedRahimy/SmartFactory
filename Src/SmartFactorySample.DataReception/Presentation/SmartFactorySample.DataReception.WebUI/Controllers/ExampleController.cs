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
    public class ExampleController : ApiControllerBase
    {
        public ExampleController()
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
                // add log
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
        [Route("CreateExample")]
        public async Task<IActionResult> CreateExample([FromBody] CreateTagInfoCommand command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("DeleteExample")]
        public async Task<IActionResult> DeleteExample([FromBody] DeleteTagInfoCommand command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("UpdateExample")]
        public async Task<IActionResult> UpdateExample([FromBody] UpdateTagInfoCommand command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("GetExample")]
        public async Task<IActionResult> GetExample([FromBody] GetTagInfoQuery command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }
    }
}
