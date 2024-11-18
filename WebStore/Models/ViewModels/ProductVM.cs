using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebStore.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem>? CategorySelectList { get; set; } = null;
        public IFormFile imageF {  get; set; }
        public int categoryVM { get; set; }
        public IEnumerable<SelectListItem>? ApplicationTypeSelectList { get; set; } = null;

    }
}
