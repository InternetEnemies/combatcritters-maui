/*
    The Deck Model represents a collection of cards
    and contain methods for managing the cards. 
*/

namespace Combat_Critters_2._0.Models
{
    public class Deck
    {
        public required string Name;
        public required List<Card> Cards { get; set; }
    }
}