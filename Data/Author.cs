using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_APP.Data
{
    [Table("Authors")]
    public partial class Author
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Bio { get; set; }

        public virtual IList<Book> Books { get; set; }
    }
}