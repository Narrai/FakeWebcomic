namespace FakeWebcomic.Storage.Models
{
    public class User : AEntity
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
