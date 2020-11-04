using System.Collections.Generic;
using System.Linq;
using ItHappened.App.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ItHappened.Tests
{
    public class CustomizationsCreateRequestsTests
    {
        [Test]
        public void CustomizationsRequestWithCommentAndRating_SerializeObjectAndDeserializeObject_CorrectConvert()
        {
            var requests = new CustomizationsCreateRequests();
            var commentRequest = new CommentCreateRequest();
            var content = "Test";
            commentRequest.Content = content;
            requests.CommentCreateRequest = commentRequest;
            var ratingRequest = new RatingCreateRequest();
            var stars = 5;
            ratingRequest.Rating = stars;
            requests.RatingCreateRequest = ratingRequest;

            var serialized = JsonConvert.SerializeObject(requests);
            var deserialized = JsonConvert.DeserializeObject<CustomizationsCreateRequests>(serialized);

            Assert.AreEqual(
                "{\"CommentCreateRequest\":{\"Content\":\"Test\"},\"RatingCreateRequest\":{\"Rating\":5}}",
                serialized);
            Assert.AreEqual(deserialized.CommentCreateRequest.Content, content);
            Assert.AreEqual(deserialized.RatingCreateRequest.Rating, stars);
        }
    }
}