﻿namespace RaythaZero.Domain.Events;

public class AdminCreatedEvent : BaseEvent, IBeforeSaveChangesNotification
{
    public User User { get; private set; }
    public bool SendEmail { get; private set; }
    public string NewPassword { get; private set; }
    public bool IsNewlyCreatedUser { get; private set; }
    public AdminCreatedEvent(User user, bool sendEmail, bool isNewlyCreatedUser, string newPassword)
    {
        User = user;
        SendEmail = sendEmail;
        NewPassword = newPassword;
        IsNewlyCreatedUser = isNewlyCreatedUser;
    }
}
