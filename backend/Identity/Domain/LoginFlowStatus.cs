
namespace Identity.Domain
{
    public enum LoginFlowStatus
    {
        Completed,
        RequiresEmailConfirmation,
        RequiresTwoFactor
    }
}
