namespace ParfumTree.Application.DTOs;

public class ProductListDto
{
    public int Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string BrandName { get; set; } =  string.Empty;
    public string CategoryName { get; set; } =  string.Empty;
    public string Gender { get; set; } =  string.Empty;
    public decimal MinPrice { get; set; }
    public bool IsInStock { get; set; }
}