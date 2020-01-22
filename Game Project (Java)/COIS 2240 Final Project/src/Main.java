/*	Project Name:	COIS 2240 Group Project
 * 	Authors:		Ryland Whillans, Sheldon Raberts
 *	Date:			2108-04-03
 * 	Description:	Simple 2D game involving shooting down asteroid to protect the planet. Also maintains a database of high scores.
 *	Resources Used:	http://www.sqlitetutorial.net/sqlite-java/
 *					https://gamedevelopment.tutsplus.com/tutorials/introduction-to-javafx-for-game-development--cms-23835
 */

import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.layout.Region;
import javafx.scene.layout.StackPane;
import javafx.stage.Stage;

public class Main extends Application {
	@Override
	public void start(Stage primaryStage) throws Exception{
		HighScores.checkDatabase();					// Checks to see if database/highscres table exist and creates them if not found
		primaryStage.setTitle("Tesla Defense");				// Set stage title
		primaryStage.setResizable(false);			// fix stage size
		StackPane root = new StackPane();			// creates root StackPane and defines size
		root.setPrefSize(1000, 1000);
		root.setMaxSize(Region.USE_PREF_SIZE, Region.USE_PREF_SIZE);
		Scene theScene = new Scene(root);
		GUI.loadStart(theScene, root);				// loads the start screen
		primaryStage.setScene(theScene);
		primaryStage.show();
	}

	public static void main(String[] args) {
		launch(args);
	}
}

