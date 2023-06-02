using DogsHouseService.Application.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Common.Exceptions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string name, object key)
			: base(string.Format(Resources.EntityNotFound, name, key)) { }
	}
}
