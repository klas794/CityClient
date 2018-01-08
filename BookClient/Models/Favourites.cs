using SQLite;

namespace CityClient.Models
{
    [Table("favourite")]
    public class Favourite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }
    }
}