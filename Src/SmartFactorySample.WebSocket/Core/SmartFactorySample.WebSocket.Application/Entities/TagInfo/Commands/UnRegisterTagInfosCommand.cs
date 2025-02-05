
using System.Collections.Generic;

namespace SmartFactorySample.WebSocket.Application.Entities.TagInfo.Commands
{
    public class UnRegisterTagInfosCommand 
	{
		public List<int> TagIds { get; set; }
	}
}
