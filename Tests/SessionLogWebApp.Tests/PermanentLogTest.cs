using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using SessionLogWebApp.Api;
using System;
using System.Linq;
using System.Threading.Tasks;
using XTI_App;
using XTI_App.Api;
using XTI_App.Fakes;
using XTI_Core;
using XTI_Core.Fakes;
using XTI_PermanentLog;
using XTI_TempLog;
using XTI_TempLog.Fakes;
using XTI_WebApp.Fakes;

namespace SessionLogWebApp.Tests
{
    public sealed class PermanentLogTest
    {
        [Test]
        public async Task ShouldStartSessionOnPermanentLog()
        {
            var input = await setup();
            var sessionKey = generateKey();
            await startSession(input, sessionKey);
            var session = await input.SessionFactory.Sessions().Session(sessionKey);
            Assert.That(session.HasStarted(), Is.True, "Should start session on permanent log");
            Assert.That(session.HasEnded(), Is.False, "Should start session on permanent log");
        }

        [Test]
        public async Task ShouldStartRequestOnPermanentLog()
        {
            var input = await setup();
            var sessionKey = generateKey();
            await startSession(input, sessionKey);
            var requestKey = generateKey();
            await startRequest(input, sessionKey, requestKey);
            var session = await input.SessionFactory.Sessions().Session(sessionKey);
            var requests = (await session.Requests()).ToArray();
            Assert.That(requests.Length, Is.EqualTo(1), "Should start request on permanent log");
        }

        [Test]
        public async Task ShouldEndRequestOnPermanentLog()
        {
            var input = await setup();
            var sessionKey = generateKey();
            await startSession(input, sessionKey);
            var requestKey = generateKey();
            await startRequest(input, sessionKey, requestKey);
            await endRequest(input, requestKey);
            var session = await input.SessionFactory.Sessions().Session(sessionKey);
            var requests = (await session.Requests()).ToArray();
            Assert.That(requests[0].HasEnded(), Is.True, "Should end request on permanent log");
        }

        [Test]
        public async Task ShouldEndSessionOnPermanentLog()
        {
            var input = await setup();
            var sessionKey = generateKey();
            await startSession(input, sessionKey);
            var requestKey = generateKey();
            await startRequest(input, sessionKey, requestKey);
            await endRequest(input, requestKey);
            await endSession(input, sessionKey);
            var session = await input.SessionFactory.Sessions().Session(sessionKey);
            Assert.That(session.HasEnded(), Is.True, "Should end session on permanent log");
        }

        [Test]
        public async Task ShouldAuthenticateSessionOnPermanentLog()
        {
            var input = await setup();
            var sessionKey = generateKey();
            await startSession(input, sessionKey);
            await authenticateSession(input, sessionKey);
            var session = await input.SessionFactory.Sessions().Session(sessionKey);
            var user = await input.AppFactory.Users().User(session.UserID);
            Assert.That(user.UserName, Is.EqualTo("someone"), "Should authenticate session on permanent log");
        }

        [Test]
        public async Task ShouldLogEventOnPermanentLog()
        {
            var input = await setup();
            var sessionKey = generateKey();
            await startSession(input, sessionKey);
            var requestKey = generateKey();
            await startRequest(input, sessionKey, requestKey);
            Exception exception;
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            await logEvent(input, requestKey, exception);
            var session = await input.SessionFactory.Sessions().Session(sessionKey);
            var requests = (await session.Requests()).ToArray();
            var events = (await requests[0].Events()).ToArray();
            Assert.That(events.Length, Is.EqualTo(1), "Should log event on permanent log");
        }

        private static Task startSession(TestInput input, string sessionKey)
        {
            var startSessionModel = new StartSessionModel
            {
                SessionKey = sessionKey,
                RemoteAddress = "my-computer",
                UserAgent = "Windows 10",
                TimeStarted = input.Clock.Now(),
                UserName = "someone"
            };
            return input.Api.PermanentLog.StartSession.Execute(startSessionModel);
        }

