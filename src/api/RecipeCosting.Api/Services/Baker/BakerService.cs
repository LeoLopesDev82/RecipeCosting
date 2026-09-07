using Microsoft.EntityFrameworkCore;
using RecipeCosting.Api.Data;
using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Models.Results;

namespace RecipeCosting.Api.Services.Baker;

/// <inheritdoc cref="IBakerService"/>
public class BakerService : IBakerService
{
    private readonly RecipeCostingDbContext _context;

    public BakerService(RecipeCostingDbContext context)
    {
        _context = context;
    }

    public async Task<BakerResponse> GetAsync(CancellationToken cancellationToken)
    {
        var settings = await _context.BakerSettings
            .AsNoTracking()
            .FirstAsync(row => row.Id == BakerSettings.SingleRowId, cancellationToken);

        return ToResponse(settings);
    }

    public async Task<WriteResult<BakerResponse>> UpdateAsync(BakerRequest request, CancellationToken cancellationToken)
    {
        var settings = await _context.BakerSettings
            .FirstAsync(row => row.Id == BakerSettings.SingleRowId, cancellationToken);

        Apply(request, settings);

        _context.ExpectVersion(settings, request.Version);

        settings.Version = request.Version + 1;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return WriteResult<BakerResponse>.Stale();
        }

        return WriteResult<BakerResponse>.Written(ToResponse(settings));
    }

    public HourlyCostResponse Preview(BakerRequest request)
    {
        var settings = new BakerSettings();

        Apply(request, settings);

        return HourlyCostHelper.Of(settings);
    }

    #region Private methods

    private static void Apply(BakerRequest request, BakerSettings settings)
    {
        settings.MonthlyIncome = request.MonthlyIncome;
        settings.HoursPerDay = request.HoursPerDay;
        settings.DaysPerWeek = request.DaysPerWeek;
        settings.MonthlyFixedCosts = request.MonthlyFixedCosts;
        settings.DefaultMarkup = request.DefaultMarkup;
        settings.CardFee = request.CardFee;
        settings.Tax = request.Tax;
    }

    private static BakerResponse ToResponse(BakerSettings settings)
    {
        return new BakerResponse
        {
            MonthlyIncome = settings.MonthlyIncome,
            HoursPerDay = settings.HoursPerDay,
            DaysPerWeek = settings.DaysPerWeek,
            MonthlyFixedCosts = settings.MonthlyFixedCosts,
            DefaultMarkup = settings.DefaultMarkup,
            CardFee = settings.CardFee,
            Tax = settings.Tax,
            Version = settings.Version,
            HourlyCost = HourlyCostHelper.Of(settings),
        };
    }

    #endregion
}
