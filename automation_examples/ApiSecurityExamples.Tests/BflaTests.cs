using RestAssured.Request.Builders;
using System.Net;
using static RestAssured.Dsl;

namespace ApiSecurityExamples.Tests
{
    [TestFixture]
    public class BflaTests
    {
        private RequestSpecification requestSpecification;

        [SetUp]
        public void Setup()
        {
            this.requestSpecification = new RequestSpecBuilder()
                .WithBaseUri("http://localhost")
                .WithPort(9876)
                .Build();
        }

        [TestCaseSource(nameof(HttpMethodsAndStatusCodes))]
        public void TestForBfla(HttpMethod httpMethod, HttpStatusCode expectedStatusCode)
        {
            Given()
                .Spec(this.requestSpecification)
                .OAuth2("max_token_readonly")
                .When()
                .Invoke("/fines/3456", httpMethod)
                .Then()
                .StatusCode(expectedStatusCode);
        }

        private static IEnumerable<TestCaseData> HttpMethodsAndStatusCodes()
        {
            yield return new TestCaseData(HttpMethod.Delete, HttpStatusCode.MethodNotAllowed)
                .SetName("HTTP DELETE is not allowed");

            yield return new TestCaseData(HttpMethod.Post, HttpStatusCode.MethodNotAllowed)
                .SetName("HTTP POST is not allowed");

            yield return new TestCaseData(HttpMethod.Put, HttpStatusCode.MethodNotAllowed)
                .SetName("HTTP PUT is not allowed");

            yield return new TestCaseData(HttpMethod.Patch, HttpStatusCode.MethodNotAllowed)
                .SetName("HTTP PATCH is not allowed");
        }
    }
}