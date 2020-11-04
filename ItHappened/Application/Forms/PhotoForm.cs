using ItHappened.Domain.Customizations;

namespace ItHappened.Application
{
    public class PhotoForm
    {
        public byte[] Image { get; }

        public PhotoForm(byte[] image)
        {
            Image = image;
        }
    }
}