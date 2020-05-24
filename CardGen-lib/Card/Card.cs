using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CardGen_srv.Card
{
    public class Card
    {
       [Key]
       public int Id { get; set; }
       
       public List<CardColor> Color = new List<CardColor>();

       public Card(string subType, string cost)
       {
           SubType = subType;
       }

       public string SubType { get; set; }
       public int? Cost { get; set; }
       public string Text { get; set; }
       
       
       
    }
}