using PersonDirectory.DTO;
using System.ComponentModel.DataAnnotations;

namespace PersonDirectory.Model
{
    public class RelatedPersonModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Relation type is required.")]
        public RelationType Type { get; set; }

        public int PersonId { get; set; }

        public int RelatedPersonId { get; set; }
    }
}
