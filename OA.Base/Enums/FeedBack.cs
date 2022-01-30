using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Base.Helpers
{
    public enum FeedBack : int
    {
        //Info

        //Success
        AddedSuccess = 200,
        EditedSuccess = 201,
        DeletedSuccess = 202,
        LoginSuccess = 203,
        OTPGenerated = 204,
        AccountVerified = 205,
        ConfirmedSuccess = 206,
        ActivatedSuccess = 207,
        FreezedSuccess = 208,
        ImageUploaded = 209,
        RestPasswordSuccess = 210,
        SendSuccessfully = 211,
        CheckoutSuccess = 212,
        PaymentStatus = 213,

        //Error
        Unauthorized = 401,
        NotFound = 404,
        AddedFail = 405,
        EditedFail = 406,
        DeletedFail = 407,
        LoginFail = 408,
        ImageUploadedFail = 409,
        OTPGeneratedFail = 410,
        AccountNotVerified = 411,
        ConfirmedFail = 412,
        FreezedFail = 413,
        RestPasswordFail = 414,
        OrderUnCompleted = 415,
        DeliveryUnAccepted = 416,
        FailTransaction = 417,

        //Error
        ServeErrorFail = 500,
        NullOrEmpty = 501,
        largeSize = 502,
        IsNotImage = 503,
        UnUsedCode = 504,
        SMSNotSend = 505,
        CodeNotUsed = 506,
        CodeInCorrect = 507,
        IsExist = 508,
        NotAllowed = 509,
        UnActivated = 510,
        DataReserved = 511, //The data is already reserved
        PhoneNumberIsExist = 512,
        AccountIsBlocked = 513,
        couldNotBeDeleted = 514,
        UserAccountNotAllowed = 515,
        UnConfirmedORFreezed = 516,
        WithoutSpace = 517,
        OrderRejected = 518,
        OrderUnRejected = 519,
        CannotPerformed = 520,
        PaymentNotCompleted = 521,
        OrderCannotDeleted = 522,
        BalanceNotEnough = 523,
        MaxmimQty = 524,
        OrderCompleted = 525,
        PaymentProcessNotCompleted = 526
    }

    public enum SyperFeedBack : int
    {
        SyperPaymentApproved = 0,
        SyperApproved = 1,
        SyperInvalidTransaction = 2,
        SyperInvalidServiceId = 3,
        SyperInvalidRequestFormat = 4,
        SyperInvalidpaymentInformation = 5,
        SyperSystemError = 6,
        SyperInvalidCustomerReference = 7,
        SyperInvalidPayee = 8,
        SyperInvalidApplication = 9,
        SyperAuthenticationFailed = 10,
        SyperConnectionFailed = 11,
        SyperConnectionTimedOut = 12,
        SyperInvalidInputEntry = 13,
        SyperInvalidTransactionId = 14
    }
}
