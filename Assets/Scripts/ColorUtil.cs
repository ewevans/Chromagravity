using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



//Color structs used 
//RGB: R (0-255), G (0-255), B (0-255)


//CIE L*a*b* color space: 
public struct Lab
{
	public double L;
	public double a;
	public double b;


	public Lab(double x, double y, double z)
	{
		this.L = x;
		this.a = y;
		this.b = z;
	}
	public static Lab operator +(Lab lab1, Lab lab2)
	{
		lab1.L += lab2.L;
		lab1.a += lab2.a;
		lab1.b += lab2.b;
		return lab1;
	}

	public static Lab operator /(Lab lab1, Lab lab2)
	{
		lab1.L /= lab2.L;
		lab1.a /= lab2.a;
		lab1.b /= lab2.b;
		return lab1;
	}
}

public static class ColorUtil
{
	//Used in averaging 25 Lab colors
	public static Lab DivideBy(double divisor, Lab lab)
	{
		lab.L /= divisor;
		lab.a /= divisor;
		lab.b /= divisor;
		return lab;
	}

	public static Color ConvertLABtoRGB(Lab lab)
	{
		double x, y, z, //CIE XYZ color space used as stepping stone for conversion //D65, (0.9505, 1.0, 1.0890)
		fy, fx, fz, //variables needed for calculation
		fr, fg, fb; //normalized rgb conversion
		int r, g, b; // final 


		fy = (lab.L + 16) / 116.0;
		fx = fy + (lab.a / 500.0);
		fz = fy - (lab.b / 200.0);

		//Attempt 2 (easyrgb)
		if ((fy*fy*fy) > 0.008856)
		{
			fy = fy * fy * fy;
		}
		else
		{
			fy = (fy - 16 / 116) / 7.787;
		}
		if ((fx * fx * fx) > 0.008856)
		{
			fx = fx * fx * fx;
		}
		else
		{
			fx = (fx - 16 / 116) / 7.787;
		}
		if ((fz * fz * fz) > 0.008856)
		{
			fz = fz * fz * fz;
		}
		else
		{
			fz = (fz - 16 / 116) / 7.787;
		}

		x = 95.047 * fx;
		y = 100.000 * fy;
		z = 108.883 * fz;

		fx = x / 100;
		fy = y / 100;
		fz = z / 100;

		fr = fx *  3.2406 + fy * -1.5372 + fz * -0.4986;
		fg = fx * -0.9689 + fy *  1.8758 + fz *  0.0415;
		fb = fx *  0.0557 + fy * -0.2040 + fz *  1.0570;

		if ( fr > 0.0031308 ) 
		{
			fr = 1.055 * ( Math.Pow(fr, ( 1.0 / 2.4 ))) - 0.055;
		}
		else
		{
			fr = 12.92 * fr;
		}
		if ( fg > 0.0031308 ) 
		{
			fg = 1.055 * ( Math.Pow(fg,( 1.0 / 2.4 ))) - 0.055;
		}
		else
		{
			fg = 12.92 * fg;
		}
		if (fb > 0.0031308)
		{
			fb = 1.055 * ( Math.Pow(fb, (1.0 / 2.4))) - 0.055;
		}
		else
		{
			fb = 12.92 * fb;
		}

		if (fr > 1) {
			fr = 1f;
		}
		if (fg > 1f) {
			fg = 1f;
		}
		if (fb > 1f) {
			fb = 1f;
		}



		return new Color((float)fr * 255.0f, (float)fb * 255.0f, (float)fg * 255.0f, 255);
	}

	public static Lab ConvertRGBtoLAB(Color rgb)
	{
		double rLinear, gLinear, bLinear;
		double x,y,z;
		Lab lab = new Lab();

		rLinear = (double)rgb.r / 255.0;
		gLinear = (double)rgb.g / 255.0;
		bLinear = (double)rgb.b / 255.0;

		double r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (
			1 + 0.055), 2.2) : (rLinear / 12.92);
		double g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (
			1 + 0.055), 2.2) : (gLinear / 12.92);
		double b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (
			1 + 0.055), 2.2) : (bLinear / 12.92);

		x = r * 0.4124 + g * 0.3576 + b * 0.1805;
		y = r * 0.2126 + g * 0.7152 + b * 0.0722;
		z = r * 0.0193 + g * 0.1192 + b * 0.9505;

		lab.L = 116.0 * Fxyz(y / 1.0) - 16;
		lab.a = 500.0 * (Fxyz(x / 0.9505) - Fxyz(y / 1.0));
		lab.b = 200.0 * (Fxyz(y / 1.0) - Fxyz(z / 1.0890));

		return lab;
	}

	//Helper function for conversions
	private static double Fxyz(double t)
	{
		return ((t > 0.008856) ? Math.Pow(t, (1.0 / 3.0)) : (7.787 * t + 16.0 / 116.0));
	}

	public static Color AvgColor(Color rgb1, Color rgb2)
	{
		Lab lab1, lab2;
		lab1 = new Lab();
		lab2 = new Lab();

		lab1 = ConvertRGBtoLAB(rgb1);
		lab2 = ConvertRGBtoLAB(rgb2);

		//return sum/2 
		return ConvertLABtoRGB(DivideBy(2,(lab1 + lab2)));
	}



	public static float GetHueLightnessFromRGB(Color rgb, ref float chroma)
	{
		float h = 0;

		float r = rgb.r / 255.0f;
		float g = rgb.g / 255.0f;
		float b = rgb.b / 255.0f;

		float max = Math.Max(r, Math.Max(g, b));
		float min = Math.Min(r, Math.Min(g, b));

		if (max == min)
		{
			h = 0f;
		}
		else if (max==r && g>=b)
		{
			h = 60.0f * (g - b) / (max - min);
		}
		else if (max == r && g < b)
		{
			h = 60.0f * (g - b) / (max - min) + 360.0f;
		}
		else if (max == g)
		{
			h = 60.0f * (b - r) / (max - min) + 120.0f;
		}
		else if (max == b)
		{
			h = 60.0f * (r - g) / (max - min) + 240.0f;
		}

		chroma = (max - min);

		return h;
	}

		


}
