using System.Runtime.Serialization;

public enum TimeFrame
{
    [EnumMember(Value = "m")]
    Minute,
    [EnumMember(Value = "h")]
    Hour,
    [EnumMember(Value = "d")]
    Day,
    [EnumMember(Value = "w")]
    Week,
    [EnumMember(Value = "mo")]
    Month,
    [EnumMember(Value = "y")]
    Year
}