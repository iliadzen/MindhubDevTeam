namespace ItHappened.Domain
{
    public class Photo
    {
        public string Name { get; }
        public byte[] File { get;  }

        public Photo(string name, byte[] file)
        {
            File = file;
            Name = name;
        }
    }
}