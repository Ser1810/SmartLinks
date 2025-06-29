using DB;
using Microsoft.AspNetCore.Http;
using Model;
using Moq;
using Npgsql;
using SmartLinks;
using SmartLinks.Interfaces;
using System.Data;
using System.Net;

namespace Tests
{
    [TestClass]
    public class Test
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                Environment.SetEnvironmentVariable("CONNECTION_STRING", "Host=localhost;Port=5433;Database=SmartLinkNew;Username=puser;Password=111");
            }

            var host = Environment.GetEnvironmentVariable("HISTORYAPI_HOST");

            if (string.IsNullOrEmpty(host))
            {
                Environment.SetEnvironmentVariable("HISTORYAPI_HOST", "localhost");
            }
        }

        protected RuleRedirect GetRuleRedirect(IDataReader reader)
        {
            var rule = new RuleRedirect
            {
                RuleDll = reader[0].ToString(),
                RedirectTo = reader[1].ToString(),
                Args = reader[2].ToString()
            };
            return rule;
        }

        [TestMethod]
        public void DBSuccessfulTest()
        {
            var sql = "select \"RuleDll\", \"RedirectTo\", \"Args\" from \"Rule\" where \"IsActive\" = true order by \"Order\"";
            var rules = SqlExecutor.ExecuteSql<RuleRedirect>(sql, GetRuleRedirect);
        }

        [TestMethod]
        public void DBSuccessfulTest2()
        {
            var sql = "INSERT INTO \"History\" (\"Url\") VALUES (null)";
            SqlExecutor.ExecuteInsert(sql, null);
        }

        [TestMethod]
        public void DBUnSuccessfullTest()
        {
            var sql = "select \"RuleDll\", \"RedirectTo\", \"Args\" from \"Rule\" where \"IsActive123\" = true order by \"Order\"";
            var actual = Assert.ThrowsException<PostgresException>(
                () => SqlExecutor.ExecuteSql<RuleRedirect>(sql, GetRuleRedirect)
            );
        }

        [TestMethod]
        public void DBConnectionFailed()
        {
            var actual = Assert.ThrowsException<Exception>(() => Connect());

            Assert.AreEqual("Error connecting to the database", actual.Message);

            void Connect()
            {
                SqlExecutor.Connect("1");
            }
        }

        [TestMethod]
        public void TimeProcessorTest()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            var smartLink = new SmartLinkService();

            var response = smartLink.SearchRule(context);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
            Assert.AreEqual("http://localhost:7173/TimeProcessor", response.Result);
        }

        [TestMethod]
        public void EmptyRuleTest()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            var smartLink = new SmartLinkService();
            smartLink.TableName = "RuleEmpty";

            var response = smartLink.SearchRule(context);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
            Assert.AreEqual(null, response.Result);
        }

        [TestMethod]
        public void TimeProcessorTest2()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            var smartLink = new SmartLinkService();
            smartLink.TableName = "Rule2";

            var response = smartLink.SearchRule(context);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
            Assert.AreEqual(null, response.Result);
        }

        [TestMethod]
        public void BrowserProcessorTest()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            var smartLink = new SmartLinkService();

            context.Request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");

            var response = smartLink.SearchRule(context);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
            Assert.AreEqual("http://localhost:7173/BrowserProcessor", response.Result);
        }

        [TestMethod]
        public void BrowserProcessorTest2()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            context.Request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");

            var smartLink = new SmartLinkService();
            smartLink.TableName = "Rule2";

            var response = smartLink.SearchRule(context);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
            Assert.AreEqual(null, response.Result);
        }

        [TestMethod]
        public void LanguageProcessorTest()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            var smartLink = new SmartLinkService();


            context.Request.Headers.Add("Accept-Language", "{[Accept-Language, ru,en;q=0.9]}");

            var response = smartLink.SearchRule(context);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
            Assert.AreEqual("http://localhost:7173/LanguageProcessor", response.Result);
        }

        [TestMethod]
        public void LanguageProcessorTest2()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            var smartLink = new SmartLinkService();
            smartLink.TableName = "Rule2";

            context.Request.Headers.Add("Accept-Language", "{[Accept-Language, ru,en;q=0.9]}");

            var response = smartLink.SearchRule(context);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
            Assert.AreEqual(null, response.Result);
        }

        [TestMethod]
        public void RedirectMiddlewareTest()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");

            context.Request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");

            var mock = new Mock<ISmartLinksService>();
            var redirectMiddleware = new RedirectMiddleware(mock.Object);

            RequestDelegate emptyDelegate = context => Task.CompletedTask;

            var response = redirectMiddleware.InvokeAsync(context, emptyDelegate);
            
            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
        }

        [TestMethod]
        public void RedirectMiddlewareTest2()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/123";
            context.Request.Host = new HostString("http://localhost:7173");

            context.Request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");

            var mock = new Mock<ISmartLinksService>();

            var redirectMiddleware = new RedirectMiddleware(mock.Object);

            RequestDelegate emptyDelegate = context => Task.CompletedTask;

            var response = redirectMiddleware.InvokeAsync(context, emptyDelegate);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
        }

        [TestMethod]
        public void RedirectMiddlewareTest3()
        {
            DefaultHttpContext context = null;

            var mock = new Mock<ISmartLinksService>();

            var redirectMiddleware = new RedirectMiddleware(mock.Object);

            RequestDelegate emptyDelegate = context => Task.CompletedTask;

            var response = redirectMiddleware.InvokeAsync(context, emptyDelegate);

            Assert.AreEqual(TaskStatus.RanToCompletion, response.Status);
        }

        [TestMethod]
        public async Task RedirectMiddlewareTest4()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "";
            context.Request.Host = new HostString("http://localhost:7173");
            context.Request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");

            var mockService = new Mock<ISmartLinksService>();
            mockService.Setup(s => s.SearchRule(It.IsAny<HttpContext>()))
                       .ReturnsAsync("http://redirected-url.com");

            var middleware = new RedirectMiddleware(mockService.Object);

            RequestDelegate emptyDelegate = ctx => Task.CompletedTask;

            // Act
            await middleware.InvokeAsync(context, emptyDelegate);

            // Assert
            Assert.AreEqual(302, context.Response.StatusCode);
            Assert.IsTrue(context.Response.Headers.ContainsKey("Location"));
            Assert.AreEqual("http://redirected-url.com", (string)context.Response.Headers["Location"]);
        }
    }
}