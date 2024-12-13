using System;
using System.Collections.Generic;
using System.Linq;
using Terrasoft.Common;
using Terrasoft.Core.Entities;

namespace MrktApolloApp.VirtualEntityQueryExecutor
{
	public interface IEsqFilterParser
	{

		string GetSearchBoxFilterValue(EntitySchemaQuery esq);
		string GetEsqFilterValueByKey(string key, EntitySchemaQuery esq);
		IEnumerable<string> GetEsqFilterValueByKey2(string key, EntitySchemaQuery esq);

	}
	public class EsqFilterParser : IEsqFilterParser
	{

		/// <summary>
		/// Determines if a given EntitySchemaQueryFilterCollection is a searchBox filter.
		/// It checks for the right side of the filter expression on the collection of filters, and when they are all the same
		/// then the filter is considered a searchBox filter.
		/// </summary>
		private static readonly Func<IEnumerable<EntitySchemaQueryFilter>, bool> IsSearchBoxFilter = filters => {
			string seenParameterValue = string.Empty;
			IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters
				= filters as EntitySchemaQueryFilter[] ?? filters.ToArray();
			if (entitySchemaQueryFilters.ToList().Count == 1) {
				return false;
			}
			foreach (EntitySchemaQueryFilter filter in entitySchemaQueryFilters.Where(f => f.RightExpressions.Any())) {
				string pv = filter.RightExpressions[0].ParameterValue.ToString();
				if (seenParameterValue.IsNullOrEmpty()) {
					seenParameterValue = pv;
				} else if (seenParameterValue != pv) {
					return false;
				}
			}
			return true;
		};

		/// <summary>
		/// We have no way to tell filters apart. SearchBox and Folders will send the same thing List of filters,
		/// the only way to tell them apart I found is to check if all filters have the same parameter value, then its
		/// searchBox otherwise assume folder. I need to distinguish them because I need to know which filter for REST request
		/// When UI send All, I assume its FullName, otherwise I will take whatever folder sends.
		/// </summary>
		private static readonly Func<EntitySchemaQueryFilterCollection,
			Dictionary<string, IEnumerable<EntitySchemaQueryFilter>>> SeparateFiltersIntoGroups
			= filters => {
				IEnumerable<IEnumerable<EntitySchemaQueryFilter>> filterInstances = filters
					.Where(f => f is EntitySchemaQueryFilterCollection && f.IsEnabled)
					.Select(i => i.GetFilterInstances());

				Dictionary<string, IEnumerable<EntitySchemaQueryFilter>> dict
					= new Dictionary<string, IEnumerable<EntitySchemaQueryFilter>>();
				int ii = 0;

				filterInstances.ForEach(f => {
					IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters
						= f as EntitySchemaQueryFilter[] ?? f.ToArray();
					dict.Add(
						IsSearchBoxFilter(entitySchemaQueryFilters) ? $"searchBoxFilter:{ii}" : $"folderFilter:{ii}",
						entitySchemaQueryFilters);
					ii++;
				});
				return dict;
			};

		/// <summary>
		/// Gets filter value from EntitySchemaQueryFilters by key
		/// </summary>
		private static readonly Func<IEnumerable<EntitySchemaQueryFilter>, string, string>
			GetFolderFilterStringValueByKey = (entitySchemaQueryFilters, key) => entitySchemaQueryFilters
				.Where(f => f.IsEnabled && f.LeftExpression.Path == key)
				.Select(f => f.RightExpressions[0].ParameterValue.ToString())
				.FirstOrDefault() ?? string.Empty;
		
		
		private static IEnumerable<string> GetFolderFilterStringCollectionValueByKey(IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters, string key){
			var rightExpressions = 
				entitySchemaQueryFilters
					.Where(f => f.IsEnabled && f.LeftExpression.Path == key)
					.Select(s => s.RightExpressions);

			var collection = new List<string>();
			foreach (var rightExpression in rightExpressions) {
				collection.AddRange(rightExpression.Select(item => item.ParameterValue.ToString()));
			}
			
			return collection;
		}
		
		public string GetSearchBoxFilterValue(EntitySchemaQuery esq){
			string sbValue = string.Empty;
			Dictionary<string, IEnumerable<EntitySchemaQueryFilter>> filterGroups 
				= SeparateFiltersIntoGroups(esq.Filters);
			
			string sbKey = filterGroups.Keys.FirstOrDefault(k => k.StartsWith("searchBoxFilter"));
			if (!sbKey.IsNullOrWhiteSpace() &&
				filterGroups.TryGetValue(sbKey ?? "", out IEnumerable<EntitySchemaQueryFilter> searchBoxFilter)) {
				IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters = searchBoxFilter as EntitySchemaQueryFilter[] ?? searchBoxFilter.ToArray();
				sbValue
					= entitySchemaQueryFilters.FirstOrDefault()?.RightExpressions?.FirstOrDefault()?.ParameterValue.ToString() ??
					string.Empty;
			}
			return sbValue;
		}
		
		public string GetEsqFilterValueByKey(string key, EntitySchemaQuery esq){
			Dictionary<string, IEnumerable<EntitySchemaQueryFilter>> filterGroups 
				= SeparateFiltersIntoGroups(esq.Filters);
			
			string value = string.Empty;
			var ffKeys = filterGroups.Keys.Where(k => k.StartsWith("folderFilter"));
			IEnumerable<string> enumerableKeys = ffKeys as string[] ?? ffKeys.ToArray();
			if(enumerableKeys.Any()) {
				enumerableKeys.ForEach(ffKey=> {
					if (!ffKey.IsNullOrWhiteSpace() &&
						filterGroups.TryGetValue(ffKey ?? "", out IEnumerable<EntitySchemaQueryFilter> folderFilter)) {
						IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters
							= folderFilter as EntitySchemaQueryFilter[] ?? folderFilter.ToArray();
						value = string.IsNullOrWhiteSpace(value) 
							? GetFolderFilterStringValueByKey(entitySchemaQueryFilters, key)
							: value;
					}
				});
			}
			return value;
		}
		public IEnumerable<string> GetEsqFilterValueByKey2(string key, EntitySchemaQuery esq){
			Dictionary<string, IEnumerable<EntitySchemaQueryFilter>> filterGroups 
				= SeparateFiltersIntoGroups(esq.Filters);
			
			IEnumerable<string> value = Array.Empty<string>();
			var ffKeys = filterGroups.Keys.Where(k => k.StartsWith("folderFilter"));
			IEnumerable<string> enumerableKeys = ffKeys as string[] ?? ffKeys.ToArray();
			if(enumerableKeys.Any()) {
				enumerableKeys.ForEach(ffKey=> {
					if (!ffKey.IsNullOrWhiteSpace() &&
						filterGroups.TryGetValue(ffKey ?? "", out IEnumerable<EntitySchemaQueryFilter> folderFilter)) {
						IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters
							= folderFilter as EntitySchemaQueryFilter[] ?? folderFilter.ToArray();
						value = !value.Any() 
							? GetFolderFilterStringCollectionValueByKey(entitySchemaQueryFilters, key)
							: value;
					}
				});
			}
			return value;
		}
	}
}