﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiCampiAgricoli.Models;

public partial class TabMicrocontrollori
{
    [Key]
    public int IdMicrocontrollore { get; set; }

    public int IdCampo { get; set; }

    public double? Latitudine { get; set; }

    public double? Longitudine { get; set; }

    [StringLength(50)]
    public string NomeMicrocontrollore { get; set; }
}