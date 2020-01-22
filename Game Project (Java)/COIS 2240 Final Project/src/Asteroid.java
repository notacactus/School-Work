import java.util.Random;

import javafx.geometry.Rectangle2D;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.image.Image;

// contains methods to create and manage asteroids
public class Asteroid {
	private Image sprite;		// asteroid sprite
	private double posX;		// x and y coordinates
	private double posY; 
	private double velX;		// x and y velocity components
	private double velY;
	private double width; 		// dimensions
	private double height;

	// Constructor for asteroids, randomizes spawnpoint/destination/size
	public Asteroid() {
		Random spawner = new Random();												// Random number generator used to determine asteroid spawning conditions
		int side = spawner.nextInt(3);												// random 0-3 used to determine spawn side
		int startPoint = spawner.nextInt(1000);										// random 0-1000 used to determine position on side
		int endPoint = spawner.nextInt(140);										// random 0-140 used to determine endpoint
		switch (side) {								
		case 0:										
			posX = startPoint;														// Assigns starting coordinates based on generated numbers
			posY = 0;
			velX = getRealVel(430 + endPoint - startPoint);							// calculates velocity components based on starting/endpoint
			velY = getCompVel(velX);
			velX *= Math.log(GameState.getGameState().getGameStage() + Math.E) / 300;
			break;
		case 1:
			posX = startPoint;
			posY = 1000;
			velX = getRealVel(425 + endPoint - startPoint);
			velY = -getCompVel(velX);
			velX *= Math.log(GameState.getGameState().getGameStage() + Math.E) / 300;
			break;
		case 2:
			posY = startPoint;
			posX = 0;
			velY = getRealVel(425 + endPoint - startPoint);
			velX = getCompVel(velY);
			velY *= Math.log(GameState.getGameState().getGameStage() + Math.E) / 300;
			break;
		case 3:
			posY = startPoint;
			posX = 1000;
			velY = getRealVel(425 + endPoint - startPoint);
			velX = -getCompVel(velY);
			velY *= Math.log(GameState.getGameState().getGameStage() + Math.E) / 300;
			break;
		}
		int size = spawner.nextInt(50);												// random 0-50 used to determine size
		width = 25 + size;
		height = 25 + size;
		sprite = new Image(getClass().getResourceAsStream("Circle.png"), width, height, true, true);	// loads image with given dimensions
	}

	// getters
	public double getPosX() {
		return posX;
	}
	public double getPosY() {
		return posY;
	}

	// adjusts position based on velocity
	private void update() {
		posX += velX;
		posY += velY;
	}

	// Updates positon of asteroid and draws it
	public void render(GraphicsContext gc) {
		update();	
		gc.drawImage(sprite, posX - width/2, posY - height/2);
	}

	// defines rectangle for collision detection
	public Rectangle2D getBoundary()
	{
		return new Rectangle2D(posX - width/2, posY - height/2, width, height);
	}

	// Calculates one velocity component given starting position
	private double getRealVel(double vel) {
		return vel * Math.sqrt(2*500*500) / Math.sqrt(vel*vel + 500*500);
	} 
	// Calculates complementary velocity component
	private double getCompVel(double vel) {
		return Math.sqrt(2*500*500 - vel)*Math.log(GameState.getGameState().getGameStage() + Math.E) / 300;
	}

	// detects collision between asteroid and each of missile/planet/player
	public boolean intersects(Missile missile)
	{
		return missile.getBoundary().intersects(this.getBoundary());
	}
	public boolean intersects(Planet planet)
	{
		return planet.getBoundary().intersects(this.getBoundary());
	}
	public boolean intersects(Player player)
	{
		return player.getBoundary().intersects(this.getBoundary());
	}
}


