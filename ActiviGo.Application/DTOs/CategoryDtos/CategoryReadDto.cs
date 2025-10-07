using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.DTOs
{
    public class CategoryReadDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        //navigation
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
    }
}
