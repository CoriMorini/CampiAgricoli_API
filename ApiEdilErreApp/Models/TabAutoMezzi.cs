﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiEdilErreApp.Models;

public partial class TabAutoMezzi
{
    [Key]
    public int IdAutoMezzo { get; set; }

    public int IdAutoMezzoTipologia { get; set; }

    [StringLength(50)]
    public string Targa { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataImmatricolazione { get; set; }

    [StringLength(100)]
    public string DescrizioneMezzo { get; set; }

    [StringLength(1024)]
    public string NoteMezzo { get; set; }

    [StringLength(50)]
    public string DTSIdVeicolo { get; set; }
}