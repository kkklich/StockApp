using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Netsoftware.Xanthos.Common.Resources.GridResources;
using Type = System.Type;

namespace Netsoftware.Xanthos.Common.Resources.Helpers;

public static class GridTableFeatures<TEntity> where TEntity : class
{
    public static IQueryable<TEntity> GetRows(GridParamsResource gridParams, IQueryable<TEntity> dbData,
        Expression<Func<TEntity, bool>> additionalFilters = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> defaultSorting = null)
    {
        var skip = gridParams.StartRow;
        var take = gridParams.EndRow - gridParams.StartRow;

        if (take <= 0)
        {
            var message = "Value of take parameter cannot be 0";
            throw new InvalidOperationException(message);
        }

        if (additionalFilters != null) dbData = dbData.Where(additionalFilters);

        if (defaultSorting != null) dbData = defaultSorting(dbData);

        var filtersQuery = CreateFilterQuery(gridParams.FilterModel);
        if (!string.IsNullOrWhiteSpace(filtersQuery)) dbData = dbData.Where(filtersQuery);

        var sortQuery = CreateSortQuery(gridParams.SortModel);
        if (!string.IsNullOrWhiteSpace(sortQuery)) dbData = dbData.OrderBy(sortQuery);

        return dbData.Skip(skip).Take(take);
    }

    public static Task<int> GetRowsCount(GridParamsResource gridParams, IQueryable<TEntity> dbData,
        Expression<Func<TEntity, bool>> additionalFilters = null)
    {
        if (additionalFilters != null) dbData = dbData.Where(additionalFilters);

        var filtersQuery = CreateFilterQuery(gridParams.FilterModel);
        if (!string.IsNullOrWhiteSpace(filtersQuery)) dbData = dbData.Where(filtersQuery);

        return dbData.CountAsync();
    }

    private static string CreateSortQuery(List<SortResource> sortModel)
    {
        var sortList = new List<string>();

        foreach (var sort in sortModel)
        {
            var capitalizedColId = sort.ColId.First().ToString().ToUpper() + sort.ColId.Substring(1);

            if (GetNestedPropertyType(capitalizedColId, typeof(TEntity)) == typeof(DateTime?))
                sortList.Add($"{capitalizedColId} == null, {capitalizedColId} {sort.Sort.ToString().ToLower()}");
            else
                sortList.Add($"{capitalizedColId} {sort.Sort.ToString().ToLower()}");
        }

        return string.Join(',', sortList);
    }

    private static Type GetNestedPropertyType(string propertyName, Type objType)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            throw new InvalidOperationException("Property name cannot be empty");

        var result = objType;

        foreach (var part in propertyName.Split('.'))
            if (result != null)
                result = result.GetProperty(part)?.PropertyType;

        return result;
    }

    private static string CreateFilterQuery(Dictionary<string, FiltersResource> filterModel)
    {
        var stringBuilder = new StringBuilder();

        foreach (var dict in filterModel)
        {
            var filters = dict.Value;
            var capitalizedPropertyName = dict.Key.First().ToString().ToUpper() + dict.Key.Substring(1);

            if (filters.Condition1 != null && filters.Condition2 != null && filters.Operator != null)
            {
                var firstCondition = filters.Condition1;
                var firstConditionQuery =
                    FilterByFilterType(firstCondition.FilterType, firstCondition.Type, capitalizedPropertyName,
                        firstCondition.Filter, firstCondition.FilterTo, firstCondition.DateFrom, firstCondition.DateTo);

                var secondCondition = filters.Condition2;
                var secondConditionQuery =
                    FilterByFilterType(secondCondition.FilterType, secondCondition.Type, capitalizedPropertyName,
                        secondCondition.Filter, secondCondition.FilterTo, secondCondition.DateFrom,
                        secondCondition.DateTo);

                if (filters.Operator == Operator.OR)
                    stringBuilder.Append(" && " + $"({firstConditionQuery} || {secondConditionQuery})");
                else if (filters.Operator == Operator.AND)
                    stringBuilder.Append(" && " + $"({firstConditionQuery} && {secondConditionQuery})");
                else
                    throw new InvalidOperationException("Operator is invalid");
            }

            else
            {
                stringBuilder.Append(" && " + FilterByFilterType(filters.FilterType, filters.Type,
                    capitalizedPropertyName, filters.Filter, filters.FilterTo, filters.DateFrom, filters.DateTo));
            }
        }

        var query = stringBuilder.ToString();

        if (query != "")
            // remove ampersands && at beggining
            query = query.Remove(0, 4);

        return query;
    }
