using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BhokMandu.Models;

public class FoodCategoryViewModel
{
    public List<Food>? Foods { get; set; }
    public SelectList? Categories { get; set; }
    public string? FoodCategory { get; set; }
    public string? SearchString { get; set; }
}