using SmartFactorySample.Simulator.Application.Common.Enums;
using SmartFactorySample.Simulator.Application.Dtos;
using SmartFactorySample.Simulator.Application.Entities.Example.Commands.CreateExample;
using SmartFactorySample.Simulator.Application.Entities.Example.Commands.DeleteExample;
using SmartFactorySample.Simulator.Application.Entities.Example.Commands.UpdateExample;
using SmartFactorySample.Simulator.Application.Entities.Example.Queries.GetExample;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SmartFactorySample.Simulator.WebUI.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> CreateExample([FromBody] CreateExampleCommand command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("DeleteExample")]
        public async Task<IActionResult> DeleteExample([FromBody] DeleteExampleCommand command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("UpdateExample")]
        public async Task<IActionResult> UpdateExample([FromBody] UpdateExampleCommand command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }

        [HttpPost]
        [Route("GetExample")]
        public async Task<IActionResult> GetExample([FromBody] GetExampleQuery command)
        {
            // add log
            var result = await Mediator.Send(command);
            return new JsonResult(Ok(CreateResponse(result)));
        }
    }
}
