using System;
using System.Collections.Generic;

namespace Chitieu.Models;

public partial class TheLoai
{
    public int Id { get; set; }

    public string Tentheloai { get; set; } = null!;

    public string? Ghichu { get; set; }

    public int? IdUser { get; set; }

    public bool? Kichhoat { get; set; }

    public virtual ICollection<ChiTieu> ChiTieus { get; set; } = new List<ChiTieu>();

    public virtual User? IdUserNavigation { get; set; }
}
