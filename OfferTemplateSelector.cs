namespace Combat_Critters_2._0;
public class OfferTemplateSelector : DataTemplateSelector
{
    public DataTemplate? CardTemplate { get; set; }
    public DataTemplate? PackTemplate { get; set; }
    public DataTemplate? CurrencyTemplate { get; set; }
    protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
    {
        if (item is not null && item.GetType().GetProperty("Type") is { } property)
        {
            var typeValue = property.GetValue(item)?.ToString()?.ToLower();

            if (!string.IsNullOrEmpty(typeValue))
            {
                return typeValue switch
                {
                    "card" => CardTemplate,
                    "pack" => PackTemplate,
                    "currency" => CurrencyTemplate,
                    _ => null // Explicitly return null for unknown types
                };
            }
        }
        return null;
    }
}