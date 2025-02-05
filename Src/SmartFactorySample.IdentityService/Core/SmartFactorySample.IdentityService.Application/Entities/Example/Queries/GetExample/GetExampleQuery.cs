using MediatR;
using SmartFactorySample.IdentityService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Application.Entities.Example.Queries.GetExample
{
    public class GetExampleQuery : IRequest<ExampleDto>
    {
        public int Id { get; set; }
    }
}
