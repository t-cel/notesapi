using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NotesAPI.Controllers.Model;
using NotesAPI.Model;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace NotesAPI.IntegrationTests
{
    public class NotesControllerIntegrationTests
    {
        private TestServer server;
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        // remove original db if exists
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<NotesDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<NotesDbContext>(options =>
                            options.UseInMemoryDatabase("TestDatabase"));

                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("Test", options => { });

                        services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                            };
                        });
                    });
                });

            server = application.Server;

            client = application.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            using (var dbContext = GetNotesDbContext())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
        }

        private NotesDbContext GetNotesDbContext()
        {
            var scope = server.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<NotesDbContext>();
        }

        [Test]
        public async Task GetNotes_Returns4Notes()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/notes");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<NoteViewModel>>(content);

            Assert.That(notes, Is.Not.Null);
            Assert.That(notes, Is.TypeOf<List<NoteViewModel>>());
            Assert.That(notes.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task GetNotesPhone_Returns2Notes()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/notes?tag=phone");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<NoteViewModel>>(content);

            Assert.That(notes, Is.Not.Null);
            Assert.That(notes, Is.TypeOf<List<NoteViewModel>>());
            Assert.That(notes.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetNotesEmail_Returns1Note()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/notes?tag=email");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<NoteViewModel>>(content);

            Assert.That(notes, Is.Not.Null);
            Assert.That(notes, Is.TypeOf<List<NoteViewModel>>());
            Assert.That(notes.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetNotesWithoutTag_Returns1Note()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/notes?tag=none");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<NoteViewModel>>(content);

            Assert.That(notes, Is.Not.Null);
            Assert.That(notes, Is.TypeOf<List<NoteViewModel>>());
            Assert.That(notes.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetNote_ReturnsNote()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/notes/2");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<NoteViewModel>(content);

            Assert.That(notes, Is.Not.Null);
            Assert.That(notes, Is.TypeOf<NoteViewModel>());
        }

        [Test]
        public async Task CreateNote_AddsEmailNote()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/notes");
            var form = new NoteFormModel
            {
                Content = "notatka test@asdas.com"
            };
            var contentJson = JsonConvert.SerializeObject(form);
            request.Content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var content = await response.Content.ReadAsStringAsync();
            var createdNote = JsonConvert.DeserializeObject<NoteViewModel>(content);

            Assert.That(createdNote, Is.Not.Null);
            Assert.That(createdNote, Is.TypeOf<NoteViewModel>());
            Assert.That(createdNote.Content, Is.EqualTo(form.Content));
            Assert.That(createdNote.Tag, Is.EqualTo(TagType.Email));

            using (var dbContext = GetNotesDbContext())
            { 
                var noteInDb = dbContext.Notes.FirstOrDefault(p => p.Id == createdNote.Id);
                Assert.That(noteInDb, Is.Not.Null);
                Assert.That(noteInDb.Content, Is.EqualTo(createdNote.Content));
                Assert.That(noteInDb.Tag, Is.EqualTo(TagType.Email));
                Assert.That(noteInDb.UserId, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task CreateNote_AddsPhoneNote()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/notes");
            var form = new NoteFormModel
            {
                Content = "notatka +48123456789 aaaaaaaaaaaaaaaa"
            };
            var contentJson = JsonConvert.SerializeObject(form);
            request.Content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var content = await response.Content.ReadAsStringAsync();
            var createdNote = JsonConvert.DeserializeObject<NoteViewModel>(content);

            Assert.That(createdNote, Is.Not.Null);
            Assert.That(createdNote, Is.TypeOf<NoteViewModel>());
            Assert.That(createdNote.Content, Is.EqualTo(form.Content));
            Assert.That(createdNote.Tag, Is.EqualTo(TagType.Phone));

            using (var dbContext = GetNotesDbContext())
            {
                var noteInDb = dbContext.Notes.FirstOrDefault(p => p.Id == createdNote.Id);
                Assert.That(noteInDb, Is.Not.Null);
                Assert.That(noteInDb.Content, Is.EqualTo(createdNote.Content));
                Assert.That(noteInDb.Tag, Is.EqualTo(TagType.Phone));
                Assert.That(noteInDb.UserId, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task CreateNote_AddsNoteWithoutTag()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/notes");
            var form = new NoteFormModel
            {
                Content = "notatkaaaaaaaaaaaaaaaaa asdasdsa xyz"
            };
            var contentJson = JsonConvert.SerializeObject(form);
            request.Content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var content = await response.Content.ReadAsStringAsync();
            var createdNote = JsonConvert.DeserializeObject<NoteViewModel>(content);

            Assert.That(createdNote, Is.Not.Null);
            Assert.That(createdNote, Is.TypeOf<NoteViewModel>());
            Assert.That(createdNote.Content, Is.EqualTo(form.Content));
            Assert.That(createdNote.Tag, Is.EqualTo(TagType.None));

            using (var dbContext = GetNotesDbContext())
            {
                var noteInDb = dbContext.Notes.FirstOrDefault(p => p.Id == createdNote.Id);
                Assert.That(noteInDb, Is.Not.Null);
                Assert.That(noteInDb.Content, Is.EqualTo(createdNote.Content));
                Assert.That(noteInDb.Tag, Is.EqualTo(TagType.None));
                Assert.That(noteInDb.UserId, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task UpdatedNote_TagChangedToPhone()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "/api/notes/3");
            var form = new NoteFormModel
            {
                Content = "test +48123123123"
            };
            var contentJson = JsonConvert.SerializeObject(form);
            request.Content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var createdNote = JsonConvert.DeserializeObject<NoteViewModel>(content);

            Assert.That(createdNote, Is.Not.Null);
            Assert.That(createdNote, Is.TypeOf<NoteViewModel>());
            Assert.That(createdNote.Content, Is.EqualTo(form.Content));
            Assert.That(createdNote.Tag, Is.EqualTo(TagType.Phone));

            using (var dbContext = GetNotesDbContext())
            {
                var noteInDb = dbContext.Notes.FirstOrDefault(p => p.Id == createdNote.Id);
                Assert.That(noteInDb, Is.Not.Null);
                Assert.That(noteInDb.Content, Is.EqualTo(createdNote.Content));
                Assert.That(noteInDb.Tag, Is.EqualTo(TagType.Phone));
                Assert.That(noteInDb.UserId, Is.EqualTo(1));
            }
        }
    }
}
