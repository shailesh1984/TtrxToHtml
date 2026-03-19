namespace TtrxToHtml.Test.Tests;

public class RegexHelperTests
{
    [Theory]
    [InlineData("<div>Hello</div>", true)]
    [InlineData("<p>Test paragraph</p>", true)]
    [InlineData("<span class='x'>Content</span>", true)]
    [InlineData("<h1>Header</h1>", true)]
    [InlineData("<a href='link'>Click</a>", true)]
    [InlineData("Just plain text", false)]
    [InlineData("123456", false)]
    [InlineData("<div>Missing closing tag", false)]
    [InlineData("No tags here < just symbol", false)]
    [InlineData("", false)]
    public void DoesItContainsHtml_ShouldReturnExpectedResult(string input, bool expected)
    {
        // Act
        var result = RegexHelper.DoesItContainsHtml(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DoesItContainsHtml_ShouldReturnFalse_ForNullInput()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => RegexHelper.DoesItContainsHtml(null));
    }

    [Fact]
    public void DoesItContainsHtml_ShouldHandleMixedContent()
    {
        // Arrange
        var input = "Some text <b>bold</b> more text";

        // Act
        var result = RegexHelper.DoesItContainsHtml(input);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DoesItContainsHtml_ShouldBeCaseInsensitive()
    {
        // Arrange
        var input = "<DIV>Uppercase Tag</DIV>";

        // Act
        var result = RegexHelper.DoesItContainsHtml(input);

        // Assert
        Assert.True(result);
    }
}
