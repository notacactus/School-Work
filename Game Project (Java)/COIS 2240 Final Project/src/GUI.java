// Background image source: https://upload.wikimedia.org/wikipedia/commons/1/13/Stars_Galaxy_Rocky_Mountain_%28Unsplash%29.jpg

import java.util.ArrayList;
import java.util.Collections;
import java.util.Iterator;
import javafx.animation.AnimationTimer;
import javafx.application.Platform;
import javafx.beans.binding.Bindings;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.event.EventHandler;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.image.Image;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.Background;
import javafx.scene.layout.BackgroundImage;
import javafx.scene.layout.BackgroundRepeat;
import javafx.scene.layout.ColumnConstraints;
import javafx.scene.layout.GridPane;
import javafx.scene.layout.HBox;
import javafx.scene.layout.RowConstraints;
import javafx.scene.layout.StackPane;
import javafx.scene.paint.Color;
import javafx.scene.text.Font;
import javafx.scene.text.TextAlignment;


// contains methods to create and load start screen, game screen, name entry screen, and high scores screen. Also contains listeners used for transitions between sceens
public final class GUI {
	//private construction to prevent instantiation
	private GUI() {
	}
	
	// creates start scene
	public static void loadStart(Scene theScene, StackPane root) {
		root.setBackground(new Background(new BackgroundImage(new Image (GUI.class.getResourceAsStream("background.jpg")), BackgroundRepeat.NO_REPEAT, BackgroundRepeat.NO_REPEAT, null, null)));	// loads/sets background image
		GridPane grid = new GridPane();																		// Creates and formats GridPane to hold scene elements
		grid.setHgap(10);
		grid.setVgap(10);
		grid.setPadding(new Insets(5, 5, 5, 5));
		grid.getColumnConstraints().addAll(new ColumnConstraints (100), new ColumnConstraints(75), new ColumnConstraints(200), new ColumnConstraints(500));
		grid.getRowConstraints().addAll(new RowConstraints(300),  new RowConstraints(75), new RowConstraints(200));
		Label lblTitle = new Label("Tesla Defence");															// Creates and formats labels to display title/instructions
		lblTitle.setFont(new Font(100));
		lblTitle.setTextFill(Color.RED);
		Label lblInstruct = new Label("How to Play: \n"
				+ "The object of this game is to defend the planet by shooting meteors before they make contact\n"
				+ "Use the mouse to aim and left click to fire\n"
				+ "Use the Left and Right arrow keys to move\n"
				+ "If the planet gets hit to many times the game will end");
		lblInstruct.setTextAlignment(TextAlignment.CENTER);
		lblInstruct.setFont(new Font(20));
		lblInstruct.setTextFill(Color.CHARTREUSE);
		Button btStart = new Button("Start");																// Creates/formats button to start game and defines event for button click
		btStart.setPrefSize(200, 100);
		btStart.setFont(new Font(40));
		btStart.setTextFill(Color.DARKGREEN);
		btStart.setOnAction((event) -> {
			root.getChildren().clear();																		// clears previous scene and loads game scene
			loadGame(theScene, root);
		});
		grid.add(lblTitle, 2, 1, 3, 1);																		// Loads elements into GridPane and adds GridPane to root to display
		grid.add(lblInstruct, 1, 2, 10, 1);
		grid.add(btStart, 3, 3, 3, 3);
		root.getChildren().add(grid);
	}

	// Method to create and load display for score/combo/stage/lives at top of screen
	private static void loadStatusDisplay(StackPane root) {
		Label lblScore = new Label();																		// Creates labels for each status display
		Label lblCombo = new Label();
		Label lblStage = new Label();
		Label lblLives = new Label();
		lblScore.textProperty().bind(Bindings.concat("Score: ", GameState.getGameState().getScoreProperty().asString()));	// Binds properties for each value to corresponding label
		lblCombo.textProperty().bind(Bindings.concat("Combo: ", GameState.getGameState().getComboProperty().asString()));
		lblStage.textProperty().bind(Bindings.concat("Stage: ", GameState.getGameState().getGameStageProperty().asString()));
		lblLives.textProperty().bind(Bindings.concat("Lives: ", GameState.getGameState().getLivesProperty().asString()));
		lblScore.setFont(new Font(25));																		// Format label font size/colour
		lblCombo.setFont(new Font(25));
		lblStage.setFont(new Font(25));
		lblLives.setFont(new Font(25));
		lblScore.setTextFill(Color.CHARTREUSE);
		lblCombo.setTextFill(Color.CHARTREUSE);
		lblStage.setTextFill(Color.CHARTREUSE);
		lblLives.setTextFill(Color.CHARTREUSE);
		GridPane grid = new GridPane();																		// Create and format GridPane to space out labels and adds the to it
		grid.getColumnConstraints().addAll(new ColumnConstraints (300), new ColumnConstraints(300), new ColumnConstraints(300));
		grid.add(lblScore, 0, 0);
		grid.add(lblCombo, 1, 0);
		grid.add(lblStage, 2, 0);
		grid.add(lblLives, 3, 0);
		root.getChildren().add(grid);																		// Adds GridPane to root
	}
	
