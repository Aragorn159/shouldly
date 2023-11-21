using Shouldly.Configuration;
using Moq;

namespace Shouldly.Tests.Configuration;

public class ShouldMatchConfigurationBuilderTests
{
    [Fact]
    public void DoNotIgnoreLineEndings_WhenIgnoreLineEndingsIsSet_ShouldRemoveIgnoreLineEndingsFlag()
    {
        // Arrange
        var config = new ShouldMatchConfiguration();
        config.StringCompareOptions = StringCompareShould.IgnoreLineEndings;
        var builder = new ShouldMatchConfigurationBuilder(config);

        builder.DoNotIgnoreLineEndings();

        // Assert
        Assert.Equal(StringCompareShould.IgnoreLineEndings, config.StringCompareOptions);
    }
}
