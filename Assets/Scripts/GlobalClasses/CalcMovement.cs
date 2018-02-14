using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcMovement {
	// the following two values are constant
	int totalNumDevices;
	int numDevInLowestRow;

	// this number is calculated mathematically based off the number of devices in the lowest level
	// and how many total devices you have.
	int totalNumLevels;


	int numDevicesInRow;
	float thetaInit;
	float thetaFinal;
	float radiusInit;
	float yLowestLevel;

	float thetaInc;
	float radiusInc;
	float yInc;
	
	float theta;
	float radius;

	int i;

	int currLevel;
	int remainingDevices;

	Vector3 newPos = new Vector3();
	Vector3 offset;

	public CalcMovement(int totalNumDevices, int numDevInLowestRow) {
		this.totalNumDevices = totalNumDevices;
		this.numDevInLowestRow = numDevInLowestRow;

		// hard coding init vals
		this.thetaInit = 190;
		this.thetaFinal = -10;
		this.radiusInit = 20;
		this.radiusInc = 2;
		this.yLowestLevel = 0;
		this.yInc = 4;

		this.offset = new Vector3(0f, 0f, 0f);

		calcInitVals();
	}
	
	public CalcMovement(int totalNumDevices, int numDevInLowestRow, float thetaInit, float thetaFinal, float radiusInit, float radiusInc, float yLowestLevel, float yInc, Vector3 offset) {
		this.totalNumDevices = totalNumDevices;
		this.numDevInLowestRow = numDevInLowestRow;

		this.thetaInit = thetaInit;
		this.thetaFinal = thetaFinal;
		this.radiusInit = radiusInit;
		this.radiusInc = radiusInc;
		this.yLowestLevel = yLowestLevel;
		this.yInc = yInc;

		this.offset = offset;

		calcInitVals();
	}

	public void setThetaInit(float thetaInit) {
		this.thetaInit = thetaInit;
	}
	public void setThetaFinal(float thetaFinal) {
		this.thetaFinal = thetaFinal;
	}
	public void setRadiusInit(float radiusInit) {
		this.radiusInit = radiusInit;
		calcInitVals();
	}
	public void setRadiusInc(float radiusInc) {
		this.radiusInc = radiusInc;
		calcInitVals();
	}
	public void setYLowestLevel(float yLowestLevel) {
		this.yLowestLevel = yLowestLevel;
	}
	public void setYInc(float yInc) {
		this.yInc = yInc;
	}	
	public void setOffset(Vector3 offset) {
		this.offset = offset;
	}

	public Vector3 getNewPos() {
		loopInteration();
		return this.newPos;
	}


	// All the methods below are private

	private void calcInitVals() {
		i = 0;
		int a = 1;
		int b = 1 + 2 * numDevInLowestRow;
		int c = -2 * totalNumDevices;
		totalNumLevels = solveQuadratic(a, b, c);
		radius = radiusInit + totalNumLevels * radiusInc;

		currLevel = totalNumLevels;
		remainingDevices = totalNumDevices;
	}

	private int solveQuadratic(int a, int b, int c) {
		float insideSquareRoot = b * b - 4 * a * c;
		float squareRoot = Mathf.Sqrt(insideSquareRoot);

		float solution1 = (-b + squareRoot) / 2 * a;
		float solution2 = (-b - squareRoot) / 2 * a;
		
		return (int) (1 + Mathf.Max(solution1, solution2));
	}

	// displays devices starting at top left corner, and going across
	private void loopInteration() {

		if (theta <= thetaFinal) {
			reset();
		}
		remainingDevices--;
		numDevicesInRow = numDevInLowestRow + currLevel;
		thetaInc = (thetaInit - thetaFinal) / numDevicesInRow;

		theta -= thetaInc;

		float xPos = radius * Mathf.Cos(theta * Mathf.Deg2Rad);
		float zPos = radius * Mathf.Sin(theta * Mathf.Deg2Rad);
		float yPos = yLowestLevel + (yInc * currLevel);

		newPos = offset + new Vector3(xPos, yPos, zPos);
		
		i++;
	}

	private void reset() {
		radius -= radiusInc;
		theta = thetaInit;
		currLevel--;
	}
}