using RecipeCrawler.Core;
using Xunit;

namespace RecipeCrawler.Tests
{
    public class CrawlerTests
    {
        [Fact]
        public void DefaultState_InvokeConstructor_ObjectIsCreated()
        {
            // arrange
            // act
            // assert
            new Crawler();
        }
    }
}