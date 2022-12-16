using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebApplication1.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Company_name { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date_of_contract_sign { get; set; }
        public int Ingredients_count { get; set; }
    }
}
