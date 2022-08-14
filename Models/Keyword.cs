using System.ComponentModel.DataAnnotations;

namespace HTTPClientAPI.Models
{
    public class Keyword
    {

        public enum Values
        {
            LOS_ANGELES = 1,
            NEW_YORK = 2
        }

        public int Id { get; set; }

        [Display(Name = "Keyword")]
        public string? Value { get; set; }

    }
}
