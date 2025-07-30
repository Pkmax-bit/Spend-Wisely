using System;
using System.Collections.Generic;

namespace Chitieu.Models;

public partial class ChiTieu
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public DateTime Ngaygio { get; set; }

    public int? IdTheloai { get; set; }

    public decimal GiaTien { get; set; }

    public string? Ghichu { get; set; }

    public virtual TheLoai? IdTheloaiNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
