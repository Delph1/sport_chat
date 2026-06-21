using Laktaren.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = string.Empty;
    public string SecondaryColor { get; set; } = string.Empty;
    public string TertiaryColor {  get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public ICollection<User> Supporters { get; set; } = new List<User>();
}