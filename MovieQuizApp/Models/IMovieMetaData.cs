using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieQuizApp.Models
{
    public interface IMovieMetaData
    {
        [Key]
         int MovieId { get; set; }
       
        [ForeignKey("Registration")]
         int UserID { get; set; }
         string Title { get; set; }
        
    }

    [MetadataType(typeof(IMovieMetaData))]
    public partial class Movy : IMovieMetaData
    {

    }
}