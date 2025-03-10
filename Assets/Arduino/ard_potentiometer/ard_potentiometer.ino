const int numReadings = 10;

int readings[numReadings];  // the readings from the analog input
int readIndex = 0;          // the index of the current reading
int total = 0;              // the running total
int average = 0;            // the average
int crntamt = 0;
int inputPin = A1;
bool down = false;


void setup() {
  Serial.begin(9600);
  for (int thisReading = 0; thisReading < numReadings; thisReading++) {
    readings[thisReading] = 0;
  }
  pinMode(12, INPUT_PULLUP);
  pinMode(11, OUTPUT);
}

void loop() {

  Potentio();
  Button();

  delay(10);
}

void Potentio(){
    total = total - readings[readIndex];
  readings[readIndex] = analogRead(inputPin);
  total = total + readings[readIndex];
  readIndex = readIndex + 1;

  if (readIndex >= numReadings) {
    readIndex = 0;
  }

  average = total / numReadings;
  if(crntamt != average/10){
      Serial.println("_"+String(average/10));
    crntamt = average/10;
  }
}

void Button() {

  
  if(digitalRead(12) == HIGH && down == false){
    down = true;
  }
  else if(digitalRead(12) == LOW && down == true){
    Serial.println("0");
    down = false;
  }

  if(digitalRead(12) == HIGH){
      digitalWrite(11, LOW );

  }
  else{
  digitalWrite(11, HIGH);

  }

}
