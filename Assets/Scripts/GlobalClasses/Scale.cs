using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale {
	public float min;
	public float max;

	public Scale(float min, float max) {
		this.min = min;
		this.max = max;
	}

	// This is one of the most helpful functions that exists when you want to change the
	// properties of an object based on their position in relation to a preset min and max val. 
	// Ex: if you have 10 objects and you want their size to be detemined by their y position. 
	// You use the constructor to pass in the min y val, and the max y val out of ALL the 10 objects. 
	// Then for each object, pass it's indiviual y val into the 
	// getScale function and it returns the percent (from 0 to 1) of where that object lies 
	// betweeen the min and max vals. You can use this returned percentage to scale the object accordingly.

	public float getScale(float val) {
		float percent = Mathf.Abs((min - val) / (max - min));
		return percent;
	}

	public Vector3 getScale(Vector3 val) {
		float x = getScale(val.x);
		float y = getScale(val.y);
		float z = getScale(val.z);
		return new Vector3(x, y, z);
	}

	public Vector3 getRainbow(float val) {
		float percent = getScale(val);
		float r = getR(percent);
		float g = getG(percent);
		float b = getB(percent);
		return new Vector3(r, g, b);
	}

	/*glm::vec3 Rainbow::getColor(float value) {
	    float percent = value / (max - min);
	    return glm::vec3(getR(percent), getG(percent), getB(percent));
	}

	glm::vec3 Rainbow::getColor(glm::vec3 value) {
	    glm::vec3 percent = value / (max - min);
	    return glm::vec3(getR(percent.x), getG(percent.y), getB(percent.z));
	}*/

	float getR(float percent) {
	    if (percent <= 0.5f) {
	        return 1.0f - (2.0f * percent);
	    }
	    else {
	        return (percent-0.5f) * 2.0f;
	    }
	}
	float getG(float percent) {
	    if (percent <= 0.25f) {
	        return percent * 4.0f;
	    }
	    else if (percent <= 0.5f) {
	        return 1f;
	    }
	    else if (percent <= 0.75f) {
	        return 1.0f - ((percent-0.5f) * 4.0f);
	    } 
	    else {
	        return 0.0f;
	    }
	}
	float getB(float percent) {
	    if (percent <= 0.25f) {
	        return 0.0f;
	    }
	    else if (percent <= 0.5f) {
	        return (percent-0.25f) * 4.0f;
	    }
	    else if (percent <= 0.75f) {
	        return 1.0f;
	    } 
	    else {
	        return 1.0f-((percent-0.75f) * 4.0f);
	    }
	}
}
