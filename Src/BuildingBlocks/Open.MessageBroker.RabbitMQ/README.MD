var properties = channel.CreateBasicProperties();
## Dành cho các thông điệp không bền vững (non-persistent). Điều này có nghĩa là các thông điệp chỉ được lưu trữ trong bộ nhớ và sẽ không tồn tại qua các khởi động lại của broker.
properties.DeliveryMode = 1;

## Chỉ định rằng thông điệp được gửi đi là bền vững (persistent). Các thông điệp bền vững sẽ được lưu trữ trên đĩa và tồn tại ngay cả khi broker khởi động lại, giúp đảm bảo không mất mát thông điệp quan trọng trong hệ thống.
properties.DeliveryMode = 2;


## mandatory: true
Tham số này xác định rằng thông điệp là bắt buộc (mandatory). Nếu RabbitMQ không thể định tuyến thông điệp tới bất kỳ hàng đợi nào với routing key đã cho, và mandatory được thiết lập là true, thông điệp sẽ được trả lại cho người gửi. Nếu mandatory là false, thông điệp sẽ bị xóa nếu không thể định tuyến được.


## basicProperties: properties
Tham số này là đối tượng IBasicProperties (properties) mà bạn đã khởi tạo và cấu hình trước đó. Đối tượng này chứa các thuộc tính của thông điệp như DeliveryMode, ContentType, Headers, v.v.