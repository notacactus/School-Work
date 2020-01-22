// Sprite image source: https://openclipart.org/detail/61201/red-racing-car-top-view

import javafx.scene.image.Image;
import javafx.geometry.*;
import javafx.scene.canvas.GraphicsContext;



// contains methods to create and manage player character
public class Player {
	private Image sprite;		// player sprite
	private double centerX;		// x and y coordinated player revolves around
	private double centerY;
	private double posX;		// x and y coordinated of player
	private double posY; 
	private double width;		// dimensions 
	private double height;
	private double posCounter;	// counter used to shift of player position

	// constructor, assigns starting position
	public Player(double centerX, double centerY)
	{  
		this.centerX = centerX;	// sets center coordinates to given values
		this.centerY = centerY;
		posX = centerX + 120;	// sets coordinates based on central position with offset to x
		posY = centerY;
		width = 40;				// sets dimensions
		height = 80;
		sprite = new Image(getClass().getResourceAsStream("carsprite.png"), width, height ,true, true); // loads image with given dimensions
		posCounter = 0;			// sets position counter to 0 
	}
	
	// getters
	public double getPosX() {
		return posX;
	}
	public double getPosY() {
		return posY;
	}
	
	// updates x and y coordinates by rotating around center
	public void update() {
		posX = (centerX + 120 * Math.cos(Math.toDegrees(posCounter))); 
		posY = (centerY + 120 * Math.sin(Math.toDegrees(posCounter)));
	}
	// updates position and draws
	public void render(GraphicsContext gc) {
		gc.drawImage(sprite, posX - width/2, posY - height/2);
	}
	// defines rectangle for collision detection at 2/3 size
	public Rectangle2D getBoundary() {
		return new Rectangle2D(posX - width/3, posY - width/3, 2*width/3, 2*height/3);
	}
	// increases or decreases position counter
	public void incPosCounter() {
		posCounter += 0.001;
	}
	public void decPosCounter() {
		posCounter -= 0.001;
	}
}    


