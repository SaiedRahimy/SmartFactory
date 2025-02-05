using SmartFactorySample.DataPresentation.Application.Common.Enums;
using System.Collections.Generic;

namespace SmartFactorySample.DataPresentation.Application.Dtos
{
    public class BaseResponseDto<T>
    {
        public List<T> Dtos;
        public bool IsSuccessful = true;
        public HttpStatusCodeEnum StatusCode;
    }
}
