using NotesAPI.Extensions;
using System.ComponentModel;

namespace NotesAPI.Controllers.Model
{
    public class ErrorResponse<T> where T : Enum
    {
        public T ErrorCode { get; set; }
        public string Description
            => ErrorCode.GetDescription();
        
        public ErrorResponse(T errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
