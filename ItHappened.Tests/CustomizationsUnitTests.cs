using System;
using System.Linq;
using NUnit.Framework;
using ItHappened.Domain;

namespace ItHappened.Tests
{
    public class CustomizationsUnitTests
    {
        [Test]
        public void TryToAddCustomizationOfExistedTypeToEvent_CustomizationAreNotAdded()
        {
            var photo = new Photo("photo", null);
            var photo2 = new Photo("photo2", null);
            var eventCustomization = new EventCustomization(Guid.NewGuid(), photo);
            eventCustomization.AddCustomization(photo2);
            var allCustomizations = eventCustomization.GetCustomizations();

            Assert.AreEqual(1, allCustomizations.Count);
        }
        
        [Test]
        public void AddCustomizationOfNewTypeToEvent_CustomizationAdded()
        {
            var photo = new Photo("photo", null);
            var longitude = 1.0;
            var tag = new Geotag(longitude, longitude);
            var eventCustomization = new EventCustomization(Guid.NewGuid(), photo);
            eventCustomization.AddCustomization(tag);
            var allCustomizations = eventCustomization.GetCustomizations();
            var one = (Photo)allCustomizations.ElementAt(0);
            var two = (Geotag)allCustomizations.ElementAt(1);
            
            Assert.AreEqual(2, allCustomizations.Count);
            Assert.AreEqual(longitude, two.Longitude);
        }
    }
}