	// Method to load and run most functions of the game
	// Inspired by examples from: https://gamedevelopment.tutsplus.com/tutorials/introduction-to-javafx-for-game-development--cms-23835
	private static void loadGame(Scene theScene, StackPane root) {
		addGameListener(root);																				// Creates listener to end game
		Canvas canvas = new Canvas(1000, 1000);																// Creates canvas/graphicscontexts to display the game elements 
		GraphicsContext gc = canvas.getGraphicsContext2D();
		root.getChildren().add(canvas);
		loadStatusDisplay(root);																			// Creates status display bar

		Planet planet = new Planet();																		// Creates planet object
		Player player = new Player(planet.getPosX(), planet.getPosY());										// Creates player object
		ArrayList<Missile> missiles = new ArrayList<Missile>();  											// Arraylist containing all missile objects in the game at any point in time
		ArrayList<Asteroid> asteroids = new ArrayList<Asteroid>();											// Arraylist containing all asteroid objects in the game at any point in time

		

		theScene.setOnKeyPressed(new EventHandler<KeyEvent>() { 											// Detects key presses and sets boolean flags to true while left, right, or space are held
			public void handle(KeyEvent e) {
				switch (e.getCode()) {
				case LEFT: GameState.getGameState().setMovingLeft(true); break;
				case RIGHT: GameState.getGameState().setMovingRight(true); break;
				case SPACE: GameState.getGameState().setFiring(true); break;
				default: break;
				}
			}
		});
		theScene.setOnKeyReleased(new EventHandler<KeyEvent>() { 											// sets flags to false when keys are released
			public void handle(KeyEvent e) {
				switch (e.getCode()) {
				case LEFT: GameState.getGameState().setMovingLeft(false); break;
				case RIGHT: GameState.getGameState().setMovingRight(false); break;
				case SPACE: GameState.getGameState().setFiring(false); break;
				default: break;
				}
			}
		});
		new AnimationTimer() {																				// Main game loop
			int asteroidCooldown = 0;																		// counters to control cooldown for missile firing/asteroid spawning
			int missileCooldown = 0;
			public void handle(long currentNanoTime) {
				gc.clearRect(0, 0, canvas.getWidth(), canvas.getHeight());									// Clears canvas
				if (GameState.getGameState().getMovingRight()) {											// Updates position of player/ renders based on if left/right arrows are held
					player.incPosCounter();
					player.update();
				}
				if (GameState.getGameState().getMovingLeft()) {
					player.decPosCounter();
					player.update();
				}
				if (GameState.getGameState().getFiring() && missileCooldown <= 0) {   						// Creates new missile if spacebar is held and missiles not on cooldown
					missiles.add(new Missile(player.getPosX(), player.getPosY(), planet.getPosX(), planet.getPosY()));
					missileCooldown = 50;
				}
				if (asteroidCooldown <= 0) {																// If asteroid spawner not on cooldown, creates a new asteroid and resets cooldown, decreasing with game stage
					asteroids.add(new Asteroid());
					asteroidCooldown = (int)(150 - 40 * Math.log(GameState.getGameState().getGameStage()));
				}
				planet.render(gc);    																		// renders the planet 
				player.render(gc);																			// renders the player
				checkCollisions(missiles, asteroids.iterator());									// Checks for collisions between asteroids and missiles
				updateMissiles(gc, missiles.iterator());													// Updates/redraws missiles and removes them if they have left screen
				updateAsteroids(gc, asteroids.iterator(), planet, player);									// Updates/redraws asteroids and removes them if they have left screen or hit planet/player
				if (missileCooldown > 0)																	// decreases cooldowns for missile/asteroid 
					missileCooldown--;
				if (asteroidCooldown > 0)
					asteroidCooldown--;
				if (GameState.getGameState().gameEndedProperty().getValue() == true)						// stops game loop when end flag has been set to true
					this.stop();
			}
		}.start();
	}
	
