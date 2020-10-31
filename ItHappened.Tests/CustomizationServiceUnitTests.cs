using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using NUnit.Framework;

namespace ItHappened.Tests
{
    public class CustomizationServiceUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockEventCustomizationRepository = new RepositoryMock<EventCustomization>();
            _mockTrackerRepository = new RepositoryMock<Tracker>();
            _mockEventRepository = new RepositoryMock<Event>();
            _customizationService = new CustomizationService(_mockTrackerRepository, _mockEventRepository,
                _mockEventCustomizationRepository);
        }

        [Test]
        public void CheckTrackerHasCustomizationOfSuchDataType_NoSuchCustomizationTypeInSet_False()
        {
            var customizations = new HashSet<CustomizationType>();
            var photo = new Photo("photo", null);

            var check = _customizationService.CheckTrackerHasCustomizationOfSuchDataType(customizations, photo);
            
            Assert.IsFalse(check);
        }
        
        [Test]
        public void CheckTrackerHasCustomizationOfSuchDataType_SuchCustomizationTypeInSet_True()
        {
            var customizations = new HashSet<CustomizationType>();
            customizations.Add(CustomizationType.Photo);
            var photo = new Photo("photo", null);

            var check = _customizationService.CheckTrackerHasCustomizationOfSuchDataType(customizations, photo);
            
            Assert.IsTrue(check);
        }

        private IRepository<EventCustomization> _mockEventCustomizationRepository;
        private RepositoryMock<Tracker> _mockTrackerRepository;
        private RepositoryMock<Event> _mockEventRepository;
        private CustomizationService _customizationService;
    }
}