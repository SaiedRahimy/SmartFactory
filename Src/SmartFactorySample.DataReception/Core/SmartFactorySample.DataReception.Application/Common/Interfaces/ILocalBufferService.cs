using SmartFactorySample.DataReception.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Common.Interfaces
{
    public interface ILocalBufferService
    {
        void Push(TagInfoDto tag);
    }
}
