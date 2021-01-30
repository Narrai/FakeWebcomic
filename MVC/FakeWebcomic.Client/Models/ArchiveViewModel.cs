using System;

namespace FakeWebcomic.Client.Models
{
    public class ArchiveViewModel
    {
        public long EntityId { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
