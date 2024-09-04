## .IsRequired
```
+ Không sử dụng IsRequired thì mặc định nó sẽ là  .IsRequired, nhưng nếu ở entity có để ? => mặc định IsRequired(false)
=> nếu yêu cầu không bắt buộc thì dùng IsRequired(false) or ? (public int? Code {get;set;}) là được
=> còn đâu muốn là Required thì để mặc định không cần config gì

```

## IsUnicode
```
+ Mặc định nếu không dùng gì sẽ là InUnicode rồi
=> Nếu muốn nó không phải Unicode thì mới cần config là IsUnicode(false)
```

# Enum

```
public enum DiscountUnit
{
    Percentage = 0,  // Phần trăm
    Amount = 1,      // Số tiền cụ thể
    Other = 2        // Các đơn vị khác (bạn có thể mở rộng theo nhu cầu)
}

builder.Property(cd => cd.DiscountUnit)
    .HasConversion(
        v => v.ToString(),      // Chuyển đổi từ enum sang chuỗi để lưu trong cơ sở dữ liệu
        v => (DiscountUnit)Enum.Parse(typeof(DiscountUnit), v)  // Chuyển đổi từ chuỗi sang enum khi đọc từ cơ sở dữ liệu
    );

```

## .OnDelete()
```
+ Nếu không chỉ định OnDelete(DeleteBehavior.Cascade) hoặc bất kỳ hành vi xóa nào khác,
Entity Framework Core sẽ sử dụng hành vi mặc định được xác định bởi cơ sở dữ liệu
của bạn.

+ Mặc định, các cơ sở dữ liệu thường sẽ sử dụng hành vi xóa giới hạn 
(DeleteBehavior.Restrict). Điều này có nghĩa là khi một bản ghi trong bảng cha bị xóa,
nếu tồn tại các bản ghi con liên quan, hành động xóa sẽ bị từ chối 
và sẽ không có bất kỳ thay đổi nào xảy ra.

+  PostgreSQL, hành vi xóa mặc định được thiết lập là RESTRICT.

```