﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ManagementSystemVersionTwo.CustomAnnotations;

namespace ManagementSystemVersionTwo.Models
{
    
    public class Project
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "Necessary")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Necessary")]
        public string Description { get; set; }

        public string Attachments { get; set; }

        public bool Finished { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Necessary")]
        [ProjectEndDate(ErrorMessage ="Wrong Data in Start Date or End Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Necessary")]
        public DateTime EndDate { get; set; }

        public virtual ICollection<ProjectsAssignedToEmployee> WorkersInMe { get; set; }

       
    }
}