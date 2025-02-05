using SmartFactorySample.WebSocket.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Application.Common.Interfaces
{
    public interface ICacheService
    {
        void SetData(TagInfoDto tag);
        void SetData(List<TagInfoDto> tags);
        TagInfoDto? GetData(int key);
        List<TagInfoDto> GetData(List<int> keys);
    }
}
