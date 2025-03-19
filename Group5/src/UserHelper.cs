using System.Security.Claims;
using Group5.src.domain.models;
using Group5.src.infrastructure;

class UserHelper {
    public static async Task<User?> GetUserModel(HttpContext httpContext, Group5DbContext dbContext) {
        int userId = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        if (userId == 0) {
            return null;
        }

        return await dbContext.Users.FindAsync(userId);
    }
}