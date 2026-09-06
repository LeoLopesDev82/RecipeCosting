using System.Text.Json.Serialization;

namespace RecipeCosting.Api.Models.Enums;

/// <summary>
/// How an ingredient is measured on the package it is bought in.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PackageUnit>))]
public enum PackageUnit
{
    /// <summary>Weight, in grams. Costed per kilogram.</summary>
    [JsonStringEnumMemberName("g")]
    Gram = 1,

    /// <summary>Volume, in millilitres. Costed per litre.</summary>
    [JsonStringEnumMemberName("ml")]
    Millilitre = 2,

    /// <summary>Countable pieces, such as eggs or cake boxes. Costed per piece.</summary>
    [JsonStringEnumMemberName("un")]
    Unit = 3,
}
