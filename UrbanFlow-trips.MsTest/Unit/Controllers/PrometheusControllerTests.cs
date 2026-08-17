using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.API.Controllers;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Unit.Controllers;

[TestClass]
public class PrometheusControllerTests
{
    [TestMethod]
    public async Task GetMetrics_ReturnsPlainTextContent()
    {
        var controller = new PrometheusController(SharedPrometheus.Instance);

        var result = await controller.GetMetrics();

        var content = result as ContentResult;
        Assert.IsNotNull(content);
        Assert.AreEqual("text/plain; version=0.0.4; charset=utf-8", content.ContentType);
        Assert.IsNotNull(content.Content);
    }
}
