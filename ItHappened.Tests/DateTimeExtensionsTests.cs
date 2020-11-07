using System;
using ItHappened.Domain.Stats;
using NUnit.Framework;
using static System.DayOfWeek;

namespace ItHappened.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void GetLast_SameDay()
        {
            // Arrange
            DayOfWeek day = Saturday;
            DateTime date = new DateTime(2020, 10, 31);
            DateTime expectedLastSaturday = date.Date;
            
            // Act
            DateTime actualLastSaturday = date.GetLast(day).Date;
            
            // Assert
            Assert.AreEqual(expectedLastSaturday, actualLastSaturday);
        }
        
        [Test]
        public void GetLast_PreviousDay()
        {
            // Arrange
            DayOfWeek day = Saturday;
            DateTime date = new DateTime(2020, 11, 01);
            DateTime expectedLastSaturday = new DateTime(2020, 10, 31);
            
            // Act
            DateTime actualLastSaturday = date.GetLast(day).Date;
            
            // Assert
            Assert.AreEqual(expectedLastSaturday, actualLastSaturday);
        }
        
        [Test]
        public void GetNext_SameDay()
        {
            // Arrange
            DayOfWeek day = Sunday;
            DateTime date = new DateTime(2020, 11, 01);
            DateTime expectedNextSunday = date.Date;
            
            // Act
            DateTime actualNextSunday = date.GetNext(day).Date;
            
            // Assert
            Assert.AreEqual(expectedNextSunday, actualNextSunday);
        }
        
        [Test]
        public void GetNext_FollowingDay()
        {
            // Arrange
            DayOfWeek day = Sunday;
            DateTime date = new DateTime(2020, 10, 31);
            DateTime expectedNextSunday = new DateTime(2020, 11, 01);
            
            // Act
            DateTime actualLastSunday = date.GetNext(day).Date;
            
            // Assert
            Assert.AreEqual(expectedNextSunday, actualLastSunday);
        }
    }
}