#nullable enable
    private static string FilterByFilterType(FilterType filterType, GridResources.Type type, string propertyName,
        object? filter, int? filterTo, DateTime? dateFrom, DateTime? dateTo)
    {
        if (filterType == FilterType.Text)
        {
            if (filter == null) throw new InvalidOperationException("Filter cannot be null in text type filtering");
            return FilterTextType(type, propertyName, filter);
        }

        if (filterType == FilterType.Number)
            return FilterNumberType(type, propertyName, filter, filterTo);
        if (filterType == FilterType.Date)
            return FilterDateType(type, propertyName, dateFrom, dateTo);
        throw new InvalidOperationException($"FilterType: {filterType} is invalid");
    }

    private static string FilterDateType(GridResources.Type type, string propertyName, DateTime? dateFrom,
        DateTime? dateTo)
    {
        if (dateFrom == null) throw new InvalidOperationException("DateFrom cannot be null in date type filtering");

        var isNullableDate = GetNestedPropertyType(propertyName, typeof(TEntity)) == typeof(DateTime?);
        var date = isNullableDate ? $"{propertyName}.Value.Date" : $"{propertyName}.Date";
        var dateFromString = $"DateTime({dateFrom.Value.Year}, {dateFrom.Value.Month}, {dateFrom.Value.Day})";

        switch (type)
        {
            case GridResources.Type.Equals:
                return $"{date} == {dateFromString}";
            case GridResources.Type.NotEqual:
                return $"{date} != {dateFromString}";
            case GridResources.Type.LessThan:
                return $"{date} < {dateFromString}";
            case GridResources.Type.GreaterThan:
                return $"{date} > {dateFromString}";
            case GridResources.Type.InRange:
                if (dateTo == null)
                    throw new InvalidOperationException(
                        "DateTo property cannot be null when trying to filter with InRange Type");

                var dateToString = $"DateTime({dateTo.Value.Year}, {dateTo.Value.Month}, {dateTo.Value.Day})";

                return $"{date} >= {dateFromString} && {date} <= {dateToString}";

            default:
                throw new InvalidOperationException("Type is invalid");
        }
    }

    private static string FilterNumberType(GridResources.Type type, string propertyName, object? filter, int? filterTo)
    {
        if (filter == null) throw new InvalidOperationException("Filter cannot be null in number type filtering");

        switch (type)
        {
            case GridResources.Type.Equals:
                return $"{propertyName} == {filter}";
            case GridResources.Type.NotEqual:
                return $"{propertyName} != {filter}";
            case GridResources.Type.LessThan:
                return $"{propertyName} < {filter}";
            case GridResources.Type.LessThanOrEqual:
                return $"{propertyName} <= {filter}";
            case GridResources.Type.GreaterThan:
                return $"{propertyName} > {filter}";
            case GridResources.Type.GreaterThanOrEqual:
                return $"{propertyName} >= {filter}";
            case GridResources.Type.InRange:
                if (filterTo == null)
                    throw new InvalidOperationException(
                        "FilterTo property cannot be null when trying to filter with InRange Type");
                return $"{propertyName} >= {filter} && {propertyName} <= {filterTo}";

            default:
                throw new InvalidOperationException("Type is invalid");
        }
    }

    private static string FilterTextType(GridResources.Type type, string propertyName, object? filter)
    {
        var searchValue = filter?.ToString()?.ToLower();

        switch (type)
        {
            case GridResources.Type.Contains:
                return $"{propertyName}.ToLower().Contains(\"{searchValue}\") == true";
            case GridResources.Type.NotContains:
                return $"{propertyName}.ToLower().Contains(\"{searchValue}\") == false";
            case GridResources.Type.Equals:
                return $"{propertyName}.ToLower() == \"{searchValue}\"";
            case GridResources.Type.NotEqual:
                return $"{propertyName}.ToLower() != \"{searchValue}\"";
            case GridResources.Type.StartsWith:
                return $"{propertyName}.ToLower().StartsWith(\"{searchValue}\")";
            case GridResources.Type.EndsWith:
                return $"{propertyName}.ToLower().EndsWith(\"{searchValue}\")";

            default:
                throw new InvalidOperationException("Type is invalid");
        }
    }
}