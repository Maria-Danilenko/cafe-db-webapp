using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Sales
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "No value specified")]
        public int Dish_id { get; set; }

        [Required(ErrorMessage = "No value specified")]
        public int Type_id { get; set; }

        [Required(ErrorMessage = "No value specified")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false)]
        public DateTime Date_of_sale { get; set; }
    }
}
