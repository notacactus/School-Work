// sprite image source: https://apod.nasa.gov/apod/image/0304/bluemarble2k_big.jpg

import javafx.geometry.Rectangle2D;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.image.Image;

// contains methods to create and manage the central planet
public class Planet {
	private double posX;					// X and Y coordinates
	private double posY;  
	private double width; 					// Dimensions
	private double height;
	private Image sprite;					// sprite image
	
	public Planet() {
		posX = 500;							// sets coordinates to center of screen
		posY = 500;
		height = 150;						// sets dimensions
		width = 150;
		sprite = new Image(getClass().getResourceAsStream("earth.png"), width, height, true, true); // loads image with given dimesions		
	}
	
	// getters
	public double getPosX() {
		return posX;
	}

	public double getPosY() {
		return posY;
	}
	
	// draws the image at given coordinates
	public void render(GraphicsContext gc) {
		gc.drawImage(sprite, posX - width/2, posY - height/2);
	}
	
	// defines rectangle for collisions as 2/3 size of planet
	public Rectangle2D getBoundary()
    {
    	return new Rectangle2D(posX - width/3, posY - height/3, 2*width/3, 2*height/3);
    }
}
