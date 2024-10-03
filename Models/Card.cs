
/*
This card model represent individual cards 
and its properties
*/

namespace Combat_Critters_2._0.Models
{
    public class Card
    {
        // Properties
        public int CardId { get; set; }                   // Unique identifier for the card
        public required string Name { get; set; }                  // Name of the card (Required)
        public int PlayCost { get; set; }                 // Cost to play the card (Required)
        public int Rarity { get; set; }                   // Rarity of the card (Required)
        public required string Image { get; set; }                    // Image URI (Required)
        public required string Type { get; set; }                  // Type of card (Required, critter, item, type_specific)


        // Description of the card
        public required string Description { get; set; }           // Description of the card


        //Card background style is based on its rarity

        //For testing putposes we will use rareCard
        // public string CardStyleKey => Rarity switch
        // {
        //     1 => "CommonCardStyle",
        //     2 => "UncommonCardStyle",
        //     3 => "RareCardStyle",
        //     4 => "EpicCardStyle",
        //     5 => "LegendaryCardStyle",
        //     _ => "CommonCardStyle" // Fallback to common if no match 
        // };

    }
}
