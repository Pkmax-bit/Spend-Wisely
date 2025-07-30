using System;
using System.Collections.Generic;

namespace Chitieu.Models;

public partial class NguoiChiTieu
{
    public int Id { get; set; }

    public string Tennguoichitieu { get; set; } = null!;

    public string? Email { get; set; }

    public string? Sdt { get; set; }

    public string? Hinhanh { get; set; }

    public string? Ghichu { get; set; }

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