        private static async Task startRequest(TestInput input, string sessionKey, string requestKey)
        {
            var startRequestModel = new StartRequestModel
            {
                RequestKey = requestKey,
                SessionKey = sessionKey,
                TimeStarted = input.Clock.Now(),
                AppType = AppType.Values.WebApp.DisplayText,
                Path = "/Fake/Current/Test/Action1"
            };
            await input.Api.PermanentLog.StartRequest.Execute(startRequestModel);
        }

        private static Task endRequest(TestInput input, string requestKey)
        {
            return input.Api.PermanentLog.EndRequest.Execute
            (
                new EndRequestModel
                {
                    RequestKey = requestKey,
                    TimeEnded = input.Clock.Now()
                }
            );
        }

        private static Task endSession(TestInput input, string sessionKey)
        {
            return input.Api.PermanentLog.EndSession.Execute
            (
                new EndSessionModel
                {
                    SessionKey = sessionKey,
                    TimeEnded = input.Clock.Now()
                }
            );
        }

        private static Task authenticateSession(TestInput input, string sessionKey)
        {
            return input.Api.PermanentLog.AuthenticateSession.Execute
            (
                new AuthenticateSessionModel
                {
                    SessionKey = sessionKey,
                    UserName = "someone"
                }
            );
        }

        private static Task logEvent(TestInput input, string requestKey, Exception exception)
        {
            return input.Api.PermanentLog.LogEvent.Execute
            (
                new LogEventModel
                {
                    EventKey = generateKey(),
                    RequestKey = requestKey,
                    Severity = AppEventSeverity.Values.CriticalError,
                    Caption = "An unexpected error occurred",
                    Message = exception.Message,
                    Detail = exception.StackTrace
                }
            );
        }

        private static string generateKey() => Guid.NewGuid().ToString("N");

        private async Task<TestInput> setup()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices
                (
                    (hostContext, services) =>
                    {
                        services.AddFakeTempLogServices();
                        services.AddFakesForXtiWebApp(hostContext.Configuration);
                        services.AddScoped<IAppApiUser, AppApiSuperUser>();
                        services.AddSingleton<IAppEnvironmentContext, FakeAppEnvironmentContext>();
                        services.AddSingleton(sp => SessionLogAppKey.AppKey);
                        services.AddScoped<SessionFactory>();
                        services.AddScoped<PermanentLog>();
                        services.AddScoped(sp =>
                        {
                            var appKey = sp.GetService<AppKey>();
                            var versionKey = AppVersionKey.Current;
                            var user = sp.GetService<IAppApiUser>();
                            var appFactory = sp.GetService<AppFactory>();
                            var clock = sp.GetService<Clock>();
                            var permanentLog = sp.GetService<PermanentLog>();
                            return new SessionLogAppApi(appKey, versionKey, user, permanentLog);
                        });
                    }
                )
                .Build();
            var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;
            var appEnvContext = (FakeAppEnvironmentContext)sp.GetService<IAppEnvironmentContext>();
            appEnvContext.Environment = new AppEnvironment
            (
                "test.user",
                "Requester",
                "my-computer",
                "Windows 10",
                AppType.Values.WebApp.DisplayText
            );
            var appFactory = sp.GetService<AppFactory>();
            var clock = sp.GetService<Clock>();
            await new AllAppSetup(appFactory, clock).Run();
            var app = await appFactory.Apps().Add(new AppKey(new AppName("Fake"), AppType.Values.WebApp), "Fake", DateTime.Now);
            var version = await app.StartNewMajorVersion(DateTime.Now);
            await version.Publishing();
            await version.Published();
            await appFactory.Users().Add(new AppUserName("test.user"), new FakeHashedPassword("Password12345"), DateTime.Now);
            await appFactory.Users().Add(new AppUserName("Someone"), new FakeHashedPassword("Password12345"), DateTime.Now);
            return new TestInput(sp);
        }

        private sealed class TestInput
        {
            public TestInput(IServiceProvider sp)
            {
                Clock = (FakeClock)sp.GetService<Clock>();
                Api = sp.GetService<SessionLogAppApi>();
                SessionFactory = sp.GetService<SessionFactory>();
                AppFactory = sp.GetService<AppFactory>();
            }

            public FakeClock Clock { get; }
            public SessionLogAppApi Api { get; }
            public SessionFactory SessionFactory { get; }
            public AppFactory AppFactory { get; }
        }
    }
}
