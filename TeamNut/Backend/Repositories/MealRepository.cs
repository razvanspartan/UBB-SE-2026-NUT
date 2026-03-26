using Microsoft.Data.SqlClient;
using System.Collections.Generic;

public class MealRepository
{
	private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=NutAppDB;Trusted_Connection=True;";

	public bool WasMealUsedRecently(int userId, int mealId)
	{
		using (SqlConnection conn = new SqlConnection(_connectionString))
		{
			// check if the meal appears in any plan from the last 3 days
			string sql = @"
                SELECT COUNT(*) 
                FROM MealPlanMeal mpm
                JOIN MealPlan mp ON mpm.mealPlanId = mp.mealplan_id
                WHERE mp.user_id = @uid 
                  AND mpm.mealId = @mid 
                  AND mp.created_at >= DATEADD(day, -3, GETDATE())";

			SqlCommand cmd = new SqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@uid", userId);
			cmd.Parameters.AddWithValue("@mid", mealId);

			conn.Open();
			int count = (int)cmd.ExecuteScalar();

			return count > 0; 
		}
	}
}