	// Method to update position and redraw missiles as well as check removal conditions
	private static void updateMissiles(GraphicsContext gc, Iterator<Missile> missileIter) {
		while (missileIter.hasNext()) {																		// Iterator for missile ArrayList
			Missile missile = missileIter.next();
			missile.render(gc);																				// Updates position/draws each missile
			if (missile.getPosX() > 1000 || missile.getPosY() > 1000 || missile.getPosX() < 0 || missile.getPosY() < 0)	// Checks if any missile has hit egdes of screen and removes it
				missileIter.remove();
		}
	}
	
	// Method to update position and redraw asteroid as well as check removal conditions
	private static void updateAsteroids(GraphicsContext gc, Iterator<Asteroid> asteroidIter, Planet planet, Player player) {
		while (asteroidIter.hasNext()) {																	// Iterator for asteroid Arraylist
			Asteroid asteroid = asteroidIter.next();
			asteroid.render(gc);																			// Updates position/draws each asteroid
			if (asteroid.intersects(planet) || asteroid.intersects(player)) {								// Checks if any asteroid has hit planet or player and deletes it/ updates GameState accordingly
				asteroidIter.remove();
				GameState.getGameState().asteroidHit();
			} else if (asteroid.getPosX() > 1000 || asteroid.getPosY() > 1000 || asteroid.getPosX() < 0 || asteroid.getPosY() < 0) {	// Checks if any asteroid has hit edges of screen and removes it
				asteroidIter.remove();
			}
		}
	}

	// Method to check for collisions between asteroids and missiles. Takes iterator for corresponding ArrayLists and removes elements from both that collide
	private static void checkCollisions(ArrayList<Missile> missiles, Iterator<Asteroid> asteroidIter) {
		Asteroid asteroid;
		Missile missile;
		Iterator<Missile> missileIter;
		while (asteroidIter.hasNext()) {																// Iterator for asteroid ArrayList
			asteroid = asteroidIter.next();
			missileIter = missiles.iterator();
			while (missileIter.hasNext()) {																// Iterator for missile ArrayList
				missile = missileIter.next();
				if(asteroid.intersects(missile)) {															// Checks for collision between each asteroid and missile
					asteroidIter.remove();																	// Removes both objects when collision is detected, then updates GameState accordingly
					missileIter.remove();
					GameState.getGameState().asteroidDestroyed();
				}
			}
		} 
	}

	// creates scene to get user to enter name
	private static void loadNameEntry(StackPane root) {
		addNameListener(root);																			 	// Creates listener for name entered flag
		GridPane grid = new GridPane();																		// Creates and formats GridPane to hold scene elements
		grid.setAlignment(Pos.CENTER);
		grid.setHgap(10);
		grid.setVgap(10);
		grid.setPadding(new Insets(5, 5, 5, 5));
		Label lblHighScoreMsg = new Label("New High Score");												// Creates and formats labels for scene
		lblHighScoreMsg.setFont(new Font(40));
		Label lblNamePrompt = new Label("Enter Name: ");
		lblNamePrompt.setFont(new Font(20));
		Label lblInvalidName = new Label("Name must be 2-15 characters");
		lblHighScoreMsg.setTextFill(Color.CHARTREUSE);
		lblNamePrompt.setTextFill(Color.CHARTREUSE);
		lblInvalidName.setTextFill(Color.RED);
		lblInvalidName.setVisible(false);
		TextField tfName = new TextField();															// Creates text field for name entry and defines event for pressing enter
		tfName.setOnAction((event) -> {
			if ((tfName.getText().length() > 15) || (tfName.getText().length() < 2)) {		// If name too long or short displays message
				lblInvalidName.setVisible(true);
			} else {																						// If name valid saves it and sets nameEntered flag to true for scene transition
				GameState.getGameState().setName(tfName.getText());
				GameState.getGameState().setNameEntered(true);
			}
		});
		HBox hBox = new HBox();																				// Adds all elements to GridPane then adds GridPane to root for display
		hBox.getChildren().addAll(lblNamePrompt, tfName);
		grid.add(lblHighScoreMsg, 0, 0);
		grid.add(hBox, 0, 3);
		grid.add(lblInvalidName, 0, 5);
		root.getChildren().add(grid);
	}

