using System;
using AutoFixture;
using ItHappened.Application;
using ItHappened.Domain;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Tests
{
    public class TrackerServiceUnitTests
    {
        [Test]
        public void UserGetsOwnTracker_GotTracker()
        {
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var customizations = fixture.Create<ISet<CustomizationType>>();
            var trackerId = Guid.NewGuid();
            var title = "Tracker";
            var userTracker = new Tracker(trackerId, user.Id, title,
                DateTime.Now, DateTime.Now, customizations);
            var mockTrackerRepository = new Mock<IRepository<Tracker>>();
            mockTrackerRepository.Setup(mock => mock.Get(trackerId)).Returns(userTracker);
            var trackerService = new TrackerService(mockTrackerRepository.Object);

            var askedTracker = trackerService.GetTracker(user.Id, trackerId);
            
            Assert.IsTrue(askedTracker.IsSome);
            Assert.AreEqual(title, askedTracker.ValueUnsafe().Title);
        }
        
        [Test]
        public void UserGetsSomeonesTracker_DidNotGetTracker()
        {
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var customizations = fixture.Create<ISet<CustomizationType>>();
            var trackerId = Guid.NewGuid();
            var userTracker = new Tracker(trackerId, Guid.NewGuid(), "Tracker",
                DateTime.Now, DateTime.Now, customizations);
            var mockTrackerRepository = new Mock<IRepository<Tracker>>();
            mockTrackerRepository.Setup(mock => mock.Get(trackerId)).Returns(userTracker);
            var trackerService = new TrackerService(mockTrackerRepository.Object);

            var askedTracker = trackerService.GetTracker(user.Id, trackerId);

            Assert.IsTrue(askedTracker.IsNone);
        }
        
        [Test]
        public void UserEditsOwnTracker_TrackerWasEdited()
        {
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var customizations = fixture.Create<ISet<CustomizationType>>();
            var trackerId = Guid.NewGuid();
            var title = "Tracker";
            var userTracker = new Tracker(trackerId, user.Id, title,
                DateTime.Now, DateTime.Now, customizations);
            var mockTrackerRepository = new Mock<IRepository<Tracker>>();
            mockTrackerRepository.Setup(mock => mock.Get(trackerId)).Returns(userTracker);
            var trackerService = new TrackerService(mockTrackerRepository.Object);
            
            
            var trackerEditingForm = fixture.Create<TrackerForm>();
            var updatedTracker = new Tracker(trackerId, user.Id, trackerEditingForm.Title,
                userTracker.CreationDate, DateTime.Now, trackerEditingForm.Customizations);
            mockTrackerRepository.Setup(mock => mock.Update(updatedTracker));
            trackerService.EditTracker(user.Id, trackerId,  trackerEditingForm);
            var editedTracker = trackerService.GetTracker(user.Id, trackerId);

            Assert.IsTrue(editedTracker.IsSome);
            Assert.AreEqual(trackerEditingForm.Title, editedTracker.ValueUnsafe().Title);
        }
    }
}