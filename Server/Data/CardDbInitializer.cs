using System.Linq;
namespace Server.Data
{
    public class CardDbInitializer
    {
        public static void Initialize(CardDbContext context)
        {
            if (!context.Card.Any())
            {
                context.Card.AddRange(
                 new Models.Card {
                     SubType = "w",
                     Cost = 1220,
                     Text = "aa",
                     Rarity = "N",
                     Type = "F",
                     Quality = "S",
                     Abilities = "None"
                 },
                 new Models.Card
                 {
                     SubType = "asf",
                     Cost = 10,
                     Text = "aaaaa",
                     Rarity = "MN",
                     Type = "T",
                     Quality = "G",
                     Abilities = "None"
                 });

            }
            context.SaveChanges();
        }
    }
}
