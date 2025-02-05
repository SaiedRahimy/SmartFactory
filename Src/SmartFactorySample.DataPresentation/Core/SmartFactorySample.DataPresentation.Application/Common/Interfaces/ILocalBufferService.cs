using SmartFactorySample.DataPresentation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Application.Common.Interfaces
{
    public interface ILocalBufferService
    {
        void Push(TagInfoDto tag);
    }
}
