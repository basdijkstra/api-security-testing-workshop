using RestAssured.Request.Builders;
using System.Net;
using static RestAssured.Dsl;

namespace ApiSecurityExamples.Tests
{
    [TestFixture]
    public class BolaTests
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

        [TestCaseSource(nameof(AccountNumbers))]
        public void TestForBola(int accountNumber)
        {
            Given()
                .Spec(this.requestSpecification)
                .OAuth2("susies_token")
                .When()
                .Get($"/account/{accountNumber}")
                .Then()
                .StatusCode(HttpStatusCode.NotFound);
        }

        private static IEnumerable<TestCaseData> AccountNumbers()
        {
            foreach(int accountNumber in Enumerable.Range(500, 100).Where(x => x != 543))
            {
                yield return new TestCaseData(accountNumber)
                    .SetName($"Susie cannot access account {accountNumber}");
            } 
        }
    }
}