using LinqKit;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Controllers.Model;
using NotesAPI.Model;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace NotesAPI.Services
{
    public class NotesService
    {
        private NotesDbContext DbContext { get; set; }

        public NotesService(NotesDbContext dbContext) 
        {
            DbContext = dbContext;
        }

        #region CreateNote()
        public async Task<Note> CreateNote(long userId, string content)
        {
            var tag = NoteUtils.GetTagFromContent(content);
            var note = new Note
            {
                Content = content,
                UserId = userId,
                DateCreatedUtc = DateTime.UtcNow,
                DateModifiedUtc = DateTime.UtcNow,
                Tag = tag
            };

            DbContext.Notes.Add(note);
            await DbContext.SaveChangesAsync();

            return note;
        }
        #endregion

        #region GetNote()
        public async Task<Note> GetNote(long id)
        {
            return await DbContext.Notes.FindAsync(id);
        }
        #endregion

        #region GetNotesForUser()
        public async Task<List<Note>> GetNotesForUser(long userId, string tag)
        {
            var predicate = PredicateBuilder.New<Note>();
            predicate.And(p => p.UserId == userId);

            if (tag != null)
            {
                predicate.And(p => p.Tag.ToLower() == tag.ToLower());
            }

            return await DbContext.Notes
                .Where(predicate)
                .ToListAsync();
        }
        #endregion

        #region UpdateNote()
        public async Task<Note> UpdateNote(long id, string content)
        {
            var note = await GetNote(id);

            if (note != null)
            {
                var tag = NoteUtils.GetTagFromContent(content);

                note.Content = content;
                note.DateModifiedUtc = DateTime.UtcNow;
                note.Tag = tag;

                await DbContext.SaveChangesAsync();
            }

            return note;
        }
        #endregion

        #region DeleteNote()
        public async Task DeleteNote(Note note)
        {
            DbContext.Remove(note);
            await DbContext.SaveChangesAsync();
        } 
        #endregion
    }
}
