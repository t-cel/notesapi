using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAPI.Controllers.Model;
using NotesAPI.Extensions;
using NotesAPI.Services;

namespace NotesAPI.Controllers
{
    [Route("api/notes")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private ILogger<NotesController> Logger { get; set; }
        private NotesService NotesService { get; set; }

        #region NotesController()
        public NotesController(NotesService notesService, ILogger<NotesController> logger)
        {
            Logger = logger;
            NotesService = notesService;
        } 
        #endregion

        #region CreateNote()
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<NoteViewModel>> CreateNote([FromBody] NoteFormModel model)
        {
            var userId = User.Id();
            var note = await NotesService.CreateNote(userId, model.Content);

            return CreatedAtAction(nameof(GetNote),
                new { id = note.Id }, new NoteViewModel { Id = note.Id, Content = note.Content, Tag = note.Tag });
        }
        #endregion

        #region GetNote()
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteFormModel>> GetNote(long id)
        {
            var userId = User.Id();
            var note = await NotesService.GetNote(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != userId)
            {
                return NotFound();
            }

            return Ok(note);
        }
        #endregion

        #region GetNotes()
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NoteViewModel>>> GetNotes([FromQuery] string tag = null)
        {
            var userId = User.Id();
            var userNotes = (await NotesService.GetNotesForUser(userId, tag))
                .Select(p => new NoteViewModel { Id = p.Id, Content = p.Content, Tag = p.Tag });

            return Ok(userNotes);
        }
        #endregion

        #region UpdateNote()
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateNote(long id, [FromBody] NoteFormModel model)
        {
            var userId = User.Id();
            var note = await NotesService.UpdateNote(id, model.Content);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != userId)
            {
                return NotFound();
            }

            return Ok(new NoteViewModel
            {
                Id = note.Id,
                Content = note.Content,
                Tag = note.Tag
            });
        } 
        #endregion

        #region DeleteNote()
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteNote(long id)
        {
            var userId = User.Id();
            var note = await NotesService.GetNote(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != userId)
            {
                return NotFound();
            }

            await NotesService.DeleteNote(note);

            return NoContent();
        }
        #endregion
    }
}
