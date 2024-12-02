namespace Combat_Critters_2._0;
public class OfferTemplateSelector : DataTemplateSelector
{
    public DataTemplate? CardTemplate { get; set; }
    public DataTemplate? PackTemplate { get; set; }
    public DataTemplate? CurrencyTemplate { get; set; }
    protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
    {
        if (item == null)
        {
            Console.WriteLine("ParsedItem is null.");
            return null;
        }
        var itemType = item.GetType();

        // Check for specific properties to determine the type
        if (itemType.GetProperty("CardId") != null)
        {
            return CardTemplate;
        }

        if (itemType.GetProperty("PackId") != null)
        {
            return PackTemplate;
        }

        if (itemType.GetProperty("_coins") != null)
        {
            return CurrencyTemplate;
        }

        Console.WriteLine("ParsedItem type could not be determined.");
        return null; // Fallback for unknown types
    }

}