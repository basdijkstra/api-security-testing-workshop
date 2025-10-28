using RestAssured.Request.Builders;
using System.Net;
using static RestAssured.Dsl;

namespace ApiSecurityExamples.Tests
{
    [TestFixture]
    public class Tests
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

        [TestCase("<script>alert(0)</script>", TestName = "JavaScript injection is not possible")]
        [TestCase("1;DROP TABLE students", TestName = "SQL injection is not possible")]
        [TestCase("$(touch /tmp/blns.fail)", TestName = "Server code injection is not possible")]
        public void TestForInjection(string valueToInject)
        {
            var injectionStudent = new
            {
                firstName = "John",
                lastName = "Smith",
                major = valueToInject
            };

            Given()
                .Spec(this.requestSpecification)
                .Body(injectionStudent)
                .When()
                .Post("/automation/student")
                .Then()
                .StatusCode(HttpStatusCode.BadRequest);
        }
    }
}