using OA.Base.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace OA.Base.Messages
{
    public class SystemMessagesEn
    {
        // ***** Info [000] ***** //

        // ***** Success [200] ***** //
        //200
        public string AddedSuccess { get; set; } = "Added successfully";
        //201
        public string EditedSuccess { get; set; } = "Edited successfully";
        //202
        public string DeletedSuccess { get; set; } = "Deleted successfully";
        //203
        public string LoginSuccess { get; set; } = "You are logged in successfully";
        //204
        public string OTPGenerated { get; set; } = "Activation code created";
        //205
        public string AccountVerified { get; set; } = "Account Verified";
        //206
        public string ConfirmedSuccess { get; set; } = "Confirmed successfully";
        //207
        public string ActivatedSuccess { get; set; } = "It has been activated successfully";
        //208
        public string FreezedSuccess { get; set; } = "It has been successfully frozen";
        //209
        public string ImageUploaded { get; set; } = "Image uploaded";
        //210
        public string RestPasswordSuccess { get; set; } = "Password changed successfully";
        //211
        public string SendSuccessfully { get; set; } = "Sent succesfully";
        //212
        public string CheckoutSuccess { get; set; } = "Successful transaction Please complete the payment process";
        //213
        public string PaymentStatus { get; set; } = "Payment completed successfully";

        // ***** Error [400] ***** //
        //401
        public string Unauthorized { get; set; } = "You cannot consume these resources";
        //404
        public string NotFound { get; set; } = "Resources not found";
        //405
        public string AddedFail { get; set; } = "add error";
        //406
        public string EditedFail { get; set; } = "Editing error";
        //407
        public string DeletedFail { get; set; } = "Delete error";
        //408
        public string LoginFail { get; set; } = "Login data error";
        //409
        public string ImageUploadedFail { get; set; } = "The image has not been uploaded";
        //410
        public string OTPGeneratedFail { get; set; } = "Activation code not created";
        //411
        public string AccountNotVerified { get; set; } = "Account not verified";
        //412
        public string ConfirmedFail { get; set; } = "Confirmation not done";
        //413
        public string FreezedFail { get; set; } = "Freezing is not done";
        //414
        public string RestPasswordFail { get; set; } = "The password was not changed";
        //415
        public string CheckoutFail { get; set; } = "Payment has not been made";
        //416
        public string PaymentStatusFail { get; set; } = "Payment completed successfully";

        // ***** Error [500] ***** //
        //500
        public string ServeErrorFail { get; set; } = "A server error occurred";
        //501
        public string NullOrEmpty { get; set; } = "file is empty";
        //502
        public string LargeSize { get; set; } = "File size is large";
        //503
        public string IsNotImage { get; set; } = "The attached file is not a picture";
        //504
        public string UnUsedCode { get; set; } = " You have an unused code. Please check previous text messages on your phone";
        //505
        public string SMSNotSend { get; set; } = "Message not sent, try again";
        //506
        public string CodeNotUsed { get; set; } = "The code has already been used";
        //507
        public string CodeInCorrect { get; set; } = "The code is incorrect";
        //508
        public string IsExist { get; set; } = "already exists";
        //509
        public string NotAllowed { get; set; } = "not allowed";
        //510
        public string UnActivated { get; set; } = "The account has not been activated";
        //511
        public string DataReserved { get; set; } = "The data is already reserved, make sure your username, phone number or email";
        //512
        public string PhoneNumberIsExist { get; set; } = "Phone number already exists";
        //513
        public string AccountIsBlocked { get; set; } = "Your account is locked, please check with the administrator";
        //514
        public string CouldNotBeDeleted { get; set; } = "The file could not be deleted, there are files associated with this file in other data";
        //515
        public string UserAccountNotAllowed { get; set; } = "Not allowed, this user may be inactive or not have the role";
        //516
        public string UnConfirmedORFreezed { get; set; } = "Not confirmed or may be frozen";
        //517
        public string WithoutSpace { get; set; } = "Username without a space Example: Odey214";
        //518
        public string OrderRejected { get; set; } = "Order has been rejected";
        //519
        public string OrderUnRejected { get; set; } = "You cannot cancel an order according to previously agreed policies";
        //520
        public string CannotPerformed { get; set; } = "This operation cannot be performed";
        //521
        public string PaymentNotCompleted { get; set; } = "Payment not completed";
        //522
        public string OrderCannotDeleted { get; set; } = "This order cannot be deleted because it is in progress";
        //523
        public string BalanceNotEnough { get; set; } = "Your balance is insufficient";
        //524
        public string MaxmimQty { get; set; } = "The quantity is large";
        //525
        public string OrderCompleted { get; set; } = "The order is already complete";
        //526
        public string PaymentProcessNotCompleted { get; set; } = "Complete the payment process first";

        public string YourMessage(FeedBack feedBack)
        {
            Type Type = Assembly.Load("OA.Base").GetTypes().First(t => t.Name == "SystemMessagesEn");
            object CreateInstance = Activator.CreateInstance(Type.GetType(Type.FullName));
            PropertyInfo prop = CreateInstance.GetType().GetProperty(feedBack.ToString());
            return prop.GetValue(CreateInstance).ToString();
        }
    }
    public class SystemMessagesAr
    {
        // ***** Info [000] ***** //

        // ***** Success [200] ***** //
        //200
        public string AddedSuccess { get; set; } = "تمت الإضافة بنجاح";
        //201
        public string EditedSuccess { get; set; } = "تم التعديل بنجاح";
        //202
        public string DeletedSuccess { get; set; } = "تم الحذف بنجاح";
        //203
        public string LoginSuccess { get; set; } = "تم تسجيل الدخول بنجاح";
        //204
        public string OTPGenerated { get; set; } = "تم إنشاء كود التفعيل";
        //205
        public string AccountVerified { get; set; } = "تم التحقق من الحساب";
        //206
        public string ConfirmedSuccess { get; set; } = "تم التأكيد بنجاح";
        //207
        public string ActivatedSuccess { get; set; } = "تم تفعيله بنجاح";
        //208
        public string FreezedSuccess { get; set; } = "تم تجميده بنجاح";
        //209
        public string ImageUploaded { get; set; } = "تم تحميل الصورة";
        //210
        public string RestPasswordSuccess { get; set; } = "تم تغيير كلمة المرور بنجاح";
        //211
        public string SendSuccessfully { get; set; } = "تم الإرسال بنجاح";
        //212
        public string CheckoutSuccess { get; set; } = "عملية ناجحة الرجاء استكمال عملية الدفع";
        //213
        public string PaymentStatus { get; set; } = "تم عملية الدفع بنجاح";

        // ***** Error [400] ***** //
        //401
        public string Unauthorized { get; set; } = "لا يمكنك استهلاك هذه الموارد";
        //404
        public string NotFound { get; set; } = "لم يتم العثور على الموارد";
        //405
        public string AddedFail { get; set; } = "إضافة خطأ";
        //406
        public string EditedFail { get; set; } = "خطأ في التعديل";
        //407
        public string DeletedFail { get; set; } = "خطأ في الحذف";
        //408
        public string LoginFail { get; set; } = "خطأ في بيانات تسجيل الدخول";
        //409
        public string ImageUploadedFail { get; set; } = "لم يتم تحميل الصورة";
        //410
        public string OTPGeneratedFail { get; set; } = "لم يتم إنشاء كود التفعيل";
        //411
        public string AccountNotVerified { get; set; } = " لم يتم التحقق من الحساب";
        //412
        public string ConfirmedFail { get; set; } = "لم تتم عمليه التأكيد";
        //413
        public string FreezedFail { get; set; } = "لم تتم عمليه التجميد";
        //414
        public string RestPasswordFail { get; set; } = "لم تتم عمليه تغيير كلمة المرور";
        //415
        public string CheckoutFail { get; set; } = "لم تتم عمليه الدفع";
        //416
        public string PaymentStatusFail { get; set; } = "تم عملية الدفع بنجاح";

        // ***** Error [500] ***** //
        //500
        public string ServeErrorFail { get; set; } = "حدث خطأ في الخادم";
        //501
        public string NullOrEmpty { get; set; } = "الملف فارغ"; 
        //502
        public string LargeSize { get; set; } = "حجم الملف كبير";
        //503
        public string IsNotImage { get; set; } = "الملف المرفق ليس صورة"; 
        //504
        public string UnUsedCode { get; set; } = " لديك رمز غير مستخدم. يرجى مراجعة الرسائل النصية السابقة في هاتفك";
        //505
        public string SMSNotSend { get; set; } = "لم يتم إرسال الرسالة ، حاول مرة أخرى"; 
        //506
        public string CodeNotUsed { get; set; } = "تم استخدام الرمز بالفعل"; 
        //507
        public string CodeInCorrect { get; set; } = "الرمز غير صحيح";
        //508
        public string IsExist { get; set; } = "موجود مسبقا"; 
        //509
        public string NotAllowed { get; set; } = "غير مسموح";
        //510
        public string UnActivated { get; set; } = " لم يتم تفعيل الحساب";
        public string DataReserved { get; set; } = "البيانات محجوزة بالفعل ، تأكد من اسم المستخدم أو رقم الهاتف أو البريد الإلكتروني";//512
        //512
        public string PhoneNumberIsExist { get; set; } = "رقم الهاتف موجود بالفعل";
        //513
        public string AccountIsBlocked { get; set; } = "حسابك مغلق ، يرجى مراجعة المسؤول";
        //514
        public string CouldNotBeDeleted { get; set; } = "تعذر حذف الملف ، هناك ملفات مرتبطة بهذا الملف في بيانات أخرى";
        //515
        public string UserAccountNotAllowed { get; set; } = "غير مسموح , قد يكون هذا المستخدم غير نشط أو ليس لديه الدور الصحيح";
        //516
        public string UnConfirmedORFreezed { get; set; } = "لم يتم تأكيده أو قد يتم تجميده";
        //517
        public string WithoutSpace { get; set; } = "اسم المستخدم بدون مسافة مثال: Odey214";
        //518
        public string OrderRejected { get; set; } = "تم رفض الطلب";
        //519
        public string OrderUnRejected { get; set; } = "لا يمكنك إلغاء الطلب وفقًا للسياسات المتفق عليها مسبقًا";
        //520
        public string CannotPerformed { get; set; } = "لا يمكن إجراء هذه العملية";
        //521
        public string PaymentNotCompleted { get; set; } = "الدفع لم يكتمل";
        //522
        public string OrderCannotDeleted { get; set; } = "لا يمكن مسح هذا الطلب لأنه قيد التنفيذ";
        //523
        public string BalanceNotEnough { get; set; } = "رصيدك غير كافي";
        //524
        public string MaxmimQty { get; set; } = "الكمية كبيرة";
        //525
        public string OrderCompleted { get; set; } = "الطلب مكتمل بالفعل";
        //526
        public string PaymentProcessNotCompleted { get; set; } = "اكمل عملية سداد اولا";

        public string YourMessage(FeedBack feedBack)
        {
            Type Type = Assembly.Load("OA.Base").GetTypes().First(t => t.Name == "SystemMessagesAr");
            object CreateInstance = Activator.CreateInstance(Type.GetType(Type.FullName));
            PropertyInfo prop = CreateInstance.GetType().GetProperty(feedBack.ToString());
            return prop.GetValue(CreateInstance).ToString();
        }
    }
}
