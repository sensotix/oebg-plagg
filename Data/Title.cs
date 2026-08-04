using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEBG.PLAGG.Data {
    [Table("Titles", Schema = "dbo")]
    public class Title {
        [Key]
        public int Id { get; set; }

        [Column("Title")]
        public string? TitleText { get; set; }
    }
}
