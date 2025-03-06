#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_ADXL345_U.h>

// Create an instance of the ADXL345 sensor
Adafruit_ADXL345_Unified accel = Adafruit_ADXL345_Unified(12345);

void setup() {
  Serial.begin(9600);

  // Initialize the sensor
  if (!accel.begin()) {
    Serial.println("Could not find a valid ADXL345 sensor, check wiring!");
    while (1);
  }

  // Set the range of the sensor (e.g., +/- 2g, 4g, 8g, or 16g)
  accel.setRange(ADXL345_RANGE_2_G);
}

void loop() {
  // Get a new sensor event
  sensors_event_t event;
  accel.getEvent(&event);

  // Read the Z-axis acceleration
  float zAcceleration = event.acceleration.z;
  float xAcceleration = event.acceleration.x;
  float yAcceleration = event.acceleration.y;

  // // Print the Z-axis value for debugging
  // Serial.print("Z-Axis Acceleration: ");
  // Serial.println(zAcceleration);

  // // Detect upward shaking (Z-axis increases)
  // if (zAcceleration > 11.0) { // Threshold for upward movement
  //   Serial.println("Upward Shake Detected!");
  // }

  // // Detect downward shaking (Z-axis decreases)
  // if (zAcceleration < 7.0) { // Threshold for downward movement
  //   Serial.println("Downward Shake Detected!");
  // }

  // Small delay for stability
  delay(100);
}