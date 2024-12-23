﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiCampiAgricoli.Models;

[Keyless]
public partial class VistaMisurazioniUtente
{
    public int IdMisurazione { get; set; }

    public int IdSensore { get; set; }

    public int IdTipologiaSensore { get; set; }

    public int IdMicrocontrollore { get; set; }

    public int IdCampo { get; set; }

    public int IdUtente { get; set; }

    public double? valoreMisurazione { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dataOraCertaMisurazione { get; set; }

    [Required]
    [StringLength(50)]
    public string DescrizioneTipologiaSensore { get; set; }
}