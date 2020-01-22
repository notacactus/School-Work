import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Collections;

// Class holding methods relating to HighScores SQLite database
// Inspired by examples from: http://www.sqlitetutorial.net/sqlite-java/
public final class HighScores {
	// private constructor to prevent instantiation
	private HighScores(){};

	// attempts to connect to database/table and recreates them if they are not found
	public static void checkDatabase() {
		String url = "jdbc:sqlite:HighScores.db";																// url of database
		String sql = "CREATE TABLE IF NOT EXISTS high_scores (name text, score integer, timestamp integer);";	// sql code to create a new table if one is not found
		try (Connection conn = DriverManager.getConnection(url);												// connect to database and execute sql code
				Statement smt = conn.createStatement()) {
			smt.executeUpdate(sql);
		} catch (SQLException e) {
			System.out.println("Failed to connect to database");
		}
	}

	// adds new score to HighScore database
	public static void addHighScore(String name, int score, long timestamp) {
		String url = "jdbc:sqlite:HighScores.db";																// url of database
		String sql = "INSERT INTO high_scores (name, score, timestamp) VALUES(?, ?, ?)";						// sql code to insert score into database
		try (Connection conn = DriverManager.getConnection(url);												// connect to database and execute sql code
				PreparedStatement ps = conn.prepareStatement(sql)) {
			ps.setString(1, name);																				// load values into wildcards of prepared statement and executes statement
			ps.setInt(2, score);
			ps.setLong(3, timestamp);
			ps.executeUpdate();
		} catch (SQLException e) {
			System.out.println("Failed to add high score to database");
		}
	}

	// removes the lowest/most recent high score from the database
	public static void removeHighScore() {
		String url = "jdbc:sqlite:HighScores.db";																// url of database
		String sql = "DELETE FROM high_scores WHERE timestamp = (SELECT MAX(timestamp) FROM (SELECT * FROM high_scores WHERE score = (SELECT MIN(score) FROM high_scores)))";	// sql code to delete lowest score with most recent timestamp
		try (Connection conn = DriverManager.getConnection(url);												// connect to database and execute sql code
				Statement smt = conn.createStatement()) {
			smt.executeUpdate(sql);
		} catch (SQLException e) {
			System.out.println("Failed to remove high score from database");
		}
	}

	// Checks if 10 high scores saved and returns false if not
	public static boolean enoughHighScores() {
		String url = "jdbc:sqlite:HighScores.db";																// url of database
		String sql = "SELECT COUNT(*) FROM high_scores";														// sql code to determine number of high scores stored in table
		try (Connection conn = DriverManager.getConnection(url);												// connect to database and execute sql code
				Statement smt = conn.createStatement();
				ResultSet rs = smt.executeQuery(sql)) {
			while(rs.next()) {
				if (rs.getInt(1) < 10)																			// returns false if number of scores < 10
					return false;
			}
		} catch (SQLException e) {
			System.out.println("Failed evaluate number of high scores in database");
		}
		return true;																							// returns true otherwise
	}

	// Compares score to high scores in database and returns true if new score is higher than any saved scores
	public static boolean higherScore(int score) {
		String url = "jdbc:sqlite:HighScores.db";																// url of database
		String sql = "SELECT name, score FROM high_scores ORDER BY score DESC";									// sql code to return sorted scores from database
		try (Connection conn = DriverManager.getConnection(url);												// connect to database and execute sql code
				Statement smt = conn.createStatement();
				ResultSet rs = smt.executeQuery(sql)) {
			while(rs.next()) {																					// iterate through results and return true if new score is higher than any in table
				if (rs.getInt("score") < score)
					return true;
			}
		} catch (SQLException e) {
			System.out.println("Failed to compare score to scores in database");
		}
		return false;																							// returns false otherwise
	}

	// loads high scores from database and returns them as 2D ArrayList
	public static ArrayList<ArrayList<Object>> getHighScoreList() {
		ArrayList<ArrayList<Object>> highScoresList = new ArrayList<ArrayList<Object>>();						// ArrayList to hold scores
		String url = "jdbc:sqlite:HighScores.db";																// url of database
		String sql = "SELECT name, score, timestamp FROM high_scores ORDER BY score DESC, timestamp ASC";		// sql code to return sorted scores from database
		try (Connection conn = DriverManager.getConnection(url);												// connect to database and execute sql code
				Statement smt = conn.createStatement();
				ResultSet rs = smt.executeQuery(sql)) {
			while(rs.next()) {																					// for each score retrieved from database, creates an ArrayList containing name/score/timestamp and adds it to highScoresList 
				ArrayList<Object> score = new ArrayList<Object>();
				Collections.addAll(score, rs.getString("name"), rs.getInt("score"), rs.getLong("timestamp"));
				highScoresList.add(score);
			}
		} catch (SQLException e) {
			System.out.println("Failed load high scores from database");
		}
		return highScoresList;																					// returns scores;
	}

}
