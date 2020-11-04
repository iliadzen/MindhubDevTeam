using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using NUnit.Framework;
using AutoFixture;
using ItHappened.Infrastructure;

namespace ItHappened.Tests
{
    public class CustomizationServiceUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockCommentRepository = new RepositoryMock<Comment>();
            _mockEventRepository = new RepositoryMock<Event>();
            _mockTrackerRepository = new RepositoryMock<Tracker>();
            _customizationService = new CustomizationService(_mockTrackerRepository, _mockEventRepository,
                _mockCommentRepository, new RepositoryMock<Rating>(), new RepositoryMock<Scale>(), 
                new RepositoryMock<Geotag>(), new RepositoryMock<Photo>());
        }

        [Test]
        public void CheckTrackerHasCustomizationOfSuchDataType_NoSuchCustomizationTypeInSet_False()
        {
            var customizations = new List<CustomizationType>();
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "TestContent");

            var check = _customizationService.CheckTrackerHasCustomizationOfSuchDataType(customizations, comment);
            
            Assert.IsFalse(check);
        }
        
        [Test]
        public void CheckTrackerHasCustomizationOfSuchDataType_SuchCustomizationTypeInSet_True()
        {
            var customizations = new List<CustomizationType>();
            customizations.Add(CustomizationType.Comment);
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "TestContent");

            var check = _customizationService.CheckTrackerHasCustomizationOfSuchDataType(customizations, comment);
            
            Assert.IsTrue(check);
        }

        [Test]
        public void AddCommentToEvent_UserAddsCommentToOwnEventWithCorrectFormAndEventExists_CommentAdded()
        {
            var userId = Guid.NewGuid();
            var tracker = EntityMaker.CreateSomeTracker(userId, _mockTrackerRepository);
            tracker.Customizations.Add(CustomizationType.Comment);
            _mockTrackerRepository.Update(tracker);
            var @event = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            var form = _fixture.Create<CommentForm>();
            
            _customizationService.AddCommentToEvent(userId, @event.Id, form);

            var comment = _mockCommentRepository.GetAll().ElementAt(0);
            Assert.AreEqual(form.Content, comment.Content);
        }
        
        [Test]
        public void AddCommentToEvent_UserAddsCommentToOwnEventWithCorrectFormAndEventNotExists_CommentNotAdded()
        {
            var userId = Guid.NewGuid();
            var tracker = EntityMaker.CreateSomeTracker(userId, _mockTrackerRepository);
            tracker.Customizations.Add(CustomizationType.Comment);
            _mockTrackerRepository.Update(tracker);
            var form = _fixture.Create<CommentForm>();
            
            _customizationService.AddCommentToEvent(userId, Guid.NewGuid(), form);

            var comments = _mockCommentRepository.GetAll();
            Assert.AreEqual(0, comments.Count);
        }
        
        [Test]
        public void AddCommentToEvent_UserAddsCommentToSomeonesEventWithCorrectFormAndEventExists_CommentNotAdded()
        {
            var userId = Guid.NewGuid();
            var tracker = EntityMaker.CreateSomeTracker(userId, _mockTrackerRepository);
            tracker.Customizations.Add(CustomizationType.Comment);
            _mockTrackerRepository.Update(tracker);
            var @event = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            var form = _fixture.Create<CommentForm>();
            
            _customizationService.AddCommentToEvent(Guid.NewGuid(), @event.Id, form);

            var comments = _mockCommentRepository.GetAll();
            Assert.AreEqual(0, comments.Count);
        }
        
        [Test]
        public void AddCommentToEvent_UserAddsCommentToOwnEventWithNullFormAndEventExists_CommentNotAdded()
        {
            var userId = Guid.NewGuid();
            var tracker = EntityMaker.CreateSomeTracker(userId, _mockTrackerRepository);
            tracker.Customizations.Add(CustomizationType.Comment);
            _mockTrackerRepository.Update(tracker);
            var @event = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            
            _customizationService.AddCommentToEvent(userId, @event.Id, null);

            var comments = _mockCommentRepository.GetAll();
            Assert.AreEqual(0, comments.Count);
        }
        
        [Test]
        public void AddCommentToEvent_UserAddsCommentToOwnEventWithFormWithEmptyContentAndEventExists_CommentNotAdded()
        {
            var userId = Guid.NewGuid();
            var tracker = EntityMaker.CreateSomeTracker(userId, _mockTrackerRepository);
            tracker.Customizations.Add(CustomizationType.Comment);
            _mockTrackerRepository.Update(tracker);
            var @event = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            var form = new CommentForm("");
            
            _customizationService.AddCommentToEvent(userId, @event.Id, form);

            var comments = _mockCommentRepository.GetAll();
            Assert.AreEqual(0, comments.Count);
        }
        
        [Test]
        public void AddCommentToEvent_UserAddsCommentButNoCommentCustomizationTypeInTracker_CommentNotAdded()
        {
            var userId = Guid.NewGuid();
            var tracker = EntityMaker.CreateSomeTracker(userId, _mockTrackerRepository);
            tracker.Customizations.Add(CustomizationType.Photo);
            _mockTrackerRepository.Update(tracker);
            var @event = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            var form = _fixture.Create<CommentForm>();
            
            _customizationService.AddCommentToEvent(userId, @event.Id, form);

            var comments = _mockCommentRepository.GetAll();
            Assert.AreEqual(0, comments.Count);
        }

        private Fixture _fixture;
        private RepositoryMock<Comment> _mockCommentRepository;
        
        private RepositoryMock<Event> _mockEventRepository;
        private RepositoryMock<Tracker> _mockTrackerRepository;
        private CustomizationService _customizationService;
    }
}