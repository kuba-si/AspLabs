using Microsoft.AspNetCore.Components.QuickGrid;
using QuickGridSamples.Core.Models;

namespace QuickGridSamples.Server.Data;

public class LocalDataService : IDataService
{
    private readonly ApplicationDbContext _dbContext;

    public LocalDataService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Country> Countries => _dbContext.Countries;

    public Task<GridItemsProviderResult<Country>> GetCountriesAsync(int startIndex, int? count, string sortBy, bool sortAscending, CancellationToken cancellationToken)
    {
        var countries = _dbContext.Countries.AsEnumerable();

        var ordered = (sortBy, sortAscending) switch
        {
            (nameof(Country.Name), true) => countries.OrderBy(c => c.Name, StringLengthComparer.Instance),
            (nameof(Country.Name), false) => countries.OrderByDescending(c => c.Name, StringLengthComparer.Instance),
            (nameof(Country.Code), true) => countries.OrderBy(c => c.Code),
            (nameof(Country.Code), false) => countries.OrderByDescending(c => c.Code),
            ("Medals.Gold", true) => countries.OrderBy(c => c.Medals.Gold),
            ("Medals.Gold", false) => countries.OrderByDescending(c => c.Medals.Gold),
            _ => countries.OrderByDescending(c => c.Medals.Gold),
        };

        var result = ordered.Skip(startIndex);

        if (count.HasValue)
        {
            result = result.Take(count.Value);
        }

        var array = result.ToArray();

        return Task.FromResult(GridItemsProviderResult.From(array, array.Length));
    }
}
