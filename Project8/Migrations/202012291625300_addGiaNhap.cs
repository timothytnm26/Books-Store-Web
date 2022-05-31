namespace Project8.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGiaNhap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        IDAdmin = c.Int(nullable: false, identity: true),
                        TaiKhoan = c.String(nullable: false, maxLength: 50, unicode: false),
                        MatKhau = c.String(nullable: false, maxLength: 50, unicode: false),
                        HoTen = c.String(maxLength: 50),
                        TrangThai = c.Boolean(),
                    })
                .PrimaryKey(t => t.IDAdmin);
            
            CreateTable(
                "dbo.ChiTietDDH",
                c => new
                    {
                        MaSach = c.Int(nullable: false),
                        MaDDH = c.Int(nullable: false),
                        SoLuong = c.Int(),
                        DonGia = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.MaSach, t.MaDDH })
                .ForeignKey("dbo.DonDatHang", t => t.MaDDH)
                .ForeignKey("dbo.Sach", t => t.MaSach)
                .Index(t => t.MaSach)
                .Index(t => t.MaDDH);
            
            CreateTable(
                "dbo.DonDatHang",
                c => new
                    {
                        MaDDH = c.Int(nullable: false, identity: true),
                        NgayDat = c.DateTime(storeType: "smalldatetime"),
                        NgayGiao = c.DateTime(storeType: "smalldatetime"),
                        TinhTrang = c.Boolean(nullable: false),
                        MaKH = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaDDH)
                .ForeignKey("dbo.KhachHang", t => t.MaKH)
                .Index(t => t.MaKH);
            
            CreateTable(
                "dbo.KhachHang",
                c => new
                    {
                        MaKH = c.Int(nullable: false, identity: true),
                        TenKH = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50, unicode: false),
                        DiaChi = c.String(nullable: false, maxLength: 250),
                        DienThoai = c.String(nullable: false, maxLength: 50, unicode: false),
                        NgaySinh = c.DateTime(storeType: "smalldatetime"),
                        TaiKhoan = c.String(nullable: false, maxLength: 50, unicode: false),
                        MatKhau = c.String(nullable: false, maxLength: 50, unicode: false),
                        NgayTao = c.DateTime(),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaKH);
            
            CreateTable(
                "dbo.Sach",
                c => new
                    {
                        MaSach = c.Int(nullable: false, identity: true),
                        MaLoai = c.Int(nullable: false),
                        MaNXB = c.Int(nullable: false),
                        MaTG = c.Int(nullable: false),
                        TenSach = c.String(nullable: false, maxLength: 250),
                        GiaNhap = c.Decimal(storeType: "money"),
                        GiaBan = c.Decimal(storeType: "money"),
                        Mota = c.String(maxLength: 500),
                        NguoiDich = c.String(maxLength: 50),
                        AnhBia = c.String(maxLength: 50, unicode: false),
                        NgayCapNhat = c.DateTime(storeType: "smalldatetime"),
                        SoLuongTon = c.Int(),
                    })
                .PrimaryKey(t => t.MaSach)
                .ForeignKey("dbo.NhaXuatBan", t => t.MaNXB)
                .ForeignKey("dbo.TacGia", t => t.MaTG)
                .ForeignKey("dbo.TheLoai", t => t.MaLoai)
                .Index(t => t.MaLoai)
                .Index(t => t.MaNXB)
                .Index(t => t.MaTG);
            
            CreateTable(
                "dbo.NhaXuatBan",
                c => new
                    {
                        MaNXB = c.Int(nullable: false, identity: true),
                        TenNXB = c.String(nullable: false, maxLength: 50),
                        DiaChi = c.String(maxLength: 250),
                        DienThoai = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.MaNXB);
            
            CreateTable(
                "dbo.TacGia",
                c => new
                    {
                        MaTG = c.Int(nullable: false, identity: true),
                        TenTG = c.String(nullable: false, maxLength: 50),
                        QueQuan = c.String(maxLength: 250),
                        NgaySinh = c.DateTime(storeType: "smalldatetime"),
                        NgayMat = c.DateTime(storeType: "smalldatetime"),
                        TieuSu = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.MaTG);
            
            CreateTable(
                "dbo.TheLoai",
                c => new
                    {
                        MaLoai = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.MaLoai);
            
            CreateTable(
                "dbo.LienHe",
                c => new
                    {
                        MaLH = c.Int(nullable: false, identity: true),
                        Ho = c.String(nullable: false, maxLength: 50),
                        Ten = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                        DienThoai = c.String(nullable: false, maxLength: 50, unicode: false),
                        NoiDung = c.String(nullable: false, maxLength: 500),
                        NgayCapNhat = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.MaLH);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sach", "MaLoai", "dbo.TheLoai");
            DropForeignKey("dbo.Sach", "MaTG", "dbo.TacGia");
            DropForeignKey("dbo.Sach", "MaNXB", "dbo.NhaXuatBan");
            DropForeignKey("dbo.ChiTietDDH", "MaSach", "dbo.Sach");
            DropForeignKey("dbo.DonDatHang", "MaKH", "dbo.KhachHang");
            DropForeignKey("dbo.ChiTietDDH", "MaDDH", "dbo.DonDatHang");
            DropIndex("dbo.Sach", new[] { "MaTG" });
            DropIndex("dbo.Sach", new[] { "MaNXB" });
            DropIndex("dbo.Sach", new[] { "MaLoai" });
            DropIndex("dbo.DonDatHang", new[] { "MaKH" });
            DropIndex("dbo.ChiTietDDH", new[] { "MaDDH" });
            DropIndex("dbo.ChiTietDDH", new[] { "MaSach" });
            DropTable("dbo.LienHe");
            DropTable("dbo.TheLoai");
            DropTable("dbo.TacGia");
            DropTable("dbo.NhaXuatBan");
            DropTable("dbo.Sach");
            DropTable("dbo.KhachHang");
            DropTable("dbo.DonDatHang");
            DropTable("dbo.ChiTietDDH");
            DropTable("dbo.Admin");
        }
    }
}
