using System.Linq;
using Terrasoft.Common;
using Terrasoft.Core.Entities;

namespace AtfTIDE.QueryExecutor
{
	public interface IEsqColumnParser
	{
		SortColumn GetSorting(EntitySchemaQuery esq);
	}

	public class EsqColumnParser : IEsqColumnParser
	{

		public SortColumn GetSorting(EntitySchemaQuery esq){
			return esq.Columns
				.Where(c => c.OrderPosition ==1)
				.Select(c=> new SortColumn {
					OrderPosition = c.OrderPosition,
					OrderDirection = c.OrderDirection,
					Name = c.Name
				}).FirstOrDefault();
		}
	}
	
	public class SortColumn
	{
		public string Name { get; set; }
		public OrderDirection OrderDirection { get; set; }
		public int OrderPosition { get; set; }
	}
}