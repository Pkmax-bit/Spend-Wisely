using System;
using System.Collections.Generic;

namespace Chitieu.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Sdt { get; set; }

    public string? Hoten { get; set; }

    public string? Hinhanh { get; set; }

    public string Password { get; set; } = null!;

    public string? Random { get; set; }

    public DateTime? Lastlogin { get; set; }

    public bool? Kichhoat { get; set; }

    public virtual ICollection<ChiTieu> ChiTieus { get; set; } = new List<ChiTieu>();

    public virtual ICollection<NguoiChiTieu> NguoiChiTieus { get; set; } = new List<NguoiChiTieu>();

    public virtual ICollection<TheLoai> TheLoais { get; set; } = new List<TheLoai>();
}
