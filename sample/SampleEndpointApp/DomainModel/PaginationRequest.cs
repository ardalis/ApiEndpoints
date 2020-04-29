using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleEndpointApp.DomainModel
{
    public class PaginationRequest
	{
		public int Page { get; set; } = 1;

		public int PerPage { get; set; } = 10;
	}
}
