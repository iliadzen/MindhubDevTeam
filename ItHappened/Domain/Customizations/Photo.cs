namespace ItHappened.Domain
{
    public class Photo : EventCustomizationData
    {
        public byte[] File { get;  }

        public Photo(string name, byte[] file)
        {
            File = file;
        }
    }
}