using MicroserviceGen.Domain;

namespace MicroserviceGen.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetTextBetween_TextWithoutEnvironment_Correct()
    {
        var start = "#api_begin";
        var end = "#api_end";
        var text = "#api_begin\ndotnet new grpc --name $name.Web\ndone\ncd ..\n#api_end";
        var expectedText = "\ndotnet new grpc --name $name.Web\ndone\ncd ..\n";

        Script.Instance.Initialize(text, Architecture.NLayer);
        var actualText = Script.Instance.GetTextBetween(start, end);
        Assert.That(actualText, Is.EqualTo(expectedText));
    }

    [Test]
    public void GetTextBetween_TextWithEnvironment_Correct()
    {
        var start = "#api_begin";
        var end = "#api_end";
        var text = "qwerr#api_begin\ndotnet new grpc --name $name.Web\ndone\ncd ..\n#api_enddwdwffewfwe";
        var expectedText = "\ndotnet new grpc --name $name.Web\ndone\ncd ..\n";

        Script.Instance.Initialize(text, Architecture.NLayer);
        var actualText = Script.Instance.GetTextBetween(start, end);
        Assert.That(actualText, Is.EqualTo(expectedText));
    }

    [Test]
    public void GetTextBetween_NoMatchText_Correct()
    {
        var start = "#api_begin";
        var end = "#api_end";
        var text = "qwerr\ndotnet new grpc --name $name.Web\ndone\ncd ..\ndwdwffewfwe";
        string? expectedText = null;

        Script.Instance.Initialize(text, Architecture.NLayer);
        var actualText = Script.Instance.GetTextBetween(start, end);
        Assert.That(actualText, Is.EqualTo(expectedText));
    }
}
