using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BooksCatalog;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    [Description("Title")]
    Title,
    [Description("Author")]
    Author,
    [Description("ISBN")]
    ISBN,
    [Description("Keyword")]
    Keyword
}