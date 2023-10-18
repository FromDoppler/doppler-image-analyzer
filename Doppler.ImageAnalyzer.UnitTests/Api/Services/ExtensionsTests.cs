namespace Doppler.ImageAnalyzer.UnitTests.Api.Services;

public class ExtensionsTests
{
    public ExtensionsTests()
    {
    }

    [Fact]
    public void LabelExtension_ToImageConfidence_WhenOk()
    {
        var label = new Label
        {
            Confidence = 90,
            Name = "Name"
        };

        var result = label.ToImageConfidence();

        Assert.True(result.Label == label.Name);
        Assert.True(result.Confidence == label.Confidence);
        Assert.False(result.IsModeration);
    }

    [Fact]
    public void CustomLabelExtension_ToImageConfidence_WhenOk()
    {
        var customLabel = new CustomLabel
        {
            Confidence = 90,
            Name = "Name"
        };

        var result = customLabel.ToImageConfidence();

        Assert.True(result.Label == customLabel.Name);
        Assert.True(result.Confidence == customLabel.Confidence);
        Assert.False(result.IsModeration);
    }

    [Fact]
    public void ModerationLabel_ToImageConfidence_WhenOk()
    {
        var moderationLabel = new ModerationLabel
        {
            Confidence = 90,
            Name = "Name"
        };

        var result = moderationLabel.ToImageConfidence();

        Assert.True(result.Label == moderationLabel.Name);
        Assert.True(result.Confidence == moderationLabel.Confidence);
        Assert.True(result.IsModeration);
    }
}
