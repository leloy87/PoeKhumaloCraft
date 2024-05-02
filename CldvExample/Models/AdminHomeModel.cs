using Microsoft.AspNetCore.Authorization;



namespace KhumaloeApp.Models
{
  
    [Authorize(Roles ="Admin")]
    public class AdminHomeModel
    {

    public void OnGet()
        {
            // Add any neccessary logic here
        }
    }
}
