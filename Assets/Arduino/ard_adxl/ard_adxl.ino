#include <Wire.h>  // Wire library - used for I2C communication

int ADXL345 = 0x53; // The ADXL345 sensor I2C address
int phase = 3;
float X_out, Y_out, Z_out;  // Outputs
float roll,pitch,rollF,pitchF=0;

  bool up = false;
 int cutamt = 0;
 float prev;

 int crnt = 0;
const int thresh = 2;
const int thresh2 = 1;

const int BUFFER_SIZE = 50;
char buf[BUFFER_SIZE];


void setup() {
  Serial.begin(9600); // Initiate serial communication for printing the results on the Serial monitor
 
  Wire.begin(); // Initiate the Wire library
  // Set ADXL345 in measuring mode
  Wire.beginTransmission(ADXL345); // Start communicating with the device
  Wire.write(0x2D); // Access/ talk to POWER_CTL Register - 0x2D
  // Enable measurement
  Wire.write(8); // Bit D3 High for measuring enable (8dec -> 0000 1000 binary)
  Wire.endTransmission();
  delay(10);

  //Off-set Calibration
  //X-axis
  Wire.beginTransmission(ADXL345);
  Wire.write(0x1E);
  Wire.write(1);
  Wire.endTransmission();
  delay(10);
  //Y-axis
  Wire.beginTransmission(ADXL345);
  Wire.write(0x1F);
  Wire.write(-2);
  Wire.endTransmission();
  delay(10);

  //Z-axis
  Wire.beginTransmission(ADXL345);
  Wire.write(0x20);
  Wire.write(-9);
  Wire.endTransmission();
  delay(10);
}

void loop() {
  if(Serial.available() > 0){
    int len = Serial.readBytes(buf, BUFFER_SIZE);
    phase = atoi(buf);
  }
   // === Read acceleromter data === //
  Wire.beginTransmission(ADXL345);
  Wire.write(0x32); // Start with register 0x32 (ACCEL_XOUT_H)
  Wire.endTransmission(false);

  Wire.requestFrom(ADXL345, 6, true); // Read 6 registers total, each axis value is stored in 2 registers
  X_out = ( Wire.read() | Wire.read() << 8); // X-axis value
  X_out = X_out / 256; //For a range of +-2g, we need to divide the raw values by 256, according to the datasheet
  Y_out = ( Wire.read() | Wire.read() << 8); // Y-axis value
  Y_out = Y_out / 256;  
  Z_out = ( Wire.read() | Wire.read() << 8); // Z-axis value
  Z_out = Z_out / 256;

  // Calculate Roll and Pitch (rotation around X-axis, rotation around Y-axis)
  roll = atan(Y_out / sqrt(pow(X_out, 2) + pow(Z_out, 2))) * 180 / PI;
  pitch = atan(-1 * X_out / sqrt(pow(Y_out, 2) + pow(Z_out, 2))) * 180 / PI;

  // Low-pass filter
  rollF = 0.94 * rollF + 0.06 * roll;
  pitchF = 0.94 * pitchF + 0.06 * pitch;

  switch(phase){
    case 0:
    Cutting(pitchF);
    break;
    case 1:
    Cutting(pitchF);
    break;
    case 2:
    Salting(pitchF);
    break;
    case 3:
    Mixing(rollF);
    break;
  }

}


void Cutting(float dat){

  if(dat > 20){
    if(up == false){
          up = true;
    }
  }
  else if(dat < 8){
    if(up == true){
          up = false;
          cutamt++;
          Serial.println(cutamt);
    }
  }

}

void Salting(float dat){
  if(up == true){
    if(dat >= crnt){
      crnt = dat;
    }
    else if(dat < crnt - thresh){
      crnt = dat;
      up = false;
    }
    else if(dat < crnt){
      crnt = dat;
    }
    
  }
  else if(up == false){
    if(dat <= crnt){
      crnt = dat;
    }
    else if(dat > crnt + thresh){
      crnt = dat;
      up = true;

      cutamt++;
      Serial.println(cutamt);
    }
    else if(dat > crnt){
      crnt = dat;
    }
  }
}


void Mixing(float dat){
  if(up == true){
    if(dat >= crnt){
      crnt = dat;
    }
    else if(dat < crnt - thresh2){
      crnt = dat;
      up = false;
    }
    else if(dat < crnt){
      crnt = dat;
    }
    
  }
  else if(up == false){
    if(dat <= crnt){
      crnt = dat;
    }
    else if(dat > crnt + thresh2){
      crnt = dat;
      up = true;

      cutamt++;
      Serial.println(cutamt);
    }
    else if(dat > crnt){
      crnt = dat;
    }
  }
}