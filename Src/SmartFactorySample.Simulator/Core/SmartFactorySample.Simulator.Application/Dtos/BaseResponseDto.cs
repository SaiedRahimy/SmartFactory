using SmartFactorySample.Simulator.Application.Common.Enums;
using System.Collections.Generic;

namespace SmartFactorySample.Simulator.Application.Dtos
{
    public class BaseResponseDto<T>
    {
        public List<T> Dtos;
        public bool IsSuccessful = true;
        public HttpStatusCodeEnum StatusCode;
    }
}
