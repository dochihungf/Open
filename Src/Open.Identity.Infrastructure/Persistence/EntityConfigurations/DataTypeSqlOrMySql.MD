## Data type

+ int: Kiểu số nguyên, tập hơp N. Ví dụ -9, 8, 1, 2, ....
+ float: Kiểu số thực, tập hợp R. Ví dụ: -0.5, 10, 9.8, ...
+ char: kiểu kí tự. Không lưu được TIẾNG VIỆT. Bộ nhớ cấp phát cứng. Ví dụ A, a, B, f, k. char(10) -> 10 ô nhớ này không ai được động vào. Luôn luôn giữ 10 ô nhớ
+ varchar: Kiểu kí tự. Không lưu được TIẾNG VIỆT. varchar(10) -> 10 ô nhớ này chỉ được lấy khi có dữ liệu bên trong. 'Hung' -> chỉ giữ 4 ô nhớ
+ nchar: n (Unicode). Kiểu kí tự, có thể lưu tiếng việt
+ nvarchar: Kiểu kí tự cấp phát động có thể lưu tiếng việt
+ date: Lưu ngày, tháng, năm, giờ
+ time: Lưu giờ, phút, giây
+ text: lưu văn bản
+ ntext: lưu văn bản Tiếng Việt



## Fluent API EF Core
+ IsUnicode(true) - default - NVARCHAR
+ IsUnicode(false) - VARCHAR