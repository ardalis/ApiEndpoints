﻿using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Endpoints.Authors
{
    public class CreateAuthorCommand
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PluralsightUrl { get; set; }
        public string TwitterAlias { get; set; }
    }
}