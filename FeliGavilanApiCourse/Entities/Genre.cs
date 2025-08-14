using System.ComponentModel.DataAnnotations;

namespace FeliGavilanApiCourse.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