	// loads high scores and creates scene to display them
	private static void loadHighScores(StackPane root) {
		GridPane grid = new GridPane();																		// Creates and formats GridPane to hold scores
		grid.setHgap(10);
		grid.setVgap(10);
		grid.setPadding(new Insets(5, 5, 5, 5));
		grid.getColumnConstraints().addAll(new ColumnConstraints (400), new ColumnConstraints(30), new ColumnConstraints(100), new ColumnConstraints(150), new ColumnConstraints(100), new ColumnConstraints (400));
		grid.getRowConstraints().addAll(new RowConstraints(300), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(25), new RowConstraints(200));
		ArrayList<ArrayList<Object>> highScoreList = HighScores.getHighScoreList();							// gets scores from database loaded into 2D ArrayList
		ArrayList<Label> scoreLabels;
		for(int i = 0; i < highScoreList.size(); i++) {														// converts score data from ArrayList into labels and adds them to GridPane
			scoreLabels = getScoreLabels(String.valueOf(i+1), String.valueOf(highScoreList.get(i).get(0)), String.valueOf(highScoreList.get(i).get(1)), (highScoreList.get(i).get(2).equals(GameState.getGameState().getTimestamp())));
			grid.add(scoreLabels.get(0), 1, i+1);
			grid.add(scoreLabels.get(1), 2, i+1, 2, 1);
			grid.add(scoreLabels.get(2), 4, i+1);
		}
		Button btExit = new Button("Exit");																// creates button to exit program
		btExit.setTextFill(Color.DARKRED);
		btExit.setPrefSize(140, 70);
		btExit.setFont(new Font(30));
		btExit.setOnAction((event) -> {
			Platform.exit();
		});
		grid.add(btExit, 3, 11, 10, 10);
		root.getChildren().add(grid);																		// adds GridPane to root to display
	}
	
	// takes an ArrayList containing score position, name, score and boolean value for whether score was new and returns ArrayList with values in labels formatted appropriately
	private static ArrayList<Label> getScoreLabels(String scorePos, String name, String score, Boolean newScore) {
		Label lblPos = new Label(scorePos);																	// loads score position/name/score into labels and sets font size
		Label lblName = new Label(name);
		Label lblScore = new Label(score);
		lblPos.setFont(new Font(25));
		lblName.setFont(new Font(25));
		lblScore.setFont(new Font(25));
		if (newScore == true) {																				// Sets font colour depending on whether score was new
			lblPos.setTextFill(Color.RED);
			lblName.setTextFill(Color.RED);
			lblScore.setTextFill(Color.RED);
		}
		else {
			lblPos.setTextFill(Color.CHARTREUSE);
			lblName.setTextFill(Color.CHARTREUSE);
			lblScore.setTextFill(Color.CHARTREUSE);
		}
		ArrayList<Label> scoreLabels = new ArrayList<Label>();												// loads labels into new ArrayList and returns them
		Collections.addAll(scoreLabels, lblPos, lblName, lblScore);
		return scoreLabels;
	}
	
	// creates listener that transitions to next scene after game finishes
	private static void addGameListener(StackPane root) {
		GameState.getGameState().gameEndedProperty().addListener(new ChangeListener<Boolean>() {			// adds change listener to gameEnded property
			@Override
			public void changed(ObservableValue<? extends Boolean> o, Boolean oldText, Boolean newText) {
				root.getChildren().clear();																	// clears previous scene
				if (!(HighScores.enoughHighScores())){														// if too few high scores in database loads name entry scene 
					loadNameEntry(root);
				} else if (HighScores.higherScore(GameState.getGameState().getScore())) {					// if new score is higher than any in database removes lowest score from database then loads name entry scene
					HighScores.removeHighScore();
					loadNameEntry(root);
				} else {																					// otherwise loads high score scene
					loadHighScores(root);
				}
				GameState.getGameState().gameEndedProperty().removeListener(this);							// removes listener after finish
			}
		});
	}

	// creates a listener that transitions to next scene after the user has entered a valid name
	private static void addNameListener(StackPane root) {
		GameState.getGameState().nameEnteredProperty().addListener(new ChangeListener<Boolean>() {			// Adds change listener to nameEntered property 
			@Override
			public void changed(ObservableValue<? extends Boolean> o, Boolean oldText, Boolean newText) {															
				HighScores.addHighScore(GameState.getGameState().getName(), GameState.getGameState().getScore(), GameState.getGameState().getTimestamp());	// adds score to database
				root.getChildren().clear();																	// Clears previous scene and load high score scene
				loadHighScores(root);
				GameState.getGameState().nameEnteredProperty().removeListener(this);						// removes listener after finished
			}
		});
	}
}
